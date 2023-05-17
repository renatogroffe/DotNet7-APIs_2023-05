using APIInjDependencias.Interfaces;

namespace APIInjDependencias.Implementations;

public class TesteA : ITesteA
{
    public Guid IdReferencia { get; }

    public TesteA()
    {
        IdReferencia = Guid.NewGuid();
    }
}

public class TesteB : ITesteB
{
    public Guid IdReferencia { get; }

    public TesteB()
    {
        IdReferencia = Guid.NewGuid();
    }
}

public class TesteC
{
    public Guid IdReferencia { get; }

    public TesteC()
    {
        IdReferencia = Guid.NewGuid();
    }
}

public class TesteInjecao
{
    private readonly ITesteA _testeA;
    private readonly ITesteB _testeB;
    private readonly TesteC _testeC;

    public TesteInjecao(
        ITesteA testeA,
        ITesteB testeB,
        TesteC testeC)
    {
        _testeA = testeA;
        _testeB = testeB;
        _testeC = testeC;
    }

    public ResultadoInjecao RetornarValoresInjecao(
        ITesteA testeA,
        ITesteB testeB,
        TesteC testeC)
    {
        var valoresA_Singleton = new ValoresInjecaoTipo<ITesteA>()
        {
            Classe = _testeA.IdReferencia,
            Endpoint = testeA.IdReferencia
        };
        var valoresB_Transient = new ValoresInjecaoTipo<ITesteB>()
        {
            Classe = _testeB.IdReferencia,
            Endpoint = testeB.IdReferencia
        };
        var valoresC_Scoped = new ValoresInjecaoTipo<TesteC>()
        {
            Classe = _testeC.IdReferencia,
            Endpoint = testeC.IdReferencia
        };

        return new ResultadoInjecao()
        {
            ValoresA = valoresA_Singleton,
            ValoresB = valoresB_Transient,
            ValoresC = valoresC_Scoped
        };
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
    public Guid Classe { get; set; }
    public Guid Endpoint { get; set; }

    public override string ToString() =>
        $"{typeof(T).Name} - Classe: {Classe.ToString()} - Endpoint: {Endpoint.ToString()}";
}