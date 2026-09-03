using AccesoDatos.Data;
using AccesoDatos.Models;
using AccesoDatos.Repositories;
using Microsoft.EntityFrameworkCore;

using var context = new AplicationDbContext();
var autorRepository = new GenericRepository<Autor>(context);
var categoriaRepository = new GenericRepository<Categoria>(context);
var libroRepository = new GenericRepository<Libro>(context);

bool salir = false;

while (!salir)
{
    Console.Clear();

    Console.WriteLine("===== BIBLIOTECA =====");
    Console.WriteLine();
    Console.WriteLine("1. Alta Autor");
    Console.WriteLine("2. Alta Categoría");
    Console.WriteLine("3. Alta Libro");
    Console.WriteLine("4. Ver Autores");
    Console.WriteLine("5. Ver Categorías");
    Console.WriteLine("6. Ver Libros");
    Console.WriteLine("7. Modificar Libro");
    Console.WriteLine("8. Eliminar Libro");
    Console.WriteLine("0. Salir");
    Console.WriteLine();
    Console.Write("Seleccione una opción: ");

    string? opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            AltaAutor();
            break;
        case "2":
            AltaCategoria();
            break;
        case "3":
            AltaLibro();
            break;
        case "4":
            VerAutores();
            break;
        case "5":
            VerCategorias();
            break;
        case "6":
            VerLibros();
            break;
        case "7":
            ModificarLibro();
            break;
        case "8":
            EliminarLibroLogico();
            break;
        case "0":
            salir = true;
            break;
        default:
            Console.WriteLine("Opción inválida.");
            Console.ReadKey();
            break;
    }
}

// ==========================================
// MÉTODOS AL FINAL DEL ARCHIVO
// ==========================================

void AltaAutor()
{
    Console.Clear();
    Console.WriteLine("===== ALTA AUTOR =====");
    Console.WriteLine();

    Console.Write("Ingrese el nombre del autor: ");
    string nombre = Console.ReadLine() ?? "";

    if (string.IsNullOrWhiteSpace(nombre))
    {
        Console.WriteLine("El nombre no puede estar vacío.");
        Console.ReadKey();
        return;
    }

    Autor nuevoAutor = new Autor { Nombre = nombre };
    autorRepository.Agregar(nuevoAutor);

    Console.WriteLine();
    Console.WriteLine("Autor registrado correctamente.");
    Console.ReadKey();
}

void AltaCategoria()
{
    Console.Clear();
    Console.WriteLine("===== ALTA CATEGORÍA =====");
    Console.WriteLine();

    Console.Write("Ingrese el nombre de la categoría: ");
    string nombre = Console.ReadLine() ?? "";

    if (string.IsNullOrWhiteSpace(nombre))
    {
        Console.WriteLine("El nombre no puede estar vacío.");
        Console.ReadKey();
        return;
    }

    Categoria nuevaCategoria = new Categoria { Nombre = nombre };
    categoriaRepository.Agregar(nuevaCategoria);

    Console.WriteLine();
    Console.WriteLine("Categoría registrada correctamente.");
    Console.ReadKey();
}

