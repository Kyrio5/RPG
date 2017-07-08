using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelectable : MonoBehaviour {

    public string description;

    public virtual string showDescription()
    {
        return description;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
