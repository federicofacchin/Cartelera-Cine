using NT1_2022_1C_B_G2.Helpers;
using System.ComponentModel.DataAnnotations;

namespace NT1_2022_1C_B_G2.ViewModels
{
    public class DatosContacto
    {

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(Restricciones.MaxLengh2, MinimumLength = Restricciones.MinLengh4, ErrorMessage = MensajesError.LenghtMinMax)]
        [Display(Name = Alias.Direccion)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Phone(ErrorMessage = MensajesError.ErrorTelefono)]
        [Display(Name = Alias.Telefono)]
        public string Telefono { get; set; }

    }
}
