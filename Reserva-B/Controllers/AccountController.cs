using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NT1_2022_1C_B_G2.Data;
using NT1_2022_1C_B_G2.Helpers;
using NT1_2022_1C_B_G2.Models;
using NT1_2022_1C_B_G2.ViewModels;


namespace NT1_2022_1C_B_G2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ReservasContext _context;
        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly RoleManager<Rol> _roleManager;
        
        public AccountController (ReservasContext context, 
            UserManager<Persona> userManager, 
            SignInManager<Persona> signInManager, 
            RoleManager<Rol> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public ActionResult Registrar()
        {
            return View();

        }

        [HttpPost]
        public async Task<ActionResult> Registrar(Registracion viewModel)
        {
            if (ModelState.IsValid)
            {
                Cliente clienteNuevo = new Cliente();
                clienteNuevo.Email = viewModel.Email;
                clienteNuevo.UserName = viewModel.Email;

                var resultadoCreacion = await _userManager.CreateAsync(clienteNuevo, viewModel.Password); 

                if (resultadoCreacion.Succeeded)
                {
                    if (!_roleManager.Roles.Any())
                    {
                        await CrearRolesBase();
                    }
                }
                
                var resultado = await _userManager.AddToRoleAsync(clienteNuevo, Configs.ClienteRolName);
               
                if (resultado.Succeeded)
                {
                    await _signInManager.SignInAsync(clienteNuevo, isPersistent: false);
                    return RedirectToAction("CompletarRegistro", "Clientes", new { id=clienteNuevo.Id});
                }

                foreach (var error in resultadoCreacion.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

           

            return View(viewModel);

        }

        public ActionResult IniciarSesion(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login viewModel)
        {
            string retunrUrl = TempData["returnUrl"] as string;

            if (ModelState.IsValid)
            {

                var resultadoSingIn = await _signInManager.PasswordSignInAsync(viewModel.Email,viewModel.Password,false,false);

                if (resultadoSingIn.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(retunrUrl))
                        return Redirect(retunrUrl);

                        return RedirectToAction("Index", "Home");
                    
                }

                ModelState.AddModelError(string.Empty, "Inicio de Sesion inválido");

            }
            return View(viewModel);
        }

        public async Task<ActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

    
        
        private async Task CrearRolesBase()
        {
            List<string> roles = new List<string> { Configs.AdminRolName, Configs.ClienteRolName, Configs.EmpleadoRolName };

            if (!_context.Roles.Any())
            {
                foreach (string rol in roles)
                {
                    await CrearRole(rol);
                }     
            }
        }

        private async Task CrearRole(string rolName)
        {
            if (!await _roleManager.RoleExistsAsync(rolName))
            {
                await _roleManager.CreateAsync(new Rol(rolName));
            }
        }

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }
}