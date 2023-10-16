using UnityEngine;
using System.Collections.Generic;

public static class UtilityExtensions
{
    public static T[] GetComponentsOnlyInChildren<T>(this Transform transform) where T: Component
    {
        List<T> group = new List<T>();

        T[] allChildren = transform.GetComponentsInChildren<T>();
        foreach (T child in allChildren)
        {
            if(child.transform.parent == transform)
                group.Add(child);
        }

        return group.ToArray();
    }
}