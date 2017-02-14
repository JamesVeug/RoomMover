/// <summary>
/// Extra Math methods
/// </summary>
/// <author>James Veugelaers</author>
public class Mathc
{
    public const int Version = 1;

    /// <summary>
    /// Wraps the values between min and max.
    /// If Value is >= max. Will return Min. If Value is less than min will return max-1
    /// </summary>
    /// <returns></returns>
    public static float Wrap(float value, float min, float max)
    {
        if (value >= max)
        {
            return min;
        }
        else if (value < min)
        {
            return max - 1;
        }

        return value;
    }
}