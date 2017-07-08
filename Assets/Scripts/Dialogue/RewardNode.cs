using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardNode : ActionNode {

    public int[] itemIDs;
    public Item[] itemReward;
    public int gilReward;

    public RewardNode(int[] items, int gil, int[] indices, bool exit = true) : base(exit, indices)
    {
        itemIDs = items;
        gilReward = gil ;
        getItemsFromIDs();
    }

    public void getItemsFromIDs()
    {
        List<Item> collectItems = new List<Item>();

        for(int i = 0; i < itemIDs.Length; i++)
        {
            Item x = GameDatabase.Instance.ItemDatabase[itemIDs[i]];
            if(x is UseItem)
            {
                collectItems.Add(new UseItem((UseItem)x));
            }
            else if(x is EquipItem)
            {
                collectItems.Add(new EquipItem((EquipItem)x));
            }
            else
            {
                collectItems.Add(new Item(x));
            }
        }

        itemReward = collectItems.ToArray();

    }



}
