namespace SistemaGestionEstudiantes.Excepciones;

public class MatriculaDuplicadaException : Exception
{
    public MatriculaDuplicadaException(string id)
        : base($"Ya existe un estudiante con el ID o matrícula '{id}'.") { }
}
