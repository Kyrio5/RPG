using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBattleSlot : MonoBehaviour
 {
    public BattleActor myActor;
    Character myCharacter;

    public Transform HPBar;
    public Transform MPBar;
    public Image ATBBar;
    public Transform FinesseBar;
    //Eidolon shit goes here

    public TMPro.TextMeshProUGUI namePlate;
    public TMPro.TextMeshProUGUI HPDisplay;
    public TMPro.TextMeshProUGUI MPDisplay;
    public GameObject Highlight;

    public Color slowColor;
    public Color hasteColor;
    public Color stopColor;
    public Color normalColor;

    void OnEnable()
    {
        if(myActor != null)
        myCharacter = (Character)myActor.myCombatant;
    }

    void Update()
    {
        //if we don't have an actor, we're not needed
        if(myActor == null || myActor.myCombatant == null || myActor.myCombatant.Name == "")
        {
            gameObject.SetActive(false);
            return;
        }
        //if we have an actor but no character, then we need to get that
        if (myActor != null && myCharacter == null)
        {
            if(myActor.myCombatant is Character)
            myCharacter = (Character)myActor.myCombatant;
        }

        //if we're activated, set the highlight
        if (myActor.MyTurn)
        {
            Highlight.SetActive(true);
        }
        else
        {
            Highlight.SetActive(false);
        }

        //updateBars
        float HPPercent = myCharacter.HP / (float)myCharacter.MaxHP;
        float MPPercent = myCharacter.MP / (float)myCharacter.MaxMP;
        float ATBPercent = myActor.ATB / (float)myActor.ATBmax;
        float FSPercent = 0f;


        namePlate.text = (HPPercent == 0) ? "<color=red>" : (HPPercent < .3) ? "<color=yellow>" : "";
        namePlate.text += myCharacter.Name;
        namePlate.text += (HPPercent < .3f) ? "</color>" : "";



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
        ATBBar.transform.localScale = new Vector3(ATBPercent, 1f, 1f);
        FinesseBar.transform.localScale = new Vector3(FSPercent, 1f, 1f);

        if (myCharacter.isSlowed())
        {
            ATBBar.color = slowColor;
        }
        else if (myCharacter.isStopped())
        {
            ATBBar.color = stopColor;
        }
        else if (myCharacter.isHasted())
        {
            ATBBar.color = hasteColor;
        }
        else
        {
            ATBBar.color = normalColor;
        }
    }
}
