using SistemaGestionEstudiantes.Excepciones;
using System.Drawing;

namespace SistemaGestionEstudiantes;

public class FrmEliminar : Form
{
    private readonly GestorEstudiantes gestor;
    private readonly TextBox txtId = new();
    private readonly Label lblDatos = new();

    public FrmEliminar(GestorEstudiantes gestor)
    {
        this.gestor = gestor;
        Estilos.PrepararFormulario(this, "Eliminar estudiante", new Size(620, 420));
        ConstruirInterfaz();
    }

    private void ConstruirInterfaz()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(40, 30, 40, 30),
            ColumnCount = 1,
            RowCount = 4
        };
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.Controls.Add(Estilos.CrearTitulo("Eliminar estudiante"), 0, 0);

        var busqueda = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        busqueda.Controls.Add(new Label { Text = "ID o matrícula:", AutoSize = true, Margin = new Padding(3, 13, 5, 3) });
        txtId.Width = 230;
        txtId.Margin = new Padding(3, 8, 5, 3);
        Button btnVerificar = Estilos.CrearBoton("Verificar", Estilos.Azul);
        btnVerificar.Width = 115;
        btnVerificar.Click += (_, _) => Verificar();
        busqueda.Controls.Add(txtId);
        busqueda.Controls.Add(btnVerificar);
        panel.Controls.Add(busqueda, 0, 1);

        lblDatos.Dock = DockStyle.Fill;
        lblDatos.TextAlign = ContentAlignment.MiddleCenter;
        lblDatos.BackColor = Estilos.AzulClaro;
        lblDatos.Text = "Escriba una matrícula y pulse Verificar.";
        panel.Controls.Add(lblDatos, 0, 2);

        var acciones = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.Right };
        Button btnEliminar = Estilos.CrearBoton("Eliminar", Estilos.Rojo);
        Button btnVolver = Estilos.CrearBoton("Volver al menú", Color.Gray);
        btnEliminar.Click += (_, _) => Eliminar();
        btnVolver.Click += (_, _) => Close();
        acciones.Controls.Add(btnEliminar);
        acciones.Controls.Add(btnVolver);
        panel.Controls.Add(acciones, 0, 3);
        Controls.Add(panel);
    }

    private void Verificar()
    {
        try
        {
            var e = gestor.BuscarPorId(txtId.Text);
            lblDatos.Text = $"{e.Id} — {e.NombreCompleto}\n{e.Carrera} | {e.EstadoAcademico}";
        }
        catch (Exception ex) when (ex is ArgumentException or EstudianteNoEncontradoException)
        {
            MostrarError(ex);
        }
    }

    private void Eliminar()
    {
        try
        {
            var estudiante = gestor.BuscarPorId(txtId.Text);
            if (MessageBox.Show($"¿Seguro que desea eliminar a {estudiante.NombreCompleto}?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            gestor.Eliminar(estudiante.Id);
            MessageBox.Show("Estudiante eliminado correctamente.", "Eliminación exitosa",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (MessageBox.Show("¿Desea eliminar otro estudiante?", "Continuar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                txtId.Clear();
                lblDatos.Text = "Escriba una matrícula y pulse Verificar.";
                txtId.Focus();
            }
            else Close();
        }
        catch (Exception ex) when (ex is ArgumentException or EstudianteNoEncontradoException)
        {
            MostrarError(ex);
        }
        finally
        {
            txtId.Enabled = true;
        }
    }

    private static void MostrarError(Exception ex) => MessageBox.Show(ex.Message, "Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
}
