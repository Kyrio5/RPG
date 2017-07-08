using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour {

    public TMPro.TextMeshProUGUI number;
    public int Damage = 0;
    public bool Guard = false;
    public bool Crit = false;
    public bool MP = false;

    public Transform follow;

    public Animator ani;

    public Color healColor;
    public Color critColor;
    public Color guardColor;

	// Use this for initialization
	void Start () {
        Debug.Log("Pop");
        Destroy(this, ani.GetCurrentAnimatorClipInfo(0).Length);

        number.text = Mathf.Abs(Damage).ToString();
        if(Damage < 0)
        {
            number.color = healColor;
        }
        else if(Damage == 0 || Guard)
        {
            number.color = guardColor;
            number.text = "Blocked";
        }
        else if (Crit)
        {
            number.text = "Critical\n" + number.text;
            number.color = critColor;
        }
        if (MP)
        {
            number.text += " MP";
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Camera.main.WorldToScreenPoint(follow.position);
	}
}
