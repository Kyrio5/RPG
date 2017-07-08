using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartySelectionMenu : Menu {

    public Transform tempPointer;
    int tempSelection = -1;

    public void Update()
    {
        for(int i = 0; i < population.Count; i++)
        {
            ((PartyPortrait)population[i]).myCharacter = GameDatabase.Instance.CurrentAvailableParty[i];
        }
    }


    public override void Cancel()
    {
        if (tempSelection >= 0)
        {
            tempSelection = -1;
            tempPointer.gameObject.SetActive(false);
        }
        else
        {
            selection = 0;
            gameObject.SetActive(false);
            PauseController.Instance.PopMenu();
        }
    }

    public override void Submit()
    {
        if (tempSelection >= 0)
        {
            if (tempSelection != selection)
            {
                Character temp = GameDatabase.Instance.CurrentAvailableParty[selection];
                GameDatabase.Instance.CurrentAvailableParty[selection] = GameDatabase.Instance.CurrentAvailableParty[tempSelection];
                GameDatabase.Instance.CurrentAvailableParty[tempSelection] = temp;
                tempSelection = -1;
                tempPointer.gameObject.SetActive(false);
            }
            else
            {
                Cancel();
            }
        }
        else {
            tempPointer.position = pointer.position;
            tempSelection = selection;
            tempPointer.gameObject.SetActive(true);
            selection = 0;
        }
    }

    public override void MenuUpdate()
    {
        updatePointer(-1);
        pointer.position = population[selection].transform.position;
    }

    public override void updatePointer(int direction)
    {
        int movement = 0;
        if (selection < 4)
        {
            
            switch (direction)
            {
                case 0:
                    movement = -1;
                    break;
                case 1:
                    movement = 1;
                    break;
                case 3:
                    movement = selection + 4;
                    break;
                default:
                    movement = 0;
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case 0:
                    movement = -2;
                    break;
                case 1:
                    movement = 2;
                    break;
                case 3:
                    movement = 1;
                    break;
                case 2:
                    if(selection % 2 == 0)
                    {
                        movement = -selection;  
                    }
                    else
                    {
                        movement = -1;
                    }
                    break;

                default:
                    movement = 0;
                    break;
            }
        }


        selection += movement;
        selection = Mathf.Clamp(selection, 0, 11);
    }

}
