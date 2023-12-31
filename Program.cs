using ArithmeticExpressionParser;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ArithmeticExpressionParser
{
    public class ExpressionParser
    {
        private Dictionary<string, double> variables;

        public ExpressionParser()
        {
            variables = new Dictionary<string, double>();
        }

        public double EvaluateExpression(string expression)
        {
            double result = 0;

            // Split the expression by semicolon.
            string[] expressions = expression.Split(';');

            foreach (string exp in expressions)
            {
                // Remove whitespaces.
                string trimmedExp = exp.Replace(" ", "");

                // Evaluate the expression.
                result = Evaluate(trimmedExp);
            }

            return result;
        }

        private double Evaluate(string expression)
        {
            // Replace variables with their values.
            foreach (KeyValuePair<string, double> variable in variables)
            {
                expression = expression.Replace(variable.Key, variable.Value.ToString());
            }

            // Evaluate the expression using built-in DataTable class.
            System.Data.DataTable dataTable = new System.Data.DataTable();
            dataTable.Columns.Add("expression", typeof(string), expression);
            System.Data.DataRow row = dataTable.NewRow();
            dataTable.Rows.Add(row);
            double result = double.Parse((string)row["expression"]);

            return result;
        }

        public void AssignVariable(string variable, double value)
        {
            variables[variable] = value;
        }
    }
}

// Example program for ExpressionParser class.

public class Program
{
    public static void Main()
    {
        var parser = new ExpressionParser();

        // Example 1: Evaluating arithmetic expressions.
        string expression1 = "x := 2.5; y := 3.7; z := x + y; z";
        double result1 = parser.EvaluateExpression(expression1);
        Console.WriteLine($"Result of expression: {expression1} = {result1}");
        Console.WriteLine();  // Newline for separation.

        // Example 2: Evaluating another arithmetic expression.
        string expression2 = "a := 5; b := 2; c := (a + b) * 3.5; c";
        double result2 = parser.EvaluateExpression(expression2);
        Console.WriteLine($"Result of expression: {expression2} = {result2}");
        Console.WriteLine();  // Newline for separation.

        // Example 3: Evaluating arithmetic expression with variables.
        string expression3 = "x := 2.5; y := 3.7; z := (x + y) / 2; z";
        double result3 = parser.EvaluateExpression(expression3);
        Console.WriteLine($"Result of expression: {expression3} = {result3}");
    }
}