using NT1_2022_1C_B_G2.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT1_2022_1C_B_G2.Models
{
    public class Funcion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [RangoFecha()]
        [Display(Name = "Fecha y Hora")]
        public DateTime FechaYHora { get; set; }

        [Display(Name = Alias.Descripcion)]
        public string DescripcionDetallada{ get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Display(Name = Alias.SalaId)]
        public int SalaId { get; set; }

        public Sala Sala { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Display(Name = Alias.Pelicula)]
        public int PeliculaId { get; set; }

        [Display(Name = "Película")]
        public Pelicula Pelicula { get; set; }

        public List<Reserva> Reservas { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        public Boolean Confirmada { get; set; }


    }
}
