using NT1_2022_1C_B_G2.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT1_2022_1C_B_G2.Models
{
    public class TipoSala
    {
        public int Id { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(Restricciones.MaxLengh2, MinimumLength = Restricciones.MinLengh2, ErrorMessage = MensajesError.LenghtMinMax)]
        [RegularExpression(@"[a-z - A-Z - 0-9]*", ErrorMessage = MensajesError.ErrorTextoStandar)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Range(Restricciones.MinInt1, int.MaxValue , ErrorMessage = MensajesError.StrMinMax)]
        public double Precio { get; set; }

        public List<Sala> Salas { get; set; }

    }
}
