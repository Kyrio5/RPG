using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleVictory : Menu {

    public BattleVictorySlot[] Party;
    public TMPro.TextMeshProUGUI gilDisplay;
    public TMPro.TextMeshProUGUI expGainedDisplay;
    public TMPro.TextMeshProUGUI gilGainedDisplay;
    public PopulatedMenu subMenu;

    int state = 0;
    float waitTime = 0;
    int divisor = 0;

    bool RoomInInventory = true;

    // Use this for initialization
    void OnEnable () {
        if(GameDatabase.Instance.lastbattleResult != 0)
        {
            gameObject.SetActive(false);
            return;
        }
        waitTime = 3;
        BattleController.Instance.EarnedExp = 0;
        BattleController.Instance.EarnedExp = 0;
        foreach (Enemy x in BattleController.Instance.DefeatedEnemies)
        {
            BattleController.Instance.EarnedExp += x.expReward;
            BattleController.Instance.EarnedGil += x.gilReward;
        }

        int offset = 0;
        for (int i= 0; i < 4; i++)
        {
            Party[i].myCharacter = GameDatabase.Instance.CurrentAvailableParty[i];

            if (GameDatabase.Instance.CurrentAvailableParty[i] != null && GameDatabase.Instance.CurrentAvailableParty[i].Name != "")
            {
                if (!GameDatabase.Instance.CurrentAvailableParty[i].isKO() && !GameDatabase.Instance.CurrentAvailableParty[i].isPetrified())
                {
                    divisor++;
                    Party[i].offset = offset;
                    offset++;
                }
            }
        }
    }

    public override void Submit()
    {
        if (state == 0)
        {
            waitTime = 0;

            if (BattleController.Instance.EarnedExp > 0 || BattleController.Instance.EarnedGil > 0)
            {
                GameDatabase.Instance.Gil += BattleController.Instance.EarnedGil;
                for (int i = 0; i < 4; i++)
                {
                    if (GameDatabase.Instance.CurrentAvailableParty[i] != null && GameDatabase.Instance.CurrentAvailableParty[i].Name != "")
                    {
                        if (!GameDatabase.Instance.CurrentAvailableParty[i].isKO() && !GameDatabase.Instance.CurrentAvailableParty[i].isPetrified())
                            GameDatabase.Instance.CurrentAvailableParty[i].EXP += BattleController.Instance.EarnedExp / divisor;


                        GameDatabase.Instance.CurrentAvailableParty[i].LevelUp();
                    }
                }
                BattleController.Instance.EarnedExp = 0;
                BattleController.Instance.EarnedGil = 0;
            }
        }
        else
        {
            switch (selection)
            {
                case 0:
                    if (RoomInInventory)
                    {
                        foreach (Item x in BattleController.Instance.Spoils)
                        {
                            GameDatabase.Instance.AddItem(x, x.quantity);
                        }
                        GameDatabase.TransitionBattle();
                    }
                    break;
                case 1:
                    if(BattleController.Instance.Spoils.Count > 0)
                        BattleController.Instance.PushMenu(subMenu);
                    break;
                case 2:
                    GameDatabase.TransitionBattle();
                    break;
            }
        }
    }

    public override void Cancel()
    {
        
    }


    public override void MenuUpdate()
    {
        switch (state)
        {
            default:
                break;
            case 1: //menu available
                if (!pointer.gameObject.activeSelf)
                {
                    pointer.gameObject.SetActive(true);
                }

                pointer.transform.position = population[selection].transform.position;
                break;
                

        }
    }

    // Update is called once per frame
    void Update () {
        gilDisplay.text = GameDatabase.Instance.Gil + " Gil";
        expGainedDisplay.text = BattleController.Instance.EarnedExp.ToString();
        gilGainedDisplay.text = BattleController.Instance.EarnedGil.ToString();
        subMenu.syncInventory(ref BattleController.Instance.Spoils);
        foreach (Item x in BattleController.Instance.Spoils)
        {
            if (!GameDatabase.Instance.FindSpace(x, x.quantity))
            {
                RoomInInventory = false;
                break;
            }
        }

        if (state == 0)
        {
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
            }
            else
            {
                
                if (BattleController.Instance.EarnedExp > 0)
                {
                    int increase = Mathf.Min(3 * divisor, BattleController.Instance.EarnedExp);
                    BattleController.Instance.EarnedExp -= increase;
                    for (int i = 0; i < 4; i++)
                    {
                        if (GameDatabase.Instance.CurrentAvailableParty[i] != null && GameDatabase.Instance.CurrentAvailableParty[i].Name != "")
                        {
                            if (!GameDatabase.Instance.CurrentAvailableParty[i].isKO() && !GameDatabase.Instance.CurrentAvailableParty[i].isPetrified())
                                GameDatabase.Instance.CurrentAvailableParty[i].EXP += increase / divisor;

                            GameDatabase.Instance.CurrentAvailableParty[i].LevelUp();
                        }
                    }
                }

                if (BattleController.Instance.EarnedGil > 0)
                {
                    BattleController.Instance.EarnedGil -= Mathf.Min(3, BattleController.Instance.EarnedGil);
                    GameDatabase.Instance.Gil += Mathf.Min(3, BattleController.Instance.EarnedGil);
                }

                if (BattleController.Instance.EarnedExp == 0 && BattleController.Instance.EarnedGil == 0)
                {
                    Debug.Log("done");
                    state = 1;
                }
            }
        }
    }
}
