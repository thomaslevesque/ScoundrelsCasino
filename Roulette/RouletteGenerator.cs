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
        private readonly List<int> _lastIndexes = new List<int>();
        private readonly IDictionary<int, int> _lastOccurrences =
            Enumerable.Range(0, _numbers.Length).ToDictionary(i => i, i => -1);

        private int _turn;

        public RouletteGenerator(Random random = null)
        {
            _random = random ?? new Random();
        }


        public string Next()
        {
            // Score depends on how long ago the index has last been generated
            // more recent = lower score, less recent = higher score
            // Scores start at 1 (0 - (-1))
            var indexScores = Enumerable.Range(0, _numbers.Length)
                                .ToDictionary(i => i, i => _turn - _lastOccurrences[i]);

            // Repeat each index {score} times
            var indexes =
                Enumerable.Range(0, _numbers.Length)
                          .SelectMany(i => Enumerable.Repeat(i, indexScores[i]))
                          .ToArray();

            int index;
            do
            {
                int indexIndex = _random.Next(indexes.Length);
                index = indexes[indexIndex];
            } while (_turn >= LongestRunAllowed && _lastIndexes.All(i => i == index)); // avoid long runs of identical numbers

            _lastOccurrences[index] = _turn;
            _lastIndexes.Add(index);
            while (_lastIndexes.Count > LongestRunAllowed)
                _lastIndexes.RemoveAt(0);
            _turn++;

            return _numbers[index];
        }
    }
}
