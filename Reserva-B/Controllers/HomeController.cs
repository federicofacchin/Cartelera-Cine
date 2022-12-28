using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NT1_2022_1C_B_G2.Data;
using NT1_2022_1C_B_G2.Models;
using NT1_2022_1C_B_G2.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NT1_2022_1C_B_G2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ReservasContext _context;

        public HomeController(ReservasContext context)
        {
            _context = context;
        }

        private void DesactivarReserva(int? id)
        {
            Reserva reserva = _context.Reservas.Find(id);

            if (reserva != null)
            {

                Funcion funcion = _context.Funciones.Find(reserva.FuncionId);

                if (funcion.FechaYHora < DateTime.Now)
                {
                    reserva.ReservaActiva = false;
                    _context.Update(reserva);
                    _context.SaveChanges();
                }
            }
        }

        public IActionResult Index(int ? generoId)
        {
            List<Reserva> reservasList = _context.Reservas.ToList();

            foreach (Reserva Reserva in reservasList)
            {
                DesactivarReserva(Reserva.Id);
            }

            List<Funcion> funcionesList = _context.Funciones.ToList();
            List<Pelicula> peliculasCartelera = new List<Pelicula>();

            foreach (var funcion in funcionesList)
            {
                int id = funcion.Id;
                int cantReservadas = FuncionesController.calcularButacasUsadas(funcion, _context);
                int butacasSala = _context.Salas.Find(funcion.SalaId).CapacidadButacas;


                if (funcion.Confirmada &&
                    cantReservadas < butacasSala &&
                    DateTime.Now <= funcion.FechaYHora)
                {
                    peliculasCartelera.Add(_context.Peliculas.Find(funcion.PeliculaId));
                    
                }
            }
            if (generoId != null && generoId != 0)
            {
                return View(peliculasCartelera.Where(p=>p.GeneroId == generoId).Distinct());
            }
            return View(peliculasCartelera.Distinct());
        }

        public IActionResult PorGenero()
        {
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        public IActionResult PorGenero(GeneroId viewModel)
        {
            var generoId = viewModel.Id;

            return RedirectToAction("Index","Home", new {generoId});
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
