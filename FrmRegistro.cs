using SistemaGestionEstudiantes.Excepciones;

namespace SistemaGestionEstudiantes;

public class FrmRegistro : FrmDatosEstudianteBase
{
    private readonly GestorEstudiantes gestor;

    public FrmRegistro(GestorEstudiantes gestor) : base("Registrar nuevo estudiante", "Registrar")
    {
        this.gestor = gestor;
        btnAccion.Click += Registrar;
    }

    private void Registrar(object? sender, EventArgs e)
    {
        try
        {
            gestor.Registrar(LeerFormulario());
            MessageBox.Show("Estudiante registrado correctamente.", "Registro exitoso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (PreguntarOtraTransaccion("¿Desea registrar otro estudiante?")) LimpiarFormulario();
        }
        catch (Exception ex) when (ex is ArgumentException or FormatException or MatriculaDuplicadaException)
        {
            MostrarError(ex);
        }
        finally
        {
            btnAccion.Enabled = true;
        }
    }
}
