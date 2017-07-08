using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : BattleAction {
    //Seperate these into Ranged and Not Ranged. Also added effects.
    public AttackAction(BattleActor user, BattleActor[] targets) : base(user, targets)
    {
        user.myAction = this;
    }

    public override void Execute()
    {
        if (BattleController.Instance.TurnQueue.Peek() == this)
        {
            if (myActor.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("ActionReady"))
            {
                myActor.Attack();
                base.Execute();
            }
            else
            {
                myActor.ActionReady();
                Debug.Log("ActionFailed");
            }
        }
    }

    public void SelectTarget()
    {
        BattleController.Instance.SelectTargets();
        ((TargetSelection)BattleController.Instance.MenuExecutionStack.Peek()).AllAvailable = false;
        ((TargetSelection)BattleController.Instance.MenuExecutionStack.Peek()).selection = 0;
        ((TargetSelection)BattleController.Instance.MenuExecutionStack.Peek()).targets = BattleController.Instance.Enemies;
        ((TargetSelection)BattleController.Instance.MenuExecutionStack.Peek()).battleAction = this;
    }


    //this goes into an animation event at point of impact
    public override void ActionEffect()
    {
        foreach (BattleActor x in targets)
        {
            int damage = myActor.myCombatant.getBattleStats()[0] - x.myCombatant.getBattleStats()[1];
            int bonus = 0;

            if (x.Guarding)
            {
                damage /= 2;
            }

            bool ranged = false;
            if (myActor.myCombatant is Enemy)
            {
                bonus = myActor.myCombatant.rawStats[0] + BattleController.FFRandom() % (Mathf.FloorToInt(myActor.myCombatant.Level + myActor.myCombatant.getBattleStats()[0] / 4) + 1);
            }
            else
            {
                ranged = myActor.myCombatant.RangedWeapon();
                bonus = myActor.myCombatant.rawStats[0] + BattleController.FFRandom() % (Mathf.FloorToInt(myActor.myCombatant.Level + myActor.myCombatant.getBattleStats()[0] / 8) + 1);

                if (!((Character)myActor.myCombatant).FrontRow && !ranged)
                {
                    damage /= 3;
                    damage *= 2;
                }

                if (ranged)
                {
                    Item xi = ((Character)myActor.myCombatant).GetOppossedWeapon();
                    xi.quantity--;
                    if(xi.quantity == 0)
                    {
                       ((Character)myActor.myCombatant).Inventory[(((Character)myActor.myCombatant).hand == Character.Handedness.LEFT)?0:1] = GameDatabase.Instance.ItemDatabase[0];
                    }

                }

            }

            damage *= bonus;
            damage = Mathf.Clamp(damage, 0, 9999);

            x.TakeDamage(damage);
        }
    }

    

}
