using Microsoft.AspNetCore.Identity;
using NT1_2022_1C_B_G2.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace NT1_2022_1C_B_G2.Models
{
    public abstract class Persona : IdentityUser<int>
    {
        [Required(ErrorMessage = MensajesError.Requerido)]
        [RegularExpression(@"[0-9]{2}[0-9]{3}[0-9]{3}", ErrorMessage = MensajesError.ErrorDNI)]
        [Display(Name = Alias.DNI)]
        
        public string Dni { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(Restricciones.MaxLengh4, MinimumLength = Restricciones.MinLengh2, ErrorMessage = MensajesError.LenghtMinMax)]
        [RegularExpression(@"[a-z A-Z]*", ErrorMessage = MensajesError.ErrorTextoStandar)]
        [Display(Name = Alias.Nombre)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(Restricciones.MaxLengh4, MinimumLength = Restricciones.MinLengh2, ErrorMessage = MensajesError.LenghtMinMax)]
        [RegularExpression(@"[a-z A-Z]*", ErrorMessage = MensajesError.ErrorTextoStandar)]
        [Display(Name = Alias.Apellido)]
        public string Apellido { get; set; }       

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(Restricciones.MaxLengh2, MinimumLength = Restricciones.MinLengh4, ErrorMessage = MensajesError.LenghtMinMax)]
        [Display(Name  = Alias.Direccion)]
        public string Direccion { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Phone(ErrorMessage = MensajesError.ErrorTelefono)]
        [Display(Name = Alias.Telefono)]
        public string Telefono { get; set; }

        public string TelefonoCodArea
        {
            get
            {
                return $"+54 {Telefono}";
            }
        }
        
        [DataType(DataType.Password)]
        [Display(Name = Alias.Password)]
        public string Password { get; set; }

    }
}
