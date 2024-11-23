using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DevNet_BusinessLayer.Services;
using DevNet_BusinessLayer.Interfaces;
using DevNet_BusinessLayer.DTOs;
using System.Linq;
using DevNet_WebAPI.Infrastructure.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly DevnetDBContext _context;
    private readonly UserAccountService _userAccountService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IJwtService jwtService, DevnetDBContext context, UserAccountService userAccountService, ILogger<AuthController> logger)
    {
        _jwtService = jwtService;
        _context = context;
        _userAccountService = userAccountService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        try
        {
            // Validaciones
            if (_context.Users.Any(u => u.Username == registerDto.Username))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El nombre de usuario ya está en uso.",
                    errors = new[] { new { description = "Username already exists" } }
                });
            }

            if (_context.Users.Any(u => u.Email == registerDto.Email))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El correo electrónico ya está registrado.",
                    errors = new[] { new { description = "Email already registered" } }
                });
            }

            // Obtener el rol de usuario
            var requestRole = _context.Roles.FirstOrDefault(r => r.Id == registerDto.RoleId);

            if (registerDto.RoleId != null)
            {
                requestRole = _context.Roles.FirstOrDefault(r => r.Id == registerDto.RoleId);
                if (requestRole == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Error en la configuración del sistema.",
                        errors = new[] { new { description = "Default user role not found" } }
                    });
                }           
            }
            else
            {
                requestRole = _context.Roles.First();
                if (requestRole == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Error en la configuración del sistema.",
                        errors = new[] { new { description = "Aun no hay roles creados en la base de datos." } }
                    });
                }
            }
       

            // Crear usuario
            var passwordHasher = new PasswordHasher<User>();
            var newUser = new User
            {
                Name = registerDto.Name,
                LastName = registerDto.LastName,
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = passwordHasher.HashPassword(null, registerDto.Password),
                ProfileImageUrl = registerDto.ProfileImageUrl ?? null,
                RoleId = requestRole.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Generar token para el nuevo usuario
            var token = _jwtService.GenerateToken(newUser.Username, requestRole.Name);

            // Devolver respuesta exitosa
            return Ok(new
            {
                success = true,
                message = "Usuario registrado exitosamente",
                data = new
                {
                    userId = newUser.Id,
                    username = newUser.Username,
                    email = newUser.Email,
                    token = token,
                    role = requestRole.Name
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el registro de usuario");

            return StatusCode(500, new
            {
                success = false,
                message = "Error interno del servidor",
                errors = new[] { new { description = "Se produjo un error al procesar la solicitud" } }
            });
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == loginDto.Username || u.Email == loginDto.Username);

            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Credenciales inválidas",
                    errors = new[] { new { description = "Invalid credentials" } }
                });
            }

            var passwordHasher = new PasswordHasher<User>();
            if (passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password)
                != PasswordVerificationResult.Success)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Credenciales inválidas",
                    errors = new[] { new { description = "Invalid credentials" } }
                });
            }

            var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            if (role == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Error en la configuración del usuario",
                    errors = new[] { new { description = "User role not found" } }
                });
            }

            var token = _jwtService.GenerateToken(user.Username, role.Name);

            return Ok(new
            {
                success = true,
                message = "Inicio de sesión exitoso",
                data = new
                {
                    userId = user.Id,
                    username = user.Username,
                    email = user.Email,
                    token = token,
                    role = role.Name
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el inicio de sesión");

            return StatusCode(500, new
            {
                success = false,
                message = "Error interno del servidor",
                errors = new[] { new { description = "Se produjo un error al procesar la solicitud" } }
            });
        }
    }
}
