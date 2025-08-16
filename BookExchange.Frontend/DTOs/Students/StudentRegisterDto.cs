using System.ComponentModel.DataAnnotations;

namespace BookExchange.Frontend.DTOs.Students;

public class StudentRegisterDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de correo inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Password { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "El campus es obligatorio")]
    public string Campus { get; set; }
}
