namespace Game2048.Core.Abstractions;

public interface IRandom
{
    int Next(int minValue, int maxValue);
	double NextDouble();
}