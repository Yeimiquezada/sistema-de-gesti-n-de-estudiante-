namespace SistemaGestionEstudiantes.Excepciones;

public class EstudianteNoEncontradoException : Exception
{
    public EstudianteNoEncontradoException(string id)
        : base($"No se encontró un estudiante con el ID o matrícula '{id}'.") { }
}
