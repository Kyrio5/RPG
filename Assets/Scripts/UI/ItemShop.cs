using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShop : Menu {

    string shopKeeper;
    string shopGreeting;

    public TMPro.TextMeshProUGUI shopKeeperName;
    public TMPro.TextMeshProUGUI shopKeeperGreeting;
    public TMPro.TextMeshProUGUI itemsOwned;
    public TMPro.TextMeshProUGUI itemsEquipped;
    public TMPro.TextMeshProUGUI gilDisplay;
    public List<Item> Inventory;
    public PopulatedMenu subMenu;
    
    PartyPortrait[] party;
    Item compareItem;

    public void InitializeShop(ItemShopNode shop)
    {
        shopKeeper = shop.shopKeeper;
        shopGreeting = shop.shopGreeting;
        Inventory = shop.inventory;
    }

    public override void Submit()
    {
        switch (selection)
        {
            case 0:
                subMenu.buy = true;
                PauseController.Instance.PushMenu(subMenu, selection);
                break;
            case 1:
                subMenu.buy = false;
                PauseController.Instance.PushMenu(subMenu, selection);
                break;
            case 2:
                Cancel();
                break;
        }

        
    }

    void Update()
    {
        shopKeeperName.text = shopKeeper;
        shopKeeperGreeting.text = shopGreeting;

        gilDisplay.text = GameDatabase.Instance.Gil.ToString();
        itemsOwned.text = " --";
        itemsEquipped.text = " --";

        if (selection == 0 || selection == 2)
        {
            subMenu.buy = true;
            subMenu.syncInventory(ref Inventory);
        }
        else if (selection == 1)
        {
            subMenu.buy = false;
            List<Item> MyItems = new List<Item>();
            foreach (Item x in GameDatabase.Instance.Inventory)
            {
                if (x.id < 5000)
                    MyItems.Add(x);
            }
            subMenu.syncInventory(ref MyItems);
        }

        if (PauseController.Instance.MenuExecutionStack.Peek() == subMenu)
        {
            compareItem = ((ListItem)subMenu.population[subMenu.selection]).containedItem;
        }
        else if(PauseController.Instance.MenuExecutionStack.Peek() == subMenu.quantizer)
        {
            compareItem = subMenu.quantizer.myItem;
        }
        else
        {
            compareItem = null;
        }

        if (compareItem != null)
        {
            int owned = 0;
            foreach (Item x in GameDatabase.Instance.Inventory)
            {
                if (x.id == compareItem.id)
                {
                    owned += x.quantity;
                }
            }
            itemsOwned.text = owned.ToString();
            

            if (compareItem is EquipItem)
            {
                int equipped = 0;
                foreach (Character x in GameDatabase.Instance.CurrentAvailableParty)
                {
                    if (x != null && x.Name != "")
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Item y = x.Inventory[i];
                            if (y != null && y.id == compareItem.id)
                            {
                                equipped += y.quantity;
                            }
                        }
                    }
                }
                itemsEquipped.text = equipped.ToString();

                int slotNumber = -1;
                if (((EquipItem)compareItem).equipType == EquipItem.EquipType.ACCESSORY)
                {
                    slotNumber = 4;
                }
                


                if (slotNumber < 0)
                {
                    foreach (Character x in GameDatabase.Instance.CurrentAvailableParty)
                    {
                        if (x != null && x.Name != "")
                        {
                            if (((EquipItem)compareItem).equipType == EquipItem.EquipType.ARROW ||
                                ((EquipItem)compareItem).equipType == EquipItem.EquipType.BULLET)
                            {
                                slotNumber = (x.hand == Character.Handedness.RIGHT) ? 1 : 0;
                            }
                            else {
                                foreach (EquipItem.EquipType y in x.WeaponTypes)
                                {
                                    if (((EquipItem)compareItem).equipType == y)
                                    {
                                        slotNumber = (x.hand == Character.Handedness.RIGHT) ? 0 : 1;
                                        break;
                                    }
                                }
                            }
                            if (slotNumber >= 0)
                            {
                                break;
                            }

                            foreach (EquipItem.EquipType y in x.ArmorTypes)
                            {
                                if (((EquipItem)compareItem).equipType == y)
                                {
                                    slotNumber = 2;
                                    break;
                                }
                            }

                            if (slotNumber >= 0)
                            {
                                break;
                            }
                            foreach (EquipItem.EquipType y in x.HeadTypes)
                            {
                                if (((EquipItem)compareItem).equipType == y)
                                {
                                    slotNumber = 3;
                                    break;
                                }
                            }

                            if (slotNumber >= 0)
                            {
                                break;
                            }
                        }
                        else
                        {
                            continue;
                        }

                        //test item stats
                        if(slotNumber >= 0)
                        {
                            int[] newStats=x.NewItemStats(slotNumber, (EquipItem)compareItem);
                            int[] oldStats = x.FullStats(-1);
                            int newSum = 0;  foreach(int i in newStats) { newSum += i; }
                            int oldSum = 0; foreach (int i in oldStats) { oldSum += i; }
                            //display result

                        }
                    }
                }
            }
        }

    }

    public override void MenuUpdate()
    {
        updatePointer(-1);
        pointer.position = population[selection].transform.position;
    }

    public override void Cancel()
    {
        PauseController.Instance.UnpauseGame();
        MenuRoot.gameObject.SetActive(false);

    }


}
