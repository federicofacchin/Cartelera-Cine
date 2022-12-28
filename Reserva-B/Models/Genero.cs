using NT1_2022_1C_B_G2.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT1_2022_1C_B_G2.Models
{
    public class Genero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(Restricciones.MaxLengh3, MinimumLength = Restricciones.MinLengh2, ErrorMessage = MensajesError.LenghtMinMax)]
        [RegularExpression(@"[a-z - A-Z]*", ErrorMessage = MensajesError.ErrorTextoStandar)]
        public string Nombre { get; set; }

        public List<Pelicula> Peliculas { get; set; }

    }
}
