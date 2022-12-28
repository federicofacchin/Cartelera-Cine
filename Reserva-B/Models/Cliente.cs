using System.Collections.Generic;

namespace NT1_2022_1C_B_G2.Models
{
    public class Cliente : Persona
    {
        public List<Reserva> Reservas { get; set; }
              
    }
}

