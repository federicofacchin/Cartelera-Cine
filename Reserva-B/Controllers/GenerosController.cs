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
    public class GenerosController : Controller
    {
        private readonly ReservasContext _context;

        public GenerosController(ReservasContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Generos.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await _context.Generos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genero == null)
            {
                return NotFound();
            }

            return View(genero);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Genero genero)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genero);

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
                        ModelState.AddModelError("Nombre", "Error.nombre de genero ya existente");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbex.Message);
                    }
                }
            }
            return View(genero);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
            {
                return NotFound();
            }
            return View(genero);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Genero genero)
        {
            if (id != genero.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genero);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbex)
                {
                    SqlException innerException = dbex.InnerException as SqlException;

                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                    {
                        ModelState.AddModelError("Nombre", "Nombre de genero ya existente, intente nuevamente");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbex.Message);
                    }
                }
                
            }
            return View(genero);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await _context.Generos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genero == null)
            {
                return NotFound();
            }

            return View(genero);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genero = await _context.Generos.FindAsync(id);
            _context.Generos.Remove(genero);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GeneroExists(int id)
        {
            return _context.Generos.Any(e => e.Id == id);
        }
    }
}
