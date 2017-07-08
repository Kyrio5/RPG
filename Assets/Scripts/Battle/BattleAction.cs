using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAction {

    public bool ActionComplete = false;
    public bool Active = false;
    public List<BattleActor> targets;
    public BattleActor myActor;

    public BattleAction(BattleActor user, BattleActor[] targets)
    {
        this.targets = new List<BattleActor>();
        myActor = user;
        if (targets != null)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                this.targets.Add(targets[i]);
            }
        }
        user.myAction = this;
    }

    public virtual void Execute()
    {
        Active = true;
    }

    public virtual void UserDead()
    {
        if(myActor.myCombatant.isKO() || myActor.myCombatant.isPetrified() || myActor.myCombatant.isStopped())
        {
            ActionComplete = true;
        }
    }

    public virtual void checkTargets()
    {
        List<BattleActor> temp = new List<BattleActor>();
        for(int i = 0; i < targets.Count; i++)
        {
            if(targets[i].myCombatant.isKO() || targets[i].myCombatant.isPetrified() || targets[i].myCombatant.isStopped())
            {
                temp.Add(targets[i]);   
            }
        }

        foreach(BattleActor x in temp)
        {
            targets.Remove(x);
        }

        if(targets.Count == 0)
        {
            if (myActor.myCombatant is Character)
            {
                if (BattleController.Instance.Enemies.Count > 0)
                    targets.Add(BattleController.Instance.Enemies[Random.Range(0, BattleController.Instance.Enemies.Count - 1)]);
                else
                {
                    ActionComplete = true;
                    return;
                }
            }
            else
            {
                targets.Add(BattleController.Instance.SelectFromAvailableMembers());
            }
        }
    }

    public virtual void ActionEffect()
    {

    }

    public override string ToString()
    {
        string returnStr = "BattleAction for " + myActor.myCombatant.Name + "; Targeting: ";
        foreach(BattleActor x in targets)
        {
            returnStr += " " + x.myCombatant.Name;
        }
        return returnStr;
    }
	
}
