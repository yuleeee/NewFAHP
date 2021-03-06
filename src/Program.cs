﻿using System;

using static System.Console;
using static NewFAHP.Numbers;

namespace NewFAHP
{
    class Program
    {
        static string[] Criteria = { "DIST", "SES", "ADS", "AS", "MFR" };

        static void Main(string[] args)
        {
            int[] values = { 0, 1, 2, 3, 4 };
            int ConfLevel = 2;

            var ComparisonMatrix = Inference.ComparisonMatrix(values, ConfLevel);

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                    Write($"{ComparisonMatrix[i, j].Item1},{ComparisonMatrix[i, j].Item2},{ComparisonMatrix[i, j].Item3},");
                WriteLine();
            }
            

             var fahp = new FAHP(ComparisonMatrix);
             var weights = fahp.CriteriaWeights;
             
            WriteLine($"TSR = {weights[0]}");
             for (int i = 1; i < 6; i++)                
                 WriteLine($"{Criteria[i - 1]} = {weights[i]}");
        }

        static int[] TakeValues()
        {
            int[] values = new int[5];
            int input;
            string read;

            for (int i = 0; i < 5; i++)
            {
                WriteLine($"Compared to TSR, is {Criteria[i]} more important to you?");
                WriteLine($"[A] Yes, {Criteria[i]} is more important\n[B] No, TSR is more important");
                WriteLine("[C] No, they're equally important");
                Write("Please enter (A/B/C): ");
                read = ReadLine();

                switch (read)
                {
                    case "A": case "a":
                        values[i] = -1;
                        break;
                    case "B": case "b":
                        values[i] = 1;
                        break;
                    default:
                        values[i] = 0;
                        break;
                }

                if (values[i] != 0)
                {
                    WriteLine($"values[{i}] = {values[i]}");
                    WriteLine($"\nHow much more important?");
                    WriteLine("[1] Weakly\n[2] Moderately\n[3] Strongly\n[4] Absolutely");
                    Write("Please enter (1—4): ");
                    input = int.Parse(ReadLine()) - 1;
                    values[i] *= input;
                    WriteLine($"values[{i}] = {values[i]}");
                }
                WriteLine();
            }

            WriteLine("\nFinally, how confident are you with your answer?");
            WriteLine("[1] Weakly\n[2] Moderately\n[3] Very much\n[4] Strongly\n[5] Absolutely");
            Write("Please enter (1—5): ");
            input = Convert.ToInt16(ReadKey().KeyChar.ToString()) - 1;
            WriteLine();

            for (int i = 0; i < 5; i++)
                WriteLine(values[i]);
            
            return values;
        }
    }
}
