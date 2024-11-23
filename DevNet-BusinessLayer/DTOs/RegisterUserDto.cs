using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevNet_WebAPI.Infrastructure.DTO
{
    public class RegisterUserDto
    {
        // Nombre del usuario (obligatorio)
        [Required]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Name { get; set; }

        // Apellido del usuario (obligatorio)
        [Required]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder los 50 caracteres.")]
        public string LastName { get; set; }

        // Nombre de usuario (obligatorio y único)
        [Required]
        [StringLength(30, ErrorMessage = "El nombre de usuario no puede exceder los 30 caracteres.")]
        public string Username { get; set; }

        // Correo electrónico (obligatorio y único)
        [Required]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Email { get; set; }

        // Contraseña (obligatoria, con validaciones opcionales de seguridad)
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; }

        // URL opcional para la imagen de perfil
        //[Url(ErrorMessage = "La URL de la imagen de perfil no es válida.")]
        public string? ProfileImageUrl { get; set; }

        // ID del rol asignado al usuario (obligatorio)
        // [Required]
        public Guid? RoleId { get; set; }
    }
}
