using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MenuSelectable 
{
    [HideInInspector]
    public Character myCharacter;

    public Image portrait;
    public TMPro.TextMeshProUGUI namePlate;

    public bool empty = true;


    public void setCharacter(Character chara)
    {
        myCharacter = chara;
    }


    public void Deactivate()
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

    public void Activate()
    {
        //Activate Slot
        if (empty)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            empty = false;
        }
    }

    void Update()
    {
        if (myCharacter == null || myCharacter.Name == "")
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }
}

