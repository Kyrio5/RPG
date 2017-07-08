using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActor : MenuSelectable {

    public Combatant myCombatant;

    public Transform frontRowPosition;
    public Transform backRowPosition;
    public Transform attackPosition;
    public Transform pointerPosition;
    public Transform damagePosition;

    public Vector3 myPosition;
    public Quaternion myRotation;

    public Transform actorModelPosition;
    public GameObject DamageText;
    public Transform TextCanvas;

    public int ATB;
    public int ATBmax;

    public Animator myAnimator;
    public bool MyTurn;

    public bool Guarding;

    public BattleAction myAction;
    public bool Acting = false;

    // Use this for initialization
    void Start () {
		if(myCombatant is Enemy)
        {
            myAnimator.SetBool("IsEnemy", true);
        }
	}

    public void setPosition(Vector3 pos)
    {
        myPosition = pos;
        myRotation = Quaternion.identity;
    }

    public void setPosition(Vector3 pos, Quaternion rot)
    {
        myRotation = rot;
        myPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {

        myAnimator.SetBool("Guard", Guarding);

        float HPPercent = myCombatant.HP / (float)myCombatant.MaxHP;
        myAnimator.SetFloat("HPPercentage", HPPercent);

        if (myCombatant is Character)
        {
            MyTurn = BattleController.GetActiveActor() == this;

            if (!Acting)
            {
                if (((Character)myCombatant).FrontRow)
                {
                    myPosition = frontRowPosition.position;
                    myRotation = frontRowPosition.rotation;
                }
                else
                {
                    myPosition = backRowPosition.position;
                    myRotation = backRowPosition.rotation;
                
                }
            }
            actorModelPosition.position = myPosition;
            actorModelPosition.rotation = myRotation;

            myAnimator.SetBool("weaponIsRanged", myCombatant.RangedWeapon());

        }
        if(myCombatant is Enemy)
        {
            if(ATB >= ATBmax)
            {
                ATB = 0;
                myAction = new AttackAction(this, new BattleActor[] { BattleController.Instance.SelectFromAvailableMembers() });
                BattleController.CreateAction(myAction);
                ActionReady();
            }
        }
        
        if(ATB >= ATBmax && Guarding)
        {
            Guarding = false;
            myAnimator.SetBool("Guard", Guarding);
        }
    }

    public void createAttack()
    {
        AttackAction attack = new AttackAction(this, null);
        attack.SelectTarget();
    }

    public void TakeDamage(int amount)
    {
        myCombatant.HP = Mathf.Clamp(myCombatant.HP - amount, 0, myCombatant.MaxHP);
        myAnimator.SetFloat("HPPercentage", myCombatant.HP / (float)myCombatant.MaxHP);
        myAnimator.SetTrigger("TakeDamage");
        if(myCombatant.HP <= 0)
        { 
            myCombatant.Die();
        }

        ShowDamageText(amount);
        
        if(myCombatant is Enemy)
        {
            Debug.Log(myCombatant.HP + "/" + myCombatant.MaxHP);
        }
        
    }

    public void ShowDamageText(int value)
    {
        DamageText text = Instantiate(DamageText, new Vector3(-1000, -1000, 1000), Quaternion.identity, TextCanvas).GetComponent<DamageText>();
        text.follow = damagePosition;
        text.Damage = value;

    }

    public void ShowMPDamageText(int value)
    {
        DamageText text = Instantiate(DamageText, new Vector3(-1000, -1000, 1000), Quaternion.identity, TextCanvas).GetComponent<DamageText>();
        text.MP = true;
        text.follow = damagePosition;
        text.Damage = value;

    }

    public void Death()
    {
        BattleController.Instance.AllBattleActors.Remove(this);
        BattleController.Instance.Enemies.Remove(this);
        BattleController.Instance.DefeatedEnemies.Add((Enemy)myCombatant);
        this.gameObject.SetActive(false);
    }

    public void ActionReady()
    {
        myAnimator.SetBool("ActionReady", true);
    }

    public void Attack()
    {
        if (BattleController.Instance.TurnQueue.Peek() == myAction)
        {
            myAnimator.SetTrigger("Attack");
            myAnimator.SetBool("ActionReady", false);
        }
    }

    public void CastSpell()
    {
        if (BattleController.Instance.TurnQueue.Peek() == myAction)
        {
            myAnimator.SetTrigger("CastSpell");
            myAnimator.SetBool("ActionReady", false);
        }
    }

    public void UseItem()
    {
        if (BattleController.Instance.TurnQueue.Peek() == myAction)
        {
            myAnimator.SetTrigger("UseItem");
            myAnimator.SetBool("ActionReady", false);
        }
    }

}
