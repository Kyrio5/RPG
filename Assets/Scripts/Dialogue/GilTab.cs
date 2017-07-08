using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GilTab : MonoBehaviour {
    public TMPro.TextMeshProUGUI gilDisplay;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gilDisplay.text = GameDatabase.Instance.Gil.ToString();
	}
}
