using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationNode : ActionNode {

    public string[] messages;
    public string[] choices;
    
    public Transform speakerObject;
    public string speakerName;

 

    bool askPay;

    public ConversationNode(string nam, string[] mess, string[] choi, int[] indices, Transform obj, bool exit = false, bool gil = false) :
        base(exit, indices)
    {
        speakerName = nam;
        messages = mess;
        choices = choi;
        speakerObject = obj;
        askPay = gil;
        
    }

    public bool Speak()
    {
        if (DialogueController.Instance.CurrentCriticalBubble == null)
        {
            if (DialogueController.Instance.ShowCriticalMessage(speakerName, messages, choices, speakerObject, true, false, askPay) != null)
            {
                return true;
            }
            return false;
        }
        return false;
    }

}
