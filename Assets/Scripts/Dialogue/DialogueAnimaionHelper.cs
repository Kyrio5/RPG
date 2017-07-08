using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimaionHelper : MonoBehaviour {
    public int state = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void setState(int x)
    {
        state = x;
    }

    public void deleteMe() {
        Destroy(transform.parent.gameObject);
    }
}
