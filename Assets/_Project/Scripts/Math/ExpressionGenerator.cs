using org.mariuszgromada.math.mxparser;
using UnityEngine;

namespace KansusGames.MadCounts.Math
{
    /// <summary>
    /// Classe capaz de gerar uma expressão aritmética de acordo com os parâmetros especificados.
    /// </summary>
    public class ExpressionGenerator
    {
        private ExpressionGeneratorData data;

        /// <summary>
        /// Cria uma nova instância desta classe.
        /// </summary>
        /// <param name="operandsCount">A quantidade de operandos da expressão.</param>
        /// <param name="minOperandValue">O menor valor possível dos operandos da expressão.</param>
        /// <param name="maxOperandValue">O maior valor possível dos operandos da expressão.</param>
        public ExpressionGenerator(ExpressionGeneratorData data)
        {
            this.data = data;
        }

        /// <summary>
        /// Gera uma expressão aritmética de acordo com os parâmetros especificados no objeto.
        /// </summary>
        /// <returns>Uma expressão aritmética aleatória.</returns>
        public GeneratedExpression Generate()
        {
            string expression = RandomOperand();
            int lastOperandIndex = data.OperandsCount - 1;

            for (int i = 1; i < data.OperandsCount; i++)
            {
                var @operator = RandomOperator();
                double result;
                string candidate;

                do
                {
                    candidate = expression + @operator + RandomOperand();
                    result = new Expression(candidate).calculate();
                }
                while (data.ForceIntegerResult && result % 1 != 0 || !data.AllowNegativeResults && result < 0);

                expression = candidate;

                if (i == lastOperandIndex) continue;

                expression = MaybeWrapInParentheses(expression);
            }

            return new GeneratedExpression(expression, new Expression(expression).calculate());
        }

        /// <summary>
        /// Retorna um operador aleatório.
        /// </summary>
        /// <returns>Um operador aleatório.</returns>
        private string RandomOperator()
        {
            int index = Random.Range(0, data.AllowedOperators.Count);
            return data.AllowedOperators[index];
        }

        /// <summary>
        /// Retorna um operando aleatório.
        /// </summary>
        /// <returns>Um operando aleatório.</returns>
        private string RandomOperand()
        {
            return Random.Range(data.MinOperandValue, data.MaxOperandValue + 1).ToString();
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
}
