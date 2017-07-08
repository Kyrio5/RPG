using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionBubble : MonoBehaviour {

    public string[] options;
    List<Transform> objectList;
    public int state;
    public int selection;

    public GameObject selectionPrefab;
    public Transform selectionPanel;
    public Transform pointer;

    
	// Use this for initialization
	void Start () {
        GameObject newOption;
        objectList = new List<Transform>();
        state = 0;
        selection = 0;
        foreach(string x in options)
        {
            newOption = Instantiate(selectionPrefab, selectionPanel);
            newOption.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = x;
            objectList.Add(newOption.transform.GetChild(0));
        }
	}
	
	// Update is called once per frame
	void Update () {
        pointer.position = objectList[selection].position;
	}
}
