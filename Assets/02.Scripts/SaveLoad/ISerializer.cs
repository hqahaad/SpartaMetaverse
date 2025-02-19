using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISerializer
{
    string Serialize<T>(T t);
    T Deserialize<T>(string json);
}
