namespace Game2048.Tests.TestRandom;

using System;
using System.Collections.Generic;
// using Game2048.Core.Abstractions;
using IRandom = global::Game2048.Core.Abstractions.IRandom;

public sealed class ScriptedRandom : IRandom
{
    private readonly Queue<int> _ints;
    private readonly Queue<double> _doubles;

    public ScriptedRandom(IEnumerable<int>? ints = null, IEnumerable<double>? doubles = null)
    {
        _ints = new Queue<int>(ints ?? Array.Empty<int>());
        _doubles = new Queue<double>(doubles ?? Array.Empty<double>());
    }

    public int Next(int minValue, int maxValue)
    {
        if (_ints.Count == 0)
            throw new InvalidOperationException("ScriptedRandom: no int values left.");
        var raw = _ints.Dequeue();
        var span = maxValue - minValue;
        if (span <= 0) return minValue;
        // нормалізуємо у допустимий діапазон
        return minValue + Math.Abs(raw % span);
    }

    public double NextDouble()
    {
        if (_doubles.Count == 0)
            throw new InvalidOperationException("ScriptedRandom: no double values left.");
        return _doubles.Dequeue();
    }
}
