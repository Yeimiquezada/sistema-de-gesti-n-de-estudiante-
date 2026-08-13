using System.Drawing;

namespace SistemaGestionEstudiantes;

public class FrmListado : Form
{
    private readonly GestorEstudiantes gestor;
    private readonly TextBox txtBusqueda = new();
    private readonly DataGridView dgvEstudiantes = Estilos.CrearTabla();

    public FrmListado(GestorEstudiantes gestor, bool enfocarBusqueda = false)
    {
        this.gestor = gestor;
        Estilos.PrepararFormulario(this, "Consulta de estudiantes", new Size(1050, 650));
        ConstruirInterfaz();
        CargarDatos();
        if (enfocarBusqueda)
            Shown += (_, _) => txtBusqueda.Focus();
        else
            Shown += (_, _) => BeginInvoke(new Action(PreguntarRepetirListado));
    }

    private void ConstruirInterfaz()
    {
        var principal = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(30),
            ColumnCount = 1,
            RowCount = 4
        };
        principal.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        principal.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        principal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        principal.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        principal.Controls.Add(Estilos.CrearTitulo("Listado y búsqueda de estudiantes"), 0, 0);

        var busqueda = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        busqueda.Controls.Add(new Label { Text = "ID o nombre:", AutoSize = true, Margin = new Padding(3, 13, 5, 3) });
        txtBusqueda.Width = 330;
        txtBusqueda.Margin = new Padding(3, 8, 5, 3);
        Button btnBuscar = Estilos.CrearBoton("Buscar", Estilos.Azul);
        btnBuscar.Width = 120;
        Button btnTodos = Estilos.CrearBoton("Mostrar todos", Estilos.Verde);
        btnTodos.Width = 145;
        btnBuscar.Click += (_, _) => Buscar();
        btnTodos.Click += (_, _) => { txtBusqueda.Clear(); CargarDatos(); };
        txtBusqueda.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) Buscar(); };
        busqueda.Controls.Add(txtBusqueda);
        busqueda.Controls.Add(btnBuscar);
        busqueda.Controls.Add(btnTodos);
        principal.Controls.Add(busqueda, 0, 1);
        principal.Controls.Add(dgvEstudiantes, 0, 2);

        Button btnVolver = Estilos.CrearBoton("Volver al menú", Color.Gray);
        btnVolver.Anchor = AnchorStyles.Right;
        btnVolver.Click += (_, _) => Close();
        principal.Controls.Add(btnVolver, 0, 3);
        Controls.Add(principal);
    }

    private void CargarDatos() => AsignarDatos(gestor.ObtenerTodos());

    private void Buscar()
    {
        try
        {
            var resultados = gestor.Buscar(txtBusqueda.Text);
            AsignarDatos(resultados);
            if (resultados.Count == 0)
                MessageBox.Show("No se encontraron coincidencias.", "Búsqueda",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            PreguntarRepetirBusqueda();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Dato requerido", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AsignarDatos(object datos)
    {
        dgvEstudiantes.DataSource = null;
        dgvEstudiantes.DataSource = datos;
        if (dgvEstudiantes.Columns.Count > 0)
        {
            dgvEstudiantes.Columns[nameof(Modelos.Estudiante.Id)].HeaderText = "ID / Matrícula";
            dgvEstudiantes.Columns[nameof(Modelos.Estudiante.NombreCompleto)].HeaderText = "Nombre completo";
            dgvEstudiantes.Columns[nameof(Modelos.Estudiante.EstadoAcademico)].HeaderText = "Estado académico";
            dgvEstudiantes.Columns[nameof(Modelos.Estudiante.FechaInscripcion)].HeaderText = "Fecha de inscripción";
            dgvEstudiantes.Columns[nameof(Modelos.Estudiante.FechaInscripcion)].DefaultCellStyle.Format = "dd/MM/yyyy";
        }
    }

    private void PreguntarRepetirBusqueda()
    {
        if (MessageBox.Show("¿Desea realizar otra búsqueda?", "Continuar",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            txtBusqueda.Clear();
            txtBusqueda.Focus();
        }
        else Close();
    }

    private void PreguntarRepetirListado()
    {
        if (MessageBox.Show("¿Desea actualizar y listar nuevamente los estudiantes?", "Continuar",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            CargarDatos();
        else
            Close();
    }
}
