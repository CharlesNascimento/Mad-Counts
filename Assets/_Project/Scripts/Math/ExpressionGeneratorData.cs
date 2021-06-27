using System.Collections.Generic;

namespace KansusGames.MadCounts.Math
{
    public class ExpressionGeneratorData
    {
        public int OperandsCount { get; set; } = 2;

        public int MinOperandValue { get; set; } = 0;

        public int MaxOperandValue { get; set; } = 10;

        public List<string> AllowedOperators { get; set; } = new List<string>() { "+", "-", "*", "/" };

        public bool ForceIntegerResult { get; set; } = false;

        public bool AllowNegativeResults = false;
    }
}
