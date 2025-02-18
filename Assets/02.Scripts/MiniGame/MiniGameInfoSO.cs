using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGameSO", menuName = "Custom/SO/MiniGameSO")]
public class MiniGameInfoSO : ScriptableObject
{
    public string gameName;
    [TextArea] 
    public string gameDescription;
}
