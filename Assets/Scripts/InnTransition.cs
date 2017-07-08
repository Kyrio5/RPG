using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnTransition : MonoBehaviour {
    InnNode node = null;
    public Animator anim;
    bool playing = false;

    public void setNode(InnNode newNode)
    {
        node = newNode;
    }

    public void StartAnimation(InnNode newNode = null)
    {
        if (newNode != null)
        {
            node = newNode;
        }
        anim.SetTrigger("PlayAnim");

    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayingAnimation"))
        {
            playing = true;
        }
        if (playing)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
                EndAnimation();
                playing = false;
            }
        }
    }


    public void EndAnimation()
    {
        if(node != null)
        {
            node.done = true;
            node.playing = false;
        }
        gameObject.SetActive(false);
    }

}
