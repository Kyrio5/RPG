using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SpeechBubble : MonoBehaviour {

    [Header("UI Components")]
    public RectTransform dialogueExpandingBox; //the message box that expands as text is written.
    public ContentSizeFitter dEBFitter;
    public TMPro.TextMeshProUGUI namePlateTextObject; //the name of the speaker
    public TMPro.TextMeshProUGUI dialogueContentTextObject; //the actual text of the box
    public Image textBoxContainerImage; //the container of the textbox and also the image of the box
    public Image continueArrowImage; // the arrow showing you need to press a button
    public Image dialogueTailImage; //the tail pointing to the speaker.

    [Header("Optional Components")]
    public Transform speakerObjectToFollow; // the object that the box follows
    public SelectionBubble choiceBox; //the choice-seletction object if needed
    public GilTab gilTab;

    [Header("Setting Variables")]
    [Range(5f, 50f)]
    public float messageSpeed; //TODO: Make this a global option.

    public bool autoAdvance; //Whether or not the box will advance on its own
    public bool showGil;
    public int waitTime = 10; //how long to pause before closing automatically if autoadvance is on

    public bool criticalTextBox; //Whether or not this box requries user input
    public bool showDialogueBox; //Whether or not the border image is show
    
    //[HideInInspector]
    public int messageIndex=0; //the index of the messages stored

    [Header("Content Variables")]
    public string speakerName; //the actual name of the speaker
    public string[] messages; //the messages this box will display before closing or showing an option
    public string[] options; //options are only displayed on the last message.
    Stack<string> richTextModifiers;

    [HideInInspector]
    public string displayedMessage; //the currently displayed string
    [HideInInspector]
    public float stringPosition = 0f; //current position for the typewriter

    int state = 0; //the current state of the bubble
    float padding = 35; //padding for how close to follow the character

    int waitTimer; //the actual timer
    float dialogueBoxHeightFromCenter;
    float dialogueBoxWidthFromCenter;

    Animator bubbleAnimatorComponent; //the animation component
    DialogueAnimaionHelper fadeAnimationHelper; //the animation helper
    
    Canvas cs; //the target canvas to display

	// Use this for initialization
	void Awake () {
        dEBFitter = dialogueExpandingBox.GetComponent<ContentSizeFitter>();
        cs = GetComponentInParent<Canvas>();
        richTextModifiers = new Stack<string>();

        bubbleAnimatorComponent = dialogueExpandingBox.GetComponent<Animator>();
        fadeAnimationHelper = dialogueExpandingBox.GetComponent<DialogueAnimaionHelper>();
	}

    void Start()
    {
        waitTimer = waitTime;
        namePlateTextObject.text = speakerName;
        dialogueTailImage.enabled = speakerObjectToFollow != null;
    }
	
    public void skipText()
    {
        if (messageIndex < messages.Length)
        {
            if (stringPosition < messages[messageIndex].Length)
            {
                if (messages[messageIndex].Length > 4)
                {
                    if(stringPosition > 4)
                    stringPosition = messages[messageIndex].Length;
                }
                else {
                    stringPosition = messages[messageIndex].Length;
                }
            }
            else
            {
                messageIndex++;
                stringPosition = 0;
                if(autoAdvance)
                    waitTimer = waitTime;

                if(messageIndex == messages.Length)
                {
                    skipText();
                }
            }

        }
        else
        {
            if(choiceBox != null)
            {
                DialogueController.Instance.previousSelectionIndex = choiceBox.selection;
            }
            fadeAnimationHelper.setState(2);
        }
            
       
    }

    public void FollowSpeaker()
    {
        Vector3 screenPosition = DialogueController.Instance.UICamera.WorldToScreenPoint(speakerObjectToFollow.position);
        screenPosition += new Vector3(0, 40, 0);
        Vector3 newPosition;
        float x;
        float y;
        x = transform.position.x;
        y = screenPosition.y;
        if (screenPosition.x < x - dialogueBoxWidthFromCenter + 45)
        {
            x = screenPosition.x + dialogueBoxWidthFromCenter - 45;
        }
        else if (screenPosition.x > x + dialogueBoxWidthFromCenter - 15)
        {
            x = screenPosition.x - dialogueBoxWidthFromCenter + 15;
        }
        
        newPosition = new Vector3(x, y, 0);
        dialogueTailImage.transform.position = new Vector3(Mathf.Clamp(screenPosition.x - 25, transform.position.x - dialogueBoxWidthFromCenter + 15, transform.position.x + dialogueBoxWidthFromCenter - 15), dialogueTailImage.transform.position.y, 0);
        transform.position = newPosition;

    }




    void maintainBounds()
    {
         Vector3 newPosition;
        float x;
        float y;
        

        if (transform.position.x < Screen.width / 2)
        {
            x = Mathf.Max(dialogueBoxWidthFromCenter + padding, transform.position.x);
        }
        else
        {
            x = Mathf.Min(Screen.width - dialogueBoxWidthFromCenter - padding, transform.position.x);
        }

        if (transform.position.y < Screen.height / 2)
        {
            y = Mathf.Max(dialogueBoxHeightFromCenter + padding, transform.position.y);
        }
        else
        {
            y = Mathf.Min(Screen.height - dialogueBoxHeightFromCenter - padding, transform.position.y);
        }
        newPosition = new Vector3(x, y, 0);

        transform.position = newPosition;
    }


	// Update is called once per frame
	void Update () {

        dialogueBoxWidthFromCenter = dialogueExpandingBox.rect.width / 2 * cs.scaleFactor;
        dialogueBoxHeightFromCenter = dialogueExpandingBox.rect.height / 2 * cs.scaleFactor;

        if (!showDialogueBox && textBoxContainerImage.enabled)
        {
            textBoxContainerImage.enabled = false;
            dialogueTailImage.enabled = false;
        }
        else if(showDialogueBox && !textBoxContainerImage.enabled)
        {
            textBoxContainerImage.enabled = true;
        }


		if(state == 1)
        {
            if (messageIndex < messages.Length)
            {
                displayedMessage = TypeWriter(messages[messageIndex]);
                Canvas.ForceUpdateCanvases();
                dialogueContentTextObject.text = displayedMessage;
                if (stringPosition >= messages[messageIndex].Length && criticalTextBox && choiceBox == null)
                {
                    continueArrowImage.enabled = true;
                }
                else
                {
                    continueArrowImage.enabled = false;
                }
                
                if (autoAdvance && stringPosition >= messages[messageIndex].Length)
                {
                    if (waitTimer > 0)
                    {
                        waitTimer--;
                    }
                    else
                    {
                        skipText();
                    }
                }
            }
            if(messageIndex == messages.Length-1 && stringPosition >= messages[messageIndex].Length)
            {
                if (options != null && options.Length != 0)
                {
                    if (choiceBox == null)
                    {
                        choiceBox = Instantiate(DialogueController.Instance.selectionPrefab, textBoxContainerImage.transform).GetComponent<SelectionBubble>();
                        choiceBox.options = options;
                    }
                }
                if (showGil)
                {
                    gilTab.gameObject.SetActive(true);
                }
            }
        }
        if (bubbleAnimatorComponent != null)
        {
            if(fadeAnimationHelper != null)
            {
                state = fadeAnimationHelper.state;
            }
            bubbleAnimatorComponent.SetInteger("State", state);
        }

        if (speakerObjectToFollow != null)
        {
            FollowSpeaker();
        }

        if (criticalTextBox)
        {
            maintainBounds();
        }
    }

    public void setState(int i)
    {
        state = i;
    }

    public string TypeWriter(string message)
    {       
        string stringPart;

        stringPosition += messageSpeed * Time.deltaTime;
        stringPosition = Mathf.Min(stringPosition, message.Length);
        stringPart = message.Substring(0, (int)Mathf.Floor(stringPosition));
        stringPart = parseRichText(stringPart);
        return stringPart;
    }


    public string parseRichText(string message)
    {
        char readChar;

        string endString = "";
        string finalString = "";

        for(int i = 0; i < message.Length; i++)
        {
            readChar = message[i];
            switch (readChar)
            {
                case '#':

                    if (richTextModifiers.Count > 0 && richTextModifiers.Peek() == "</color>")
                    {
                        finalString += richTextModifiers.Pop();
                        
                    }
                    else {
                        richTextModifiers.Push("</color>");
                        finalString += "<color=yellow>";
                    }
                    break;
                case '`':
                    if (richTextModifiers.Count > 0 && richTextModifiers.Peek() == "</b>")
                    {
                        finalString += richTextModifiers.Pop();
                    }
                    else {
                        richTextModifiers.Push("</b>");
                        finalString += "<b>";
                    }
                    break;
                case '~':
                    if (richTextModifiers.Count > 0 && richTextModifiers.Peek() == "</i>")
                    {
                        finalString += richTextModifiers.Pop();
                    }
                    else {
                        richTextModifiers.Push("</i>");
                        finalString += "<i>";
                    }
                    break;
                case '*':
                    string number = "";
                    if (message.Length > i + 1)
                    {
                        readChar = message[i + 1];
                        while (char.IsDigit(readChar))
                        {
                            number += readChar;
                            i++;
                            if (i+1 < message.Length)
                            {
                                readChar = message[i + 1];
                            }
                            else
                                break;
                            
                        }
                        finalString += "<sprite=" + number + ">";
                    }
    
                    break;
                default:
                    finalString += readChar;
                    break;

            }

        }
        if(richTextModifiers.Count > 0)
        foreach (string s in richTextModifiers)
        {
            endString += s;
        }
        finalString += endString;
        
        richTextModifiers.Clear();

        return finalString;

    }

    public void LateUpdate()
    {
        dEBFitter.enabled = false;
        dEBFitter.enabled = true;

    }

}