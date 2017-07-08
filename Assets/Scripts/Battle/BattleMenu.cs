using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : Menu {
    
    public Character myCharacter;
    public GameObject Row, Guard;

    public MenuSelectable CharAbility, WeapAbility;
    public TMPro.TextMeshProUGUI CALabel, WALabel;
    public Transform pointerImage;


    bool rowSelect, guardSelect;

    void OnEnable()
    {
        CALabel.text = "<color=#909090>"+myCharacter.AbilityName+"</color>";
        WALabel.text = "";
        
    }

    void OnDisable()
    {
        Row.SetActive(false);
        Guard.SetActive(false);
        rowSelect = false;
        guardSelect = false;
    }

    public override void Cancel()
    {
        if (!PauseController.Instance.paused)
        {
            gameObject.SetActive(false);
            BattleController.Instance.PopMenu();
            BattleController.Instance.CycleTurns();
        }
    }

    public override void MenuUpdate()
    {
        if (!PauseController.Instance.paused)
        {
            Row.SetActive(rowSelect);
            Guard.SetActive(guardSelect);
            pointerImage.gameObject.SetActive(!rowSelect && !guardSelect);

            pointer.position = population[selection].transform.GetChild(0).position;
        }

        if(BattleController.Instance.CurrentActiveMember < 0)
        {
            gameObject.SetActive(false);
            BattleController.Instance.PopMenu();
        }
    }

    public override void Submit()
    {
        if (!PauseController.Instance.paused)
        {
            //We need to create a BattleAction. This can be an item, an ability, an attack, or a general action
            //general actions include things that are instant like Guarding or Covering.
            //actions will always end the turn, so changing rows does not produce an action.
            if (rowSelect)
            {
                myCharacter.FrontRow = !myCharacter.FrontRow;
                Cancel();
                return;
            }
            else if (guardSelect)
            {
                BattleController.Instance.Party[BattleController.Instance.CurrentActiveMember].ATB = 0;
                BattleController.Instance.Party[BattleController.Instance.CurrentActiveMember].Guarding = true;
                BattleController.Instance.EndTurn();
                
            }
            else {
                switch (selection)
                {
                    case 0: //Attack
                        BattleController.Instance.AllBattleActors[BattleController.Instance.CurrentActiveMember].createAttack();
                        break;
                    case 1:
                        
                        break;

                    case 3: //Item Menu
                        BattleController.Instance.ItemMenu();
                        break;
                    default:
                        break;
                }
            }
            

        }
    }
    

    public override void updatePointer(int direction)
    {
        if (!PauseController.Instance.paused)
        {
            switch (direction)
            {
                case 0:
                    if (!rowSelect && !guardSelect)
                    {
                        selection--;
                    }
                    break;
                case 1:
                    if (!rowSelect && !guardSelect)
                    {
                        selection++;
                    }
                    break;
                case 2:
                    if (rowSelect)
                    {
                        rowSelect = false;
                    }
                    else
                    {
                        guardSelect = true;
                    }
                    break;
                case 3:
                    if (guardSelect)
                    {
                        guardSelect = false;
                    }
                    else
                    {
                        rowSelect = true;
                    }
                    break;
                default:
                    break;
            }
        }
        
        selection = Mathf.Clamp(selection, 0, population.Count-1);

    }
}
