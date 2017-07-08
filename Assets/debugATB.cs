using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugATB : MonoBehaviour {
    TMPro.TextMeshProUGUI myText;

	// Use this for initialization
	void Start () {
        myText = GetComponent<TMPro.TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
        string x = BattleController.Instance.ActionDelay.ToString() + "\n";
        foreach(BattleAction z in BattleController.Instance.TurnQueue)
        {
            x += z.ToString() + "\n";
        }

        myText.text = x;
	}
}
