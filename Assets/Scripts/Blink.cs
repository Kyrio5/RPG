using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Blink : MonoBehaviour {

    Image sprt;
    
	// Use this for initialization
	void OnEnable () {
        sprt = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        sprt.enabled = !sprt.enabled;
	}
}
