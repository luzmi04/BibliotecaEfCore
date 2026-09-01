using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
{
    public  class Autor
    {
        public int Id {  get; set; }
        public string Nombre { get; set; } = string.Empty;
        public List<Libro> Libros { get; set; } = new List<Libro>();
    }
}
