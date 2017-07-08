using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable {
    public List<Item> contents;
    public int gilContents;
    bool fail;
    List<string> messages;
	// Use this for initialization
	void Start () {
        contents = new List<Item>();
        messages = new List<string>();
        ////randomize contents
        for(int i = 0; i < Random.Range(0,8); i++)
        {
            var x = GameDatabase.Instance.ItemDatabase[Random.Range(1, 12)];
            if(x is EquipItem)
            {
                x = new EquipItem((EquipItem)x);
            }
            else if(x is UseItem)
            {
                x = new UseItem((UseItem)x);
            }
            else
            {
                Debug.Log("Uhoh");
            }
            if (x.isStackable())
            {
                x.quantity = Random.Range(1, 12);
            }
            contents.Add(x);
        }
        if(Random.Range(0,2) == 0)
        {
            gilContents = 0;
        }
        else
        gilContents = Random.Range(0, 15) * 100;
        ////end randomize


		foreach(Item x in contents)
        {
            string builder = "Found " + x.getNameForBubble();
            if (x.quantity > 1)
            {
                builder += " x " + x.quantity; 
            }
            builder += "!";
            messages.Add(builder);
        }
        if(gilContents > 0)
        {
            messages.Add("Found " + gilContents + " gil!");
        }
        if(messages.Count == 0)
        {
            messages.Add("...It was empty.");
        }
	}

    public override void DoInteraction()
    {
        switch (state)
        {
            case 0:
                if (DialogueController.Instance.CurrentCriticalBubble == null)
                {
                    if (DialogueController.Instance.ShowCriticalMessage("", messages.ToArray(), null, "Top", true, false) != null)
                        state = 1;
                }
                break;
            case 1:
                
                    foreach(Item x in contents)
                    {
                        if (!GameDatabase.Instance.FindSpace(x, x.quantity))
                        {
                            fail = true;
                        }
                    }

                if (fail)
                {
                    if (DialogueController.Instance.CurrentCriticalBubble == null)
                    {
                        string[] failmessage = { "But you can't carry it, so you put it back..." };
                        if (DialogueController.Instance.ShowCriticalMessage("", failmessage, null, "Top", true, false) != null)
                        {
                            state = 0;
                            Deactivate();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < contents.Count; i++)
                    {
                        Item x = contents[i];
                        GameDatabase.Instance.AddItem(x, x.quantity);
                    }
                    GameDatabase.Instance.Gil += gilContents;
                    contents.Clear();
                    gilContents = 0;
                    state = 2;
                    awake = false;
                    Deactivate();


                }
                break;
            default:
                //nothing happens
                break;
        }
    }

}