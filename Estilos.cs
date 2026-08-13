using System.Drawing;

namespace SistemaGestionEstudiantes;

internal static class Estilos
{
    public static readonly Color Azul = Color.FromArgb(26, 71, 122);
    public static readonly Color AzulClaro = Color.FromArgb(232, 241, 250);
    public static readonly Color Verde = Color.FromArgb(40, 130, 90);
    public static readonly Color Rojo = Color.FromArgb(180, 55, 55);

    public static void PrepararFormulario(Form formulario, string titulo, Size tamano)
    {
        formulario.Text = titulo;
        formulario.StartPosition = FormStartPosition.CenterScreen;
        formulario.Size = tamano;
        formulario.MinimumSize = tamano;
        formulario.Font = new Font("Segoe UI", 10F);
        formulario.BackColor = Color.White;
    }

    public static Label CrearTitulo(string texto) => new()
    {
        Text = texto,
        Font = new Font("Segoe UI", 18F, FontStyle.Bold),
        ForeColor = Azul,
        AutoSize = true,
        Margin = new Padding(3, 3, 3, 18)
    };

    public static Button CrearBoton(string texto, Color color)
    {
        return new Button
        {
            Text = texto,
            BackColor = color,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Height = 42,
            Width = 190,
            Cursor = Cursors.Hand,
            Margin = new Padding(6)
        };
    }

    public static DataGridView CrearTabla()
    {
        return new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.Fixed3D
        };
    }
}
