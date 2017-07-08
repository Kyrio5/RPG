using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenu : Menu {

    public Character myCharacter;
    public CharacterSlot characterSlot;
    public Menu subMenu;

    public bool Active = false;
    public TMPro.TextMeshProUGUI AbilityName;

    public void LateUpdate()
    {
        switch (myCharacter.AbilityType.ToString())
        {
            case "Assault":

                AbilityName.text = myCharacter.AbilityType.ToString();
                break;

            case "WhiteMagic":

                AbilityName.text = "W.Magic";
                break;

            case "BlackMagic":

                AbilityName.text = "B.Magic";
                break;

            case "Dragon":

                AbilityName.text = myCharacter.AbilityType.ToString();
                break;
        }
        AbilityName.text = myCharacter.AbilityName;
        AbilityName.transform.parent.gameObject.SetActive(false);
        AbilityName.transform.parent.gameObject.SetActive(true);
    }

    public override string showDescription()
    {
        return population[0].showDescription();
    }

    public override void Submit()
    {
        PauseController.Instance.PushMenu(subMenu);
    }
        
    public override void MenuUpdate()
    {
        AbilityName.text = myCharacter.AbilityName;
        if (!Active)
        {
            characterSlot.myCharacter = myCharacter;
            ((PopulatedMenu)subMenu).syncInventory(ref myCharacter.Abilities);
            AbilityName.text = myCharacter.AbilityName;

            Active = true;
        }
        updatePointer(-1);
        pointer.position = population[0].transform.position;
    }

    public override void Cancel()
    {
        Active = false;
        characterSlot.Deactivate();
        gameObject.SetActive(false);
        ((PartyPanel)PauseController.Instance.Menus[1]).intendedOp = PauseController.Instance.AbilityMenu;
        PauseController.Instance.PopMenu();
    }

    public override void updatePointer(int direction)
    {
        //nothing happens
    }
}
