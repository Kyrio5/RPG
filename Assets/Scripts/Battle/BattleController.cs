using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MenuController {
    //make singleton
    public static BattleController Instance { get; set; }

    //Battle Actions are queued up, and tell the controller what to cause to happen. When they are finished, they are popped off the queue.
    public Queue<BattleAction> TurnQueue;

    //Characters in the party
    public List<BattleActor> Party;
    //Enemies in the opposing party
    public List<BattleActor> Enemies;

    public CharacterBattleSlot[] CharDisplays;
    public BattleMenu menu;
    public TargetSelection targSelect;

    public BattleVictory endScreen;

    //The queue of members currently able to act. Stored as their index in the Party list.
    public Queue<int> ActiveMembers;
    //A list of all physical 3D space representations of the actors in the battle.
    public List<BattleActor> AllBattleActors; //FirstFour are always Party.

    public List<Enemy> DefeatedEnemies;
    public List<Item> Spoils;

    public PopulatedMenu ListSelect;


    //The only player character that can currently decide their action
    public int CurrentActiveMember = -1;

    //A boolean to check if the player is currently navigating a menu.
    bool inMenu;

    //The maximum amount of time in seconds to wait before conducting another action from the queue
    public float MaxDelay = .5f;
    //The current time in seconds left until the next action
    public float ActionDelay;

    public int EarnedGil;
    public int EarnedExp;

    float runTimer;
    bool waiting;
    /*
    
   
    HOW THIS WILL WORK
    
    Actors all begin with random ATB value based on their Speed stat.

    Every tick, all actors will increase their ATB bar (also based on their speed stat). When the bar is full:
    If they are a party member, they're added to ActiveMembers, with the one on top being currently active and the menu is available.
    If they are an enemy, they select an action and are added to the action queue. 
    (Battle setting: Active/Passive. If Passive, ATB bars are frozen and enemy actions are not passed until player is outside of menu.)
    
    Everytime an action finishes, ActionDelay will be set to MaxDelay, then reduced by one every second.
    Only when ActionDelay reaches 0 does a new action pop off the Queue, unless overridden by something.
    If the action queue is empty, ActionDelay will be set to MaxDelay once the first action is stored.
    
    If an actor has died, all of their actions will be skipped with no delay.
    If a target of an action has died, a new target will be selected at random from those still alive.
    
    If all actors from one side has died, then it results in a victory.
    Enemy victories take precedence over player victories.

    an Enemy victory results in a Game Over unless otherwise scripted.

    */
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }

        TurnQueue = new Queue<BattleAction>();
        MenuExecutionStack = new Stack<Menu>();
        SelectionStack = new Stack<int>();
        DefeatedEnemies = new List<Enemy>();
        Spoils = new List<Item>();
        ActiveMembers = new Queue<int>();
        Enemies = new List<BattleActor>();


        for (int i = 0; i < 4; i++)
        {
            Party[i].myCombatant = GameDatabase.Instance.CurrentAvailableParty[i];
            CharDisplays[i].myActor = Party[i];

            if (AllBattleActors[i].myCombatant == null || AllBattleActors[i].myCombatant.Name == "")
            {
                AllBattleActors[i].gameObject.SetActive(false);
            }
        }
        GameObject[] SetEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject x in SetEnemies)
        {
            BattleActor y = x.GetComponent<BattleActor>();
            if (y != null)
            {
                Enemies.Add(y);
                AllBattleActors.Add(y);
            }
        }
        //Set battle actors to combatants
        //Temporary enemy for this battle

        Enemies[0].myCombatant = new Enemy("Shady Man", new int[] { 12, 4, 2, 0, 7, 2, 3 }, 5, new float[] { 1, 1, 1, 1, 1, 1, 1, 1 }, null, 20, 120, null, 1250);
        Enemies[0].myCombatant.HP = Enemies[0].myCombatant.MaxHP;
        
    }

    public void ItemMenu()
    {
        PushMenu(ListSelect, 3);
        ListSelect.sortable = true;
        ListSelect.syncInventory(ref GameDatabase.Instance.Inventory); 
    }


    public void ResetTimer()
    {
        ActionDelay = MaxDelay;
    }

    public BattleActor SelectFromAvailableMembers()
    {
        List<BattleActor> Available = new List<BattleActor>();
        for (int i = 0; i < 4; i++)
        {
            BattleActor x = AllBattleActors[i];
            if (x.myCombatant != null)
            {
                if (!x.myCombatant.isKO() && !x.myCombatant.isPetrified())
                {
                    Available.Add(x);
                }
            }
        }
        if (Available.Count > 0)
            return Available[Random.Range(0, Available.Count)];
        return null;

    }


    // Use this for initialization
    void Start() {
        //load in all the actors. Party is defined by GameDatabase. 




        //initialize the battle
        Initialize();
    }

    public static int PeekSelection()
    {
        return Instance.SelectionStack.Peek();
    }

    public void Initialize()
    {
        //set up all the atb bars, basically
        foreach (BattleActor x in AllBattleActors)
        {
            if (x.gameObject.activeInHierarchy)
            {
                //set atb length for each character
                int speed = x.myCombatant.getBattleStats()[6];
                int randomNum = FFRandom();
                x.ATBmax = ((60 - speed) * 160);
                if (x.ATBmax > 0) {
                    int division = (Mathf.FloorToInt(randomNum / (float)x.ATBmax) * x.ATBmax);
                    if (division > 0)
                        x.ATB = randomNum % division;
                    else
                        x.ATB = 0;
                }
            }
        }
        //Also set up all of the preconditions such as Death or other Status Effects that affect battle.
        //TODO

    }

    public static int FFRandom() {
        return Random.Range(0, 225) * 257;
    }

    public bool TryRun()
    {

        int partyAVG = 0;
        int enemyAVG = 0;
        int Chance = 0;
        float number = 0;
        foreach (BattleActor x in Party)
        {
            if (x.myCombatant != null)
            {
                partyAVG += x.myCombatant.Level;
                number++;
            }
        }
        partyAVG = Mathf.FloorToInt(partyAVG / number);

        number = 0;
        foreach (BattleActor x in Enemies)
        {
            if (x.gameObject.activeInHierarchy)
            {
                enemyAVG += x.myCombatant.Level;
                number++;
            }
        }
        enemyAVG = Mathf.FloorToInt(enemyAVG / number);
        if (enemyAVG > 0)
        {
            Chance = Mathf.FloorToInt(partyAVG * Mathf.FloorToInt(200 / enemyAVG) / 16);
        }

        Debug.Log(Chance);
        if (FFRandom() % 100 < Chance)
        {

            BattleRun();
            return true;
        }

        return false;
    }

    public static void CreateAction(BattleAction action)
    {
        if (Instance.ActionDelay <= 0)
        {
            Instance.ResetTimer();
        }
        Instance.TurnQueue.Enqueue(action);
    }


    public void SelectTargets()
    {
        PushMenu(targSelect);
    }


    //Once every "Tick"
    void Update() {
        //battleSpeed affects growth rate
        int TickRate = 14;


        //if we got active members
        if (ActiveMembers.Count > 0)
        {
            //the next living combatant takes the stage
            while (ActiveMembers.Count > 0 &&
                (Party[ActiveMembers.Peek()].myCombatant.isKO() ||
                Party[ActiveMembers.Peek()].myCombatant.isPetrified() ||
                Party[ActiveMembers.Peek()].myCombatant.isStopped()))
            {
                ActiveMembers.Dequeue();
            }
            if (ActiveMembers.Count > 0)
                CurrentActiveMember = ActiveMembers.Peek();
            else
                CurrentActiveMember = -1;
        }
        //if not
        else
        {
            //we got nothing
            CurrentActiveMember = -1;
        }

        //if the game isn't paused or showing a cutscene
        if (!waiting && !PauseController.Instance.paused)
        {
            //Queue holds Battle Actions.
            //Only one battle action should be available at one time.
            //if the Queue is empty, the timer is set when an action is created.
            //when the timer hits zero, the action will be activated.
            //the action will play an animation
            //during this time the queue should wait until it's signalled to end the action.
            //when the action has ended, it is dequeued
            //if there are still actions in the queue, the timer is reset.

            //check if we lost
            int dead = 0;
            foreach (BattleActor x in Party)
            {
                if (x.myCombatant != null)
                {
                    if (x.myCombatant.isKO() || x.myCombatant.isStopped() || x.myCombatant.isPetrified())
                    {
                        dead++;
                    }
                }
                else
                {
                    dead++;
                }
            }
            if (dead == 4)
            {
                waiting = true;
                BattleLoss();
                return;
            }
            //check if we won
            if (Enemies.Count == 0)
            {
                //if so we wait for the cutscene
                waiting = true;
                //but we haven't done that yet so just win
                BattleWin();
                return;
            }

            //increase ATBs
            //for all combatants
             for (int i = 0; i < AllBattleActors.Count; i++)
            {
                //get the actors
                BattleActor x = AllBattleActors[i];

                //if they exist
                if (x.myCombatant != null && x.myCombatant.Name != "")
                {
                    //if they're knocked out
                    if (x.myCombatant.isKO())
                    {
                        //set their atb to zero
                        x.ATB = 0;
                    }
                    //if they're stopped or stoned
                    else if (x.myCombatant.isStopped() || x.myCombatant.isPetrified())
                    {
                        //don't increase the atb
                        x.ATB += 0;
                    }
                    //if they're slow
                    else if (x.myCombatant.isSlowed())
                    {
                        //tick up only a bit
                        x.ATB += (TickRate - Mathf.CeilToInt(TickRate / 3f));
                    }
                    //if they're hasted
                    else if (x.myCombatant.isHasted())
                    {
                        //tick up more than usual
                        x.ATB += (TickRate + (TickRate / 2));
                    }
                    //if nothing else
                    else
                    {
                        //tick up normally
                        x.ATB += TickRate;
                    }
                }

                //clamp the atb bar
                x.ATB = Mathf.Clamp(x.ATB, 0, x.ATBmax);
                //if the atb bar is full
                if (x.ATB == x.ATBmax)
                {
                    //put them in the queue for selecting actions, assuming they're not already in there.
                    if (x.myCombatant is Character && !ActiveMembers.Contains(i))
                    {
                        ActiveMembers.Enqueue(i);
                    }
                }
            }

            //if no one is ready yet
            if (ActiveMembers.Count == 0)
            {
                //don't show the menu
                menu.gameObject.SetActive(false);

                //and pop it from the execution

                if (MenuExecutionStack.Count > 0 && MenuExecutionStack.Peek() == menu)
                {
                    PopMenu();
                }
            }
            //otherwise
            else
            {
                //if the menu isn't up
                if (!menu.gameObject.activeInHierarchy)
                {
                    //set it to the right character
                    menu.myCharacter = (Character)GetActiveActor().myCombatant;
                    //and put it on the MES
                    PushMenu(menu);
                }
            }

            //if we read the triggers
            if (InputReader.DeviceActive() && InputReader.LeftTrigger() && InputReader.RightTrigger())
            {
                foreach (BattleActor x in Party)
                {
                    if (x.gameObject.activeInHierarchy)
                        x.myAnimator.SetBool("Running", true);
                }
                Debug.Log("running");
                //run up the run timer
                runTimer += Time.deltaTime;
                //every second
                if (runTimer > 1)
                {
                    //we reduce the timer by 1 second
                    runTimer--;
                    //then test the run chance
                    waiting = TryRun();

                }
            }

            //otherwise
            else
            {
                foreach (BattleActor x in Party)
                {
                    if (x.gameObject.activeInHierarchy)
                        x.myAnimator.SetBool("Running", false);
                }
                //reset the timer
                runTimer = 0;
            }



          
            //TurnQueue stuff


            //is the action delay 0?
            if (ActionDelay > 0)
            {
                //if not, tick it down
                ActionDelay -= Time.deltaTime;
            }
            //if it has run out
            else
            {
                //check the turnqueue. if there's an action there, then
                if (TurnQueue.Count > 0)
                {
                    //check for dead targets and remove them
                    TurnQueue.Peek().checkTargets();
                    //check if the actor is dead
                    TurnQueue.Peek().UserDead();
                    //if the action is complete, then don't do anything but remove it
                    if (TurnQueue.Peek().ActionComplete)
                    {
                        Debug.Log("Action Complete");
                        //remove the action
                        TurnQueue.Dequeue();
                        //if there's still something there
                        if (TurnQueue.Count > 0)
                        {
                            //reset the timer, otherwise let it stay at zero
                            ResetTimer();
                        }
                    }
                    //if the action isn't active yet
                    else if (!TurnQueue.Peek().Active)
                    {
                        //execute it
                        Debug.Log("Executing action");
                        TurnQueue.Peek().Execute();
                    }
                }
            }
        }

        if (!PauseController.Instance.paused)
        {
            //if we have something on the stack
            if (MenuExecutionStack.Count > 0)
            {
                //do menu controls
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

                //menuupdate
                MenuExecutionStack.Peek().MenuUpdate();
            }
        }
    }



    public static void BattleRun()
    {
        GameDatabase.Instance.lastbattleResult = 2;
        GameDatabase.TransitionBattle();
    }

    public static void BattleWin()
    {
        Instance.waiting = true;
        //Instance.DefeatedEnemies.Add(Instance.Enemies[0]);
        GameDatabase.Instance.lastbattleResult = 0;
        Instance.PushMenu(Instance.endScreen);
    }

    public static void BattleLoss()
    {
        GameDatabase.Instance.lastbattleResult = 1;
        GameDatabase.TransitionBattle();
    }


    public static BattleActor GetActiveActor()
    {
        if (Instance.ActiveMembers.Count > 0)
            return Instance.AllBattleActors[Instance.ActiveMembers.Peek()];
        else
            return null;
    }


    public void CycleTurns()    
    {
        int temp = ActiveMembers.Dequeue();
        ActiveMembers.Enqueue(temp);
    }

    public void EndTurn()
    {
        ActiveMembers.Dequeue();
    }


}
