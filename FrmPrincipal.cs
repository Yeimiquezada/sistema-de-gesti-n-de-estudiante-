using System.Drawing;

namespace SistemaGestionEstudiantes;

public class FrmPrincipal : Form
{
    private readonly GestorEstudiantes gestor;

    public FrmPrincipal(GestorEstudiantes gestor)
    {
        this.gestor = gestor;
        Estilos.PrepararFormulario(this, "Sistema de Gestión de Estudiantes - UCE", new Size(720, 620));
        FormClosing += ConfirmarSalida;
        ConstruirInterfaz();
    }

    private void ConstruirInterfaz()
    {
        var contenedor = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(55, 35, 55, 35),
            ColumnCount = 1,
            RowCount = 4
        };
        contenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        contenedor.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        contenedor.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        contenedor.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        contenedor.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        Label titulo = Estilos.CrearTitulo("Sistema de Gestión de Estudiantes");
        titulo.Anchor = AnchorStyles.None;
        var subtitulo = new Label
        {
            Text = "Universidad Central del Este (UCE)\nProgramación Básica",
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.DimGray,
            Anchor = AnchorStyles.None,
            Margin = new Padding(3, 0, 3, 25)
        };

        var botones = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3 };
        botones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        botones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        for (int i = 0; i < 3; i++) botones.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

        AgregarOpcion(botones, "Registrar estudiante", 0, 0, () => new FrmRegistro(gestor).ShowDialog(this));
        AgregarOpcion(botones, "Listar estudiantes", 1, 0, () => new FrmListado(gestor).ShowDialog(this));
        AgregarOpcion(botones, "Buscar estudiante", 0, 1, () => new FrmListado(gestor, true).ShowDialog(this));
        AgregarOpcion(botones, "Actualizar estudiante", 1, 1, () => new FrmActualizar(gestor).ShowDialog(this));
        AgregarOpcion(botones, "Eliminar estudiante", 0, 2, () => new FrmEliminar(gestor).ShowDialog(this));
        AgregarOpcion(botones, "Salir del sistema", 1, 2, Close, Estilos.Rojo);

        var estado = new Label
        {
            Text = "Los datos se almacenan temporalmente en memoria mediante List<Estudiante>.",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.DimGray,
            BackColor = Estilos.AzulClaro,
            Padding = new Padding(10)
        };

        contenedor.Controls.Add(titulo, 0, 0);
        contenedor.Controls.Add(subtitulo, 0, 1);
        contenedor.Controls.Add(botones, 0, 2);
        contenedor.Controls.Add(estado, 0, 3);
        Controls.Add(contenedor);
    }

    private static void AgregarOpcion(TableLayoutPanel panel, string texto, int columna, int fila,
        Action accion, Color? color = null)
    {
        Button boton = Estilos.CrearBoton(texto, color ?? Estilos.Azul);
        boton.Dock = DockStyle.Fill;
        boton.Click += (_, _) => accion();
        panel.Controls.Add(boton, columna, fila);
    }

    private void ConfirmarSalida(object? sender, FormClosingEventArgs e)
    {
        if (MessageBox.Show("¿Seguro que desea salir del sistema?", "Confirmar salida",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            e.Cancel = true;
    }
}
