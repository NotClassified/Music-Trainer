using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChanceItem
{
    public int value;
    public float percentChance;
    public bool perserveChance;

    public ChanceItem(int value, float percentChance, bool perserveChance)
    {
        this.value = value;
        this.percentChance = percentChance;
        this.perserveChance = perserveChance;
    }
}
