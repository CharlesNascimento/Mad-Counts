using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe capaz de gerar uma expressão aritmética de acordo com os parâmetros especificados.
/// </summary>
public class ExpressionGenerator
{
    int operandsCount;
    int minOperandValue;
    int maxOperandValue;

    List<string> operators = new List<string>();

    /// <summary>
    /// Cria uma nova instância desta classe.
    /// </summary>
    /// <param name="operandsCount">A quantidade de operandos da expressão.</param>
    /// <param name="minOperandValue">O menor valor possível dos operandos da expressão.</param>
    /// <param name="maxOperandValue">O maior valor possível dos operandos da expressão.</param>
    public ExpressionGenerator(int operandsCount, int minOperandValue, int maxOperandValue)
    {
        this.operandsCount = operandsCount;
        this.minOperandValue = minOperandValue;
        this.maxOperandValue = maxOperandValue;

        operators.Add("+");
        operators.Add("-");
        operators.Add("*");
        operators.Add("/");
    }

    /// <summary>
    /// Gera uma expressão aritmética de acordo com os parâmetros especificados no objeto.
    /// </summary>
    /// <returns>Uma expressão aritmética aleatória.</returns>
    public string Generate()
    {
        string expression = RandomOperand();
        int lastOperandIndex = operandsCount - 1;

        for (int i = 1; i < operandsCount; i++)
        {
            expression += RandomOperator() + RandomOperand();

            if (i == lastOperandIndex) continue;

            expression = MaybeWrapInParentheses(expression);
        }

        return expression;
    }

    /// <summary>
    /// Retorna um operador aleatório.
    /// </summary>
    /// <returns>Um operador aleatório.</returns>
    private string RandomOperator()
    {
        int index = Random.Range(0, operators.Count - 1);
        return operators[index];
    }

    /// <summary>
    /// Retorna um operando aleatório.
    /// </summary>
    /// <returns>Um operando aleatório.</returns>
    private string RandomOperand()
    {
        return Random.Range(minOperandValue, maxOperandValue + 1).ToString();
    }

    /// <summary>
    /// De maneira aleatória, envolve a expressão passada em parênteses, ou retorna
    /// a própria expressão sem nenhuma alteração.
    /// </summary>
    /// <param name="expression">A expressão.</param>
    /// <returns>Expressão envolvida por parênteses, ou ela própria inalterada.</returns>
    private string MaybeWrapInParentheses(string expression)
    {
        bool wrap = Random.Range(0, 2) == 0 ? false : true;

        if (wrap)
        {
            return "(" + expression + ")";
        }

        return expression;
    }
}
