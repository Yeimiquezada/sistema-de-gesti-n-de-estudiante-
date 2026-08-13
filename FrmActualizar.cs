using SistemaGestionEstudiantes.Excepciones;
using System.Drawing;

namespace SistemaGestionEstudiantes;

public class FrmActualizar : FrmDatosEstudianteBase
{
    private readonly GestorEstudiantes gestor;
    private string? idOriginal;
    private readonly TextBox txtIdBuscar = new();

    public FrmActualizar(GestorEstudiantes gestor) : base("Actualizar estudiante", "Guardar cambios")
    {
        this.gestor = gestor;
        AgregarBuscador();
        HabilitarEdicion(false);
        btnAccion.Click += GuardarCambios;
    }

    private void AgregarBuscador()
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(3, 5, 3, 3) };
        panel.Controls.Add(new Label { Text = "ID a localizar:", AutoSize = true, Margin = new Padding(3, 10, 5, 3) });
        txtIdBuscar.Width = 220;
        txtIdBuscar.Margin = new Padding(3, 6, 5, 3);
        Button btnBuscar = Estilos.CrearBoton("Cargar", Estilos.Azul);
        btnBuscar.Width = 110;
        btnBuscar.Height = 36;
        btnBuscar.Click += (_, _) => CargarEstudiante();
        panel.Controls.Add(txtIdBuscar);
        panel.Controls.Add(btnBuscar);
        campos.RowCount = 8;
        campos.RowStyles.Clear();
        campos.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
        for (int i = 0; i < 7; i++)
            campos.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28F));
        for (int i = campos.Controls.Count - 1; i >= 0; i--)
        {
            Control c = campos.Controls[i];
            if (campos.GetRow(c) < 7) campos.SetRow(c, campos.GetRow(c) + 1);
        }
        campos.Controls.Add(panel, 0, 0);
        campos.SetColumnSpan(panel, 2);
    }

    private void CargarEstudiante()
    {
        try
        {
            var estudiante = gestor.BuscarPorId(txtIdBuscar.Text);
            idOriginal = estudiante.Id;
            CargarFormulario(estudiante);
            HabilitarEdicion(true);
        }
        catch (Exception ex) when (ex is ArgumentException or EstudianteNoEncontradoException)
        {
            MostrarError(ex);
        }
    }

    private void GuardarCambios(object? sender, EventArgs e)
    {
        try
        {
            if (idOriginal is null) throw new InvalidOperationException("Primero debe cargar un estudiante.");
            gestor.Actualizar(idOriginal, LeerFormulario());
            MessageBox.Show("Datos actualizados correctamente.", "Actualización exitosa",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (PreguntarOtraTransaccion("¿Desea actualizar otro estudiante?")) PrepararNuevaOperacion();
        }
        catch (Exception ex) when (ex is ArgumentException or FormatException or InvalidOperationException or MatriculaDuplicadaException)
        {
            MostrarError(ex);
        }
        finally
        {
            btnAccion.Enabled = idOriginal is not null;
        }
    }

    private void PrepararNuevaOperacion()
    {
        idOriginal = null;
        txtIdBuscar.Clear();
        LimpiarFormulario();
        HabilitarEdicion(false);
        txtIdBuscar.Focus();
    }

    private void HabilitarEdicion(bool habilitar)
    {
        foreach (Control c in new Control[] { txtId, txtNombre, txtEdad, cboSexo, txtCarrera, cboEstado, dtpFecha })
            c.Enabled = habilitar;
        btnAccion.Enabled = habilitar;
    }
}
