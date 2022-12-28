using Microsoft.AspNetCore.Mvc;
using NT1_2022_1C_B_G2.Helpers;
using System.ComponentModel.DataAnnotations;

namespace NT1_2022_1C_B_G2.ViewModels
{
    public class Registracion
    {
        [Required(ErrorMessage = MensajesError.Requerido)]
        [EmailAddress(ErrorMessage = MensajesError.ErrorMail)]
        //[Remote(action: "EmailDisponible", Controllers: "Account")]
        [Display(Name = Alias.Email)]
        public string Email { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = Alias.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = MensajesError.PassMissmatch)]
        [Display(Name = Alias.Password)]
        public string ConfirmarPassword { get; set; }

    }
}
