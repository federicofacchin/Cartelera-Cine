using NT1_2022_1C_B_G2.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace NT1_2022_1C_B_G2.Models
    
{
    public class Reserva
    {
        public int Id { get; set; }

        [Display(Name = Alias.FechaAlta)]
        [DataType(DataType.Date)]
        public DateTime FechaAlta { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Range(Restricciones.MinInt1, int.MaxValue , ErrorMessage = MensajesError.StrMinMax)]
        [Display(Name = Alias.CantidadButacas)]
        public int CantidadButacas { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Display(Name = Alias.Cliente)]
        public int ClienteId { get; set; }

        [Display(Name = Alias.Cliente)]
        public Cliente Cliente { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Display(Name = Alias.Funcion)]
        public int FuncionId { get; set; }

        [Display(Name = Alias.Funcion)]
        public Funcion Funcion { get; set; }

        public bool ReservaActiva { get; set; }
    }

}
