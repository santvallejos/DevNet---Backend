using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Models;
using DevNet_WebAPI.Infrastructure.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly DevnetDBContext _context;

    public AuthController(JwtService jwtService, DevnetDBContext context)
    {
        _jwtService = jwtService;
        _context = context;
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

        // Verificar la contraseña usando PasswordHasher (asegurándose de que las contraseñas no se almacenan en texto claro)
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
    public IActionResult Register([FromBody] RegisterUserDto registerDto)
    {
        // Verificar si el nombre de usuario ya existe en la base de datos
        if (_context.Users.Any(u => u.Username == registerDto.Username))
        {
            return BadRequest("El nombre de usuario ya está en uso.");
        }

        // Verificar si el correo electrónico ya está registrado
        if (_context.Users.Any(u => u.Email == registerDto.Email))
        {
            return BadRequest("El correo electrónico ya está registrado.");
        }

        // Verificar si el RoleId existe
        var role = _context.Roles.FirstOrDefault(r => r.Id == registerDto.RoleId);
        if (role == null)
        {
            return BadRequest("El rol especificado no existe.");
        }

        // Crear una nueva instancia de User
        var passwordHasher = new PasswordHasher<User>();
        var newUser = new User
        {
            Name = registerDto.Name,                          // Asignar el nombre
            LastName = registerDto.LastName,                  // Asignar el apellido
            Username = registerDto.Username,                  // Asignar el nombre de usuario
            Email = registerDto.Email,                        // Asignar el correo electrónico
            Password = passwordHasher.HashPassword(null, registerDto.Password), // Hashear la contraseña
            ProfileImageUrl = registerDto.ProfileImageUrl,    // Asignar la URL de la imagen de perfil (si la tiene)
            RoleId = registerDto.RoleId,                      // Asignar el ID del rol
            CreatedAt = DateTime.UtcNow                       // Asignar la fecha de creación
        };

        // Guardar el nuevo usuario en la base de datos
        _context.Users.Add(newUser);
        _context.SaveChanges();

        return Ok("Usuario registrado con éxito.");
    }
}
