using SistemaGestionEstudiantes.Excepciones;
using SistemaGestionEstudiantes.Modelos;

namespace SistemaGestionEstudiantes;

/// <summary>Administra en memoria las operaciones CRUD de estudiantes.</summary>
public class GestorEstudiantes
{
    private readonly List<Estudiante> estudiantes = new();

    public IReadOnlyList<Estudiante> ObtenerTodos() =>
        estudiantes.OrderBy(e => e.NombreCompleto).Select(e => e.Copiar()).ToList();

    public void Registrar(Estudiante estudiante)
    {
        ValidarEstudiante(estudiante);
        if (ExisteMatricula(estudiante.Id))
            throw new MatriculaDuplicadaException(estudiante.Id);

        estudiante.Id = estudiante.Id.Trim();
        estudiante.NombreCompleto = estudiante.NombreCompleto.Trim();
        estudiante.Carrera = estudiante.Carrera.Trim();
        estudiantes.Add(estudiante.Copiar());
    }

    public Estudiante BuscarPorId(string id)
    {
        ValidarTextoRequerido(id, "ID o matrícula");
        Estudiante? encontrado = estudiantes.FirstOrDefault(e =>
            e.Id.Equals(id.Trim(), StringComparison.OrdinalIgnoreCase));
        return encontrado?.Copiar() ?? throw new EstudianteNoEncontradoException(id);
    }

    public IReadOnlyList<Estudiante> Buscar(string criterio)
    {
        ValidarTextoRequerido(criterio, "criterio de búsqueda");
        string texto = criterio.Trim();
        return estudiantes
            .Where(e => e.Id.Contains(texto, StringComparison.OrdinalIgnoreCase)
                     || e.NombreCompleto.Contains(texto, StringComparison.OrdinalIgnoreCase))
            .OrderBy(e => e.NombreCompleto)
            .Select(e => e.Copiar())
            .ToList();
    }

    public void Actualizar(string idOriginal, Estudiante datosActualizados)
    {
        ValidarEstudiante(datosActualizados);
        Estudiante existente = estudiantes.FirstOrDefault(e =>
            e.Id.Equals(idOriginal.Trim(), StringComparison.OrdinalIgnoreCase))
            ?? throw new EstudianteNoEncontradoException(idOriginal);

        bool idPerteneceAOtro = estudiantes.Any(e =>
            !ReferenceEquals(e, existente) &&
            e.Id.Equals(datosActualizados.Id.Trim(), StringComparison.OrdinalIgnoreCase));
        if (idPerteneceAOtro)
            throw new MatriculaDuplicadaException(datosActualizados.Id);

        existente.Id = datosActualizados.Id.Trim();
        existente.NombreCompleto = datosActualizados.NombreCompleto.Trim();
        existente.Edad = datosActualizados.Edad;
        existente.Sexo = datosActualizados.Sexo;
        existente.Carrera = datosActualizados.Carrera.Trim();
        existente.EstadoAcademico = datosActualizados.EstadoAcademico;
        existente.FechaInscripcion = datosActualizados.FechaInscripcion;
    }

    public void Eliminar(string id)
    {
        Estudiante existente = estudiantes.FirstOrDefault(e =>
            e.Id.Equals(id.Trim(), StringComparison.OrdinalIgnoreCase))
            ?? throw new EstudianteNoEncontradoException(id);
        estudiantes.Remove(existente);
    }

    public bool ExisteMatricula(string id) => estudiantes.Any(e =>
        e.Id.Equals(id.Trim(), StringComparison.OrdinalIgnoreCase));

    public static int ConvertirEdad(string textoEdad)
    {
        if (!int.TryParse(textoEdad, out int edad))
            throw new FormatException("La edad debe ser un número entero válido.");
        return edad;
    }

    public static void ValidarEstudiante(Estudiante estudiante)
    {
        ArgumentNullException.ThrowIfNull(estudiante);
        ValidarTextoRequerido(estudiante.Id, "ID o matrícula");
        ValidarTextoRequerido(estudiante.NombreCompleto, "nombre completo");
        ValidarTextoRequerido(estudiante.Carrera, "carrera");
        if (estudiante.Edad < 14 || estudiante.Edad > 100)
            throw new ArgumentException("La edad debe estar entre 14 y 100 años.");
        if (estudiante.FechaInscripcion.Date > DateTime.Today)
            throw new ArgumentException("La fecha de inscripción no puede ser futura.");
    }

    private static void ValidarTextoRequerido(string? valor, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException($"El campo {campo} es obligatorio.");
    }
}
