using NT1_2022_1C_B_G2.Helpers;
using NT1_2022_1C_B_G2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NT1_2022_1C_B_G2.ViewModels
{
    public class ConfirmarFuncion
    {

        [Required(ErrorMessage = MensajesError.Requerido)]
        public Boolean Confirmada { get; set; }

    }
}
