using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly ReservasContext _context;
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly RoleManager<Rol> _roleManager;

        public ClientesController(ReservasContext context, UserManager<Persona> userManager,
            SignInManager<Persona> signInManager,
            RoleManager<Rol> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Dni,Nombre,Apellido,Email,Direccion,FechaAlta,Telefono,Password")] Cliente cliente)
        {
            cliente.UserName = cliente.Email;


            if (ModelState.IsValid)
            {
                var clienteExiste = _context.Personas.Where(p => p.Dni == cliente.Dni).First();


                if (clienteExiste != null)
                {
                    ModelState.AddModelError(string.Empty, "Cliente existente, pruebe otro dni");
                    return View(cliente);
                }
                
                var resultadoCreacion = await _userManager.CreateAsync(cliente, cliente.Password);

                if (resultadoCreacion.Succeeded)
                {
                    await _userManager.AddToRoleAsync(cliente, Configs.ClienteRolName);
                }

                return RedirectToAction(nameof(Index));
                
            }
            return View(cliente);
        }


        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Cliente"))
            {
                int idUsuario = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (id.Value != idUsuario) return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }


        [Authorize(Roles = "Cliente,Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Dni,Nombre,Apellido,Email,Direccion,FechaAlta,Telefono")] Cliente cliente) 
            
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cltEnDb = _context.Clientes.Find(cliente.Id);
                    cltEnDb.Direccion = cliente.Direccion;
                    cltEnDb.Telefono = cliente.Telefono;
                    _context.Update(cltEnDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            return View("Home");
        }


        [Authorize(Roles = "Cliente")]
        public IActionResult MiPerfil()
        {
            var id = _userManager.GetUserId(HttpContext.User);
            return RedirectToAction("Details", "Clientes", new { id });
        }


        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> EditMyProfile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Cliente"))
            {
                int idUsuario = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (id.Value != idUsuario) return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [Authorize(Roles = "Cliente,Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMyProfile(int id, DatosContacto ViewModel)
        {
            if (id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cltEnDb = _context.Clientes.Find(id);
                    cltEnDb.Direccion = ViewModel.Direccion;
                    cltEnDb.Telefono = ViewModel.Telefono;
                    await _userManager.UpdateAsync(cltEnDb);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MiPerfil", "Clientes");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Cliente,Empleado")]
        public async Task<IActionResult> CompletarRegistro(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Cliente"))
            {
                int idUsuario = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (id.Value != idUsuario) return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [Authorize(Roles = "Cliente,Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompletarRegistro(int id, Cliente ViewModel)
        {
            if (id != ViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cltEnDb = _context.Clientes.Find(ViewModel.Id);
                    cltEnDb.Direccion = ViewModel.Direccion;
                    cltEnDb.Telefono = ViewModel.Telefono;
                    cltEnDb.Nombre = ViewModel.Nombre;
                    cltEnDb.Apellido = ViewModel.Apellido;
                    cltEnDb.Dni = ViewModel.Dni;
                    await _userManager.UpdateAsync(cltEnDb);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(ViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                    
                }
                
            }
            return RedirectToAction("Index","Home");
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
