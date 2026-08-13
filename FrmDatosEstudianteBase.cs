using SistemaGestionEstudiantes.Modelos;
using System.Drawing;

namespace SistemaGestionEstudiantes;

public abstract class FrmDatosEstudianteBase : Form
{
    protected readonly TextBox txtId = new();
    protected readonly TextBox txtNombre = new();
    protected readonly TextBox txtEdad = new();
    protected readonly ComboBox cboSexo = new();
    protected readonly TextBox txtCarrera = new();
    protected readonly ComboBox cboEstado = new();
    protected readonly DateTimePicker dtpFecha = new();
    protected readonly Button btnAccion;
    protected readonly Button btnVolver;
    protected readonly TableLayoutPanel campos;

    protected FrmDatosEstudianteBase(string titulo, string textoAccion)
    {
        Estilos.PrepararFormulario(this, titulo, new Size(670, 650));
        btnAccion = Estilos.CrearBoton(textoAccion, Estilos.Verde);
        btnVolver = Estilos.CrearBoton("Volver al menú", Color.Gray);
        campos = ConstruirCampos();
        ConstruirBase(titulo);
    }

    private void ConstruirBase(string titulo)
    {
        var principal = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(45, 30, 45, 30),
            RowCount = 3,
            ColumnCount = 1
        };
        principal.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        principal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        principal.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        principal.Controls.Add(Estilos.CrearTitulo(titulo), 0, 0);
        principal.Controls.Add(campos, 0, 1);

        var acciones = new FlowLayoutPanel
        {
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            Anchor = AnchorStyles.Right
        };
        btnVolver.Click += (_, _) => Close();
        acciones.Controls.Add(btnAccion);
        acciones.Controls.Add(btnVolver);
        principal.Controls.Add(acciones, 0, 2);
        Controls.Add(principal);
    }

    private TableLayoutPanel ConstruirCampos()
    {
        var panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 7 };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66));
        for (int i = 0; i < 7; i++) panel.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28F));

        cboSexo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboSexo.DataSource = Enum.GetValues<Sexo>();
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEstado.DataSource = Enum.GetValues<EstadoAcademico>();
        dtpFecha.Format = DateTimePickerFormat.Short;
        dtpFecha.MaxDate = DateTime.Today;

        AgregarCampo(panel, "ID o matrícula:", txtId, 0);
        AgregarCampo(panel, "Nombre completo:", txtNombre, 1);
        AgregarCampo(panel, "Edad:", txtEdad, 2);
        AgregarCampo(panel, "Sexo:", cboSexo, 3);
        AgregarCampo(panel, "Carrera:", txtCarrera, 4);
        AgregarCampo(panel, "Estado académico:", cboEstado, 5);
        AgregarCampo(panel, "Fecha de inscripción:", dtpFecha, 6);
        return panel;
    }

    private static void AgregarCampo(TableLayoutPanel panel, string etiqueta, Control control, int fila)
    {
        var label = new Label { Text = etiqueta, AutoSize = true, Anchor = AnchorStyles.Left };
        control.Dock = DockStyle.Fill;
        control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        control.Margin = new Padding(3, 8, 3, 8);
        panel.Controls.Add(label, 0, fila);
        panel.Controls.Add(control, 1, fila);
    }

    protected Estudiante LeerFormulario()
    {
        return new Estudiante
        {
            Id = txtId.Text,
            NombreCompleto = txtNombre.Text,
            Edad = GestorEstudiantes.ConvertirEdad(txtEdad.Text),
            Sexo = (Sexo)cboSexo.SelectedItem!,
            Carrera = txtCarrera.Text,
            EstadoAcademico = (EstadoAcademico)cboEstado.SelectedItem!,
            FechaInscripcion = dtpFecha.Value.Date
        };
    }

    protected void CargarFormulario(Estudiante estudiante)
    {
        txtId.Text = estudiante.Id;
        txtNombre.Text = estudiante.NombreCompleto;
        txtEdad.Text = estudiante.Edad.ToString();
        cboSexo.SelectedItem = estudiante.Sexo;
        txtCarrera.Text = estudiante.Carrera;
        cboEstado.SelectedItem = estudiante.EstadoAcademico;
        dtpFecha.Value = estudiante.FechaInscripcion;
    }

    protected void LimpiarFormulario()
    {
        txtId.Clear();
        txtNombre.Clear();
        txtEdad.Clear();
        txtCarrera.Clear();
        cboSexo.SelectedIndex = 0;
        cboEstado.SelectedIndex = 0;
        dtpFecha.Value = DateTime.Today;
        txtId.Focus();
    }

    protected bool PreguntarOtraTransaccion(string pregunta)
    {
        DialogResult respuesta = MessageBox.Show(pregunta, "Continuar",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (respuesta == DialogResult.No) Close();
        return respuesta == DialogResult.Yes;
    }

    protected static void MostrarError(Exception ex) => MessageBox.Show(ex.Message, "Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
}
