using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelection : Menu {

    public BattleAction battleAction;
    public bool AllAvailable;
    public bool All;
    public bool TPK;
    public Transform SecondPointer;
    public bool TargetingPlayers;

    public List<BattleActor> targets;


	// Use this for initialization
	void Start () {
        targets = new List<BattleActor>();
        
    }
	
  

    public override void MenuUpdate()
    {
        if (!pointer.gameObject.activeInHierarchy)
        {
            pointer.gameObject.SetActive(true);
        }

        if (TPK)
        {
            targets = BattleController.Instance.AllBattleActors;
        }

        if (All || TPK)
        {
            selection++;
        }
        updatePointer(-1);
    }

    public override void updatePointer(int direction)
    {
        
        if (!TPK)
        {
            switch (direction)
            {
                case 0:
                    selection--;
                    break;
                case 1:
                    selection++;
                    break;
                case 2:
                    if (TargetingPlayers && !All)
                    {
                        TargetingPlayers = false;
                        targets = BattleController.Instance.Enemies;
                        if (selection >= targets.Count)
                        {
                            selection = 0;
                        }
                        else if (selection < 0)
                        {
                            selection = targets.Count - 1;
                        }
                    }
                    else if (TargetingPlayers && All)
                    {
                        All = false;
                    }
                    else if (!TargetingPlayers && !All && AllAvailable)
                    {
                        All = true;
                    }
                    break;
                case 3:
                    if (!TargetingPlayers && !All)
                    {
                        targets = BattleController.Instance.Party;
                        if (selection >= targets.Count)
                        {
                            selection = 0;
                        }
                        else if (selection < 0)
                        {
                            selection = targets.Count - 1;
                        }
                        TargetingPlayers = true;
                    }
                    else if (!TargetingPlayers && All)
                    {
                        All = false;
                    }
                    else if (TargetingPlayers && !All && AllAvailable)
                    {
                        All = true;
                    }
                    break;
                default:
                    break;
            }
        }
        int i = 0;
        while(!targets[selection].gameObject.activeInHierarchy)
        {
            i++;
            if(i > 10)
            {
                Cancel();
                break;
            }
            if(targets.Count == 0)
            {
                break;
            }
            selection++;

            if (selection >= targets.Count)
            {
                selection = 0;
            }
            else if (selection < 0)
            {
                selection = targets.Count - 1;
            }

        }

        pointer.position = Camera.main.WorldToScreenPoint(targets[selection].transform.GetChild(0).transform.position);

        if (selection < 4 && targets == BattleController.Instance.Party)
        {
            SecondPointer.gameObject.SetActive(true);
            SecondPointer.position = population[selection].transform.GetChild(0).position;
        }
    }

    public override void Submit()
    {
        if (All)
        {
            battleAction.targets = targets;
        }
        else
            battleAction.targets = new List<BattleActor>() {targets[selection]};

        pointer.gameObject.SetActive(false);
        SecondPointer.gameObject.SetActive(false);

        BattleController.CreateAction(battleAction);
        battleAction.myActor.ActionReady();
        battleAction.myActor.ATB = 0;
        battleAction = null;
        BattleController.Instance.PopMenu();
        BattleController.Instance.PopMenu();
        BattleController.Instance.EndTurn();
    }

    public override void Cancel()
    {
        if(battleAction is ItemAction)
        {
            ((ItemAction)battleAction).ReturnItem();
        }
        pointer.gameObject.SetActive(false);
        SecondPointer.gameObject.SetActive(false);
        BattleController.Instance.PopMenu();
    }


    // Update is called once per frame
    void Update () {
        if (battleAction is ItemAction)
        {
            if (((ItemAction)battleAction).myItem.getName() == "Megalixer" ||
                ((ItemAction)battleAction).myItem.getName() == "Megaphoenix")
            {
                All = true;
            }
        }

        if (selection >= targets.Count)
        {
            selection = 0;
        }
        else if (selection < 0)
        {
            selection = targets.Count - 1;
        }
    }
}
