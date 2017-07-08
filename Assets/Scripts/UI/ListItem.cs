using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItem : MenuSelectable {
    public Item containedItem;
    public TMPro.TextMeshProUGUI itemName;
    public TMPro.TextMeshProUGUI valueDisplay;
    public TMPro.TextMeshProUGUI quantityDisplay;
    public bool usable;

    public bool shop;
    public bool sell;

    // Use this for initialization
    void Start() {

	}
    
    public override string showDescription()
    {
        if (containedItem != null)
            return containedItem.getDescription();
        else return "";
    }
    	
    public void UpdateDisplay()
    {

        itemName.text = "";
        quantityDisplay.text = "";
        valueDisplay.text = "";
        description = "";

        usable = false;

        

        if (containedItem != null && containedItem.quantity > 0)
        {
            if (shop)
            {
                if (sell)
                {
                    if (containedItem.id < 5000)
                        valueDisplay.text = (containedItem.value / 10).ToString();
                }
                else {
                    if (containedItem.id < 5000)
                        valueDisplay.text = containedItem.value.ToString();
                }
            }

            if (containedItem is UseItem)
            {
                if (!shop && ((UseItem)containedItem).targets == UseItem.Target.ALLY ||
                   ((UseItem)containedItem).targets == UseItem.Target.ALLALLY ||
                   ((UseItem)containedItem).targets == UseItem.Target.ALL)
                {
                    usable = true;
                }
                else if (shop && sell)
                {
                    usable = true;
                }
                else if(shop && containedItem.value <= GameDatabase.Instance.Gil)
                {
                    usable = true;
                }

                if (containedItem.isStackable() && containedItem.quantity > 1)
                {
                    quantityDisplay.text = "x " + containedItem.quantity;
                }

                itemName.text = containedItem.getNameWithIcon();

                if (!usable)
                {
                    itemName.text = "<color=#909090>" + itemName.text + "</color>";
                }
            }
            if (containedItem is EquipItem)
            {
                if (!shop && ((EquipMenu)PauseController.Instance.Menus[3]).gameObject.activeInHierarchy) {
                    usable = true;
                }
                else if(shop && sell)
                {
                    usable = true;
                }
                else if(shop && containedItem.value <= GameDatabase.Instance.Gil)
                {
                    usable = true;
                }

                else {
                    usable = false;
                }

                if (containedItem.isStackable() && containedItem.quantity > 1)
                {
                    quantityDisplay.text = "x " + containedItem.quantity;
                }

                itemName.text = containedItem.getNameWithIcon();

                if (!usable)
                {
                    itemName.text = "<color=#909090>" + itemName.text + "</color>";
                }
            }
            if (containedItem.ListType == Item.ItemType.ABILITY)
            {
                if (((Ability)containedItem).Learned)
                {
                    if (((Ability)containedItem).group == Ability.TargetGroups.MENUUSABLE ||
                        ((Ability)containedItem).group == Ability.TargetGroups.MENUUSABLE_ALLABLE)
                    {
                        usable = ((Ability)containedItem).isUsable();
                    }
                    quantityDisplay.text = ((Ability)containedItem).MPCost.ToString();
                    itemName.text = containedItem.getNameWithIcon();

                    if (!usable)
                    {
                        itemName.text = "<color=#909090>" + itemName.text + "</color>";
                    }
                }

            }
        }
        else
        {
            containedItem = null;
        }
    }
    
}
