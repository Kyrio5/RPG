using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AlertBubble : MonoBehaviour {
    public Transform follow;
    Image myImage;
	// Use this for initialization
	void Awake()
    {
        myImage = GetComponent<Image>();
    }

	// Update is called once per frame
	void Update () {

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(follow.position);
        transform.position = screenPosition;
        
    }

    public void Toggle(bool on)
    {
        myImage.enabled = on;
    }


}
