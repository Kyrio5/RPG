using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GilBitScript : MonoBehaviour {
    public TMPro.TextMeshProUGUI gilDisplay;
    public TMPro.TextMeshProUGUI playTime;
    public TMPro.TextMeshProUGUI location;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gilDisplay.text = GameDatabase.Instance.Gil.ToString();
        System.TimeSpan gameTime = System.TimeSpan.FromSeconds(GameDatabase.Instance.playTimeTicks);
        playTime.text = string.Format("{0:D3}:{1:D2}:{2:D2}", gameTime.Hours, gameTime.Minutes, gameTime.Seconds);
        location.text = GameDatabase.Instance.Location;
	}
}
