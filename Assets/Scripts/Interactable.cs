using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    bool active;
    public bool awake=true;
    protected int state = 0;

    // Use this for initialization
    void Start () {
		
	}
	
    public void Activate()
    {
        if(awake)
        active = true;
    }
    public void Deactivate()
    {
        active = false;
    }

	// Update is called once per frame
	void Update () {
        if (active)
        {
            DoInteraction();

        }
	}

    public virtual void DoInteraction()
    {

    }
}
