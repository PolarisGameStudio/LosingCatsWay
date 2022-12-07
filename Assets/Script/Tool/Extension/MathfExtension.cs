using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathfExtension
{
    #region Clamp

    public static bool IsBetween(float value, float min, float max)
    {
        return value > min && value < max;
    }

    /// <summary>
    /// min = Inclusive, max = Inclusive
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min">Inclusive</param>
    /// <param name="max">Inclusive</param>
    /// <returns></returns>
    public static bool IsBetweenEqual(float value, float min, float max)
    {
        return value >= min && value <= max;
    }

    /// <summary>
    /// min = Exclusive, max = Inclusive
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min">Exclusive</param>
    /// <param name="max">Inclusive</param>
    /// <returns></returns>
    public static bool IsBetweenEqualMax(float value, float min, float max)
    {
        return value > min && value <= max;
    }

    /// <summary>
    /// min = Inclusive, max = Exclusive
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min">Inclusive</param>
    /// <param name="max">Exclusive</param>
    /// <returns></returns>
    public static bool IsBetweenEqualMin(float value, float min, float max)
    {
        return value >= min && value < max;
    }

    #endregion

    #region Closest

    /// <summary>
    /// Insert value and return the closest one value between min and max.
    /// Return max when value in middle point.
    /// </summary>
    /// <param name="value">Current value.</param>
    /// <param name="min">Smallest value.</param>
    /// <param name="max">Biggest value.</param>
    /// <returns></returns>
    public static float ToClosestValue(float value, float min, float max)
    {
        float midPoint = (min + max) / 2;
        if (value >= midPoint)
            return max;
        else
            return min;
    }

    /// <summary>
    /// Return closest value with list of value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static float ToClosestValue(float value, List<float> values)
    {
        float min = 0;
        float max = 0;

        for (int i = 0; i < values.Count; i++)
        {
            if (value < values[i]) continue;
            else if (value >= values[i])
            {
                min = values[i];
                break;
            }
        }

        for (int i = 0; i < values.Count; i++)
        {
            if (value > values[i]) continue;
            else if (value <= values[i])
            {
                max = values[i];
                break;
            }
        }

        return ToClosestValue(value, min, max);
    }

    #endregion

    //?H?????v??
    public static int RandomRate(int[] rate, int total)
    {
        int r = Random.Range(1, total + 1);
        int t = 0;
        for (int i = 0; i < rate.Length; i++)
        {
            t += rate[i];

            if (r < t)
                return i;
        }
        return 0;
    }

    public static T GetRandom<T>(this IList<T> list)
    {
        if (list.Count <= 0)
            return default;

        return list[Random.Range(0, list.Count)];
    }

    public static void GetNumberRangeByTen(int value, out int start, out int end)
    {
        start = Mathf.FloorToInt(value / 10f) * 10 + 1;
        end = start + 9;
    }
}
