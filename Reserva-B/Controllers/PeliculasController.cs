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
using NT1_2022_1C_B_G2.Helpers;
namespace NT1_2022_1C_B_G2.Controllers
{
    [Authorize(Roles = "Empleado,Administrador")]
    public class PeliculasController : Controller
    {
        private readonly ReservasContext _context;

        public PeliculasController(ReservasContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reservasContext = _context.Peliculas.Include(p => p.Genero);
            return View(await reservasContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .Include(p => p.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

       
        public async Task<IActionResult> VerReservas(int? id)
        {
            return RedirectToAction("ReservasPorPelicula", "Reservas", new { id });
        }

        public async Task<IActionResult> FuncionesActivas(int? id)
        {
            var funciones =_context.Funciones.Include(f => f.Sala).ThenInclude(s => s.TipoSala).Where(f => f.PeliculaId == id);
            var funcionesConfirmadas = funciones.Where(f => f.Confirmada == true);
            return View(await funcionesConfirmadas.Where(f => f.FechaYHora <= DateTime.Now.AddDays(7)).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaLanzamiento,Titulo,Descripcion,GeneroId")] Pelicula pelicula)
        {
            if (ModelState.IsValid)
            {
                

                Genero genero = _context.Generos.Find(pelicula.GeneroId);
                pelicula.Genero = genero;
                pelicula.Foto = Configs.FotoDefault;
                _context.Add(pelicula);
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
                        ModelState.AddModelError("Titulo", "Error, el titulo de la pelicula ya existe");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbex.Message);
                    }
                }
            }
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
            return View(pelicula);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);

            if (pelicula == null)
            {
                return NotFound();
            }
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
            return View(pelicula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,EditPelicula Viewmodel)
        {
            

            if (ModelState.IsValid)
            {
                try
                {
                    var peliculaDb = _context.Peliculas.Find(Viewmodel.Id);
                    peliculaDb.Titulo = Viewmodel.Titulo;
                    peliculaDb.FechaLanzamiento = Viewmodel.FechaLanzamiento;
                    peliculaDb.Descripcion = Viewmodel.Descripcion;
                    peliculaDb.GeneroId = Viewmodel.GeneroId;
                    _context.Update(peliculaDb);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbex)
                {
                    SqlException innerException = dbex.InnerException as SqlException;

                    if (!PeliculaExists(Viewmodel.Id))
                    {
                        return NotFound();
                    }

                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                    {
                        ModelState.AddModelError("Titulo", "Error, el titulo de la pelicula ya existe");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbex.Message);
                    }
                }
                
            }
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", Viewmodel.GeneroId);
            return View(Viewmodel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .Include(p => p.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaExists(int id)
        {
            return _context.Peliculas.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Empleado")]
        public IActionResult Balance(int? id)
        {
            if(id != null)
            {
                var funciones = _context.Funciones.Where(f => f.PeliculaId == id);
                var nopasadas = funciones.Where(f => f.FechaYHora.Month >= DateTime.Now.Month);
                var funcionesDelMes = nopasadas.Where(f => f.FechaYHora <= DateTime.Today).ToList();
                double AcumMes = 0;

                if(funcionesDelMes.Any())
                {
                    foreach (Funcion funcion in funcionesDelMes)
                    {
                        var reservas = _context.Reservas.Where(r => r.FuncionId == funcion.Id);
                        var sala = _context.Salas.Find(funcion.SalaId);
                        var tipoSala = _context.TipoSalas.Find(sala.TipoSalaId);
                        var precioSala = tipoSala.Precio;
                        foreach (Reserva reserva in reservas)
                        {
                            AcumMes += reserva.CantidadButacas * precioSala;
                        }
                    }
                }
                
                ViewData["Pelicula"] = _context.Peliculas.Find(id).Titulo;
                return View(AcumMes);
            }

            return View("Index");
        }
    }
}
