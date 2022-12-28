using Microsoft.AspNetCore.Http;
using NT1_2022_1C_B_G2.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace NT1_2022_1C_B_G2.ViewModels
{
    public class CrearPelicula
    {
       
        public IFormFile Imagen { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [FechaFija()]
        [Display(Name = Alias.FechaLanzamiento)]
        [DataType(DataType.Date)]
        public DateTime FechaLanzamiento { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(Restricciones.MaxLengh2, MinimumLength = Restricciones.MinLengh2, ErrorMessage = MensajesError.LenghtMinMax)]
        [Display(Name = Alias.Titulo)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [StringLength(Restricciones.MaxLengh1, MinimumLength = Restricciones.MinLengh1, ErrorMessage = MensajesError.LenghtMinMax)]
        [DataType(DataType.MultilineText)]
        [Display(Name = Alias.Descripcion)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = MensajesError.Requerido)]
        [Display(Name = Alias.GeneroId)]
        public int GeneroId { get; set; }

    }
}

