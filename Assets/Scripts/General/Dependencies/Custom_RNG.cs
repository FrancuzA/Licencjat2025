using System;

public static class Custom_RNG
{
    private static Random random;

    public static void Init(int seed = -1)
    {
        if (seed == -1)
            seed = DateTime.Now.Millisecond;

        random = new Random(seed);
    }

    /// <summary>
    /// Zwraca losow¹ liczbê zmiennoprzecinkow¹ z przedzia³u [min, max)
    /// </summary>
    public static float Range(float min, float max)
    {
        if (random == null) Init();
        return (float)(random.NextDouble() * (max - min) + min);
    }

    /// <summary>
    /// Zwraca losow¹ liczbê ca³kowit¹ z przedzia³u [min, max) (max wy³¹czone)
    /// </summary>
    public static int Range(int min, int max)
    {
        if (random == null) Init();
        return random.Next(min, max);
    }

    public static float NextFloat(float min, float max)
    {
        return Range(min, max);
    }
}