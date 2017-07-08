using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchingDialogueTest : Interactable {
    public PlayerController player;
    public SortedList<int, ActionNode> dialogueTree;
    public int nodeIndex;
    int branchHold = -1;
    // Use this for initialization
    void Start () { 

        player = GameObject.FindObjectOfType<PlayerController>();

        nodeIndex = 0;
        dialogueTree = new SortedList<int, ActionNode>();
        //0
        string[] talk = {"Hello, Adventurer!", "Welcome to the Incipisphere.\nIt's here where we begin our journey.", "Or at least where you begin your journey.\nI doubt I'm involved much at all.", "Get what I'm saying?"};
        string[] choices = { "Of course!", "Huh?" };
        dialogueTree.Add(0, new ConversationNode("Stranger", talk, choices, new int[] { 1, 2 }, transform));

        //1
        talk = new string[] {"Now I'm sure you have questions, yes?"};
        choices = new string[] { "Is there a place I can rest?", "I need supplies.", "I want to fight you.", "Nevermind." };
        dialogueTree.Add(1, new ConversationNode("Stranger", talk, choices, new int[] { 3, 4, 24, 12 }, transform));

        //2
        talk = new string[] { "Well I tried. What's say we just\npretend you understand?" };
        dialogueTree.Add(2, new ConversationNode("Stranger", talk, new string[] { }, new int[] { 1 }, transform));

        //3
        talk = new string[] { "Yes, of course, just 200 a night." };
        choices = new string[] { "Okay.", "Nevermind." };
        dialogueTree.Add(3, new ConversationNode("Stranger", talk, choices, new int[] { 11, 12 }, transform, false, true));

        //4
        talk = new string[] { "I've got a few things for sale, take a look?" };
        dialogueTree.Add(4, new ConversationNode("Stranger", talk, new string[] { }, new int[] { 8 }, transform));
        
        //5
        talk = new string[] { "Oh you've got balls.", "And I should know.", "I am one!", "...", "Anyway, a fight is 300 gil a pop."};
        choices = new string[] { "Sure", "Bullshit" };
        dialogueTree.Add(5, new ConversationNode("Stranger", talk, choices, new int[] { 22, 10 }, transform, false, true));

        //6
        talk = new string[] { "Hope you enjoyed your stay! " };
        dialogueTree.Add(6, new ConversationNode("Stranger", talk, new string[] { }, new int[] { 1 }, transform, true));

        //7
        talk = new string[] { "You don't have enough gil, sorry." };
        dialogueTree.Add(7, new ConversationNode("Stranger", talk, new string[] { }, new int[] { 1 }, transform, true));

        //8 is a shop
        dialogueTree.Add(8, new ItemShopNode("Stranger", "Welcome to the mysterious shop! How mysterious~!", new int[] { 1, 2, 4, 8, 11, 12, 13, 19, 37, 38 }, new int[] { 12 }));
        //9 is a battle
        dialogueTree.Add(9, new BattleNode("Battle", new int[] { 13, 14, 15 }));

        //10
        talk = new string[] { "Hey it's a living." };
        dialogueTree.Add(10, new ConversationNode("Stranger", talk, new string[] { }, new int[] { 1 }, transform, true));

        //11 is an inn
        dialogueTree.Add(11, new InnNode(200, new int[] { 6, 7 }));
        //need a bool for follow through or exit conversation?
        //12
        talk = new string[] { "See you later!" };
        dialogueTree.Add(12, new ConversationNode("Stranger", talk, new string[0], new int[] { 1 }, transform, true));

        //13
        talk = new string[] { "Wow, you're super strong! Have 500 gil." };
        dialogueTree.Add(13, new ConversationNode("Stranger", talk, new string[0], new int[] { 21 }, transform,false,false));

        //14
        talk = new string[] { "Oops, you lost. Congratulations?" };
        dialogueTree.Add(14, new ConversationNode("Stranger", talk, new string[0], new int[] { 1 }, transform, true));

        //15
        talk = new string[] { "Did...", "Did you just run away?", "Hey, more money for me." };
        dialogueTree.Add(15, new ConversationNode("Stranger", talk, new string[0], new int[] { 1 }, transform, true));

        //16
        dialogueTree.Add(16, new ConditionNode(ConditionNode.TestFor.GlobalFlag, 0, new int[] { 18, 17 }));

        //17
        talk = new string[] { "And a token of your victory, why not." };
        dialogueTree.Add(17, new ConversationNode("Stranger", talk, new string[0], new int[] { 19 }, transform));

        //18
        talk = new string[] { "I don't have anything else to give you." };
        dialogueTree.Add(18, new ConversationNode("Stranger", talk, new string[0], new int[] { 1 }, transform, true));
        
        //19
        dialogueTree.Add(19, new RewardNode(new int[] { 5 }, 0, new int[] { 43 }));

        //20
        dialogueTree.Add(20, new FlagNode(0, true, new int[] { 1 }, true));


        //21
        dialogueTree.Add(21, new RewardNode(new int[] { }, 500, new int[] { 16 }, false));

        //22
        dialogueTree.Add(22, new CostNode(true, 0, 0, 300, new int[]{9, 7}));

        //23
        dialogueTree.Add(23, new FlagNode(1, true, new int[] { 5 }));

        //24
        dialogueTree.Add(24, new ConditionNode(ConditionNode.TestFor.GlobalFlag, 1, new int[] { 25, 23 }));

        //25
        talk = new string[] { "A fight's 300 gil a pop." };
        choices = new string[] { "Sure", "Bullshit" };
        dialogueTree.Add(25, new ConversationNode("Stranger", talk, choices, new int[] { 22, 10 }, transform, false, true));
    }

    public override void DoInteraction()
    {
        player.InControl = false;
        if (state == 0) //we are speaking
        {
            if (dialogueTree.ContainsKey(nodeIndex))
            {
                if (dialogueTree[nodeIndex] is ConversationNode)
                {
                    if (((ConversationNode)dialogueTree[nodeIndex]).Speak())
                    {
                        if (dialogueTree[nodeIndex].exit)
                        {
                            player.InControl = true;
                            Deactivate();
                        }
                        if (((ConversationNode)dialogueTree[nodeIndex]).choices.Length == 0)
                            nodeIndex = dialogueTree[nodeIndex].branchIndices[0];
                        else
                        {
                            state = 1;
                        }
                    }
                }
                else if (dialogueTree[nodeIndex] is ItemShopNode)
                {
                    if (DialogueController.Instance.CurrentCriticalBubble == null)
                    {
                        ((ItemShopNode)dialogueTree[nodeIndex]).Open();
                        nodeIndex = dialogueTree[nodeIndex].branchIndices[0];

                    }

                }
                else if (dialogueTree[nodeIndex] is InnNode)
                {
                    if (DialogueController.Instance.CurrentCriticalBubble == null)
                    {
                        branchHold = ((InnNode)dialogueTree[nodeIndex]).TryInn();
                        state = 1;
                    }
                }
                else if (dialogueTree[nodeIndex] is BattleNode)
                {
                    if (DialogueController.Instance.CurrentCriticalBubble == null)
                    {
                        GameDatabase.TransitionBattle(((BattleNode)dialogueTree[nodeIndex]).myBattle);
                        state = 1;
                    }
                }
                else if (dialogueTree[nodeIndex] is ConditionNode)
                {
                    if (((ConditionNode)dialogueTree[nodeIndex]).TestCondition())
                    {
                        nodeIndex = dialogueTree[nodeIndex].branchIndices[0];
                    }
                    else
                        nodeIndex = dialogueTree[nodeIndex].branchIndices[1];
                }
                else if (dialogueTree[nodeIndex] is RewardNode)
                {

                    bool fail = false;
                    foreach (Item x in ((RewardNode)dialogueTree[nodeIndex]).itemReward)
                    {
                        if (!GameDatabase.Instance.FindSpace(x, x.quantity))
                        {
                            fail = true;
                            break;
                        }
                    }

                    if (DialogueController.Instance.CurrentCriticalBubble == null)
                    {
                        List<string> messages = new List<string>();
                        int gilContents = ((RewardNode)dialogueTree[nodeIndex]).gilReward;
                        foreach (Item x in ((RewardNode)dialogueTree[nodeIndex]).itemReward)
                        {
                            string builder = "Recieved " + x.getNameForBubble();
                            if (x.quantity > 1)
                            {
                                builder += " x " + x.quantity;
                            }
                            builder += "!";
                            messages.Add(builder);
                        }
                        if (gilContents > 0)
                        {
                            messages.Add("Recieved " + gilContents + " gil!");
                        }
                        if (messages.Count == 0)
                        {
                            messages.Add("Recieved nothing!");
                        }
                        if (fail)
                        {
                            messages.Add("But your inventory is full.");
                        }
                        else
                        {
                            foreach(Item x in ((RewardNode)dialogueTree[nodeIndex]).itemReward)
                            {
                                GameDatabase.Instance.AddItem(x, x.quantity);
                            }
                            GameDatabase.Instance.Gil += ((RewardNode)dialogueTree[nodeIndex]).gilReward;
                        }

                        if (DialogueController.Instance.ShowCriticalMessage("", messages.ToArray(), null, "Top", true, false) != null)
                            state = 1;


                    }
                }
                else if (dialogueTree[nodeIndex] is FlagNode)
                {
                    ((FlagNode)dialogueTree[nodeIndex]).ChangeFlag();
                    nodeIndex = dialogueTree[nodeIndex].branchIndices[0];
                }
                else if (dialogueTree[nodeIndex] is CostNode)
                {
                    if (DialogueController.Instance.CurrentCriticalBubble == null)
                    {
                        if (((CostNode)dialogueTree[nodeIndex]).GetCost())
                        {
                            nodeIndex = dialogueTree[nodeIndex].branchIndices[0];
                        }
                        else
                            nodeIndex = dialogueTree[nodeIndex].branchIndices[1];
                    }
                }
            }
            else
            {
                nodeIndex = 1;
            }
        }
        else if (state == 1) //we are listening
        {
            if (dialogueTree[nodeIndex] is ConversationNode)
            {
                if (DialogueController.Instance.previousSelectionIndex >= 0)
                {
                    if (DialogueController.Instance.CurrentCriticalBubble == null)
                    {
                        nodeIndex = dialogueTree[nodeIndex].branchIndices[DialogueController.Instance.previousSelectionIndex];
                        DialogueController.Instance.previousSelectionIndex = -1;
                        state = 0;
                    }
                }
            }
            else if(dialogueTree[nodeIndex] is InnNode)
            {
                if(branchHold == 0)
                {
                    ((InnNode)dialogueTree[nodeIndex]).InnSuccess();
                }

                if (((InnNode)dialogueTree[nodeIndex]).done)
                {
                    ((InnNode)dialogueTree[nodeIndex]).done = false;
                    nodeIndex = ((InnNode)dialogueTree[nodeIndex]).branchIndices[branchHold];
                    state = 0;
                }
            }
            else if(dialogueTree[nodeIndex] is BattleNode)
            {
                if(GameDatabase.Instance.lastbattleResult >= 0)
                {
                    nodeIndex = (dialogueTree[nodeIndex].branchIndices[GameDatabase.Instance.lastbattleResult]);
                    state = 0;
                }
            }
            else if(dialogueTree[nodeIndex] is RewardNode)
            {
                if (dialogueTree[nodeIndex].exit)
                {
                    player.InControl = true;
                    Deactivate();
                }
                nodeIndex = (dialogueTree[nodeIndex].branchIndices[0]);
                state = 0;
            }
        }
    }


    public void makeSelection(int selection)
    {
        nodeIndex = selection;
    }


}
