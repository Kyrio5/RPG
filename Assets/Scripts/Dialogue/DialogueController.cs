using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour {

    public Camera UICamera;
    public static DialogueController Instance { get; set; }
    public SpeechBubble CurrentCriticalBubble;
    public Canvas[] targets;


    public int previousSelectionIndex = -1;
    // Prefabs
    public GameObject DialoguePrefab;
    public Transform selectionPrefab;

    public Transform Top, Middle, Bottom;


    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }
    }

    //Update is called once every frame
    void Update()
    {
        if (!PauseController.Instance.paused) {

            if (CurrentCriticalBubble != null)
            {
                if (InputReader.SubmitButton())
                {
                    CurrentCriticalBubble.skipText();
                }

                if (CurrentCriticalBubble.choiceBox != null)
                {
                    if (InputReader.MenuDown())
                    {
                        CurrentCriticalBubble.choiceBox.selection++;// -= (int)Mathf.Sign(Input.GetAxis("VerticalMenu"));
                    }
                    else if (InputReader.MenuUp())
                    {
                        CurrentCriticalBubble.choiceBox.selection--;// -= (int)Mathf.Sign(Input.GetAxis("VerticalMenu"));
                    }


                    if (CurrentCriticalBubble.choiceBox.selection < 0)
                    {
                        CurrentCriticalBubble.choiceBox.selection = CurrentCriticalBubble.choiceBox.options.Length - 1;
                    }
                    else if (CurrentCriticalBubble.choiceBox.selection >= CurrentCriticalBubble.choiceBox.options.Length)
                    {
                        CurrentCriticalBubble.choiceBox.selection = 0;
                    }
                }
            }
        }
    }

    public SpeechBubble ShowMessage(string name, string[] messages, Transform speaker, bool boxVisible, bool auto)
    {
        SpeechBubble bubble;

        bubble = Instantiate(DialoguePrefab, targets[0].transform).GetComponent<SpeechBubble>();
        bubble.speakerName = name;
        bubble.speakerObjectToFollow = speaker;
        bubble.messages = messages;
        bubble.showDialogueBox = boxVisible;
        bubble.autoAdvance = auto;
        bubble.FollowSpeaker();
        return bubble;
    }

    public SpeechBubble ShowMessage(string name, string[] messages, Vector3 position, bool boxVisible, bool auto)
    {
        SpeechBubble bubble;

        bubble = Instantiate(DialoguePrefab, position, Quaternion.identity, targets[0].transform).GetComponent<SpeechBubble>();
        bubble.speakerName = name;
        bubble.messages = messages;
        bubble.showDialogueBox = boxVisible;
        bubble.autoAdvance = auto;

        return bubble;
    }

    public SpeechBubble ShowCriticalMessage(string name, string[] messages, string[] options, Vector3 position, bool boxVisible, bool auto, bool gil = false)
    {
        if (CurrentCriticalBubble == null)
        {
            SpeechBubble bubble;

            bubble = Instantiate(DialoguePrefab, position, Quaternion.identity, targets[1].transform).GetComponent<SpeechBubble>();
            bubble.speakerName = name;
            bubble.messages = messages;
            bubble.autoAdvance = auto;
            bubble.showDialogueBox = boxVisible;
            bubble.criticalTextBox = true;
            bubble.options = options;
            bubble.showGil = gil;
            CurrentCriticalBubble = bubble;

            return bubble;
        }
        else
        {
            Debug.Log("Can't display message. Critical dialogue slot not open.");
            return null;
        }
    }


    public SpeechBubble ShowCriticalMessage(string name, string[] messages, string[] options, string position, bool boxVisible, bool auto, bool gil = false)
    {
        if (CurrentCriticalBubble == null)
        {
            SpeechBubble bubble;
            Vector3 displayLocation;
            switch (position)
            {
                case "Top":
                    displayLocation = Top.position;
                    break;
                case "Bottom":
                    displayLocation = Bottom.position;
                    break;
                default:
                    displayLocation = Middle.position;
                    break;
            }

            bubble = Instantiate(DialoguePrefab, displayLocation, Quaternion.identity, targets[1].transform).GetComponent<SpeechBubble>();
            bubble.speakerName = name;
            bubble.messages = messages;
            bubble.autoAdvance = auto;
            bubble.showDialogueBox = boxVisible;
            bubble.criticalTextBox = true;
            bubble.options = options;
            bubble.showGil = gil;
            CurrentCriticalBubble = bubble;

            return bubble;
        }
        else
        {
            Debug.Log("Can't display message. Critical dialogue slot not open.");
            return null;
        }
    }


    public SpeechBubble ShowCriticalMessage(string name, string[] messages, string[] options, Transform speaker, bool boxVisible, bool auto, bool gil = false)
    {
        if (CurrentCriticalBubble == null)
        {
            SpeechBubble bubble;

            bubble = Instantiate(DialoguePrefab, targets[1].transform).GetComponent<SpeechBubble>();
            bubble.speakerName = name;
            bubble.speakerObjectToFollow = speaker;
            bubble.messages = messages;
            bubble.autoAdvance = auto;
            bubble.criticalTextBox = true;
            bubble.showDialogueBox = boxVisible;
            bubble.options = options;
            CurrentCriticalBubble = bubble;
            bubble.showGil = gil;
            bubble.FollowSpeaker();
            return bubble;
        }
        else
        {
            Debug.Log("Can't display message. Critical dialogue slot not open.");
            return null;
        }
    }


}


