using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlotMenu : Menu {

    public EquipMenu myParent;
    public bool remove = false;
    int lastSelection = -1;
    List<Item> refinedList;
    public void Awake()
    {
        myParent = transform.parent.GetComponent<EquipMenu>();
    }

    public override void Submit()
    {
        if (remove)
        {
            if (myParent.myCharacter.Inventory[selection].id < 5000)
            {
                if(((EquipItem)myParent.myCharacter.Inventory[selection]).TwoHanded)
                {
                    if(myParent.myCharacter.Inventory[Mathf.Abs(selection - 1)].id < 5000)
                    {
                        if (GameDatabase.Instance.FindSpace(myParent.myCharacter.Inventory[Mathf.Abs(selection - 1)], myParent.myCharacter.Inventory[selection].quantity))
                        {
                            GameDatabase.Instance.AddItem(myParent.myCharacter.Inventory[Mathf.Abs(selection - 1)], myParent.myCharacter.Inventory[Mathf.Abs(selection - 1)].quantity);
                            myParent.myCharacter.Inventory[Mathf.Abs(selection - 1)] = new Item(GameDatabase.Instance.ItemDatabase[0]);

                            if (GameDatabase.Instance.FindSpace(myParent.myCharacter.Inventory[selection], myParent.myCharacter.Inventory[selection].quantity))
                            {
                                GameDatabase.Instance.AddItem(myParent.myCharacter.Inventory[selection], myParent.myCharacter.Inventory[selection].quantity);
                                myParent.myCharacter.Inventory[selection] = new Item(GameDatabase.Instance.ItemDatabase[0]);
                            }
                        }
                    }
                    else
                    {
                        if (GameDatabase.Instance.FindSpace(myParent.myCharacter.Inventory[selection], myParent.myCharacter.Inventory[selection].quantity))
                        {
                            GameDatabase.Instance.AddItem(myParent.myCharacter.Inventory[selection], myParent.myCharacter.Inventory[selection].quantity);
                            myParent.myCharacter.Inventory[selection] = new Item(GameDatabase.Instance.ItemDatabase[0]);
                        }
                    }
                }
                else
                {
                    if (GameDatabase.Instance.FindSpace(myParent.myCharacter.Inventory[selection], myParent.myCharacter.Inventory[selection].quantity))
                    {
                        GameDatabase.Instance.AddItem(myParent.myCharacter.Inventory[selection], myParent.myCharacter.Inventory[selection].quantity);
                        myParent.myCharacter.Inventory[selection] = new Item(GameDatabase.Instance.ItemDatabase[0]);
                    }
                }
                if (GameDatabase.Instance.FindSpace(myParent.myCharacter.Inventory[selection], myParent.myCharacter.Inventory[selection].quantity))
                {
                    GameDatabase.Instance.AddItem(myParent.myCharacter.Inventory[selection], myParent.myCharacter.Inventory[selection].quantity);
                    myParent.myCharacter.Inventory[selection] = new Item(GameDatabase.Instance.ItemDatabase[0]);
                }
            }
        }
        else
        {
            lastSelection = -1;
            if(refinedList.Count > 0)
                PauseController.Instance.PushMenu(myParent.EquipPopMenu, selection);
        }
    }

    public override void MenuUpdate()
    {
        refinedList = new List<Item>();

        if (!pointer.gameObject.activeSelf)
        {
            pointer.gameObject.SetActive(true);
        }

        if (myParent.myCharacter != null)
        {
            if (selection != lastSelection)
            {
                refinedList = myParent.myCharacter.refineInventory(selection);
                myParent.EquipPopMenu.syncInventory(ref refinedList);
            }
        }
        updatePointer(-1);
        pointer.position = population[selection].transform.position;
    }

    public override void Cancel()
    {

        pointer.gameObject.SetActive(false);
        PauseController.Instance.PopMenu();
        myParent.EquipPopMenu.ClearInventory();

    }
}
