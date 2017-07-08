using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EquipMenu : Menu {

    [HideInInspector]
    public Character myCharacter = null;


    public CharacterEquipSlot CharDisplay;
    public EquipSlotMenu EquipInventory;
    public PopulatedMenu EquipPopMenu;

    public TMPro.TextMeshProUGUI newStats;


    public void GetNewStats()
    {
        string statstext = "";
        int[] oldstats = new int[7];
        int[] newstats = new int[7];
        if(PauseController.Instance.MenuExecutionStack.Peek() == EquipPopMenu)
        {
            statstext = "";
            EquipItem itemPeek = ((ListItem)EquipPopMenu.population[EquipPopMenu.selection]).containedItem as EquipItem;
            oldstats = myCharacter.FullStats(-1);
            if (itemPeek != null)
            {
                newstats = myCharacter.NewItemStats(PauseController.PeekSelection(), itemPeek);

                for (int i = 0; i < 7; i++)
                {
                    if (oldstats[i] < newstats[i])
                    {
                        statstext += "<sprite=61>\t" + newstats[i] + "\n";
                    }
                    else if (oldstats[i] > newstats[i])
                    {
                        statstext += "<sprite=62>\t" + newstats[i] + "\n";
                    }
                    else
                    {
                        statstext += "\n";
                    }
                }
                newStats.text = statstext;
            }
        }
        else if(PauseController.Instance.MenuExecutionStack.Peek() == EquipInventory && EquipInventory.remove)
        {
            statstext = "";
            oldstats = myCharacter.FullStats(-1);
            newstats = myCharacter.FullStats(EquipInventory.selection);
            for (int i = 0; i < 7; i++)
            {
                if (oldstats[i] < newstats[i])
                {
                    statstext += "<sprite=61>\t" + newstats[i] + "\n";
                }
                else if (oldstats[i] > newstats[i])
                {
                    statstext += "<sprite=62>\t" + newstats[i] + "\n";
                }
                else
                {
                    statstext += "\n";
                }
            }
            newStats.text = statstext;
        }
        else
        {
            newStats.text = "";
        }
    }


    public void OnEnable()
    {

        CharDisplay.setCharacter(myCharacter);
    }

    public override void MenuUpdate()
    {
        
        base.MenuUpdate();
        
    }


    public override void Submit()
    {
        switch (selection)
        {
            case 0:
                Equip(false);
                break;
            case 1:
                Equip(true);
                break;
            case 2:
                Optimize();
                break;
        }

    }

    public void Equip(bool remove)
    {
        if (remove)
        {
            EquipInventory.remove = true;
            PauseController.Instance.PushMenu(EquipInventory, selection);
        }
        else
        {

            EquipInventory.remove = false;
            PauseController.Instance.PushMenu(EquipInventory, selection);
        }
    }

    public void Optimize()
    {
        List<Item> testEquips;
        EquipItem[] strongest = new EquipItem[5];
        int lastSum;
        int i = 0;
        lastSum = 0;
        testEquips = myCharacter.refineInventory(i);
        if (myCharacter.Inventory[i] is EquipItem)
            testEquips.Add(myCharacter.Inventory[i]);

        foreach(int j in myCharacter.FullStats(-1))
        {
            lastSum += j;
        }

        for(i = (myCharacter.hand == Character.Handedness.LEFT)?1:0; i < 5; i++)
        {
            lastSum = 0;
            testEquips = new List<Item>();
            testEquips = myCharacter.refineInventory(i);
            if(myCharacter.Inventory[i] is EquipItem)
            testEquips.Add(myCharacter.Inventory[i]);

            foreach (int j in myCharacter.FullStats(-1))
                lastSum += j;


            foreach (Item x in testEquips)
            {
                if (x is EquipItem)
                {
                    int sum = 0;
                    int[] stats = myCharacter.NewItemStats(i, (EquipItem)x);
                    foreach (int j in stats)
                    {
                        sum += j;
                    }

                    if (sum > lastSum)
                    {
                        strongest[i] = (EquipItem)x;
                        lastSum = sum;
                    }
                }
            }
            if (i < 2)
            {
                i = 1;
            }
               
        }
        for (i = 0; i < 5; i++)
        {
            if (strongest[i] != null && strongest[i] is EquipItem && strongest[i] != myCharacter.Inventory[i])
                myCharacter.equipItem(strongest[i], i);
        }
    }

    public override void Cancel()
    {
        myCharacter = null;
        gameObject.SetActive(false);
        PauseController.Instance.PopMenu();

    }

    public void LateUpdate()
    {
        GetNewStats();
    }

}
