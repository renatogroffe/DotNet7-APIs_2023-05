using FunctionAppIsolatedDI.Interfaces;

namespace FunctionAppIsolatedDI.Implementations;

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

    public object RetornarValoresInjecao(
        ITesteA testeA,
        ITesteB testeB,
        TesteC testeC)
    {
        var valoresA_Singleton = new { ClasseTeste = _testeA.IdReferencia,
            Function = testeA.IdReferencia };
        var valoresB_Transient = new { ClasseTeste = _testeB.IdReferencia,
            Function = testeB.IdReferencia };
        var valoresC_Scoped = new { ClasseTeste = _testeC.IdReferencia,
            Function = testeC.IdReferencia };

        return new { valoresA_Singleton, valoresB_Transient, valoresC_Scoped };
    }
}