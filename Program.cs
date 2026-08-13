namespace SistemaGestionEstudiantes;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FrmPrincipal(new GestorEstudiantes()));
    }
}
