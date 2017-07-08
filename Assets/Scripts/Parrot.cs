using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrot : Interactable {

    public SpeechBubble mine;

    string[] speech = { "Care for a battle? 200 Gil per try." };
    string[] speech2 = { "Sure.", "No thanks." };
    string[] empty = { };
    
    // Use this for initialization
    void Start () {

	}

    public override void DoInteraction()
    {
        if (state == 0)
        {
            if (DialogueController.Instance.CurrentCriticalBubble == null)
            {
                if (DialogueController.Instance.ShowCriticalMessage("Shady Man", speech, speech2, transform, true, false) != null)
                    state = 1;
            }
        }

        if (state == 1)
        {
            if (DialogueController.Instance.previousSelectionIndex == 0)
            {
                state = 2;
            }
            else if (DialogueController.Instance.previousSelectionIndex == 1)
            {
                state = 3;
            }
        }

        if (state == 2)
        {
            string[] speech3 = { "...You don't have enough gil." };
            if (DialogueController.Instance.ShowCriticalMessage("Shady Man", speech3, empty, transform, true, false) != null)
            {
                state = 0;
                Deactivate();
            }
        }

        if (state == 3)
        {
            string[] speech3 = { "Let me know if you change your mind!" };
            if (DialogueController.Instance.ShowCriticalMessage("Shady Man", speech3, empty, transform, true, false) != null)
            {
                state = 0;
                Deactivate();
            }
        }

        DialogueController.Instance.previousSelectionIndex = -1;
    }
}
