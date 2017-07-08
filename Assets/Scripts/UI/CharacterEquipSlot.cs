using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterEquipSlot : CharacterSlot {

    public TMPro.TextMeshProUGUI Handedness;

    public TMPro.TextMeshProUGUI EquipSlots;
    public TMPro.TextMeshProUGUI StatDisplay;
    public TMPro.TextMeshProUGUI ProfDisplay;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (myCharacter != null)
        {
            portrait.sprite = myCharacter.portrait;
            namePlate.text = myCharacter.Name;
            switch (myCharacter.hand)
            {
                case Character.Handedness.LEFT:
                    Handedness.text = "L.Handed";
                    break;
                case Character.Handedness.RIGHT:
                    Handedness.text = "R.Handed";
                    break;
                default:
                    Handedness.text = "Ambidex.";
                    break;
            }
        }
	}
    

    void UpdateInventoryDisplay()
    {
        EquipSlots.text = "";
        foreach(Item x in myCharacter.Inventory)
        {
            if(x is EquipItem)
            {
                EquipSlots.text += x.getNameWithIcon();
                if(((EquipItem)x).equipType == EquipItem.EquipType.ARROW || ((EquipItem)x).equipType == EquipItem.EquipType.BULLET)
                {
                    EquipSlots.text += " x" + x.quantity;
                }
            }
            else
            {
                EquipSlots.text += "----";
            }
            EquipSlots.text += "\n";
        }
    }

    void LateUpdate()
    {
        UpdateInventoryDisplay();
        UpdateStatDisplay();
        UpdateProficiencyDisplay();
    }

    void UpdateStatDisplay()
    {
        StatDisplay.text = "";
        for(int i = 0; i < 7; i++)
        {
            StatDisplay.text += myCharacter.FullStats(-1)[i] + "\n";
        }           
    }

    void UpdateProficiencyDisplay()
    {
        ProfDisplay.text = "<sprite=" + EquipItem.TypeSprite[myCharacter.getProficient()[0]]+">\t"+ Mathf.FloorToInt(myCharacter.Primary*100)+"% "+
            "<sprite=" + EquipItem.TypeSprite[myCharacter.getProficient()[1]] + ">\t" + Mathf.FloorToInt(myCharacter.Secondary * 100) + "% " +
            "<sprite=" + EquipItem.TypeSprite[myCharacter.getProficient()[2]] + ">\t" + Mathf.FloorToInt(myCharacter.Tertiary * 100) + "% ";

    }
}
