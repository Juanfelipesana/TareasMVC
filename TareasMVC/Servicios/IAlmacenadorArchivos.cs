using TareasMVC.Models;

namespace TareasMVC.Servicios
{
    public interface IAlmacenadorArchivos
    {
        Task Borrar(string ruta, string contenedor);
        Task<AlmacenarArchivoResultado[]> Almacenar(string contenedor, 
            IEnumerable<IFormFile> archivos);//Iformfile es el tipo de dato usado en ASPnetcore para representar cualquier tipo de archivo.
    }
}
