using Mono.Cecil.Cil;
using UnityEngine;

public class Formatter
{
    /// <summary>
    /// 정수 값을 K, M 등으로 단축한 문자열을 반환
    /// 1000 => 1.0K
    /// </summary>
    /// <param name="value">큰 정수 값</param>
    /// <returns>소수 첫째자리까지의 문자열</returns>
    public static string ShortenInteger(long value)
    {
        long overPoint = 0;
        long underPoint = 0;

        // T
        if (value >= 1000_000_000_000)
        {
            value /= 100_000_000_000;

            underPoint = value % 10;
            overPoint = value / 10;

            return $"{overPoint}.{underPoint}B";
        }

        // B
        if (value >= 1000_000_000)
        {
            value /= 100_000_000;

            underPoint = value % 10;
            overPoint = value / 10;

            return $"{overPoint}.{underPoint}B";
        }

        // M
        if(value >= 1000_000)
        {
            value /= 100_000;

            underPoint = value % 10;
            overPoint = value / 10;

            return $"{overPoint}.{underPoint}M";
        }

        // K
        if(value >= 1000)
        {
            value /= 100;

            underPoint = value % 10;
            overPoint = value / 10;

            return $"{overPoint}.{underPoint}K";
        }

        return value.ToString();
    }
}
