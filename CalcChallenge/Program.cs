// See https://aka.ms/new-console-template for more information
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System;
using CommandLine;
using CalcChallenge;

Console.WriteLine("Hello, World!");

Parser.Default.ParseArguments<CalculatorOptions>(args)
          .WithParsed(o =>
          {
              var calculator = new Calculator(o.AllowNegative, o.UpperBound);
              Console.CancelKeyPress += (sender, eventArgs) =>
              {
                  Console.WriteLine("\nExiting the calculator...");
                  Environment.Exit(0);
              };

              while (true)
              {
                  try
                  {
                      Console.Write("Enter operation (+, -, *, /) and numbers (or Ctrl+C to exit): ");
                      var input = Console.ReadLine();
                      int result = calculator.PerformOperation(input);
                      Console.WriteLine($"Result: {result}");
                  }
                  catch (Exception ex)
                  {
                      Console.WriteLine(ex.Message);
                  }
              }
          });
