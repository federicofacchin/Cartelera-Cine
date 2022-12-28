using System;
using System.ComponentModel.DataAnnotations;

namespace NT1_2022_1C_B_G2.Helpers
{
    public class RangoFechaAttribute : ValidationAttribute
    {
        private DateTime _fechaPiso;
        private DateTime _fechaTecho;
        public RangoFechaAttribute()
        {
            _fechaPiso = DateTime.Now.AddHours(1);
            _fechaTecho = DateTime.Now.AddDays(90);
            ErrorMessage = "la fecha debe estar entre " + _fechaPiso + " y el " + _fechaTecho;
        }

        public override bool IsValid(object value)
        {

            if (value is DateTime Fecha)
            {
                bool v = _fechaPiso < Fecha & Fecha < _fechaTecho;
                var FechaOk = v;
                   
                return v;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessage);

        }
    }
}