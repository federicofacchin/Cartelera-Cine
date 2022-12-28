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

namespace NT1_2022_1C_B_G2.Controllers
{
    [Authorize(Roles = "Empleado,Administrador")]
    public class TipoSalasController : Controller
    {
        private readonly ReservasContext _context;

        public TipoSalasController(ReservasContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoSalas.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSala = await _context.TipoSalas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoSala == null)
            {
                return NotFound();
            }

            return View(tipoSala);
        }

        public IActionResult Create()
        {
            return View(new TipoSala());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio")] TipoSala tipoSala)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(tipoSala);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateException dbex)
                {

                    SqlException innerException = dbex.InnerException as SqlException;

                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                    {
                        ModelState.AddModelError("Nombre", "Error, Ese tipo de sala ya existe");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbex.Message);
                    }
                    
                }
            }
            return View(tipoSala);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSala = await _context.TipoSalas.FindAsync(id);

            if (tipoSala == null)
            {
                return NotFound();
            }
            return View(tipoSala);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio")] TipoSala tipoSala)
        {
            if (id != tipoSala.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                try
                {
                    _context.Update(tipoSala);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbex)
                {
                    SqlException innerException = dbex.InnerException as SqlException;
                    if (!TipoSalaExists(tipoSala.Id))
                    {
                        return NotFound();
                    }

                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                    {
                        ModelState.AddModelError("Nombre", "Error, Ese tipo de sala ya existe");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbex.Message);
                    }

                }
                
            }
            return View(tipoSala);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSala = await _context.TipoSalas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoSala == null)
            {
                return NotFound();
            }

            return View(tipoSala);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoSala = await _context.TipoSalas.FindAsync(id);
            _context.TipoSalas.Remove(tipoSala);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoSalaExists(int id)
        {
            return _context.TipoSalas.Any(e => e.Id == id);
        }
    }
}
