using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chances", menuName = "Add Chance Set")]
public class ChanceSet : ScriptableObject
{
    [ContextMenuItem("ResetChances", "ResetChances")]
    [ContextMenuItem("BalanceChances", "BalanceChances")]
    public ChanceItem[] set;


    public ChanceSet(int minInclusive, int maxInclusive)
    {
        int amount = maxInclusive - minInclusive;
        set = new ChanceItem[amount];
        float splitChance = 100f / amount;
        for (int i = minInclusive; i <= maxInclusive; i++)
        {
            set[i] = new ChanceItem(i, splitChance, false);
        }
    }
    public int RandomValue()
    {
        if (set is null)
        {
            Debug.LogError("set has not been initialized");
            return 0;
        }

        int index = -1;
        float rand = Random.Range(0f, 100f);
        while (rand >= 0)
        {
            rand -= set[++index].percentChance;

            if (index == set.Length)
            {
                index--;
                Debug.LogWarning("total of chances is too low");
                break;
            }
        }
        return set[index].value;
    }



    public void ResetChances()
    {
        float splitChance = 100f / set.Length;
        for (int i = 0; i < set.Length; i++)
        {
            set[i].percentChance = splitChance;
        }
    }
    public void BalanceChances()
    {
        float total = 0;
        List<ChanceItem> nonPreservedItems = new List<ChanceItem>();
        foreach (ChanceItem item in set)
        {
            total += item.percentChance;

            if (!item.perserveChance) //if this item's chance can be changed, add to list
                nonPreservedItems.Add(item);
        }
        if (nonPreservedItems.Count == 0)
            return; //cannot balance chances, every item's chance is preserved 

        //distribute the difference to each item, so that the total of all chances will be 100
        total -= 100f;
        float splitDiff = total / nonPreservedItems.Count;
        foreach (ChanceItem item in nonPreservedItems)
        {
            item.percentChance -= splitDiff;
        }
    }

}
