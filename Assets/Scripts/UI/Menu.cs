using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public int selection = 0;
    public List<MenuSelectable> population;
    public Transform pointer;
    public Transform MenuRoot;

    // Use this for initialization
    void Start () {
		
	}
	
    public virtual string showDescription()
    {
        return population[selection].showDescription();
    }

	// Update is called once per frame
	public virtual void MenuUpdate () {
        updatePointer(-1);
        pointer.position = population[selection].transform.GetChild(0).position;
    }

    public virtual void updatePointer(int direction)
    {
        int movement = 0;
        switch (direction)
        {
            case 0:
                movement = -1;
                break;
            case 1:
                movement = 1;
                break;
            default:
                movement = 0;
                break;
        }
        selection += movement;
        if(selection > population.Count-1) { selection = 0; }
        else if(selection < 0) { selection = population.Count-1; }
    }


    public virtual void Submit()
    {
        switch (selection)
        {
            case 0:
                PauseController.Instance.ItemMenu(); 
                break;
            case 1:
                PauseController.Instance.SelectCharacter(selection);
                break;
            case 2:

                PauseController.Instance.SelectCharacter(selection);
                break;
            case 3:

                break;
            case 4:
                PauseController.Instance.SelectCharacter(selection);
                break;

            case 5:
                PauseController.Instance.Party();
                break;
            case 6:
                PauseController.Instance.Formation();
                break;
        }
    }

    public virtual void Cancel()
    {
            PauseController.Instance.UnpauseGame();
      
    }
}
