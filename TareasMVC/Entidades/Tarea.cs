using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TareasMVC.Entidades
{
    public class Tarea
    {
        //Definimos todas las propiedades que EF Core me va a ayudar a convertir en columnas de la tabla.
        //Aquí solo es una clase, la configuración como entidad la hacemos en la pieza central. (APPDBContext)
        public int Id { get; set; }//Llave primaria, autoincrementable.
        //Data Annotations
        [StringLength(250)]
        [Required]
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public DateTime FechaCreacion { get; set;}
        public string UsuarioCreacionId { get; set; } //ID del usuario que ha creado una tarea
        public  IdentityUser UsuarioCreacion { get; set; }
        public  List<Paso> Pasos { get; set; }//Relación uno a muchos.
        public List<ArchivoAdjunto> ArchivosAdjuntos { get; set; }
    }
}
