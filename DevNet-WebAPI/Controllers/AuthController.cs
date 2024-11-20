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
    private readonly UserAccountService _userAccountService;

    public AuthController(IJwtService jwtService, DevnetDBContext context, UserAccountService userAccountService)
    {
        _jwtService = jwtService;
        _context = context;
        _userAccountService = userAccountService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        try
        {
            string token = _userAccountService.LoginUser(loginDto);
            return Ok(token);
        }
        catch (Exception)
        {
            return BadRequest("Credenciales Incorrectas.");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        var state = await _userAccountService.RegisterUserAsync(registerDto);

        if (state) return Ok("Usuario registrado con éxito.");

        return BadRequest("No se pudo registrar al usuario.");
    }
}
