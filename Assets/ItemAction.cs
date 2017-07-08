using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAction : BattleAction {

    public UseItem myItem;
    
    public ItemAction(BattleActor user, BattleActor[] targs, UseItem item): base(user,targs)
    {
        myItem = item;
        item.quantity--;
        if(item.quantity == 0)
        {
           GameDatabase.Instance.Inventory[GameDatabase.Instance.Inventory.IndexOf(item)] = GameDatabase.Instance.ItemDatabase[0];
        }
    }

    public override void checkTargets()
    {
        base.checkTargets();
    }

    public override void UserDead()
    {
        List<BattleActor> temp = new List<BattleActor>();
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].myCombatant.isKO() || targets[i].myCombatant.isPetrified() || targets[i].myCombatant.isStopped())
            {
                temp.Add(targets[i]);
            }
        }

        foreach (BattleActor x in temp)
        {
            targets.Remove(x);
        }

        if (targets.Count == 0)
        {
            if (myActor.myCombatant is Character)
            {
                if (BattleController.Instance.Enemies.Count > 0)
                    targets.Add(BattleController.Instance.Enemies[Random.Range(0, BattleController.Instance.Enemies.Count - 1)]);
                else
                {
                    ActionComplete = true;
                    ReturnItem();
                    return;
                }
            }
            else
            {
                targets.Add(BattleController.Instance.SelectFromAvailableMembers());
            }
        }
    }

    public override void Execute()
    {
        if (BattleController.Instance.TurnQueue.Peek() == this)
        {
            if (myActor.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("ActionReady"))
            {
                myActor.UseItem();
                base.Execute();
            }
            else
            {
                myActor.ActionReady();
                Debug.Log("ActionFailed");
            }
        }
    }

    public override void ActionEffect()
    {
        myItem.Activate(targets);
    }

    public void ReturnItem()
    {
        if (myItem.quantity > 0)
        {
            myItem.quantity++;
        }
        else
        {
            GameDatabase.Instance.AddItem(myItem, 1);
        }
    }

}
