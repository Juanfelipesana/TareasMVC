using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TareasMVC.Models;
using TareasMVC.Servicios;

namespace TareasMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext context;

        public UsuariosController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = new IdentityUser() { Email = modelo.Email, UserName = modelo.Email };

            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);

            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(modelo);
            }
        }

        [AllowAnonymous]
        public IActionResult Login(string mensaje = null)
        {
            if (mensaje is not null)
            {
                ViewData["mensaje"] = mensaje; //Pasando el mensaje a la vista.
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await signInManager.PasswordSignInAsync(modelo.Email,
                modelo.Password, modelo.Recuerdame, lockoutOnFailure: false);


            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrecto.");
                return View(modelo);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        //[AllowAnonymous]
        //[HttpGet]
        ////Redirigir el usuario a la fuente de autenticación, pueden ser varios proveedores.
        //public ChallengeResult LoginExterno(string proveedor, string urlRetorno = null)
        //{
        //    var urlRedireccion = Url.Action("RegistrarUsuarioExterno", values: new { urlRetorno });//La accion RUExterno recibe la información del usuario
        //    var propiedades = signInManager.ConfigureExternalAuthenticationProperties(proveedor, urlRedireccion);//Configura las propiedades de autenticación externa
        //    return new ChallengeResult(proveedor, propiedades);
        //}

        //[AllowAnonymous]
        //public async Task<IActionResult> RegistrarUsuarioExterno(string urlRetorno = null,
        //    string remoteError = null) //Error que puede retornar el proveedor de identidad
        //{
        //    urlRetorno = urlRetorno ?? Url.Content("~/"); //Si es nulo mandamos al route de la aplicación

        //    var mensaje = "";

        //    if (remoteError is not null)
        //    {
        //        mensaje = $"Error del proveedor externo: {remoteError}"; //Se guarda el error del proveedor en un string para mostrarlo al usuario.
        //        return RedirectToAction("login", routeValues: new { mensaje });
        //    }
            
        //    //Para casos donde la data del login externo es nulo.
        //    var info = await signInManager.GetExternalLoginInfoAsync(); 
        //    if (info is null)
        //    {
        //        mensaje = "Error cargando la data de login externo";
        //        return RedirectToAction("login", routeValues: new { mensaje });
        //    }

        //    var resultadoLoginExterno = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
        //        info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

        //    // Si ya la cuenta existe
        //    if (resultadoLoginExterno.Succeeded)
        //    {
        //        return LocalRedirect(urlRetorno);
        //    }

        //    //Si el usuario no tiene cuenta y hay que crearla
        //    string email = "";//Extraeremos el email del usuario

        //    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email)) //Extraemos el email del usuario
        //    {
        //        email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //    }
        //    else 
        //    {
        //        mensaje = "Error leyendo el email del usuario del proveedor";
        //        return RedirectToAction("login", routeValues: new { mensaje });
        //    }

        //    var usuario = new IdentityUser { Email = email, UserName = email }; //con el Email ya se puede instanciar el IdentityUser.

        //    var resultadoCrearUsuario = await userManager.CreateAsync(usuario);//Creación del usuario.

        //    //Si no se pudo crear el usuario.
        //    if(!resultadoCrearUsuario.Succeeded) 
        //    {
        //        mensaje = resultadoCrearUsuario.Errors.First().Description; //Se toma el primer error para mostrar.
        //        return RedirectToAction("login", routeValues: new { mensaje });
        //    }

        //    //Sí se pudo crear el usuario.
        //    var resultadoAgregarLogin = await userManager.AddLoginAsync(usuario, info);

        //    ///Login del usuario.
        //    if (resultadoAgregarLogin.Succeeded) 
        //    {
        //        await signInManager.SignInAsync(usuario, isPersistent: true, info.LoginProvider);
        //        return LocalRedirect(urlRetorno);
        //    }
        //    //Si el paso anterior no fue exitoso.
        //    mensaje = "Ha ocurrido un error agregando el login";
        //    return RedirectToAction("login", routeValues: new { mensaje });
        //}

        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Listado(string mensaje = null)
        {
            var usuarios = await context.Users.Select(u => new UsuarioViewModel
            {
                Email =u.Email
            }).ToListAsync();//Select a la tabla de usuarios, tomamos solo el Email del usuario, para convertirlo en el Email del UserVM

            var modelo = new UsuariosListadoViewModel();
            modelo.Usuarios = usuarios;
            modelo.Mensaje = mensaje;
            return View(modelo);
        }
        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> HacerAdmin (string email)
        {
            var usuario = await context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();

            if(usuario is null)
            {
                return NotFound();
            }

            await userManager.AddToRoleAsync(usuario, Constantes.RolAdmin);

            return RedirectToAction("Listado", 
                routeValues: new {mensaje ="Rol asignado correctamente a " + email});
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> RemoverAdmin(string email)
        {
            var usuario = await context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();

            if (usuario is null)
            {
                return NotFound();
            }

            await userManager.RemoveFromRoleAsync(usuario, Constantes.RolAdmin);

            return RedirectToAction("Listado",
                routeValues: new { mensaje = "Rol removido correctamente a " + email });
        }
    }
}
