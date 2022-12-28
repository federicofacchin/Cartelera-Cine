using System;
using System.Collections.Generic;
using System.Data;
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
    
    public class ReservasController : Controller
    {
        private readonly ReservasContext _context;
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly RoleManager<Rol> _roleManager;

        public ReservasController(ReservasContext context, UserManager<Persona> userManager,
            SignInManager<Persona> signInManager,
            RoleManager<Rol> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            if (User.IsInRole("Cliente")) {
                int ClienteId = Int32.Parse(_userManager.GetUserId(HttpContext.User));
                var reservasCliente = _context.Reservas.Include(r => r.Cliente)
                    .Include(r => r.Funcion).Where(r => r.ClienteId == ClienteId);
                
                return View(await reservasCliente.ToListAsync());
            }
            var reservasDeClientes = _context.Reservas.Include(r => r.Cliente)
                    .Include(r => r.Funcion);

            ViewData["FuncionId"] = new SelectList(_context.Funciones, "Id", "DescripcionDetallada");
            return View(await reservasDeClientes.OrderByDescending(r => r.FuncionId).ThenBy(r => r.FechaAlta).ToListAsync());
        }

        public async Task<IActionResult> ReservasPorFuncion(int id)
        {
            var reservas = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcion).Where(r => r.FuncionId == id);

            return View(await reservas.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> ReservaActiva()
        {
            int ClienteId = Int32.Parse(_userManager.GetUserId(HttpContext.User));
            var reservasCliente = _context.Reservas.Include(r => r.Cliente)
                .Include(r => r.Funcion).Where(r => r.ClienteId == ClienteId && r.ReservaActiva);

            return View("Index" , await reservasCliente.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> ReservasPasadas()
        {
            int ClienteId = Int32.Parse(_userManager.GetUserId(HttpContext.User));

            var reservasCliente = _context.Reservas.Include(r => r.Cliente)
                .Include(r => r.Funcion)
                .Where(r => r.ClienteId == ClienteId && !r.ReservaActiva);

            return View("Index", await reservasCliente.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Funcion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        [HttpGet]
        public IActionResult ElegirButacas(int id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("IniciarSesion", "Account");
            }
            var peliculas = new SelectList(_context.Peliculas.Where(p => p.Id == id), "Id", "Titulo");
            
            ViewData["Pelicula"] = peliculas;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ElegirButacas(Reservar1 viewmodel)
        {
            if (ModelState.IsValid)
            {
                var peliculas = new SelectList(_context.Peliculas.Where(p => p.Id == viewmodel.PeliculaId), "Id", "Titulo");

                ViewData["Pelicula"] = peliculas;
                return RedirectToAction("ElegirFuncionDisponible", viewmodel);
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Cliente")]
        public IActionResult ElegirFuncionDisponible(Reservar1 viewmodel)
        {
            var funciones = _context.Funciones.Where(f => f.PeliculaId == viewmodel.PeliculaId);
            var nopasadas = funciones.Where(f => f.FechaYHora > DateTime.Now);
            var proximas = nopasadas.Where(f => f.FechaYHora <= DateTime.Now.AddDays(7)).ToList();
            var funcionesDisponibles = new List<Funcion>();

            foreach (var funcion in proximas)
            {
                bool butacas = hayButacas(funcion.Id, viewmodel.CantidadButacas);
                if (butacas)
                {
                    funcionesDisponibles.Add(funcion);
                }
            }
            
            ViewData["Funciones"] = new SelectList(funcionesDisponibles, "Id", "DescripcionDetallada");
            ViewData["Pelicula"] = new SelectList(_context.Peliculas.Where(p => p.Id == viewmodel.PeliculaId), "Id", "Titulo");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ElegirFuncionDisponible([Bind("Id,CantidadButacas,ClienteId,FuncionId,ReservaActiva,FechaAlta")] Reserva reserva)
        {
            int ClienteId = Int32.Parse(_userManager.GetUserId(HttpContext.User));
            var reservasCliente = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcion).Where(r => r.ClienteId == ClienteId);
            var funcion = await _context.Funciones.FindAsync(reserva.FuncionId);
            reserva.ClienteId = ClienteId;

            if (ModelState.IsValid)
            {
                

                foreach (Reserva reservaactual in reservasCliente)
                {
                    if (reservaactual.Funcion.FechaYHora < DateTime.Now)
                    {
                        reservaactual.ReservaActiva = false;
                    }

                }
                if (reservasCliente.Any(r => r.ReservaActiva == true))
                {
                    ModelState.AddModelError(string.Empty, "Ya tiene una reserva activa, debe cancelarla para generar una nueva");
                    return View();
                }

                reserva.ReservaActiva = true;
                reserva.FechaAlta = DateTime.Now;

                _context.Add(reserva);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }



            ModelState.AddModelError("FuncionId", "debe seleccionar una funcion");
            return View();
        }

        [Authorize(Roles = "Cliente")]
        public IActionResult Create(int? id)
        {
            var funciones = _context.Funciones.Where(f => f.PeliculaId == id);
            var nopasadas = funciones.Where(f => f.FechaYHora > DateTime.Now);
            var proximas = nopasadas.Where(f => f.FechaYHora <= DateTime.Now.AddDays(7));

            ViewData["FuncionId"] = new SelectList(proximas, "Id", "DescripcionDetallada");
            
            return View();
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservar ViewModel)
        {
            int ClienteId = Int32.Parse(_userManager.GetUserId(HttpContext.User));
            var reservasCliente = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcion).Where(r => r.ClienteId == ClienteId);
            var funcion = await _context.Funciones.FindAsync(ViewModel.FuncionId);

            foreach (Reserva reservaactual in reservasCliente)
            {
                if (reservaactual.Funcion.FechaYHora < DateTime.Now)
                {
                    reservaactual.ReservaActiva = false;
                }

            }

            if (reservasCliente.Any(r => r.ReservaActiva == true))
            {
                ModelState.AddModelError(string.Empty, "Ya tiene una reserva activa, debe cancelarla para generar una nueva");
                return View();
            }

            if (ModelState.IsValid)
            {

                if (hayButacas(ViewModel.CantidadButacas, ViewModel.FuncionId)) {
                    var reserva = new Reserva();
                    reserva.ClienteId = ClienteId;
                    reserva.FuncionId = ViewModel.FuncionId;
                    reserva.CantidadButacas = ViewModel.CantidadButacas;
                    reserva.FechaAlta = DateTime.Now;
                    reserva.ReservaActiva = true;

                    _context.Add(reserva);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }

                string mensajeError = "la cantidad de butacas debe ser menor a : " + FuncionesController.calcularButacasUsadas(funcion, _context);
                ModelState.AddModelError(string.Empty, mensajeError);
            }
            ViewData["FuncionId"] = new SelectList(_context.Funciones.Where(f => f.PeliculaId == funcion.Id), "Id", "DescripcionDetallada");
            return View();
        }

        private bool hayButacas( int funcionId,int cantidadButacas)
        {
            var butacasDisponibles = calcularButacasLibres(funcionId);

            return cantidadButacas <= butacasDisponibles;
        }

        public  int calcularButacasLibres(int funcionId)
        {
            int acumButacas = 0;
            var reservas = _context.Reservas.Where(r => r.FuncionId == funcionId);
            var funcion =  _context.Funciones.Find(funcionId);
            var salaid = funcion.SalaId;
            var sala =  _context.Salas.Find(salaid);

            var cantButacas = sala.CapacidadButacas;

            foreach (Reserva r in reservas)
            {
                acumButacas += r.CantidadButacas;
            }
            return cantButacas - acumButacas;
        }

        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Funcion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var reserva = await _context.Reservas.FindAsync(id);
            var funcion = await _context.Funciones.FindAsync(reserva.FuncionId);

            if (funcion.FechaYHora >= DateTime.Now.AddDays(1))
            {
                _context.Update(funcion);
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "No se puede Cancelar con una anticipacion menor a 24hs");
            return RedirectToAction(nameof(Delete),new { id = id});
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }

    }
}
