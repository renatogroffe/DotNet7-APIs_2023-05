namespace APIContagem.Models;

public class StatusApi
{
    public string? Producer { get; set; }
    public bool Healthy { get; set; }
    public string? Mensagem { get; set; }
}