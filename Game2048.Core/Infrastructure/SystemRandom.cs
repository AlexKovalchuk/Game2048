using Game2048.Core.Abstractions;
namespace Game2048.Core.Infrastructure;

public class SystemRandom : IRandom
{
    private readonly Random _randomiser;
    public SystemRandom(int? seed = null)
    {
        _randomiser = seed.HasValue ? new Random(seed.Value) : new Random();
    }
    public int Next(int minValue, int maxValue)
    {
        return _randomiser.Next(minValue, maxValue);
    }

    public double NextDouble()
    {
        return _randomiser.NextDouble();
    }
}