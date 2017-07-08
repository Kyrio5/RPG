using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimatorFunctions : MonoBehaviour {
    public BattleActor myActor;

    public void ActivateEffect()
    {
        myActor.myAction.Active = true;
    }

    public void TeleportToTarget()
    {
        myActor.Acting = true;
        myActor.setPosition(BattleController.Instance.TurnQueue.Peek().targets[0].attackPosition.position,
            BattleController.Instance.TurnQueue.Peek().targets[0].attackPosition.rotation);
    }

    public void EndTeleport()
    {
        myActor.Acting = false;
    }

    public void lookAt()
    {
        myActor.Acting = true;
        if (BattleController.Instance.TurnQueue.Peek().targets.Count == 1)
        {
            myActor.setPosition(transform.position, Quaternion.LookRotation(BattleController.Instance.TurnQueue.Peek().targets[0].transform.position, Vector3.up));
        }
    }

    public void endAction()
    {
        myActor.Acting = false;
        myActor.myAction.Active = false;
        myActor.myAction.ActionComplete = true;
    }
    
    public void SetState(int state)
    {
        myActor.myAnimator.SetInteger("State", state);
    }

    public void doActionEffect()
    {
        myActor.myAction.ActionEffect();
    }

    public void KillEnemy()
    {
        myActor.Death();
    }
}
