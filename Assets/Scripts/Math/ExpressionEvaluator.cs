using org.mariuszgromada.math.mxparser;

/// <summary>
/// Classe avaliadora de expressão aritmética.
/// </summary>
public class ExpressionEvaluator
{
    /// <summary>
    /// Avalia a expressão aritmética passada e retorna o resultado em formato numérico.
    /// </summary>
    /// <param name="expression">A expressão a ser avaliada.</param>
    /// <returns>O resultado da expressão aritmética em formato numérico.</returns>
    public int Evaluate(string expression)
    {
        Expression e = new Expression(expression);
        return (int)e.calculate();
    }
}