void AltaLibro()
{
    Console.Clear();
    Console.WriteLine("===== ALTA LIBRO =====");
    Console.WriteLine();

    var autores = autorRepository.ObtenerTodos();
    var categorias = categoriaRepository.ObtenerTodos();

    if (autores.Count == 0 || categorias.Count == 0)
    {
        Console.WriteLine("Debe registrar al menos un Autor y una Categoría antes de cargar un libro.");
        Console.ReadKey();
        return;
    }

    Console.Write("Ingrese el título del libro: ");
    string titulo = Console.ReadLine() ?? "";

    Console.Write("Ingrese el año de publicación: ");
    if (!int.TryParse(Console.ReadLine(), out int anio))
    {
        Console.WriteLine("El año ingresado no es válido.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("\n--- Autores disponibles ---");
    foreach (var autor in autores)
    {
        Console.WriteLine($"{autor.Id}. {autor.Nombre}");
    }
    Console.Write("Seleccione el ID del autor: ");
    if (!int.TryParse(Console.ReadLine(), out int autorId) || !autores.Any(a => a.Id == autorId))
    {
        Console.WriteLine("Autor inválido.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("\n--- Categorías disponibles ---");
    foreach (var cat in categorias)
    {
        Console.WriteLine($"{cat.Id}. {cat.Nombre}");
    }
    Console.Write("Seleccione el ID de la categoría: ");
    if (!int.TryParse(Console.ReadLine(), out int categoriaId) || !categorias.Any(c => c.Id == categoriaId))
    {
        Console.WriteLine("Categoría inválida.");
        Console.ReadKey();
        return;
    }

    Libro nuevoLibro = new Libro
    {
        Titulo = titulo,
        AnioPublicacion = anio,
        AutorId = autorId,
        CategoriaId = categoriaId,
        Activo = true
    };

    libroRepository.Agregar(nuevoLibro);

    Console.WriteLine();
    Console.WriteLine("Libro registrado correctamente.");
    Console.ReadKey();
}

void VerAutores()
{
    Console.Clear();
    Console.WriteLine("===== AUTORES REGISTRADOS =====");
    Console.WriteLine();
    var autores = autorRepository.ObtenerTodos();

    if (autores.Count == 0)
    {
        Console.WriteLine("No hay autores registrados.");
        Console.ReadKey();
        return;
    }

    foreach (var autor in autores)
    {
        Console.WriteLine($"ID: {autor.Id} | Nombre: {autor.Nombre}");
    }
    Console.ReadKey();
}

void VerCategorias()
{
    Console.Clear();
    Console.WriteLine("===== CATEGORÍAS REGISTRADAS =====");
    Console.WriteLine();
    var categorias = categoriaRepository.ObtenerTodos();

    if (categorias.Count == 0)
    {
        Console.WriteLine("No hay categorías registradas.");
        Console.ReadKey();
        return;
    }

    foreach (var cat in categorias)
    {
        Console.WriteLine($"ID: {cat.Id} | Nombre: {cat.Nombre}");
    }
    Console.ReadKey();
}

void VerLibros()
{
    Console.Clear();
    Console.WriteLine("===== LIBROS ACTIVOS REGISTRADOS =====");
    Console.WriteLine();

    var libros = libroRepository.ObtenerTodosCon("Autor")
                                 .Where(l => l.Activo)
                                 .ToList();

    if (libros.Count == 0)
    {
        Console.WriteLine("No hay libros activos registrados.");
        Console.ReadKey();
        return;
    }

    foreach (var libro in libros)
    {
        Console.WriteLine($"ID: {libro.Id}");
        Console.WriteLine($"Título: {libro.Titulo}");
        Console.WriteLine($"Año de publicación: {libro.AnioPublicacion}");
        Console.WriteLine($"Autor: {libro.Autor?.Nombre}");
        Console.WriteLine("------------------------------");
    }

    Console.ReadKey();
}

void ModificarLibro()
{
    Console.Clear();
    Console.WriteLine("===== MODIFICAR LIBRO =====");
    Console.WriteLine();

    var libros = libroRepository.ObtenerTodos().Where(l => l.Activo).ToList();
    if (libros.Count == 0)
    {
        Console.WriteLine("No hay libros para modificar.");
        Console.ReadKey();
        return;
    }

    foreach (var l in libros)
    {
        Console.WriteLine($"ID: {l.Id} | Título: {l.Titulo}");
    }

    Console.Write("\nIngrese el ID del libro que desea modificar: ");
    if (!int.TryParse(Console.ReadLine(), out int idLibro))
    {
        Console.WriteLine("ID inválido.");
        Console.ReadKey();
        return;
    }

    var libroModificar = context.Libros.Find(idLibro);
    if (libroModificar == null || !libroModificar.Activo)
    {
        Console.WriteLine("Libro no encontrado.");
        Console.ReadKey();
        return;
    }

    Console.Write($"Ingrese el nuevo título (Actual: {libroModificar.Titulo}): ");
    string nuevoTitulo = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(nuevoTitulo))
    {
        libroModificar.Titulo = nuevoTitulo;
    }

    context.Libros.Update(libroModificar);
    context.SaveChanges();

    Console.WriteLine("\nLibro modificado exitosamente.");
    Console.ReadKey();
}

void EliminarLibroLogico()
{
    Console.Clear();
    Console.WriteLine("===== ELIMINAR LIBRO (LÓGICO) =====");
    Console.WriteLine();

    var libros = libroRepository.ObtenerTodos().Where(l => l.Activo).ToList();
    if (libros.Count == 0)
    {
        Console.WriteLine("No hay libros activos para eliminar.");
        Console.ReadKey();
        return;
    }

    foreach (var l in libros)
    {
        Console.WriteLine($"ID: {l.Id} | Título: {l.Titulo}");
    }

    Console.Write("\nIngrese el ID del libro a eliminar: ");
    if (!int.TryParse(Console.ReadLine(), out int idLibro))
    {
        Console.WriteLine("ID inválido.");
        Console.ReadKey();
        return;
    }

    var libroAEliminar = context.Libros.Find(idLibro);
    if (libroAEliminar == null)
    {
        Console.WriteLine("No se encontró el libro.");
        Console.ReadKey();
        return;
    }

    libroAEliminar.Activo = false;
    context.Libros.Update(libroAEliminar);
    context.SaveChanges();

    Console.WriteLine("\nLibro eliminado lógicamente de forma exitosa.");
    Console.ReadKey();
}