using System;
using System.ComponentModel.DataAnnotations;

namespace NT1_2022_1C_B_G2.Helpers
{
    public class FechaFijaAttribute : ValidationAttribute
    {
        private DateTime _fechaPiso;
        private DateTime _fechaTecho;
        public FechaFijaAttribute()
        {
            _fechaPiso = new DateTime(1900,01,01);
            _fechaTecho = DateTime.Now.AddYears(10);
            ErrorMessage = "la fecha debe estar entre" + _fechaPiso + " y el " + _fechaTecho;
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