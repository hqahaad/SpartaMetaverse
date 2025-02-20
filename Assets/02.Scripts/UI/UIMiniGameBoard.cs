using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMiniGameBoard : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI TitleTmp { get; private set; }
    [field: SerializeField] public TextMeshProUGUI DescTmp { get; private set; }
    [field: SerializeField] public TextMeshProUGUI RankingTmp { get; private set; }
}
