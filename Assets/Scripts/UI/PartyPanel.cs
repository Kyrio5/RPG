using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyPanel : Menu {
    // Use this for initialization
    public delegate bool Operation(int selection, List<Combatant> target);
    public Operation intendedOp;

    public Transform tempPointer;
    int tempSelection = -1;

    void Awake () {
        Refresh();
	}

   

    public bool All=false;

	// Update is called once per frame
	void Update () {

        Refresh();
    }

    public override string showDescription()
    {
        if (intendedOp != null)
        {
            if (intendedOp.Target is UseItem)
            {
                    var x = GameDatabase.Instance.Inventory[PauseController.Instance.SelectionStack.Peek()];
                    return x.getNameWithIcon() + " x" + x.quantity;
            }
            else if(intendedOp.Target is Ability)
            {
                if (All)
                {
                    return "Target all party members";
                }
                else
                {
                    if (GameDatabase.Instance.CurrentAvailableParty[selection] != null)
                        return GameDatabase.Instance.CurrentAvailableParty[selection].Name;
                    return "";
                }
            }
        }
                    return population[selection].showDescription();
    }

    void Refresh()
    {
        for (int i = 0; i < 4; i++)
        {
            if (GameDatabase.Instance.CurrentAvailableParty[i] == null)
            {
                population[i].GetComponent<CharacterSlot>().myCharacter = null;

            }
            else {
                population[i].GetComponent<CharacterSlot>().myCharacter = GameDatabase.Instance.CurrentAvailableParty[i];

            }
        }
    }

    public override void MenuUpdate()
    {
        if (All)
        {
            selection++;
        }
        if (!pointer.gameObject.activeSelf)
        {
            pointer.gameObject.SetActive(true);
        }

        if (intendedOp != null)
        {
            if (intendedOp.Target is UseItem)
            {
                var x = GameDatabase.Instance.Inventory[PauseController.Instance.SelectionStack.Peek()];
                if (x.quantity == 0)
                {
                    GameDatabase.Instance.Inventory[PauseController.Instance.SelectionStack.Peek()] = GameDatabase.Instance.ItemDatabase[0];
                    Cancel();

                }
            }
            else if (intendedOp.Target is Ability)
            {
                Ability x = intendedOp.Target as Ability;
                if(x.MPCost > x.myCharacter.MP)
                {
                    Cancel();
                }
            }
        }
        base.MenuUpdate();
    }

    public override void updatePointer(int direction)
    {
        
        if (intendedOp != null && intendedOp.Target is Ability)
        {
            Ability x = intendedOp.Target as Ability;
            if (x.group == Ability.TargetGroups.MENUUSABLE_ALLABLE)
            {
                switch (direction)
                {
                    case 2:
                        if (!All)
                        {
                            All = true;
                        }
                        break;
                    case 3:
                        if (All)
                        {
                            All = false;
                        }
                        break;
                }
            }
        }

        base.updatePointer(direction);
    }


    public override void Submit()
    {
        if (intendedOp != null)
        {
            if (intendedOp.Target is UseItem)
            {
                var x = GameDatabase.Instance.Inventory[PauseController.Instance.SelectionStack.Peek()];
                if (x.quantity >= 1 && SelectCharacterForFunction())
                {
                    x.quantity--;
                }
                else
                {

                }
            }
            else
            {
                SelectCharacterForFunction();
            }
        }
        else
        {
            if (tempSelection < 0)
            {
                tempSelection = selection;
                tempPointer.gameObject.SetActive(true);
                tempPointer.position = pointer.position;
            }
            else
            {
                if (selection != tempSelection)
                {
                    Character temp = GameDatabase.Instance.CurrentAvailableParty[selection];
                    GameDatabase.Instance.CurrentAvailableParty[selection] = GameDatabase.Instance.CurrentAvailableParty[tempSelection];
                    GameDatabase.Instance.CurrentAvailableParty[tempSelection] = temp;

                }
                else
                {
                    if(((CharacterSlot)population[selection]).myCharacter != null)  
                        ((CharacterSlot)population[selection]).myCharacter.FrontRow = !((CharacterSlot)population[selection]).myCharacter.FrontRow;
                }
                tempPointer.gameObject.SetActive(false);
                tempSelection = -1;
            }

        }
    }

    public override void Cancel()
    {
        if (tempSelection > 0)
        {
            tempSelection = -1;
            tempPointer.gameObject.SetActive(false);
        }
        else {
            All = false;
            pointer.gameObject.SetActive(false);
            intendedOp = null;
            PauseController.Instance.PopMenu();
        }
    }

    bool SelectCharacterForFunction()
    {
        List<Combatant> inBattle = new List<Combatant>();
        for(int i = 0; i < 4; i++)
        {
            inBattle.Add(GameDatabase.Instance.CurrentAvailableParty[i]);
        }


        if (All)
        {
            return intendedOp(-1, inBattle);
        }
        if (population[selection].GetComponent<CharacterSlot>().myCharacter != null)
        {
            return intendedOp(selection, inBattle);
        }
        return false;
    }
}
