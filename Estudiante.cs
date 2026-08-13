namespace SistemaGestionEstudiantes.Modelos;

/// <summary>Representa un estudiante registrado en el sistema.</summary>
public class Estudiante
{
    public string Id { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int Edad { get; set; }
    public Sexo Sexo { get; set; }
    public string Carrera { get; set; } = string.Empty;
    public EstadoAcademico EstadoAcademico { get; set; }
    public DateTime FechaInscripcion { get; set; }

    public Estudiante Copiar() => (Estudiante)MemberwiseClone();
}
