using Microsoft.Identity.Client;
using System.Security.Claims;

namespace TareasMVC.Servicios
{
    public interface IServicioUsuarios
    {
        string ObtenerUsuarioId();
    }

    public class ServicioUsuarios: IServicioUsuarios
    {
        private HttpContext HttpContext;

        public ServicioUsuarios(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext = httpContextAccessor.HttpContext;
            
        }

        public string ObtenerUsuarioId()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = HttpContext.User.Claims.
                    Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

                return idClaim.Value;
            }
            else
            { 
                throw new Exception("El usuario no está autenticado"); 
            }
        }
    }
}
