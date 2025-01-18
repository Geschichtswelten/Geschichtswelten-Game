using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializedList <T>
{
    public List<T> list;

    public SerializedList(List<T> list)
    {
        this.list = list;
    }
}
