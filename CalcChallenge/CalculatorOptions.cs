using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcChallenge
{
    public class CalculatorOptions
    {
        [Option("allow-negative", Required = false, HelpText = "Allow negative numbers")]
        public bool AllowNegative { get; set; }

        [Option("upper-bound", Required = false, HelpText = "Upper bound for numbers")]
        public int UpperBound { get; set; } = 1000;
    }
}
