namespace TareasMVC.Entidades
{
    public class Paso
    {
        public Guid Id { get; set; }//Guid viene de Global Unique Identifier, strings aleatorios, útil para tablas muy grandes.
        public int TareaId { get; set; }
        public Tarea Tarea { get; set; } //Propiedad de navegación: Nos permitirá cargar la data relacionada de una manera sencilla / Relación uno a uno
        public string Descripcion { get; set; }
        public bool Realizado { get; set; }
        public int Orden { get; set; }
    }
}
