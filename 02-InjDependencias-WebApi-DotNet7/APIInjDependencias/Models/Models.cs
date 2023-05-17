using System.Runtime.InteropServices;
namespace APIInjDependencias.Models;

public class InfoInicializacao
{
    public string? Horario { get; set; }
    public string Framework { get; init; } = RuntimeInformation.FrameworkDescription;
}

public class ResultadoTeste
{
    public string? Action { get; set; }
    public string? HorarioAtual { get; set; }
    public string? HorarioInicializacao { get; set; }
    public string? Framework { get; set; }
}