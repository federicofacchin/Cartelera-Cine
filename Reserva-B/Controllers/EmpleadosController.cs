using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NT1_2022_1C_B_G2.Data;
using NT1_2022_1C_B_G2.Helpers;
using NT1_2022_1C_B_G2.Models;
using NT1_2022_1C_B_G2.ViewModels;

namespace NT1_2022_1C_B_G2.Controllers
{

    [Authorize(Roles = "Empleado,Administrador")]
    public class EmpleadosController : Controller
    {
        private readonly ReservasContext _context;
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly RoleManager<Rol> _roleManager;

        public EmpleadosController(ReservasContext context, UserManager<Persona> userManager,
            SignInManager<Persona> signInManager,
            RoleManager<Rol> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Empleados.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Legajo,Id,Dni,Nombre,Apellido,Email,Direccion,FechaAlta,Telefono")] Empleado empleado)
        {
            
            empleado.UserName = empleado.Email;

            if (ModelState.IsValid)
            {
                var resultadoCreacion = await _userManager.CreateAsync(empleado, Configs.DefaultPass);

                if (resultadoCreacion.Succeeded)
                {
                    await _userManager.AddToRoleAsync(empleado, Configs.EmpleadoRolName);
                    return RedirectToAction(nameof(Index));
                }

                var empleadoExistente = _context.Empleados.FirstOrDefault(e => e.Email == empleado.Email);
                if(empleadoExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "El email ya existe, pruebe con otro");
                    return View(empleado);
                }
                   
                    
                   
                
            }
            return View(empleado);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DatosContacto empleado)
        {
            if (id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var emplEnDb = _context.Empleados.Find(id);
                    emplEnDb.Direccion = empleado.Direccion;
                    emplEnDb.Telefono = empleado.Telefono;
                    _context.Update(emplEnDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }
    }
}
