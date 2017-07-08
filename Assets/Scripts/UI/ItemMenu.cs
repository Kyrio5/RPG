using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemMenu : Menu {

    public PopulatedMenu subMenu;
    int lastSelection = -1;
    

    public override void MenuUpdate()
    {
        if(selection != lastSelection)
        {
            switch (selection)
            {
                case 0:
                    subMenu.syncInventory(ref GameDatabase.Instance.Inventory);
                    break;
                case 1:
                    subMenu.syncInventory(ref GameDatabase.Instance.KeyInventory);
                    break;
                case 2:
                    subMenu.syncInventory(ref GameDatabase.Instance.Inventory);
                    break;
            }
        }
        lastSelection = selection;
        updatePointer(-1);
        pointer.position = population[selection].transform.GetChild(0).position;
    }

    public override void updatePointer(int direction)
    {
        int movement = 0;
        switch (direction)
        {
            case 2:
                movement = -1;
                break;
            case 3:
                movement = 1;
                break;
            default:
                movement = 0;
                break;
        }
        selection += movement;
        selection = Mathf.Clamp(selection, 0, population.Count - 1);
    }


    public override void Submit()
    {
        switch (selection)
        {
            case 0:
                subMenu.sortable = true;
                PauseController.Instance.PushMenu(subMenu, selection);
                break;
            case 1:
                subMenu.sortable = false;
                PauseController.Instance.PushMenu(subMenu, selection);
                break;
            case 2:
                GameDatabase.SortInv(GameDatabase.Instance.Inventory);
                subMenu.syncInventory(ref GameDatabase.Instance.Inventory);
                break;
        }
    }

    public override void Cancel()
    {
        lastSelection = -1;
        gameObject.SetActive(false);
        PauseController.Instance.PopMenu();
    }
}
