using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Net.Mime.MediaTypeNames;

namespace RandomNumberGenerators
{
    class Program
    {
        static void Main(string[] args)
        {
            // Choose the number of random values to generate
            const int numValues = 10000;

            // Choose the random number generator engines to use
            var engines = new List<IRandomNumberGenerator> {
                new DefaultRandomNumberGenerator(),
                new XorShiftRandomNumberGenerator(),
                new MersenneTwisterRandomNumberGenerator(),
            };

            // Generate random values using each engine
            var valuesByEngine = engines.ToDictionary(
                engine => engine.Name,
                engine => engine.Generate(numValues).ToList()
            );

            // Allow user to specify a custom distribution
            Console.WriteLine("Enter a comma-separated list of probabilities for each value (e.g. 0.1, 0.2, 0.3, 0.4):");
            var input = Console.ReadLine();
            var probabilities = input.Split(',').Select(double.Parse).ToList();

            // Normalize probabilities to sum to 1
            var sumProbabilities = probabilities.Sum();
            probabilities = probabilities.Select(p => p / sumProbabilities).ToList();

            // Use custom distribution to transform the generated values
            var customValuesByEngine = engines.ToDictionary(
                engine => engine.Name,
                engine => valuesByEngine[engine.Name].Select(v => CustomDistributionTransform(v, probabilities)).ToList()
            );

            // Visualize the distribution of values for each engine
            var chart = new Chart();
            chart.ChartAreas.Add(new ChartArea());

            foreach (var engine in engines)
            {
                var series = new Series(engine.Name);
                series.ChartType = SeriesChartType.Column;
                series.Points.DataBindY(customValuesByEngine[engine.Name]);
                chart.Series.Add(series);
            }

            chart.Width = 800;
            chart.Height = 600;
            chart.Titles.Add("Distribution of Random Values");
            chart.ChartAreas[0].AxisX.Title = "Value";
            chart.ChartAreas[0].AxisY.Title = "Count";

            var form = new Form();
            form.Controls.Add(new ChartControl(chart));
            Application.Run(form);
        }

        // Transform a random value using a custom distribution
        static double CustomDistributionTransform(double value, List<double> probabilities)
        {
            var cumulativeProbabilities = new List<double> { 0 };
            for (int i = 0; i < probabilities.Count; i++)
            {
                cumulativeProbabilities.Add(cumulativeProbabilities[i] + probabilities[i]);
            }

            for (int i = 0; i < probabilities.Count; i++)
            {
                if (value < cumulativeProbabilities[i + 1])
                {
                    return i;
                }
            }

            return probabilities.Count - 1;
        }
    }

    // Interface for a random number generator engine
    interface IRandomNumberGenerator
    {
        string Name { get; }
        List<double> Generate(int numValues);
    }

    // Default random number generator engine
    class DefaultRandomNumberGenerator : IRandomNumberGenerator
    {
        public string Name { get { return "System.Random"; } }

        public List<double> Generate(int numValues)
        {
            var random = new Random();
            return Enumerable.Range(0, numValues).Select(_ => random.NextDouble()).ToList();
        }
    }

    // XorShift random number generator engine
    class XorShiftRandomNumberGenerator : IRandomNumber
public class LCG
    {
        private long m;  // modulus
        private long a;  // multiplier
        private long c;  // increment
        private long x;  // seed (current value)

        public LCG(long m, long a, long c, long seed)
        {
            this.m = m;
            this.a = a;
            this.c = c;
            this.x = seed;
        }

        public long Next(long minValue, long maxValue)
        {
            x = (a * x + c) % m;  // calculate the next value using LCG formula
            long range = maxValue - minValue;
            return minValue + x % range;  // return the next value in the specified range
        }
    }
    LCG lcg = new LCG(2147483648, 1103515245, 12345, DateTime.Now.Ticks);  // create an instance of LCG
    long nextRandom = lcg.Next(0, 100);  // generate the next random number in the range [0, 100)
