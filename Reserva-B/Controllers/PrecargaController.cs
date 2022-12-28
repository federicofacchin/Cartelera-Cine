using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NT1_2022_1C_B_G2.Helpers;
using NT1_2022_1C_B_G2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using NT1_2022_1C_B_G2.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace NT1_2022_1C_B_G2.Controllers
{
    public class PreCargaController : Controller
    {

        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<Rol> _rolManager;
        private readonly ReservasContext _reservasContext;

        public PreCargaController(UserManager<Persona> userManager, RoleManager<Rol> rolManager, ReservasContext contexto)
        {
            _userManager = userManager;
            _rolManager = rolManager;
            _reservasContext = contexto;
        }

        public IActionResult CargaBase()
        {
            try
            {
                _reservasContext.Database.EnsureDeleted();
                _reservasContext.Database.Migrate();
                CrearRolesBase().Wait();
                CreoEmpleados().Wait();
                CreoClientes().Wait();
                CreoTipoSala().Wait();
                CreoSala().Wait();
                CreoGeneros().Wait();
                CreoPeliculas().Wait();
                CreoFunciones().Wait();
                CreoReservas().Wait();
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }

            return RedirectToAction("Index", "Home");
        }



        private async Task CreoEmpleados()
        {
            int cantEmpleados = Configs.CantEmpleados;
            int indexEmpleados = _reservasContext.Empleados.Count() + 1;

            if (!_reservasContext.Empleados.Any())
            {
                for (int i = indexEmpleados; i <= cantEmpleados; i++)
                {
                    Empleado emp = new Empleado()
                    {
                        Email = Configs.EmpleadoBase + i.ToString() + Configs.Dominio,
                        UserName = Configs.EmpleadoBase + i.ToString() + Configs.Dominio,
                        Apellido = Configs.EmpleadoRolName,
                        Nombre = (Configs.EmpleadoBase + i).ToString(),
                        Dni = (Configs.DNI + 1000 * i).ToString(),
                        Direccion = (Configs.Direccion + i).ToString(),

                    };

                    var resultadoCreacion = await _userManager.CreateAsync(emp, Configs.DefaultPass);

                    if (resultadoCreacion.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(emp, Configs.EmpleadoRolName); ;
                    }
                }
            }
        }

        private async Task CreoClientes()
        {
            int cantClientes = Configs.CantClientes;
            int indexClientes = _reservasContext.Clientes.Count() + 1;

            if (!_reservasContext.Clientes.Any())
            {
                for (int i = indexClientes; i <= cantClientes; i++)
                {
                    Cliente clt = new Cliente()
                    {
                        Email = Configs.ClienteBase + i.ToString() + Configs.Dominio,
                        UserName = Configs.ClienteBase + i.ToString() + Configs.Dominio,
                        Apellido = Configs.ClienteRolName,
                        Nombre = (Configs.ClienteBase + i).ToString(),
                        Dni = (Configs.DNI + i).ToString(),
                        Direccion = (Configs.Direccion + i).ToString(),

                    };

                    var resultadoCreacion = await _userManager.CreateAsync(clt, Configs.DefaultPass);

                    if (resultadoCreacion.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(clt, Configs.ClienteRolName); ;
                    }
                }
            }
        }

        private async Task CrearRolesBase()
        {
            List<string> roles = new List<string> { Configs.AdminRolName, Configs.ClienteRolName, Configs.EmpleadoRolName };

            if (!_reservasContext.Roles.Any())
            {
                foreach (string rol in roles)
                {
                    await CrearRole(rol);
                }
            }
        }

        private async Task CrearRole(string rolName)
        {
            if (!await _rolManager.RoleExistsAsync(rolName))
            {
                await _rolManager.CreateAsync(new Rol(rolName));
            }
        }

        private async Task CreoTipoSala()
        {
         List<string> Tipos = new List<string> { Configs.TipoSala1, Configs.TipoSala2, Configs.TipoSala3 };

            if (!_reservasContext.TipoSalas.Any())
            {
              int cont = 1;
              foreach (string tipo in Tipos)
                {
                    TipoSala tipoSala = new TipoSala()
                    {
                        Nombre = tipo,
                        Precio = cont * 10,
                    };
                    cont++;
                    
                    _reservasContext.TipoSalas.Add(tipoSala);
                    await _reservasContext.SaveChangesAsync();
                }
            }

        }

        private async Task CreoSala()
        {
            int cantSalas = Configs.CantSalas;
            int indexSalas = _reservasContext.Salas.Count() + 1;

            if (!_reservasContext.Salas.Any())
            {
                for (int i = indexSalas; i <= cantSalas; i++)
                {
                    Sala sala = new Sala()
                    {
                        Numero = i,
                        CapacidadButacas = 100,
                        TipoSalaId = i,
                    };

                    _reservasContext.Salas.Add(sala);
                    await _reservasContext.SaveChangesAsync();
                }
            }
        }

        private async Task CreoPeliculas()
        {
            List<Pelicula> Peliculas = new List<Pelicula> { };

            if (!_reservasContext.Peliculas.Any())
            {
                Pelicula pelicula1 = new Pelicula()
                {
                    FechaLanzamiento = Configs.Lanzamiento1,
                    Titulo = Configs.Pelicula1,
                    Descripcion = $"Una pelicula de {Configs.Genero1}",
                    GeneroId = 1,
                    Foto = Configs.Foto1,
                };

                Pelicula pelicula2 = new Pelicula()
                {
                    FechaLanzamiento = Configs.Lanzamiento2,
                    Titulo = Configs.Pelicula2,
                    Descripcion = $"Una pelicula de {Configs.Genero2}",
                    GeneroId = 2,
                    Foto = Configs.Foto2,
                };

                Pelicula pelicula3 = new Pelicula()
                {
                    FechaLanzamiento = Configs.Lanzamiento3,
                    Titulo = Configs.Pelicula3,
                    Descripcion = $"Una pelicula de {Configs.Genero3}",
                    GeneroId = 3,
                    Foto = Configs.Foto3,
                };

                Peliculas.Add(pelicula1);
                Peliculas.Add(pelicula2);
                Peliculas.Add(pelicula3);

                foreach (Pelicula pelicula in Peliculas)
                {
                    _reservasContext.Peliculas.Add(pelicula);
                    await _reservasContext.SaveChangesAsync();
                }
            }
        }

        private async Task CreoFunciones()
        {
            List<Funcion> Funciones = new List<Funcion>();

            if (!_reservasContext.Funciones.Any())
            {
                Funcion funcion1 = new Funcion()
                {
                    FechaYHora = Configs.FechaYHora1,
                    SalaId = 1,
                    PeliculaId = 1,
                    DescripcionDetallada = $" {Configs.Pelicula1} - Sala {Configs.TipoSala1} - {Configs.FechaYHora1}",
                    Confirmada = true,
                };

                Funcion funcion2 = new Funcion()
                {
                    FechaYHora = Configs.FechaYHora3,
                    SalaId = 2,
                    PeliculaId = 2,
                    DescripcionDetallada = $" {Configs.Pelicula2} - Sala {Configs.TipoSala2} - {Configs.FechaYHora3}",
                    Confirmada = true,
                };

                Funcion funcion3 = new Funcion()
                {
                    FechaYHora = Configs.FechaYHora3,
                    SalaId = 3,
                    PeliculaId = 3,
                    DescripcionDetallada = $" {Configs.Pelicula3} - Sala {Configs.TipoSala3} - {Configs.FechaYHora3}",
                    Confirmada = true,
                };
                Funcion funcion4 = new Funcion()
                {
                    FechaYHora = Configs.FechaYHora4,
                    SalaId = 1,
                    PeliculaId = 3,
                    DescripcionDetallada = $" {Configs.Pelicula3} - Sala {Configs.TipoSala1} - {Configs.FechaYHora4}",
                    Confirmada = true,
                };
                Funcion funcion5 = new Funcion()
                {
                    FechaYHora = Configs.FechaYHora6,
                    SalaId = 2,
                    PeliculaId = 1,
                    DescripcionDetallada = $" {Configs.Pelicula1} - Sala {Configs.TipoSala2} - {Configs.FechaYHora6}",
                    Confirmada = true,
                };
                Funcion funcion6 = new Funcion()
                {
                    FechaYHora = Configs.FechaYHora5,
                    SalaId = 2,
                    PeliculaId = 2,
                    DescripcionDetallada = $" {Configs.Pelicula2} - Sala {Configs.TipoSala2} - {Configs.FechaYHora5}",
                    Confirmada = true,
                };
                Funciones.Add(funcion1);
                Funciones.Add(funcion2);
                Funciones.Add(funcion3);
                Funciones.Add(funcion4);
                Funciones.Add(funcion5);
                Funciones.Add(funcion6);

                foreach (Funcion funcion in Funciones)
                {
                    _reservasContext.Funciones.Add(funcion);
                    await _reservasContext.SaveChangesAsync();
                }
            }

        }
        private async Task CreoReservas()
        {
            List<Reserva> reservas = new List<Reserva>();

            if (!_reservasContext.Reservas.Any())
            {
                Reserva reserva1 = new Reserva() // reserva activa
                {
                    FechaAlta = DateTime.Now.AddDays(-3),
                    CantidadButacas = 15,
                    FuncionId = 1,
                    ReservaActiva = true,
                    ClienteId = 11
                };
                Reserva reserva2 = new Reserva() //reserva no activa 
                {
                    FechaAlta = DateTime.Now.AddDays(-5),
                    CantidadButacas = 7,
                    FuncionId = 2,
                    ReservaActiva = true,
                    ClienteId = 12
                };
                Reserva reserva3 = new Reserva() // reserva activa
                {
                    FechaAlta = DateTime.Now,
                    CantidadButacas = 4,
                    FuncionId = 3,
                    ReservaActiva = true,
                    ClienteId = 13
                };
                Reserva reserva4 = new Reserva() // Reserva activa
                {
                    FechaAlta = DateTime.Now.AddDays(-1),
                    CantidadButacas = 8,
                    FuncionId = 4,
                    ReservaActiva = true,
                    ClienteId = 14
                };
                Reserva reserva5 = new Reserva()// reserva inactiva
                {
                    FechaAlta = DateTime.Now.AddDays(-7),
                    CantidadButacas = 27,
                    FuncionId = 6,
                    ReservaActiva = true,
                    ClienteId = 15
                };
                reservas.Add(reserva1);
                reservas.Add(reserva2);
                reservas.Add(reserva3);
                reservas.Add(reserva4);
                reservas.Add(reserva5);

                foreach (Reserva reserva in reservas)
                {
                    _reservasContext.Reservas.Add(reserva);
                    await _reservasContext.SaveChangesAsync();
                }

            }
        }
        private async Task CreoGeneros()
        {
            List<string> Generos = new List<string> { Configs.Genero1, Configs.Genero2, Configs.Genero3,
                Configs.Genero4, Configs.Genero5, Configs.Genero6, Configs.Genero7};

            if (!_reservasContext.Generos.Any())
            {
                foreach (string GeneroActual in Generos)
                {
                    Genero genero = new Genero()
                    {
                        Nombre = GeneroActual,
                        
                    };

                    _reservasContext.Generos.Add(genero);
                    await _reservasContext.SaveChangesAsync();
                }
            }

        }

    }
}
