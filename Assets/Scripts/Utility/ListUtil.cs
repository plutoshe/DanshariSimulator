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
public static class DeepCloneUtil {
    public static T DeepClone<T>(T obj)
    {
        using (var ms = new System.IO.MemoryStream())
        {
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }
}