using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyPortrait : MenuSelectable {

    public Character myCharacter;
    Image myPortrait;
    bool required = false;

    public void Awake()
    {
        myPortrait = GetComponent<Image>();
    }

    public void Update()
    {
        if(myCharacter == null || myCharacter.Name == "")
        {
            myPortrait.enabled = false;
        }
        else
        {
            myPortrait.enabled = true;
            myPortrait.sprite = myCharacter.smallPortrait;
        }
    }

    public override string showDescription()
    {
        if (myCharacter != null)
            return myCharacter.Name + " Lv. " + myCharacter.Level;
        else
            return "";
    }
}
