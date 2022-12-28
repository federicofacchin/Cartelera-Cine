using NT1_2022_1C_B_G2.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT1_2022_1C_B_G2.ViewModels
{
    public class EditarSala
    {
        [Required(ErrorMessage = MensajesError.Requerido)]
        [Range(Restricciones.MinInt1, int.MaxValue, ErrorMessage = MensajesError.StrMinMax)]
        public int Numero { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Range(Restricciones.MinInt1, int.MaxValue, ErrorMessage = MensajesError.StrMinMax)]
        [Display(Name = Alias.CantidadButacas)]
        public int CapacidadButacas { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Display(Name = Alias.TipoSala)]
        public int TipoSalaId { get; set; }

    }
}
