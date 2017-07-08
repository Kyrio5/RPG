using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;
using InControl;


public class PauseController : MenuController {
    //TODO: Seperate functions into menu controller abstract class so pausecontroller isn't also controlling shops and battles
    public static PauseController Instance { get; set; }
    public bool MenuAvailable;
    public bool paused = false;
    public int mode;
    
    public GameObject Gilbits;
    public GameObject NoMPauseScreen;
    public TMPro.TextMeshProUGUI DescriptionLabel;

        public List<Menu> Menus;

    public Image veneer;
    public InnTransition fader;

    //public bool inputDelay = false;

	// Use this for initialization
	void Awake () {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }
        MenuExecutionStack = new Stack<Menu>();
        SelectionStack = new Stack<int>();
    }
	
    public void PauseGame(bool menu)
    {
        if (menu && MenuAvailable)
        {
            MenuExecutionStack.Clear();
            PushMenu(Menus[0]);

            Menus[0].gameObject.SetActive(true);
            Menus[1].gameObject.SetActive(true);
            Menus[0].transform.SetAsLastSibling();
            Gilbits.SetActive(true);
            veneer.gameObject.SetActive(true);
        }
        else
        {
            NoMPauseScreen.gameObject.SetActive(true);
        }
            paused = true;
            Time.timeScale = 0;

    }

    public void UnpauseGame()
    {
        NoMPauseScreen.gameObject.SetActive(false);
        MenuExecutionStack.Clear();
        SelectionStack.Clear();
        Menus[0].gameObject.SetActive(false);
        Menus[1].gameObject.SetActive(false);
        Gilbits.SetActive(false);
        paused = false;
        veneer.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public bool Party()
    {
        PushMenu(Menus[6], 5);
        return true;
    }

    public bool ItemMenu()
    {
        PushMenu(Menus[2], 0);
        return true;
    }

    public bool StatusMenu(int characterSlot, List<Combatant> group)
    {
        StatusMenu x = (StatusMenu)Menus[5];
        x.myCharacter = ((CharacterSlot)Menus[1].population[characterSlot]).myCharacter;
        PushMenu(Menus[5], characterSlot);
        return true;
    }

    public bool EquipMenu(int characterSlot, List<Combatant> group)
    {
        EquipMenu x = (EquipMenu)Menus[3];
        x.myCharacter = ((CharacterSlot)Menus[1].population[characterSlot]).myCharacter;
        PushMenu(Menus[3], characterSlot);
        return true;
    }

    public bool AbilityMenu(int characterSlot, List<Combatant> group)
    {
        AbilityMenu x = Menus[4] as AbilityMenu;
        x.myCharacter = ((CharacterSlot)Menus[1].population[characterSlot]).myCharacter;
        if (!x.myCharacter.StatusEffects[0])
        {
            PushMenu(Menus[4], characterSlot);
            return true;
        }
        return false;
    }

    public void SelectCharacter(int mode)
    {
        this.mode = 0;
        PartyPanel x = (PartyPanel)Menus[1];
        switch (mode)
        {
            case 1: //ability
                x.intendedOp = AbilityMenu;
                break;
            case 2: //equip
                x.intendedOp = EquipMenu;
                break;
            case 3: //eidolon
                x.intendedOp = EquipMenu;
                break;
            case 4: //status
                x.intendedOp = StatusMenu;
                break;
            case 6: //formation
                x.intendedOp = null;
                break;
        }
        PushMenu(Menus[1], mode);
    }
    public void SelectCharacter(PartyPanel.Operation op, int itemSelect)
    {
        PartyPanel x = (PartyPanel)Menus[1];
        mode = 1;
        x.intendedOp = op;
        PushMenu(Menus[1], itemSelect);
    }

    //Move this to shopController or some shit
    public void ItemShop(ItemShopNode shop)
    {
        
        paused = true;
        Time.timeScale = 0;
        //Camera.main.GetComponent<PostProcessingBehaviour>().enabled = true;
        Menus[7].gameObject.SetActive(true);
        ((ItemShop)Menus[7]).InitializeShop(shop);
        PushMenu(Menus[7]);
    }

    public void Formation()
    {
        PartyPanel x = (PartyPanel)Menus[1];

        x.intendedOp = null;
        PushMenu(Menus[1], 6);
    }

    public void setCharacterOperation(PartyPanel.Operation op)
    {
        PartyPanel x=(PartyPanel)Menus[2];
        x.intendedOp = op;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (!paused)
        {
            if (InputReader.MenuButton() && MenuAvailable)
            {
                PauseGame(true);
            }
            else if (InputReader.PauseButton())
            {
                PauseGame(false);
            }
        }
        else
        {
            if (NoMPauseScreen.activeSelf)
            {
                if (InputReader.PauseButton())
                {
                    UnpauseGame();
                }
            }
            else {
                if (MenuExecutionStack.Count > 0)
                {
                    MenuExecutionStack.Peek().MenuUpdate();

                    if(DescriptionLabel.gameObject.activeInHierarchy)
                        DescriptionLabel.text = MenuExecutionStack.Peek().showDescription();


                    if (InputReader.MenuUp())
                    {
                        MenuExecutionStack.Peek().updatePointer(0);
                    }
                    else if (InputReader.MenuDown())
                    {
                        MenuExecutionStack.Peek().updatePointer(1);
                    }
                    else if (InputReader.MenuLeft())
                    {
                        MenuExecutionStack.Peek().updatePointer(2);
                    }
                    else if (InputReader.MenuRight())
                    {
                        MenuExecutionStack.Peek().updatePointer(3);
                    }

                    if (InputReader.SubmitButton())
                    {
                        MenuExecutionStack.Peek().Submit();
                    }
                    if (InputReader.CancelButton())
                    {
                        MenuExecutionStack.Peek().Cancel();
                    }
                }
                else
                {
                    PauseGame(true);
                }

            }
        }
    }

    public static EquipMenu getEquipMenu()
    {
        return (EquipMenu)Instance.Menus[3];
    }

    public static int PeekSelection()
    {
        return Instance.SelectionStack.Peek();
    }

}
