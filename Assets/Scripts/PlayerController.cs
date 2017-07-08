using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerController : MonoBehaviour {
    public bool InControl = false;
    public CharacterController controller;
    public float moveSpeed;
    Vector3 direction;
    Vector3 moveDirection;

    public AlertBubble alert;

	// Use this for initialization
	void Awake() {
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

        Ray lookingAt = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (InControl && !PauseController.Instance.paused && DialogueController.Instance.CurrentCriticalBubble == null)
        {
            var inputDevice = InputManager.ActiveDevice;
            direction = new Vector3(inputDevice.LeftStickX, 0, inputDevice.LeftStickY);
            if (direction.sqrMagnitude > .1f)
            {
                controller.transform.LookAt(transform.position + direction);
                controller.transform.rotation *= Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
            }
            controller.SimpleMove(transform.forward.normalized * (moveSpeed * direction.magnitude));

            if (Physics.Raycast(lookingAt, out hit, 1))
            {
                Interactable test = hit.collider.gameObject.GetComponent<Interactable>();
                if (test != null && test.awake)
                {
                    alert.Toggle(true);

                    if (!PauseController.Instance.paused && InputReader.SubmitButton())
                    {
                        test.Activate();
                    }
                }
            }
            else
            {
                alert.Toggle(false);
            }
        }
        else
        {
            alert.Toggle(false);
        }
    }
}
