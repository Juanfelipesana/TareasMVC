using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TareasMVC.Entidades;
using TareasMVC.Servicios;


namespace TareasMVC.Controllers
{
    [Route("api/archivos")]
    public class ArchivosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly string contenedor = "archivosadjuntos";

        public ArchivosController(ApplicationDbContext context,
            IAlmacenadorArchivos almacenadorArchivos,
            IServicioUsuarios servicioUsuarios)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.servicioUsuarios = servicioUsuarios;
        }

        [HttpPost("{tareaId:int}")]
        public async Task<ActionResult<IEnumerable<ArchivoAdjunto>>> Post(int tareaId,
            [FromForm] IEnumerable<IFormFile> archivos)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            var tarea = await context.Tareas.FirstOrDefaultAsync(t => t.Id == tareaId);

            if(tarea == null)
            {
                return NotFound();
            }

            if(tarea.UsuarioCreacionId != usuarioId)
            {
                return Forbid();
            }

            var existenArchivosAdjuntos =
                await context.ArchivosAdjuntos.AnyAsync(a => a.TareaId == tareaId); //Verificación si ya existen archivos para esa tarea

            var ordenMayor = 0;

            if(existenArchivosAdjuntos)
            {
                ordenMayor = await context.ArchivosAdjuntos
                    .Where(a => a.TareaId == tareaId).Select(a => a.Orden).MaxAsync();
            }

            var resultados = await almacenadorArchivos.Almacenar(contenedor, archivos);

            var archivosAdjuntos = resultados.Select((resultado,indice) => new ArchivoAdjunto
            {
                TareaId = tareaId,
                FechaCreacion = DateTime.UtcNow,
                Url = resultado.URL,
                Titulo = resultado.Titulo,
                Orden = ordenMayor + indice + 1 //El índice me permite determinar en qué elemento del IEnumerable me encuentro.
            }).ToList();

            context.AddRange(archivosAdjuntos);
            await context.SaveChangesAsync();

            return archivosAdjuntos.ToList();
        }
    }
}
