using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrigger : MonoBehaviour {

    public Transform cam1, cam2;
    Transform goal;

    void Start()
    {
        goal = cam1;
    }

    void OnTriggerEnter(Collider other)
    {
        goal = cam2;
    }

    void OnTriggerExit(Collider other)
    {
        goal = cam1;
    }

    void Update()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, goal.position, Time.deltaTime);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, goal.rotation, Time.deltaTime);
    }
}
