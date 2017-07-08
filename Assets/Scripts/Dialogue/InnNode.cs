using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnNode : ActionNode {

    public int cost;
    public bool playing;
    public bool done;
    InnTransition transitionObj;

	public InnNode(int cost, int[] indices, bool exit = false) : base(exit, indices)
    {
        this.cost = cost;
        transitionObj = PauseController.Instance.fader;
    }

    public int TryInn()
    {
        if(GameDatabase.Instance.Gil >= cost)
        {
            GameDatabase.Instance.Gil -= cost;
            foreach(Character x in GameDatabase.Instance.CurrentAvailableParty)
            {
                if (x != null && x.Name != "")
                {
                    x.HP = x.MaxHP;
                    x.MP = x.MaxMP;
                    for (int i = 0; i < x.StatusEffects.Length; i++)
                    {
                        x.StatusEffects[i] = false;
                    }
                }
            }
            return 0;
        }
        done = true;
        return 1;
    }
    
    public void InnSuccess()
    {
        if (!playing && !done)
        {
            transitionObj.gameObject.SetActive(true);
            transitionObj.StartAnimation(this);
            playing = true;
        }
    }

}
