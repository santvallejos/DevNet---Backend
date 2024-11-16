using DevNet_WebAPI.Infrastructure.DTO;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        // Aquí se validan las credenciales del usuario.
        if (loginDto.Username == "admin" && loginDto.Password == "password")
        {
            // Generar token para el usuario autenticado.
            var token = _jwtService.GenerateToken(loginDto.Username, "Admin");

            return Ok(new { Token = token });
        }

        return Unauthorized("Credenciales inválidas.");
    }
}