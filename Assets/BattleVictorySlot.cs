using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleVictorySlot : CharacterSlot {

    public TMPro.TextMeshProUGUI LevelDisplay;
    public TMPro.TextMeshProUGUI ExpDisplay;
    public Transform expBar;
    public Transform offsetPos;
    public int offset;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        offsetPos.localPosition = new Vector3(50 * offset, 0);
        if(myCharacter == null || myCharacter.Name == "")
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        float expPercent = myCharacter.EXP / (float)myCharacter.NextEXP;

        expBar.localScale = new Vector3(expPercent, 1, 1);
        namePlate.text = myCharacter.Name;
        portrait.sprite = myCharacter.portrait;
        LevelDisplay.text = "Lv. " + myCharacter.Level;
        ExpDisplay.text = myCharacter.EXP + "/" + myCharacter.NextEXP;
        if(myCharacter.isKO() || myCharacter.isPetrified())
        {
            if(myCharacter.isKO())
                namePlate.color = Color.red;
            portrait.color = Color.gray;
        }
	}
}
