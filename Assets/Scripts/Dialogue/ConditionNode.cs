using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : ActionNode {

    public enum TestFor{
        CharacterInParty, ItemInInventory, KeyItem, GlobalFlag
    }

    TestFor Condition;
    int Value;
    //Conditions:
    //Character in party
    //Item in inventory
    //Item in Key Inventory
    //Global flag

    public ConditionNode(TestFor conditionType, int conditionValue, int[] indices): base(false, indices){
        Condition = conditionType;
        Value = conditionValue;
    }

    public bool TestCondition()
    {
        switch (Condition)
        {
            case TestFor.CharacterInParty:
                return GameDatabase.Instance.CurrentAvailableParty.Contains(GameDatabase.Instance.PartyMembers[Value]);
                
            case TestFor.ItemInInventory:
                foreach(Item x in GameDatabase.Instance.Inventory)
                {
                    if (x.id == GameDatabase.Instance.ItemDatabase[Value].id)
                    {
                        return true;
                    }
                }
                return false;
                
            case TestFor.KeyItem:
                return false;
                //return GameDatabase.Instance
                
            case TestFor.GlobalFlag:
                return GameDatabase.Instance.Flags[Value];
            default:
                return false;
        }
    }
}
