using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extra methods to help with Lists
/// </summary>
/// <author>James Veugelaers</author>
public static class ListExtensions
{
    public static string ToString<T>(this List<T> obj)
    {
        if (obj == null)
        {
            return "";
        }

        string result = "";
        for (int i = 0; i < obj.Count; i++)
        {
            if (i > 0)
            {
                result += ", ";
            }

            T item = obj[i];
            result += item.ToString();
        }
        return "[" + result + "]";
    }

    public static void Shuffle<T>(this List<T> alpha)
    {
        for (int i = 0; i < alpha.Count; i++)
        {
            T temp = alpha[i];
            int randomIndex = UnityEngine.Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
    }

    public static bool IsEmpty<T>(this List<T> list)
    {
        return list == null || list.Count == 0;
    }

    public static T RandomElement<T>(this List<T> list)
    {
        if (list.IsEmpty())
        {
            return default(T);
        }

        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T Back<T>(this List<T> list)
    {
        if (list.IsEmpty())
        {
            return default(T);
        }

        return list[list.Count-1];
    }

    public static T Front<T>(this List<T> list)
    {
        if (list.IsEmpty())
        {
            return default(T);
        }

        return list[0];
    }
}
