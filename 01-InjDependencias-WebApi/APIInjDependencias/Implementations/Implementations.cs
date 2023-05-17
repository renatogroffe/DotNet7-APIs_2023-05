using APIInjDependencias.Interfaces;

namespace APIInjDependencias.Implementations;

public class TesteA : ITesteA
{
    public Guid IdReferencia { get; }

    public TesteA()
    {
        this.IdReferencia = Guid.NewGuid();
    }
}

public class TesteB : ITesteB
{
    public Guid IdReferencia { get; }

    public TesteB()
    {
        this.IdReferencia = Guid.NewGuid();
    }
}

public class TesteC
{
    public Guid IdReferencia { get; }

    public TesteC()
    {
        this.IdReferencia = Guid.NewGuid();
    }
}

public class ResultadoInjecao
{
    public ValoresInjecaoTipo<ITesteA>? ValoresA { get; set; }
    public ValoresInjecaoTipo<ITesteB>? ValoresB { get; set; }
    public ValoresInjecaoTipo<TesteC>? ValoresC { get; set; }
}

public class ValoresInjecaoTipo<T>
{
    public Guid Construtor { get; set; }
    public Guid Action { get; set; }

    public override string ToString() =>
        $"{typeof(T).Name} - Construtor: {Construtor.ToString()} - Action: {Action.ToString()}";
}