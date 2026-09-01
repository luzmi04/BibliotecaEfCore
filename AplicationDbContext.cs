using AccesoDatos.Models;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.Data;

public class AplicationDbContext : DbContext
{
    public DbSet<Autor> Autores { get; set; }
    public DbSet<Libro> Libros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(
    $"Data Source={Path.Combine(AppContext.BaseDirectory, "biblioteca.db")}");

    }
}