using Microsoft.EntityFrameworkCore;

namespace TareasMVC.Entidades
{
    public class ArchivoAdjunto
    {
        public Guid id { get; set; }
        public int TareaId { get; set; }
        public Tarea Tarea { get; set; }
        [Unicode]//Para tener varchar y no Nvarchar. Si se necesitara guardar caracteres especiales o emojis se usa nvarchar
        public string Url { get; set; }
        public string Titulo { get; set; }
        public int Orden {get; set; }
        public DateTime FechaCreacion { get; set; }

    }
}
