using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NT1_2022_1C_B_G2.Data;
using NT1_2022_1C_B_G2.Models;
using NT1_2022_1C_B_G2.ViewModels;

namespace NT1_2022_1C_B_G2.Controllers
{
    [Authorize(Roles = "Empleado,Administrador")]
    public class FuncionesController : Controller
    {
        private readonly ReservasContext _context;

        public FuncionesController(ReservasContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var FuncionesContext = _context.Funciones.Include(f => f.Pelicula).Include(f => f.Sala).Include(f => f.Reservas);
            return View(await FuncionesContext.OrderByDescending(f => f.FechaYHora).ThenBy(f => f.PeliculaId).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcion == null)
            {
                return NotFound();
            }

            ViewData["ButacasDisponibles"] = funcion.Sala.CapacidadButacas - FuncionesController.calcularButacasUsadas(funcion, _context); 

            return View(funcion);
        }

        public async Task<IActionResult> VerReservas(int? id)
        {
            var reservas = await _context.Funciones.Include(f => f.Reservas).Where(f => f.Id == id).ToListAsync();
            return RedirectToAction("ReservasPorFuncion","Reservas" , new { id});
        }

        public IActionResult Create()
        {
            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo");
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Numero");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaYHora,SalaId,PeliculaId,Confirmada")] Funcion funcion)
        {
            if (ModelState.IsValid)
            {
                var funciones = _context.Funciones.Where(f => f.SalaId == funcion.SalaId);
                var funcionHastaHmax= funciones.Where(f => f.FechaYHora <= funcion.FechaYHora.AddHours(2));
                var funcionesRango = funcionHastaHmax.Where(f => f.FechaYHora >= funcion.FechaYHora.AddHours(-2)).Count();
               
                    var pelicula = await _context.Peliculas.FindAsync(funcion.PeliculaId);  
                    var sala = await _context.Salas.FindAsync(funcion.SalaId);
                    var tipoSala = await _context.TipoSalas.FindAsync(sala.TipoSalaId);
                    funcion.DescripcionDetallada = $" {pelicula.Titulo} - Sala {tipoSala.Nombre} - {funcion.FechaYHora}";

                    _context.Add(funcion);
                    try
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException dbex)
                    {
                        SqlException innerException = dbex.InnerException as SqlException;

                        if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                        {
                            ModelState.AddModelError(string.Empty, "Funcion duplicada.Intente nuevamente");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, dbex.Message);
                        }
                    }

                    //REVISAR como hacer para que ademas de una funcion duplicada no este dentro del rango horario +2 -2 hs

                    //if (funcionesRango != 0)
                    //{
                    //    ModelState.AddModelError(string.Empty, "Ya existe una funcion dentro del rango horario, intente nuevamente");
                    //}

                
            }
            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Descripcion", funcion.PeliculaId);
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Id", funcion.SalaId);
            return View(funcion);
        }

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var funcion = await _context.Funciones.FindAsync(id);
        //    if (funcion == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(funcion);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, ConfirmarFuncion viewModel)
        //{
        //    var funcionDb = _context.Funciones.FirstOrDefault(x => x.Id == id);

        //    if (funcionDb == null)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var reservas = _context.Reservas.Where( r => r.FuncionId == funcionDb.Id).Count();
        //        if (reservas == 0)
        //        {
        //            funcionDb.Confirmada = viewModel.Confirmada;
        //            _context.Update(funcionDb);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }

        //        ModelState.AddModelError(string.Empty, "Esta funcion no se puede desconfirmar, ya tiene reservas hechas");
        //    }
        //    return View(funcionDb);
        //}

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (funcion == null)
            {
                return NotFound();
            }

            var reservas = _context.Reservas.Where(f => f.FuncionId == id).Count();

            if (reservas == 0)
            {
                _context.Funciones.Remove(funcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["mensajeError"] = "La funcion no se puede cancelar, tiene reservas activas";
            return View(funcion); //revisar
        }

        public async Task<IActionResult> Habilitar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (funcion == null)
            {
                return NotFound();
            }

            var reservasExistentes = _context.Reservas.Where(f => f.FuncionId == id).ToList();

            if (!reservasExistentes.Any())
            {
                funcion.Confirmada = !funcion.Confirmada;
                _context.Funciones.Update(funcion);
                await _context.SaveChangesAsync();
            }

            //ModelState.AddModelError(string.Empty , "La funcion no se puede deshabilitar tiene reservas activas");
            return RedirectToAction(nameof(Index));
        }

        private bool FuncionExists(int id)
        {
            return _context.Funciones.Any(e => e.Id == id);
        }

        public static  int calcularButacasUsadas(Funcion funcion, ReservasContext _context)
        {

            int cantReservadas = 0;
            
            int butacasSala = _context.Salas.Find(funcion.SalaId).CapacidadButacas;
            List<Reserva> reservasDeEsaFuncion = _context.Reservas.Where(r => r.FuncionId == funcion.Id).ToList();

            foreach (var reserva in reservasDeEsaFuncion)
            {
                cantReservadas = cantReservadas + reserva.CantidadButacas;
            }

            return cantReservadas;
        }

    }
}
