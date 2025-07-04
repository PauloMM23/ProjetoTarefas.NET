namespace ProjetoTarefas.Models;

public class Tarefa
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendente";

    public int ProjetoId { get; set; }
    public Projeto? Projeto { get; set; }
}