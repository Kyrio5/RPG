
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDatabase : MonoBehaviour {

    //singleton
    public static GameDatabase Instance { get; set; }
    public Sprite[] charSprites;

    [Header("Reference Databases")]
    public List<Item> ItemDatabase;
    public List<Character> PartyMembers;
    [Header("Runtime Databases")]
    public List<Item> Inventory;
    public List<Item> KeyInventory;
    [HideInInspector]
    public List<Character> CurrentAvailableParty;

    public List<Item> Assault;
    public List<Item> WhiteMagic;
    public List<Item> Dragon;
    public List<Item> BlackMagic;
    public List<Item> Dyne;
    public List<Item> Sing;

    public int Gil;
    public double playTimeTicks=0;

    public string Location = "Test Area - Incipisphere";

    string currentWorldScene;
    string currentBattleScene;

    public int lastbattleResult = -1;
    public List<bool> Flags;
    //exit battle
    public static void TransitionBattle()
    {
        if (!SceneManager.GetSceneByName(Instance.currentWorldScene).isLoaded)
        {
            SceneManager.LoadSceneAsync(Instance.currentWorldScene, LoadSceneMode.Additive);
        }
        SceneManager.GetSceneByName(Instance.currentWorldScene).GetRootGameObjects()[0].SetActive(true);
        SceneManager.UnloadSceneAsync(Instance.currentBattleScene);
        PauseController.Instance.MenuAvailable = true;
    }
    //enter battle
    public static void TransitionBattle(string battleScene)
    {
        PauseController.Instance.MenuAvailable = false;
        Instance.currentBattleScene = battleScene;
        Instance.lastbattleResult = -1;
        if (!SceneManager.GetSceneByName(Instance.currentBattleScene).isLoaded)
        {
            SceneManager.LoadSceneAsync(battleScene, LoadSceneMode.Additive);
        }
        SceneManager.GetSceneByName(Instance.currentWorldScene).GetRootGameObjects()[0].SetActive(false);   
    }
    
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //end loading screen?
        scene.GetRootGameObjects()[0].SetActive(true);
    }

    void OnLevelUnloaded(Scene scene)
    {
        //start loading screen?
    }

    void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        SceneManager.sceneUnloaded += OnLevelUnloaded;
        
        currentWorldScene = SceneManager.GetActiveScene().name;
        Flags = new List<bool>();
        for(int i = 0; i <40; i++)
        {
            Flags.Add(false);
        }
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }

       // QualitySettings.vSyncCount = 0;
       // Application.targetFrameRate = 30;

        Inventory = new List<Item>();
        //PartyMembers = new List<Character>();

        KeyInventory = new List<Item>();
        ItemDatabase = new List<Item>();

        CurrentAvailableParty = new List<Character>();

        Assault = new List<Item>();
        //Assault.Add(new Ability(0, "Crush", "Deal a heavy blow to one target", 0, 8, 5, Ability.TargetGroups.ENEMIESFIRST));
        //Assault.Add(new Ability(1, "Piercing Attack", "Deal damage while ignoring defense of target", 0,  12, 5, Ability.TargetGroups.ENEMIESFIRST));
        //Assault.Add(new Ability(2, "Armor Break", "Attack while temporarily lowering defense", 0,  30, 12, Ability.TargetGroups.ENEMIESFIRST));
        //Assault.Add(new Ability(3, "Power Break", "Attack while temporarily lowering attack", 0,  35, 18, Ability.TargetGroups.ENEMIESFIRST));
        //Assault.Add(new Ability(4, "Shock", "Deal damage equal to health lost", 0,  20, 24, Ability.TargetGroups.ENEMIESFIRST));
        //Assault.Add(new Ability(5, "Trine Attack", "Deal Non-Elemental damage to single target", 0,  108, 35, Ability.TargetGroups.ENEMIESFIRST));

        for (int i= Assault.Count - 1; i < 19; i++)
        {
            Assault.Add(new Item(9999, "", "", 0, false));
        }


        WhiteMagic = new List<Item>();
        WhiteMagic.Add(new Ability(0, "Cure", "Heal a small amount of HP", 36, 5, 0, Ability.TargetGroups.MENUUSABLE_ALLABLE));
        WhiteMagic.Add(new Ability(1, "Cura", "Heal a medium amount of HP", 36,  26, 12, Ability.TargetGroups.MENUUSABLE_ALLABLE));
        WhiteMagic.Add(new Ability(2, "Curaga", "Heal a large amount of HP", 36,  58, 32, Ability.TargetGroups.MENUUSABLE_ALLABLE));
        WhiteMagic.Add(new Item(9999, "", "", 0, false));
        WhiteMagic.Add(new Ability(3, "Life", "Revive a fallen ally", 36,  40, 18, Ability.TargetGroups.MENUUSABLE_ALLABLE));
        WhiteMagic.Add(new Ability(4, "Full-Life", "Fully revive a fallen ally", 36,  127, 40, Ability.TargetGroups.MENUUSABLE));
        WhiteMagic.Add(new Item(9999, "", "", 0, false));
        WhiteMagic.Add(new Item(9999, "", "", 0, false));
        //WhiteMagic.Add(new Ability(5, "Stona", "Cure petrification", 36,  8, 6, Ability.TargetGroups.MENUUSABLE));
        //WhiteMagic.Add(new Ability(6, "Esuna", "Cure all status ailments", 36,  32, 20, Ability.TargetGroups.MENUUSABLE));
        //WhiteMagic.Add(new Ability(7, "Shell", "Increased Magic Defense", 36,  24, 9, Ability.TargetGroups.ALLIESFIRST));
        //WhiteMagic.Add(new Ability(8, "Protect", "Increased Physical Defense", 36, 24, 15, Ability.TargetGroups.ALLIESFIRST));

        for (int i = WhiteMagic.Count - 1; i < 19; i++)
        {
            WhiteMagic.Add(new Item(9999, "", "", 0, false));
        }

        ItemDatabase.Add(new Item(5000, "", "", 0, false));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "Weed", "A weed. Kinda dank.", 32, true, UseItem.UseType.Potion, UseItem.Target.ALLY, 50));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "Meme", "A pretty dank meme.", 32, true, UseItem.UseType.HiPotion, UseItem.Target.ALLY, 400));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "XPotion", "Restores All HP", 32, true, UseItem.UseType.XPotion, UseItem.Target.ALLY, 1200));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "Ether", "Restores 100 MP", 33, true, UseItem.UseType.Ether, UseItem.Target.ALLY, 150));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "Turbo Ether", "Restores All MP", 33, true, UseItem.UseType.FullEther, UseItem.Target.ALLY, 600));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "Elixer", "Restores All HP and MP", 35, true, UseItem.UseType.Elixer, UseItem.Target.ALLY, 1500));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "MegaLixer", "Restores Everyone's HP and MP", 35, true, UseItem.UseType.Megalixer, UseItem.Target.ALLALLY, 2800));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "Phoenix Down", "Revives a fallen character", 25, true, UseItem.UseType.PhoenixDown, UseItem.Target.ALLY, 500));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "MegaPhoenix", "Revives all fallen characters", 25, true, UseItem.UseType.MegaPhoenix, UseItem.Target.ALLALLY, 1500));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "Panacea", "Cures all Ailments", 34, true, UseItem.UseType.Panacea, UseItem.Target.ALLY, 250));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "Eye Drops", "Cures Blindness", 34, true, UseItem.UseType.EyeDrops, UseItem.Target.ALLY, 70));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "Echo Screen", "Cures Silence", 34, true, UseItem.UseType.EchoScreen, UseItem.Target.ALLY, 70));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "Antidote", "Cures Poison", 34, true, UseItem.UseType.Antidote, UseItem.Target.ALLY, 70));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "Softener", "Cures Petrify", 34, true, UseItem.UseType.Soft, UseItem.Target.ALLY, 70));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "Tranquilizer", "Cures Berserk", 34, true, UseItem.UseType.Tranquilizer, UseItem.Target.ALLY, 70));
        ItemDatabase.Add(new UseItem(ItemDatabase.Count, "Prayer Tag", "Cures Zombie", 34, true, UseItem.UseType.MagicTag, UseItem.Target.ALLY, 70));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Short Sword", "A simple, small bladed weapon", true, EquipItem.EquipType.SWORD, new[] { 7, 0, 0, 0, 0, 0, 0 }, false, false, 300));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Iron Sword", "A heavy blade wrought of iron", true, EquipItem.EquipType.SWORD, new[] { 12, 0, 0, 0, 0, 0, 0 }, false, false, 450));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Steel Sword", "A strong sword made from steel", true, EquipItem.EquipType.SWORD, new[] { 19, 0, 0, 0, 0, 0, 0 }, false, false, 530));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Shovel", "A simple farm tool", true, EquipItem.EquipType.AXE, new[] { 9, 0, 0, 0, 0, 0, 0 }, false, false, 80));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Hatchet", "An axe made for chopping wood", true, EquipItem.EquipType.AXE, new[] { 12, 0, 0, 0, 0, 0, 0 }, false, false, 120));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Great Sword", "A large weapon for two hands", true, EquipItem.EquipType.GSWORD, new[] { 10, 0, 0, 0, 0, 0, 0 }, true, false, 460));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Buster Sword", "Less a sword and more a large hunk of iron", true, EquipItem.EquipType.GSWORD, new[] { 26, 0, 0, 0, 0, 0, -2 }, true, false, 890));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Buckler", "A small shield", true, EquipItem.EquipType.SHIELD, new[] { 0, 8, 0, 0, 0, 0, 0 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Wooden Kite", "A shield made of wood", true, EquipItem.EquipType.SHIELD, new[] { 0, 18, 0, 0, 0, 0, 0 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Breastplate", "A heavy iron breastplate", true, EquipItem.EquipType.HARMOR, new[] { 0, 20, 0, 0, 0, 0, -2 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Padded Shirt", "A thick cloth armor", true, EquipItem.EquipType.MARMOR, new[] { 0, 12, 0, 0, 0, 0, 0 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Dark Cloak", "A cloak made of pitch black material", true, EquipItem.EquipType.LARMOR, new[] { 0, 8, 0, 12, 5, 0, 2 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Steepled Hat", "A pointed hat", true, EquipItem.EquipType.HAT, new[] { 0, 9, 5, 2, 8, 0, 0 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Sallet", "A heavy head armor", true, EquipItem.EquipType.HELM, new[] { 0, 10, 0, 0, 0, 0, 0 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Iron Spear", "A heavy polearm wrought of iron", true, EquipItem.EquipType.SPEAR, new[] { 12, 0, 0, 0, 0, 0, 0 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Mage Staff", "A magic staff good for focusing energy", true, EquipItem.EquipType.STAFF, new[] { 9, 0, 4, 6, 6, 0, 0 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Mage Masher", "A blade useful against magic users", true, EquipItem.EquipType.KNIFE, new[] { 12, 0, 0, 0, 0, 0, 0 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Dagger", "A simple small blade", true, EquipItem.EquipType.KNIFE, new[] { 19, 0, 0, 0, 0, 0, 0 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Light Rod", "A simple prayer rod", true, EquipItem.EquipType.ROD, new[] { 5, 0, 6, 3, 0, 0, 0 }, true, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Shortbow", "A light shortrange bow", true, EquipItem.EquipType.BOW, new[] { 15, 0, 0, 0, 0, 0, 0 }, true, false, 200));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Hunter Bow", "A medium weight bow", true, EquipItem.EquipType.BOW, new[] { 21, 0, 0, 0, 0, 0, 0 }, true, false, 390));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Wooden Arrow", "A standard arrow",  true, EquipItem.EquipType.ARROW, new[] { 8, 0, 0, 0, 0, 0, 0 }, false, false, 20));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Iron Arrow", "A solid arrow made of iron", true, EquipItem.EquipType.ARROW, new[] { 10, 0, 0, 0, 0, 0, 0 }, false, false, 80));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Steel Arrow", "A sharpened steel arrow", true, EquipItem.EquipType.ARROW, new[] { 15, 0, 0, 0, 0, 0, 0 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Poison Arrow", "An arrow tipped with poison", true, EquipItem.EquipType.ARROW, new[] { 9, 0, 0, 0, 0, 0, 0 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Thunder Ring", "A ring that reduces Lightning damage", true, EquipItem.EquipType.ACCESSORY, new[] { 0, 5, 8, 5, 0, -3, 0 }, false, false));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Heavy Ring", "A ring that boosts defense at the cost of speed", true, EquipItem.EquipType.ACCESSORY, new[] { 0, 25, 8, 5, 0, 0, -2 }, false, false, 5200));
        ItemDatabase.Add(new EquipItem(ItemDatabase.Count, "Reflect Ring", "A ring that reduces Lightning damage", true, EquipItem.EquipType.ACCESSORY, new[] { 0, 0, 12, 0, 0, 0, 0 }, false, false));


        //PartyMembers.Add(new Character("Null", "", "", null, 0, null, null, null, null, null, null));
        //PartyMembers.Add(new Character("Matthew", "Cumguzzler", "Healsluttery", charSprites[0], 99, WhiteMagic,
        //    new[] { 4, 2, 8, 6, 5, 3, 6 },
        //    new[] { EquipItem.EquipType.STAFF, EquipItem.EquipType.ROD, EquipItem.EquipType.KNIFE },
        //    new[] { EquipItem.EquipType.KNIFE },
        //    new[] { EquipItem.EquipType.LARMOR },
        //     new[] { EquipItem.EquipType.HAT }));
        // PartyMembers.Add(new Character("Milon", "Soldier", "Assault", charSprites[1], 27, Assault, 
        //     new[] { 8, 6, 1, 2, 4, 9, 2 },
        //     new[] { EquipItem.EquipType.SWORD, EquipItem.EquipType.AXE, EquipItem.EquipType.GSWORD },
        //     new[] { EquipItem.EquipType.SHIELD },
        //     new[] { EquipItem.EquipType.HARMOR },
        //    new[] { EquipItem.EquipType.HELM }));

        for (int i = 0; i < 12; i++)
        {
            CurrentAvailableParty.Add(null);
        }
        PartyMembers[1].Abilities = Dragon;
        PartyMembers[2].Abilities = BlackMagic;
        PartyMembers[3].Abilities = WhiteMagic;
        PartyMembers[4].Abilities = Assault;

        CurrentAvailableParty[0]= PartyMembers[4];
        CurrentAvailableParty[1]= PartyMembers[2];
 


        for(int i = 0; i < 12; i++)
        {
            if(CurrentAvailableParty[i] != null)
            {
                CurrentAvailableParty[i].initialize();
                CurrentAvailableParty[i].statsByLevel();
                CurrentAvailableParty[i].AbilityByLevel();

            }
        }

        Inventory.Add(new EquipItem((EquipItem)ItemDatabase[17]));

        Inventory.Add(new EquipItem((EquipItem)ItemDatabase[36]));


        for (int i = 0; i < 57; i++)
        {
            Inventory.Add(ItemDatabase[0]);
        }

        Instance.AddItem(new EquipItem((EquipItem)ItemDatabase[38]), 5);
    }
    
    public bool AddItem(Item item, int Quant = 1)
    {
        Item newItem;
        if (item is UseItem)
        {
            newItem = new UseItem((UseItem)item);
        }
        else if (item is EquipItem)
        {
            newItem = new EquipItem((EquipItem)item);

        }
        else
        {
            newItem = new Item(item);
        }

        if (FindSpace(newItem, Quant))
        {
            if (newItem.id < 5000)
            {
                if (newItem.isStackable())
                {
                    foreach (Item x in Inventory)
                    {
                        if (newItem.id == x.id)
                        {
                            int toNines = 99 - x.quantity;
                            Inventory[Inventory.IndexOf(x)].quantity += Mathf.Min(Quant, toNines);
                            Quant -= Mathf.Min(Quant, toNines);
                        }
                        else if(x.id >= 5000)
                        {
                            newItem.quantity = Quant;
                            Inventory[Inventory.IndexOf(x)] = newItem;
                            Quant -= Mathf.Min(99, Quant);
                        }
                        if(Quant == 0)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    foreach(Item x in Inventory)
                    {
                        if(x.id >= 5000)
                        {
                            newItem.quantity = 1;
                            Inventory[Inventory.IndexOf(x)] = newItem;
                            Quant--;
                            if (Quant == 0)
                            {
                                return true;
                            }
                            else
                            {
                                if (item is UseItem)
                                {
                                    newItem = new UseItem((UseItem)item);
                                }
                                else if (item is EquipItem)
                                {
                                    newItem = new EquipItem((EquipItem)item);

                                }
                                else
                                {
                                    newItem = new Item(item);
                                }
                            }
                        }
                    }
                }

                if(Quant == 0)
                {
                    return true;
                }
            }
            return true;
        }
        return false;
    }

    public bool FindSpace(Item item, int Quant = 1)
    {
        
        //if the item isn't empty
        if (item.id < 5000)
        {
            //and if the item is stackable
            if (item.isStackable())
            {
                //search through the inventory for similar items
                foreach (Item x in Inventory)
                {
                    //as we find them reduce quant until we hit 0
                    if (item.id == x.id)
                    {
                        int toNines = 99 - x.quantity;
                        Quant -= Mathf.Min(toNines, Quant);
                    }
                    else if (x.id >= 5000)
                    {
                        //or if we find an emtpy, dump it all
                        Quant -= Mathf.Min(99, Quant);
                    }
                    //if we hit zero, return true
                    if (Quant == 0)
                    {
                        return true;
                    }
                }
            }
            //if it's not stackable
            else {
                //find an empty space in the inventory
                foreach (Item x in Inventory)
                {
                    if (x.id >= 5000)
                    {
                        //dump it there
                        Quant--;
                        //return true
                    }

                    if (Quant == 0)
                    {
                        return true;
                    }
                }
            }
            //if we fail all of that
            //there's no room, return false;
            return false;
        }
        return true;
    }

    public static void SortInv(List<Item> Inventory)
    {
        SortItemByIndex sort = new SortItemByIndex();
        Inventory.Sort(sort);

        for (int i = 0; i < Inventory.Count; i++)
        {
            int j = i;
            int runsum = 0;
            int countMe = 0;
            if (!Inventory[i].isStackable())
            {
                continue;
            }
            for (; j < Inventory.Count; j++)
            {
                if (Inventory[i].id == Inventory[j].id)
                {
                    runsum += Inventory[j].quantity;
                    Inventory[j].quantity = 0;
                    countMe++;
                }
                else
                {
                    break;
                }

            }
            j = i;

            for (; j < i + countMe; j++)
            {
                if (runsum > 99)
                {
                    Inventory[j].quantity = 99;
                    runsum -= 99;
                }
                else
                {
                    Inventory[j].quantity += runsum;
                    runsum = 0;
                    break;
                }
            }
            j = i;

            for (; j < i + countMe; j++)
            {
                if (Inventory[j].quantity <= 0)
                {
                   Inventory[j] = new Item(Instance.ItemDatabase[0]);
                }
            }
            countMe--;
            if (countMe >= 0)
            {
                i += countMe;
            }
        }
        Inventory.Sort(sort);
    }

    void Update()
    {
        playTimeTicks += Time.unscaledDeltaTime;
    }

    class SortItemByIndex : IComparer<Item>
    {
        int IComparer<Item>.Compare(Item x, Item y)
        {
            return x.CompareTo(y);
        }
    }

    
}
