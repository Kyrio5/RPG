using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusSlot : CharacterSlot {

    public TMPro.TextMeshProUGUI levelPlate;
    public TMPro.TextMeshProUGUI classPlate;
    public TMPro.TextMeshProUGUI statusBar;
    public TMPro.TextMeshProUGUI HPDisplay;

    public TMPro.TextMeshProUGUI MPDisplay;
    public TMPro.TextMeshProUGUI Eidolon;

    public RectTransform HPBar;
    public RectTransform MPBar;
    public RectTransform XPBar;

    // Use this for initialization
    void Start()
    {
        if (myCharacter == null)
        {
            //Deactivate Slot
            if (!empty)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
                empty = true;
            }
        }
    }

    public override string showDescription()
    {
        if (myCharacter == null)
            return "";
        return myCharacter.Name;
    }

    // Update is called once per frame
    void Update()
    {
        float HPPercent = 0f;
        float MPPercent = 0f;

        //Check if Slot has a Character
        if (myCharacter == null || myCharacter.Name == "")
        {
            Deactivate();
        }
        else
        {
            Activate();
            //Calculate Percentages
            HPPercent = (float)myCharacter.HP / (float)myCharacter.MaxHP;
            MPPercent = (float)myCharacter.MP / (float)myCharacter.MaxMP;

            //Display Eidolon if Applicable
            if (myCharacter.Eidolon != null)
            {
                Eidolon.text = (myCharacter.Eidolon.HP == 0) ? "<color=red>" : "";
                Eidolon.text += "<sprite=39>" + myCharacter.Eidolon.Name;
                Eidolon.text += (myCharacter.Eidolon.HP == 0) ? "</color>" : "";
            }
            else
            {
                Eidolon.text = "";
            }

            //Assign Portrait
            portrait.sprite = myCharacter.portrait;
            if (myCharacter.FrontRow)
            {
                portrait.transform.localPosition = new Vector3(0, 0, 0);
            }
            else
            {
                portrait.transform.localPosition = new Vector3(25, 0, 0);
            }

            //Fill in Name
            namePlate.text = myCharacter.Name;
            levelPlate.text = "Lv. " + myCharacter.Level;
            classPlate.text = myCharacter.Class;

            //Display HP
            HPDisplay.text = (HPPercent < .1f) ? "<color=red>" : (HPPercent < .3) ? "<color=yellow>" : "";
            HPDisplay.text += myCharacter.HP;
            HPDisplay.text += (HPPercent < .3f) ? "</color>" : "";
            HPDisplay.text += "/" + myCharacter.MaxHP;

            //Display MP
            MPDisplay.text = (MPPercent < .3f) ? "<color=yellow>" : "";
            MPDisplay.text += myCharacter.MP;
            MPDisplay.text += (MPPercent < .3f) ? "</color>" : "";
            MPDisplay.text += "/" + myCharacter.MaxMP;

            //Scale Bars
            HPBar.localScale = new Vector3(HPPercent, 1f, 1f);
            MPBar.localScale = new Vector3(MPPercent, 1f, 1f);
            XPBar.localScale = new Vector3((float)myCharacter.EXP / (float)myCharacter.NextEXP, 1f, 1f);

            //Show Status Effects
            statusBar.text = myCharacter.GetStatusAsSymbols();
        }
    }
}
