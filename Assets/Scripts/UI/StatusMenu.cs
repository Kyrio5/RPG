using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusMenu : Menu {

    public Character myCharacter;

    public CharacterStatusSlot slot;
    public TMPro.TextMeshProUGUI EquipSlots;
    public TMPro.TextMeshProUGUI StatDisplay;
    public TMPro.TextMeshProUGUI ElemResistDisplay;
    public TMPro.TextMeshProUGUI WAbilityText;
    public TMPro.TextMeshProUGUI NAbilityText;

    public MenuSelectable WeaponAbility;
    public MenuSelectable NaturalAbility;

    public TMPro.TextMeshProUGUI ExpText;

    public void OnEnable()
    {
        slot.myCharacter = myCharacter;
    }

    public void UpdateAbility()
    {
        switch (myCharacter.AbilityType.ToString())
        {
            case "Assault":
                NaturalAbility.description = "Tactical strikes with a blade";
                NAbilityText.text = "<color=#606060>"+myCharacter.AbilityType.ToString()+"</color>";
                break;

            case "WhiteMagic":
                NaturalAbility.description = "Health and medical magic";
                NAbilityText.text = "<color=#606060>W.Magic</color>";
                break;

            case "BlackMagic":
                NaturalAbility.description = "Manipulating the forces of destruction";
                NAbilityText.text = "<color=#606060>B.Magic</color>";
                break;

            case "Dragon":
                NaturalAbility.description = "Martial arts learned from studying dragons";
                NAbilityText.text = "<color=#606060>" + myCharacter.AbilityType.ToString() + "</color>";
                break;
        }
        
    }

    public void LateUpdate()
    {
        ExpText.text = myCharacter.EXP + "/" + myCharacter.NextEXP;
        UpdateAbility();
        UpdateInventoryDisplay();
        UpdateResistDisplay();
        UpdateStatDisplay();
    }


    void UpdateStatDisplay()
    {
        StatDisplay.text = "";
        for (int i = 0; i < 7; i++)
        {
            StatDisplay.text += myCharacter.FullStats(-1)[i] + "\n";
        }

    }

    void UpdateResistDisplay()
    {
        ElemResistDisplay.text = Mathf.Floor(myCharacter.Fire * 100f) + "%\n" +
                                Mathf.Floor(myCharacter.Ice * 100f) + "%\n" +
                                Mathf.Floor(myCharacter.Lightning * 100f) + "%\n" +
                                Mathf.Floor(myCharacter.Water * 100f) + "%\n" +
                                Mathf.Floor(myCharacter.Wind * 100f) + "%\n" +
                                Mathf.Floor(myCharacter.Holy * 100f) + "%\n" +
                                Mathf.Floor(myCharacter.Darkness * 100f) + "%\n";

    }

    public override void Submit()
    {
        ///Nothing
    }

    public override void Cancel()
    {
        slot.Deactivate();
        gameObject.SetActive(false);
        ((PartyPanel)PauseController.Instance.Menus[1]).intendedOp = PauseController.Instance.StatusMenu;
        PauseController.Instance.PopMenu();
    }

    public override void MenuUpdate()
    {
        updatePointer(-1);
        pointer.position = population[selection].transform.position;
    }

    void UpdateInventoryDisplay()
    {
        EquipSlots.text = "";
        foreach (Item x in myCharacter.Inventory)
        {
            if (x is EquipItem)
            {
                EquipSlots.text += x.getNameWithIcon();
            }
            else
            {
                EquipSlots.text += "----";
            }
            EquipSlots.text += "\n";
        }
    }
}
