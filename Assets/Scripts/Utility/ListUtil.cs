using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    public static void AddToFront<T>(this List<T> list, T item)
    {
        // omits validation, etc.
        list.Insert(0, item);
    }

    public static T Last<T>(this List<T> list)
    {
        if (list.Count > 0)
            return list[list.Count - 1];
        return default(T);
    }
}
