using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostNode : ActionNode
{

    bool UseGil;
    int itemID;
    int itemQuantity;
    int gilCost;

    public CostNode(bool giluse, int id, int quant, int gil, int[] indices):
        base(false, indices)
    {
        UseGil = giluse;
        itemID = id;
        itemQuantity = quant;
        gilCost = gil;
    }

    public bool GetCost()
    {
        if (UseGil)
        {
            if(GameDatabase.Instance.Gil >= gilCost)
            {
                GameDatabase.Instance.Gil -= gilCost;
                return true;
            }

            return false;
        }

        bool fail = false;
        int collection;
        collection = 0;
        foreach(Item x in GameDatabase.Instance.Inventory)
        {
            if(x.id == itemID)
            {
                collection += Mathf.Min(x.quantity, itemQuantity);
                if (collection == itemQuantity)
                {
                    break;
                }
            }
        }
        fail = collection < itemQuantity;

        if (fail)
        {
            return false;
        }
        
        for (int i = 0; i < GameDatabase.Instance.Inventory.Count; i++)
        { 
            Item x = GameDatabase.Instance.Inventory[i];
            if(x.id == itemID)
            {
                x.quantity -= Mathf.Min(x.quantity, itemQuantity);
                collection -= Mathf.Min(x.quantity, itemQuantity);
            }
            if(x.quantity == 0)
            {
                GameDatabase.Instance.Inventory[i] = GameDatabase.Instance.ItemDatabase[0];
            }
            if(collection == 0)
            {
                break;
            }
        }
        return true;


    }

}

