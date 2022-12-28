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
    public class SalasController : Controller
    {
        private readonly ReservasContext _context;
        private String mensajeError = "Error, el numero de la sala ya existe";
        public SalasController(ReservasContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reservasContext = _context.Salas.Include(s => s.TipoSala);
            return View(await reservasContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["TipoSalaId"] = new SelectList(_context.TipoSalas, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero,CapacidadButacas,TipoSalaId")] Sala sala)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sala);
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
                        ModelState.AddModelError("Numero", mensajeError);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbex.Message);
                    }
                }
            }
            ViewData["TipoSalaId"] = new SelectList(_context.Set<TipoSala>(), "Id", "Nombre", sala.TipoSalaId);
            return View(sala);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas.FindAsync(id);            

            if (sala == null)
            {
                return NotFound();
            }
            var reservas = _context.Funciones.Where(f => f.SalaId == id).Select(f => f.Reservas);

            if (!reservas.Any())
            {
                ViewData["TipoSalaId"] = new SelectList(_context.Set<TipoSala>(), "Id", "Nombre", sala.TipoSalaId);
                return View(sala);
            }
            ModelState.AddModelError(string.Empty, "Error. La sala tiene funciones con reservas asigandas");
            return View(sala);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditarSala sala)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var salaDb = await _context.Salas.FindAsync(id);
            if (ModelState.IsValid)
            {
                
                salaDb.CapacidadButacas = sala.CapacidadButacas;
                salaDb.TipoSalaId = sala.TipoSalaId;
                salaDb.Numero = sala.Numero;
                try
                {
                    _context.Update(salaDb);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateException dbex)
                {
                    SqlException innerException = dbex.InnerException as SqlException;

                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                    {
                        ModelState.AddModelError("Numero", "Error, la sala con ese numero ya existe");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbex.Message);
                    }
                }

            }
            ViewData["TipoSalaId"] = new SelectList(_context.Set<TipoSala>(), "Id", "Nombre", salaDb.TipoSalaId);
            return View(salaDb);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas
                .Include(s => s.TipoSala)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sala == null)
            {
                return NotFound();
            }

            return View(sala);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sala = await _context.Salas.FindAsync(id);
            _context.Salas.Remove(sala);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaExists(int id)
        {
            return _context.Salas.Any(e => e.Id == id);
        }
    }
}
