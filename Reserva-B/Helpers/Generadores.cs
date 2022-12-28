using NT1_2022_1C_B_G2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NT1_2022_1C_B_G2.Helpers
{
    public class Generadores
    {
        private static long _ultimoLegajo = 1000;
        public static long generarLegajo()
        {
            return _ultimoLegajo++;
        }

    }
}
