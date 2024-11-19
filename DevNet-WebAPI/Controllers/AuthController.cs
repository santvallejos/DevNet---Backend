using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DevNet_BusinessLayer.Services;
using DevNet_BusinessLayer.Interfaces;
using DevNet_BusinessLayer.DTOs;
using System.Linq;
using DevNet_WebAPI.Infrastructure.DTO;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly DevnetDBContext _context;
    private readonly IUserAccountService _userAccountService;

    public AuthController(IJwtService jwtService, DevnetDBContext context, IUserAccountService userAccountService)
    {
        _jwtService = jwtService;
        _context = context;
        _userAccountService = userAccountService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        // Buscar al usuario en la base de datos
        var user = _context.Users
            .FirstOrDefault(u => u.Username == loginDto.Username);

        if (user == null)
        {
            return Unauthorized("Credenciales inválidas.");
        }

        // Verificar la contraseña
        var passwordHasher = new PasswordHasher<User>();
        if (passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password) != PasswordVerificationResult.Success)
        {
            return Unauthorized("Credenciales inválidas.");
        }

        // Obtener el nombre del rol usando RoleId del usuario
        var role = _context.Roles
            .FirstOrDefault(r => r.Id == user.RoleId);

        if (role == null)
        {
            return Unauthorized("El rol del usuario no existe.");
        }

        // Generar el token para el usuario autenticado
        var token = _jwtService.GenerateToken(user.Username, role.Name);  // Usamos el nombre del rol

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        var state = await _userAccountService.RegisterUserAsync(registerDto);

        if (state) return Ok("Usuario registrado con éxito.");

        return BadRequest("No se pudo registrar al usuario.");
    }
}
