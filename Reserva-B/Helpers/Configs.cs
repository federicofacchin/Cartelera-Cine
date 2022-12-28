using System;
using System.Globalization;

namespace NT1_2022_1C_B_G2.Helpers
{
    public static class Configs
    {
        public const int CantClientes = 10;
        public const int CantEmpleados = 5;
        public const int CantSalas = 3;
        public const string EmpleadoBase = "Empleado";
        public const string ClienteBase = "Cliente";
        public const string AdminBase = "Admin";
        public const string SalaBase = "Sala";
        public const string Dominio = "@ort.edu.ar";
        public const string DefaultPass = "Password1!";
        public const int DNI = 10000000;
        public const string Direccion  = "Calle Faslsa";
        public const string AdminRolName = "Administrador";
        public const string EmpleadoRolName = "Empleado";
        public const string ClienteRolName = "Cliente";
        public const string TipoSala1 = "Regular";
        public const string TipoSala2 = "Pulmen";
        public const string TipoSala3 = "3D";
        public const string Genero1 = "Accion";
        public const string Genero2 = "Ficcion";
        public const string Genero3 = "Terror";
        public const string Genero4 = "Aventura";
        public const string Genero5 = "Suspenso";
        public const string Genero6 = "Thriller";
        public const string Genero7 = "Fantasia";
        public const string Pelicula1 = "Pulp fiction";
        public const string Pelicula2 = "Blade Runner";
        public const string Pelicula3 = "Alien";
        public const string Foto1 = "pulp-fiction.jpg";
        public const string Foto2 = "blade-runner.jpg";
        public const string Foto3 = "alien.jpg";
        public static DateTime Lanzamiento1 = new DateTime(1994,5,21);
        public static DateTime Lanzamiento2 = new DateTime(1982, 6, 25); 
        public static DateTime Lanzamiento3 = new DateTime(1979, 5, 25);
        public static DateTime FechaYHora1 = DateTime.Now.AddDays(4);
        public static DateTime FechaYHora2 = DateTime.Now.AddDays(-3);
        public static DateTime FechaYHora3 = DateTime.Now.AddDays(5);
        public static DateTime FechaYHora4 = DateTime.Now.AddDays(6);
        public static DateTime FechaYHora5 = DateTime.Now.AddDays(9);
        public static DateTime FechaYHora6 = DateTime.Now.AddDays(-10);
        public const int ButacasPorSala = 100;
        public const string FotoDefault = "default.png";
    }
}
