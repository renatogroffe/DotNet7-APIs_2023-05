namespace APIContagem.Models;

public class ResultadoContador
{
    public int ValorAtual { get; set; }
    public string Horario { get; init; } = $"{DateTime.Now:HH:mm:ss}";

    public string? Producer { get; set; }
    public string? Kernel { get; set; }
    public string? Framework { get; set; }
    public string? Mensagem { get; set; }
}