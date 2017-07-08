using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopNode : ActionNode {

    public string shopKeeper;
    public string shopGreeting;
    public List<Item> inventory;
    

    public ItemShopNode(string name, string message, List<Item> inventory, int[] indices, bool exit = false):
        base(exit, indices)
    {
        shopKeeper = name;
        shopGreeting = message;
        this.inventory = inventory;
        GameDatabase.SortInv(this.inventory);
    }

    public ItemShopNode(string name, string message, int[] itemIndecies, int[] indices, bool exit = false):
        base(exit, indices)
    {
        shopGreeting = message;
        shopKeeper = name;
        inventory = new List<Item>();
        for(int i = 0; i < itemIndecies.Length; i++)
        {
            if(GameDatabase.Instance.ItemDatabase[itemIndecies[i]] is EquipItem)
            {
                inventory.Add(new EquipItem((EquipItem)GameDatabase.Instance.ItemDatabase[itemIndecies[i]]));
            }
            else if(GameDatabase.Instance.ItemDatabase[itemIndecies[i]] is UseItem)
            {

                inventory.Add(new UseItem((UseItem)GameDatabase.Instance.ItemDatabase[itemIndecies[i]]));
            }
            else
            {
                inventory.Add(new Item(GameDatabase.Instance.ItemDatabase[itemIndecies[i]]));
            }
        }
    }

    public void Open()
    {
        PauseController.Instance.ItemShop(this);
    }
}
