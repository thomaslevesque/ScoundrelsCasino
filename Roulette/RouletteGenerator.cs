using System;
using System.Collections.Generic;
using System.Linq;

namespace Roulette
{
    class RouletteGenerator
    {
        // Treat numbers as strings, to distinguish between 0 and 00
        private static readonly string[] _numbers =
            "0-28-9-26-30-11-7-20-32-17-5-22-34-15-3-24-36-13-1-00-27-10-25-29-12-8-19-31-18-6-21-33-16-4-23-35-14-2"
            .Split('-');

        private const int LongestRunAllowed = 2;

        private readonly Random _random;
        private readonly List<string> _lastNumbers = new List<string>();
        private readonly IDictionary<string, int> _lastOccurrences = _numbers.ToDictionary(i => i, i => -1);

        private int _turn;

        public RouletteGenerator(Random random = null)
        {
            _random = random ?? new Random();
        }


        public string Next()
        {
            // Score depends on how long ago the number has last been generated
            // more recent = lower score, less recent = higher score
            // Scores start at 1 (0 - (-1))
            var numberScores = _numbers.ToDictionary(i => i, i => _turn - _lastOccurrences[i]);

            // Repeat each number {score} times
            var weightedNumbers = _numbers
                .SelectMany(i => Enumerable.Repeat(i, numberScores[i]))
                .ToArray();

            string number;
            do
            {
                int index = _random.Next(weightedNumbers.Length);
                number = weightedNumbers[index];
            } while (_turn >= LongestRunAllowed && _lastNumbers.All(i => i == number)); // avoid long runs of identical numbers

            _lastOccurrences[number] = _turn;
            _lastNumbers.Add(number);
            while (_lastNumbers.Count > LongestRunAllowed)
                _lastNumbers.RemoveAt(0);
            _turn++;

            return number;
        }
    }
}
