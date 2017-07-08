using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulatedMenu : Menu {

    public enum Context
    {
        PAUSE_MENU, BATTLE, SHOP
    };


    [Header("UI Elements")]
    RectTransform rectTransform;
    public RectTransform listContentPanel;
    public GameObject elementPrefab;
    public Transform pointerTemp;
    public Transform moreDown, moreUp;
    public QuantitySelection quantizer;

    [Header("Properties")]
    public int maxInventory;

    public Context context;
    public bool sortable;
    public bool extendPastEnd=true;

    public int realItems = 0;
    int tempSelection = -1;
    int startIndex = 0;
    public int numRows;
    public int numCols;

    public bool buy = false;

    
    // Use this for initialization
    void Awake() {
        rectTransform = (RectTransform)transform;
        population = new List<MenuSelectable>();
        for (int i = 0; i < maxInventory; i++)
        {
            ListItem newItem = Instantiate(elementPrefab, listContentPanel).GetComponent<ListItem>();
            if(context == Context.SHOP)
            {
                newItem.shop = true;
            }
            population.Add(newItem);
        }

        UpdateScreenPos();
    }

    public void syncInventory(ref List<Item> populate)
    {
        realItems = 0;
        int i = 0;

        for (i = 0; i < population.Count; i++)
        {
            if (context == Context.SHOP)
            {
                ((ListItem)population[i]).sell = !buy;
            }
        }

        i = 0;

        if (populate != null)
        {
            for (i = 0; i < populate.Count; i++)
            {
                if (i >= population.Count)
                {
                    break;
                }
                else {
                    if (populate[i] != null)
                    {
                        population[i].GetComponent<ListItem>().containedItem = populate[i];
                        realItems++;
                    }
                    else
                    {
                        population[i].GetComponent<ListItem>().containedItem = new Item(GameDatabase.Instance.ItemDatabase[0]);
                    }

                    population[i].GetComponent<ListItem>().UpdateDisplay();
                }
            }
        }
        for (; i < population.Count; i++)
        {
            population[i].GetComponent<ListItem>().containedItem = new Item(GameDatabase.Instance.ItemDatabase[0]);
            population[i].GetComponent<ListItem>().UpdateDisplay();
        }

    }

    public void ReSync()
    {
        for (int i = 0; i < population.Count; i++)
        {
            if (context == Context.SHOP)
            {
                ((ListItem)population[i]).sell = !buy;
            }
            population[i].GetComponent<ListItem>().UpdateDisplay();
        }
    }

    public void ClearInventory()
    {
        List<Item> empty = new List<Item>();
        syncInventory(ref empty);
    }

    public override string showDescription()
    {
        if (((ListItem)population[selection]).containedItem is UseItem)
            return base.showDescription();
        else if (((ListItem)population[selection]).containedItem is Ability)
        {
            if (((Ability)((ListItem)population[selection]).containedItem).Learned)
            {
                return base.showDescription();
            }
            else
                return "";
        }

        return base.showDescription();

    }

    public void returnUpdate(int selection)
    {
        ((ListItem)population[selection]).UpdateDisplay();
    }

    public override void MenuUpdate()
    {
        if (!MenuRoot.gameObject.activeInHierarchy)
        {
            MenuRoot.gameObject.SetActive(true);
        }
        if (!pointer.gameObject.activeSelf)
        {
            pointer.gameObject.SetActive(true);
            ReSync();
        }

        base.MenuUpdate();
    }

    public override void Submit()
    {
        if (sortable && tempSelection < 0)
        {
            tempSelection = selection;
            pointerTemp.gameObject.SetActive(true);
            pointerTemp.position = population[tempSelection].transform.GetChild(0).position;

        }
        else if (sortable && tempSelection >= 0)
        {
            if (selection != tempSelection)
            {
                if (((ListItem)population[selection]).containedItem != null && ((ListItem)population[tempSelection]).containedItem != null)
                {
                    if (((ListItem)population[selection]).containedItem.getName() == ((ListItem)population[tempSelection]).containedItem.getName())
                    {
                        MergeSlots(selection, tempSelection);
                    }
                    else
                    {
                        SwapSlots(selection, tempSelection);
                    }
                }
                else {
                    SwapSlots(selection, tempSelection);
                }
            }
            else
            {

                SelectItem();
            }
            tempSelection = -1;
            pointerTemp.gameObject.SetActive(false);
        }
        else
        {
            SelectItem();
        }
    }

    public void SwapSlots(int i, int j)
    {
        Item temp = GameDatabase.Instance.Inventory[i];
        GameDatabase.Instance.Inventory[i] = GameDatabase.Instance.Inventory[j];
        GameDatabase.Instance.Inventory[j] = temp;
        ((ListItem)population[i]).containedItem = GameDatabase.Instance.Inventory[i];
        ((ListItem)population[j]).containedItem = GameDatabase.Instance.Inventory[j];
        ((ListItem)population[i]).UpdateDisplay();
        ((ListItem)population[j]).UpdateDisplay();
    }

    public void MergeSlots(int i, int j)
    {

        if (((ListItem)population[i]).containedItem.quantity < 99)
        {
            int need = 99 - ((ListItem)population[i]).containedItem.quantity;
            if (((ListItem)population[j]).containedItem.quantity < need)
            {
                ((ListItem)population[i]).containedItem.quantity += ((ListItem)population[j]).containedItem.quantity;
                ((ListItem)population[j]).containedItem.quantity = 0;
            }
            else
            {
                ((ListItem)population[i]).containedItem.quantity += need;
                ((ListItem)population[j]).containedItem.quantity -= need;
            }
        }
        ((ListItem)population[i]).UpdateDisplay();
        ((ListItem)population[j]).UpdateDisplay();
    }

    public void SelectItem()
    {
        pointer.gameObject.SetActive(false);
        Item item = ((ListItem)population[selection]).containedItem;
        if (context == Context.PAUSE_MENU)
        {
            if (item != null)
            {
                if (item is UseItem && ((ListItem)population[selection]).usable)
                {
                    if (((UseItem)item).targets == UseItem.Target.ALLALLY || ((UseItem)item).targets == UseItem.Target.ALL)
                    {
                        ((PartyPanel)PauseController.Instance.Menus[1]).All = true;
                    }
                    ((PartyPanel)PauseController.Instance.Menus[1]).intendedOp = ((UseItem)item).Activate;
                    PauseController.Instance.PushMenu(PauseController.Instance.Menus[1], selection);
                }
                else if (item is Ability && ((ListItem)population[selection]).usable)
                {
                    ((PartyPanel)PauseController.Instance.Menus[1]).intendedOp = ((Ability)item).Activate;
                    PauseController.Instance.PushMenu(PauseController.Instance.Menus[1], selection);
                }
                else if (item is EquipItem && ((ListItem)population[selection]).usable)
                {
                    if (item.id < 5000)
                    {
                        equipItem((EquipItem)item, PauseController.PeekSelection());
                        Cancel();
                    }
                }

            }
        }
        else if(context == Context.SHOP)
        {
            if (((ListItem)population[selection]).usable)
            {
                if (buy)
                {

                    quantizer.sell = false;

                }
                else
                {

                    quantizer.sell = true;

                }
                quantizer.myItem = ((ListItem)population[selection]).containedItem;
                PauseController.Instance.PushMenu(quantizer, selection);
            }
        }
        else if(context == Context.BATTLE)
        {
            if(GameDatabase.Instance.lastbattleResult == 0)
            {
                BattleController.Instance.Spoils.Remove(BattleController.Instance.Spoils[selection]);
                syncInventory(ref BattleController.Instance.Spoils);
                if(BattleController.Instance.Spoils.Count == 0)
                {
                    Cancel();
                }
                updatePointer(-1);
            }
            else
            {
                if (((ListItem)population[selection]).containedItem is UseItem)
                {
                    BattleController.Instance.SelectTargets();
                    ((TargetSelection)BattleController.Instance.MenuExecutionStack.Peek()).AllAvailable = false;
                    ((TargetSelection)BattleController.Instance.MenuExecutionStack.Peek()).selection = 0;
                    ((TargetSelection)BattleController.Instance.MenuExecutionStack.Peek()).targets = BattleController.Instance.Party;
                    
                    ItemAction action = new ItemAction(BattleController.Instance.Party[BattleController.Instance.CurrentActiveMember], null, ((UseItem)(((ListItem)population[selection]).containedItem)));
                    ((TargetSelection)BattleController.Instance.MenuExecutionStack.Peek()).battleAction = action;
                    MenuRoot.gameObject.SetActive(false);
                }
            }
        }
    }

    public void equipItem(EquipItem item, int slotIndex)
    {
        PauseController.getEquipMenu().myCharacter.equipItem(item, slotIndex);
    }

    public void equipItem(EquipItem item)
    {
        EquipItem newItem = new EquipItem(item);
        Dictionary<int, EquipItem> holdItems = new Dictionary<int, EquipItem>();
        bool ammoSafe = false;
        bool safeToEquip = false;
        
        //first, we check the slots to see if we need to remove anything
        //weapon slots are the most complicated
        if (PauseController.PeekSelection() == 0 || PauseController.PeekSelection() == 1)
        {
            int clear = 0;
            int otherSlot = (PauseController.PeekSelection() == 0) ? 1 : 0;
            //if the item is two-handed, we need to clear out both hands
            if (item.TwoHanded)
            {

                //we need to check if the slots are clear, so keep track in a var;


                //check first slot
                if ((PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()].id < 5000))
                {
                    //if it's not empty there's a possibility that we're equipping a bow or gun
                    if (item.equipType == EquipItem.EquipType.BOW || item.equipType == EquipItem.EquipType.GUN)
                    {
                        //are we replacing an older firearm?
                        if (item.equipType == ((EquipItem)(PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()])).equipType)
                        {
                            //then we leave anything in the otherslot alone
                            ammoSafe = true;
                        }
                        //if we're equipping a different weapon type, ammmosafe remains false
                    }
                    //regardless of any of this, we need to clear the slot we're equipping to
                    //hold that item
                    holdItems.Add(PauseController.PeekSelection(),((EquipItem)PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()]));
                    //clear the slot
                    PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()] = GameDatabase.Instance.ItemDatabase[0];
                    //flag first clear
                    clear++;
                    
                }
                else
                {
                    //if the slot is empty then we just increase clear
                    clear++;
                }
                
                //check if we successfully cleared the first slot
                if (clear > 0)
                {
                    //now to check the other slot, but only if it wasn't ammoSafe
                    if (!ammoSafe)
                    {
                        if (PauseController.getEquipMenu().myCharacter.Inventory[otherSlot].id < 5000)
                        {
                            //hold that item
                            holdItems.Add(otherSlot, ((EquipItem)PauseController.getEquipMenu().myCharacter.Inventory[otherSlot]));
                            //clear the slot
                            PauseController.getEquipMenu().myCharacter.Inventory[otherSlot] = GameDatabase.Instance.ItemDatabase[0];
                            //flag next clear
                            clear++;
                        }
                        else
                        {
                            clear++;
                        }
                    }
                    else
                    {
                        clear++;
                    }
                }

                //if we cleared both slots
                if(clear == 2)
                {
                    //we flag the slot as safe
                    safeToEquip = true;
                }
            }
            //if it's not 2-handed
            else
            {
                //we should check the otherslot and see if it is
                if ((PauseController.getEquipMenu().myCharacter.Inventory[otherSlot]).id < 5000)
                {
                    if (((EquipItem)PauseController.getEquipMenu().myCharacter.Inventory[otherSlot]).TwoHanded)
                    {
                        //is it a bow or gun?
                        if (((EquipItem)PauseController.getEquipMenu().myCharacter.Inventory[otherSlot]).equipType == EquipItem.EquipType.BOW ||
                            ((EquipItem)PauseController.getEquipMenu().myCharacter.Inventory[otherSlot]).equipType == EquipItem.EquipType.GUN)
                        {
                            //then we're equipping ammo
                                clear++;
                        }
                        else {
                            //hold that item
                            holdItems.Add(otherSlot, ((EquipItem)PauseController.getEquipMenu().myCharacter.Inventory[otherSlot]));
                            //clear the slot
                            PauseController.getEquipMenu().myCharacter.Inventory[otherSlot] = GameDatabase.Instance.ItemDatabase[0];
                            //flag first clear
                            clear++;
                        }
                    }
                    else
                    {
                        //if it's not two handed, then it's clear
                        clear++;
                    }
                }
                else
                {
                    //if it's empty, it's clear
                    clear++;
                }

                //now check the equip slot
                if(PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()].id < 5000)
                {
                    //if the item is stackable
                    if (item.isStackable())
                    {
                        //check to see if the thing we're replacing is the same stuff
                        if(item.id == PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()].id)
                        {
                            //then leave it there for now
                            clear++;
                        }
                        //if not, then just treat it the same
                        else {
                            //hold that item
                            holdItems.Add(PauseController.PeekSelection(), ((EquipItem)PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()]));
                            //clear the slot
                            PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()] = GameDatabase.Instance.ItemDatabase[0];
                            //flag second clear
                            clear++;
                        }
                    }
                    else {
                        //hold that item
                        holdItems.Add(PauseController.PeekSelection(), ((EquipItem)PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()]));
                        //clear the slot
                        PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()] = GameDatabase.Instance.ItemDatabase[0];
                        //flag second clear
                        clear++;
                    }
                }
                else
                {
                    clear++;
                }

                if(clear == 2)
                {
                    safeToEquip = true;
                }
            }
        }
        //if it's any other slot
        else
        {
            if (PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()].id < 5000)
            {
                //hold that item
                holdItems.Add(PauseController.PeekSelection(), ((EquipItem)PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()]));
                //clear the slot
                PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()] = GameDatabase.Instance.ItemDatabase[0];
                //flag safe to equip
                safeToEquip = true;
            }
            else
            {
                safeToEquip = true;
            }
        }
        //if we can't put them back in, we reverse the entire process
        if (safeToEquip)
        {
            bool fail = false;
            foreach (KeyValuePair<int, EquipItem> x in holdItems)
            {
                if (!GameDatabase.Instance.FindSpace(x.Value, x.Value.quantity))
                {
                    fail = true;
                    break;
                }
            }

            if (!fail)
            {
                foreach (KeyValuePair<int, EquipItem> x in holdItems)
                {
                    GameDatabase.Instance.AddItem(x.Value, x.Value.quantity);
                }
                holdItems.Clear();

                if (item.isStackable())
                {
                    if (item.equipType == EquipItem.EquipType.ARROW || item.equipType == EquipItem.EquipType.BULLET || item.equipType == EquipItem.EquipType.THROWING)
                    {
                        if (PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()].id == item.id)
                        {
                            int toNines = 99 - PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()].quantity;
                            PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()].quantity += Mathf.Min(item.quantity, toNines);
                            item.quantity -= Mathf.Min(item.quantity, toNines);

                            if (item.quantity == 0)
                            {
                                newItem.quantity = item.quantity;
                                GameDatabase.Instance.Inventory[GameDatabase.Instance.Inventory.IndexOf(item)] = GameDatabase.Instance.ItemDatabase[0];
                            }
                        }
                        else
                        {
                            newItem.quantity = item.quantity;
                            PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()] = newItem;
                            GameDatabase.Instance.Inventory[GameDatabase.Instance.Inventory.IndexOf(item)] = GameDatabase.Instance.ItemDatabase[0];
                        }
                    }
                    else
                    {
                        item.quantity--;
                        if (item.quantity == 0)
                        {
                            GameDatabase.Instance.Inventory[GameDatabase.Instance.Inventory.IndexOf(item)] = GameDatabase.Instance.ItemDatabase[0];
                        }
                        newItem.quantity = 1;
                        PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()] = newItem;
                    }
                }
                else if (item.TwoHanded)
                {
                    if (((EquipMenu)PauseController.Instance.Menus[3]).myCharacter.hand == Character.Handedness.LEFT)
                    {
                        ((EquipMenu)PauseController.Instance.Menus[3]).myCharacter.Inventory[1] = newItem;
                    }
                    else
                    {
                        ((EquipMenu)PauseController.Instance.Menus[3]).myCharacter.Inventory[0] = newItem;
                    }

                    GameDatabase.Instance.Inventory[GameDatabase.Instance.Inventory.IndexOf(item)] = GameDatabase.Instance.ItemDatabase[0];
                }
                else
                {
                    PauseController.getEquipMenu().myCharacter.Inventory[PauseController.PeekSelection()] = newItem;
                    GameDatabase.Instance.Inventory[GameDatabase.Instance.Inventory.IndexOf(item)] = GameDatabase.Instance.ItemDatabase[0];
                }

            }
            else
            {
                foreach (KeyValuePair<int, EquipItem> x in holdItems)
                {
                    PauseController.getEquipMenu().myCharacter.Inventory[x.Key] = x.Value;
                }
                holdItems.Clear();
            }
        }
        else
        {
            foreach (KeyValuePair<int, EquipItem> x in holdItems)
            {
                PauseController.getEquipMenu().myCharacter.Inventory[x.Key] = x.Value;
            }
            holdItems.Clear();
        }

    }

    public override void Cancel()
    {
        if (tempSelection >= 0)
        {
            tempSelection = -1;
            pointerTemp.gameObject.SetActive(false);
        }
        else {
            startIndex = 0;
            listContentPanel.transform.localPosition = new Vector3(0, 50 * (startIndex / 2), 0);
            pointer.gameObject.SetActive(false);

            if (context == Context.PAUSE_MENU)
            {
                PauseController.Instance.PopMenu();
            }
            else if(context == Context.SHOP)
            {
                PauseController.Instance.PopMenu();
            }
            if(context == Context.BATTLE)
            {
                BattleController.Instance.PopMenu();
                if(GameDatabase.Instance.lastbattleResult == -1)
                    MenuRoot.gameObject.SetActive(false);
            }
        }
    }

    void UpdateScreenPos()
    {
        rectTransform.rect.Set(rectTransform.position.x,rectTransform.position.y,((RectTransform)transform.parent).rect.width, rectTransform.rect.height);


        numRows = Mathf.FloorToInt(rectTransform.rect.height / 50);
        numCols = Mathf.FloorToInt(rectTransform.rect.width / 400);


        listContentPanel.transform.localPosition = new Vector3(0, 50 * (startIndex / numCols), 0);
        if(startIndex == 0)
        {
            moreUp.gameObject.SetActive(false);
        }
        else
        {
            moreUp.gameObject.SetActive(true);
        }

        if(startIndex + (numRows * numCols) >= population.Count || (!extendPastEnd && startIndex +(numRows * numCols) >= realItems))
        {
            moreDown.gameObject.SetActive(false);
        }
        else
        {
            moreDown.gameObject.SetActive(true);
        }

    }

    public override void updatePointer(int direction)
    {
        int movement = 0;
        if (numCols > 1)
        {
            switch (direction)
            {
                case 0:
                    movement = -numCols;
                    break;
                case 1:
                    movement = numCols;
                    break;
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
        }
        else
        {
            switch (direction)
            {
                case 0:
                    movement = -1;
                    break;
                case 1:
                    movement = 1;
                    break;
                default:
                    movement = 0;
                    break;
            }
        }

        selection += movement;
        if (!extendPastEnd)
        {
            selection = Mathf.Clamp(selection, 0, realItems -1 );
        }
        else {
            selection = Mathf.Clamp(selection, 0, population.Count - 1);
        }
        if (selection < startIndex)
        {
            startIndex -= numCols;
        }
        else if (Mathf.Floor(selection - startIndex)/numCols >= numRows)
        {
            startIndex += numCols;
        }

        UpdateScreenPos();    
    }
}
