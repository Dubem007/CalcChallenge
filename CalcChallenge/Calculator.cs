using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalcChallenge
{
    public class Calculator
    {
        private readonly bool _allowNegative;
        private readonly int _upperBound;

        public Calculator(bool allowNegative, int upperBound)
        {
            _allowNegative = allowNegative;
            _upperBound = upperBound;
        }

        public int PerformOperation(string input)
        {
            string numbersStr = string.Empty;
            string formula = string.Empty;
            if (string.IsNullOrWhiteSpace(input) || input.Length < 2)
            {
                throw new Exception("Invalid input. Please enter an operation followed by numbers.");
            }

            char operation = input[0];
            if (char.IsDigit(operation))
            {
                numbersStr = input.Trim();
            }
            else if (operation == '/' && input[1] == '/')
            {
                numbersStr = input.Trim();
                operation = char.MinValue;
            } 
            else if (operation == '/' && input[1] != '/') 
            {
                var numr = input.Substring(1);
                numbersStr = numr.Trim();
            }
            else
            {
                var numr = input.Substring(1);
                numbersStr = numr.Trim();
            }


            int[] numbers = ParseNumbers(numbersStr);

            if (!_allowNegative)
            {
                var negativeNumbers = numbers.Where(n => n < 0).ToArray();
                if (negativeNumbers.Length > 0)
                    throw new Exception($"Negative numbers not allowed: {string.Join(", ", negativeNumbers)}");
            }

            numbers = numbers.Select(n => n > _upperBound ? 0 : n).ToArray();
            if (char.IsDigit(operation))
            {
                operation = '+';
                formula = string.Join(operation, numbers);
            }
            else if (operation == '/' && input[1] == '/') 
            {
                operation = '+';
                formula = string.Join(operation, numbers);
            }
            else
            {
                formula = string.Join('+', numbers);
            }
         

            int result;
            switch (operation)
            {
                case '+':
                    result = numbers.Sum();
                    break;
                case '-':
                    result = numbers.Length > 0 ? numbers[0] - numbers.Skip(1).Sum() : 0;
                    break;
                case '*':
                    result = numbers.Aggregate(1, (acc, num) => acc * num);
                    break;
                case '/':
                    result = numbers.Length > 0 ? numbers[0] / numbers.Skip(1).Aggregate(1, (acc, num) => acc * num) : 0;
                    break;
                default:
                    result = numbers.Sum();
                    break;
                    //throw new Exception($"Invalid operation: {operation}");
            }

            Console.WriteLine($"{formula} = {result}");
            return result;
        }

        private int[] ParseNumbers(string numbersStr)
        {
            if (numbersStr.StartsWith("//"))
            {
                int delimiterEndIndex = numbersStr.IndexOf("\\n");
                if (delimiterEndIndex == -1)
                {
                    throw new Exception("Invalid format: newline character not found after custom delimiters");
                }

                string delimiterPart = numbersStr.Substring(2, delimiterEndIndex - 2);
                string numberPart = numbersStr.Substring(delimiterEndIndex + 1);

                string[] delimiters;

                if (delimiterPart.StartsWith("[") && delimiterPart.EndsWith("]"))
                {
                    // Multiple custom delimiters
                    delimiters = Regex.Matches(delimiterPart, @"\[(.*?)\]")
                        .Select(m => Regex.Escape(m.Groups[1].Value))
                        .ToArray();
                }
                else
                {
                    // Single character custom delimiter
                    delimiters = new[] { Regex.Escape(delimiterPart) };
                }

                // Use regex to split the string by any non-digit character
                string[] parts = Regex.Split(numberPart, @"\D+");

                // Convert the parts to integers, replacing empty parts or invalid numbers with 0
                int[] numbers = parts.Select(part => int.TryParse(part, out int number) ? number : 0).ToArray();
                return numbers;
                //return Regex.Split(numberPart, string.Join("|", delimiters))
                //    .Where(s => !string.IsNullOrWhiteSpace(s))
                //    .Select(s => int.TryParse(s, out int n) ? n : 0)
                //    .ToArray();
            }
            else
            {
                var delimitersval = numbersStr.Replace("\\n", ",").ToString();
                return delimitersval.Replace("\n", ",").Split(',')
                    .Select(s => int.TryParse(s, out int n) ? n : 0)
                    .ToArray();
            }
        }

    }
}
