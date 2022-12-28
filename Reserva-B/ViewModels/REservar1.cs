using NT1_2022_1C_B_G2.Helpers;
using NT1_2022_1C_B_G2.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace NT1_2022_1C_B_G2.ViewModels
    {
        public class Reservar1
        {
        public int PeliculaId { get; set; }


        [Display(Name = Alias.FechaAlta)]
        [DataType(DataType.Date)]
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        [Range(Restricciones.MinInt1, int.MaxValue, ErrorMessage = MensajesError.StrMinMax)]
        [Display(Name = Alias.CantidadButacas)]
        public int CantidadButacas { get; set; }

        [Display(Name = Alias.Funcion)]
        public int FuncionId { get; set; }


    }
}


