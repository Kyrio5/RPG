using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character : Combatant
{
    public enum AbilityTypes
    {
        None, Assault, BlackMagic, WhiteMagic, Dragon, Dyne
    };
    public AbilityTypes AbilityType;

    public Character(string name, string charClass, string abilityName, Sprite portrait, int level, List<Item> abilities, int[] baseStats,
        EquipItem.EquipType[] WeapTypes, EquipItem.EquipType[] ArmorTypes, EquipItem.EquipType[] HeadTypes) : base(name, baseStats, level)
    {
        HP = 1;
        MP = 1;
        MaxHP = 1;
        MaxMP = 1;
        Class = charClass;
        this.portrait = portrait;
        startLevel = level;
        AbilityName = abilityName;
        Abilities = abilities;
        this.WeaponTypes = WeapTypes;
        this.ArmorTypes = ArmorTypes;
        this.HeadTypes = HeadTypes;
        Inventory = new List<Item>(5);
        Inventory.Add(new Item(GameDatabase.Instance.ItemDatabase[0]));
        Inventory.Add(new Item(GameDatabase.Instance.ItemDatabase[0]));
        Inventory.Add(new Item(GameDatabase.Instance.ItemDatabase[0]));
        Inventory.Add(new Item(GameDatabase.Instance.ItemDatabase[0]));
        Inventory.Add(new Item(GameDatabase.Instance.ItemDatabase[0]));
    }

    public Character(Character copy) : base(copy)
    {
        Name = copy.Name;
        Class = copy.Class;
        portrait = copy.portrait;
        startLevel = copy.startLevel;
        AbilityName = copy.AbilityName;
        Level = startLevel;
        baseStats = copy.baseStats;
        Abilities = copy.Abilities;
        hand = copy.hand;
        this.WeaponTypes = copy.WeaponTypes;
        this.ArmorTypes = copy.ArmorTypes;
        this.HeadTypes = copy.HeadTypes;
        Inventory = new List<Item>(5);
        for(int i = 0; i < 5; i++)
        {
            if(copy.Inventory[i] is EquipItem)
                Inventory.Add(new EquipItem((EquipItem)copy.Inventory[i]));
            else
                Inventory.Add(new Item(copy.Inventory[i]));
        }
        rawStats = copy.rawStats;
        Primary = copy.Primary;
        Secondary = copy.Secondary;
        Tertiary = copy.Tertiary;
    }

    public string[] LevelUp()
    {
        List<string> skillsLearned = new List<string>();
        while(EXP >= NextEXP)
        {
            Level++;
            EXP -= NextEXP;

            statsByLevel();
            skillsLearned.AddRange(AbilityByLevel());
        }
        
        return skillsLearned.ToArray();
    }

    public enum Statuses
    {
        KO, Blind, Poison, Silence, Petrified, Slow, Haste, Stop, Shell, Protect, Berserk, Confuse, AutoLife, Reflect, Zombie, All
    };

    public enum Handedness
    {
        RIGHT, LEFT, AMBIDEX
    }

    [Header("Personal Info")]
    public string Class, AbilityName;
    public Sprite portrait;
    public Sprite smallPortrait;
    public Handedness hand;
    [Header("Character Growth")]
    public int startLevel;
    public List<Item> Abilities;

    [Header("Stats")]
    public int EXP = 0;
    public int NextEXP;
    public float Primary = 1f, Secondary = .95f, Tertiary = .9f;
    public EquipItem.EquipType[] WeaponTypes;
    public EquipItem.EquipType[] ArmorTypes;
    public EquipItem.EquipType[] HeadTypes;

    [Header("Status")]
    public bool FrontRow;

    [Header("Equipment")]
    public List<Item> Inventory;

    [Header("Eidolon")]
    public Character Eidolon;

    public string GetStatusAsSymbols()
    {
        if (StatusEffects != null)
        {
            string status = "";
            for (int i = 0; i < 14; i++)
            {
                if (StatusEffects[i])
                {
                    status += "<sprite=" + (i + 40) + ">";
                    if(i == 0)
                    {
                        break;
                    }
                }
            }
            return status;
        }
        return "";
    }

    public EquipItem.EquipType[] getProficient()
    {
        return WeaponTypes;
    }

    public void initialize()
    {
        rawStats = new int[7];
        StatusEffects = new bool[15];
        Primary = 1;
        Secondary = .9f;
        Tertiary = .85f;

        Fire = 1;
        Earth = 1;
        Lightning = 1;
        Holy = 1;
        Darkness = 1;
        Ice = 1;
        Water = 1;
        Wind = 1;

        Inventory = new List<Item>(5);
        Inventory.Add(new Item(GameDatabase.Instance.ItemDatabase[0]));
        Inventory.Add(new Item(GameDatabase.Instance.ItemDatabase[0]));
        Inventory.Add(new Item(GameDatabase.Instance.ItemDatabase[0]));
        Inventory.Add(new Item(GameDatabase.Instance.ItemDatabase[0]));
        Inventory.Add(new Item(GameDatabase.Instance.ItemDatabase[0]));


    }

    public List<Item> refineInventory(int selection)
    {
        List<Item> refinedList = new List<Item>();
        switch (selection)
        {
            case 0:
                if (Inventory[1].id < 5000)
                {
                    if (((EquipItem)Inventory[1]).equipType == EquipItem.EquipType.BOW)
                    {
                        for (int i = 0; i < GameDatabase.Instance.Inventory.Count; i++)
                        {
                            if (GameDatabase.Instance.Inventory[i] is EquipItem)
                            {
                                var x = GameDatabase.Instance.Inventory[i] as EquipItem;
                                if (((EquipItem)x).equipType == EquipItem.EquipType.ARROW)
                                    refinedList.Add(x);
                            }
                        }
                        break;
                    }
                    else if (((EquipItem)Inventory[1]).equipType == EquipItem.EquipType.GUN)
                    {
                        for (int i = 0; i < GameDatabase.Instance.Inventory.Count; i++)
                        {
                            if (GameDatabase.Instance.Inventory[i] is EquipItem)
                            {
                                var x = GameDatabase.Instance.Inventory[i] as EquipItem;
                                if (((EquipItem)x).equipType == EquipItem.EquipType.BULLET)
                                    refinedList.Add(x);
                            }
                        }
                        break;
                    }
                }
                for (int i = 0; i < GameDatabase.Instance.Inventory.Count; i++)
                {
                    if (GameDatabase.Instance.Inventory[i] is EquipItem)
                    {
                        var x = GameDatabase.Instance.Inventory[i] as EquipItem;
                        foreach (EquipItem.EquipType y in WeaponTypes)
                        {
                            if (((EquipItem)x).equipType == y)
                                refinedList.Add(x);
                        }
                    }
                }
                break;
            case 1:
                if (Inventory[0].id < 5000)
                {
                    if (((EquipItem)Inventory[0]).equipType == EquipItem.EquipType.BOW)
                    {
                        for (int i = 0; i < GameDatabase.Instance.Inventory.Count; i++)
                        {
                            if (GameDatabase.Instance.Inventory[i] is EquipItem)
                            {
                                var x = GameDatabase.Instance.Inventory[i] as EquipItem;
                                if (((EquipItem)x).equipType == EquipItem.EquipType.ARROW)
                                    refinedList.Add(x);
                            }
                        }
                        break;
                    }
                    else if (((EquipItem)Inventory[0]).equipType == EquipItem.EquipType.GUN)
                    {
                        for (int i = 0; i < GameDatabase.Instance.Inventory.Count; i++)
                        {
                            if (GameDatabase.Instance.Inventory[i] is EquipItem)
                            {
                                var x = GameDatabase.Instance.Inventory[i] as EquipItem;
                                if (((EquipItem)x).equipType == EquipItem.EquipType.BULLET)
                                    refinedList.Add(x);
                            }
                        }
                        break;
                    }
                }

                for (int i = 0; i < GameDatabase.Instance.Inventory.Count; i++)
                {
                    if (GameDatabase.Instance.Inventory[i] is EquipItem)
                    {
                        var x = GameDatabase.Instance.Inventory[i] as EquipItem;
                        foreach (EquipItem.EquipType y in WeaponTypes)
                        {
                            if (((EquipItem)x).equipType == y)
                                refinedList.Add(x);
                        }
                    }
                }

                break;

            case 2:
                for (int i = 0; i < GameDatabase.Instance.Inventory.Count; i++)
                {
                    if (GameDatabase.Instance.Inventory[i] is EquipItem)
                    {
                        var x = GameDatabase.Instance.Inventory[i] as EquipItem;
                        foreach (EquipItem.EquipType y in ArmorTypes)
                        {
                            if (((EquipItem)x).equipType == y)
                                refinedList.Add(x);
                        }
                    }
                }
                break;
            case 3:
                for (int i = 0; i < GameDatabase.Instance.Inventory.Count; i++)
                {
                    if (GameDatabase.Instance.Inventory[i] is EquipItem)
                    {
                        var x = GameDatabase.Instance.Inventory[i] as EquipItem;
                        foreach (EquipItem.EquipType y in HeadTypes)
                        {
                            if (((EquipItem)x).equipType == y)
                                refinedList.Add(x);
                        }
                    }
                }

                break;
            case 4:

                for (int i = 0; i < GameDatabase.Instance.Inventory.Count; i++)
                {
                    if (GameDatabase.Instance.Inventory[i] is EquipItem)
                    {
                        var x = GameDatabase.Instance.Inventory[i] as EquipItem;
                        if (((EquipItem)x).equipType == EquipItem.EquipType.ACCESSORY)
                            refinedList.Add(x);
                    }
                }

                break;
        }
        return refinedList;
    }

    public EquipItem GetHandedWeapon()
    {
        int handIndex = 0;
        if(hand == Handedness.LEFT)
        {
            handIndex = 1;
        }
        if(Inventory[handIndex].id < 5000)
        {
            return (EquipItem)Inventory[handIndex];
        }
        return null;
    }
    public EquipItem GetOppossedWeapon()
    {
        int handIndex = 1;
        if (hand == Handedness.LEFT)
        {
            handIndex = 0;
        }
        if (Inventory[handIndex].id < 5000)
        {
            return (EquipItem)Inventory[handIndex];
        }
        return null;
    }


    public override bool RangedWeapon()
    {
        EquipItem handed = null, ammo = null;
        int handIndex = 0;
        if (hand == Handedness.LEFT)
            handIndex = 1;

        if (Inventory[handIndex] is EquipItem)
        {
            handed = (EquipItem)Inventory[handIndex];
        }
        if (Inventory[Mathf.Abs(handIndex - 1)] is EquipItem)
        {
            ammo = (EquipItem)Inventory[Mathf.Abs(handIndex - 1)];
        }

        if(handed != null)
        {
            if(handed.equipType == EquipItem.EquipType.BOW || 
                handed.equipType == EquipItem.EquipType.GUN)
            {
                if(ammo != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;   
    }

    public void statsByLevel()
    {
        if (baseStats != null)
        {
            rawStats[0] = Mathf.FloorToInt(baseStats[0] + (Level * .3f)); //ATT
            rawStats[1] = Mathf.FloorToInt(baseStats[1] + (Level * .4f)); //DEF
            rawStats[2] = Mathf.FloorToInt(baseStats[2] + (Level * .3f)); //SPR
            rawStats[3] = Mathf.FloorToInt(baseStats[3] + (Level * .15f)); //WIS
            rawStats[4] = Mathf.FloorToInt(baseStats[4] + (Level * .12f)); //MAG
            rawStats[5] = Mathf.FloorToInt(baseStats[5] + (Level * .2f)); //VIT
            rawStats[6] = Mathf.FloorToInt(baseStats[6] + (Level * .1f)); //SPD
            
            float hpPercent = HP / (float)MaxHP;
            float mpPercent = MP / (float)MaxMP;


            MaxHP = Mathf.FloorToInt(rawStats[5] * 250 * (Level / 50f));
            MaxMP = Mathf.FloorToInt(rawStats[4] * 200 * (Level / 100f) / 10);

            NextEXP = Mathf.FloorToInt(100 + Mathf.Pow(1.13122f, Level));


            MaxHP = Mathf.Clamp(MaxHP, 1, 9999);
            HP = Mathf.FloorToInt(MaxHP * hpPercent);
            MaxMP = Mathf.Clamp(MaxMP, 1, 999);
            MP = Mathf.FloorToInt(MaxMP * mpPercent);

        }
    }

    public string[] AbilityByLevel()
    {
        List<string> learnedAbilities = new List<string>();
        switch (AbilityType)
        {
           case AbilityTypes.Assault:
                Abilities = GameDatabase.Instance.Assault;
                break;

            case AbilityTypes.WhiteMagic:
                Abilities = GameDatabase.Instance.WhiteMagic;
                break;
            default:
                Abilities = null;
                break; 
        }

        if (Abilities != null && Abilities.Count > 0)
        {
            foreach (Item x in Abilities)
            {

                if (x is Ability)
                {
                    Ability y = x as Ability;
                    if (y.myCharacter == null)
                    {
                        y.myCharacter = this;
                    }
                    if (y.LevelLearned <= Level)
                    {
                        y.Learned = true;
                        learnedAbilities.Add(y.getNameWithIcon());
                    }
                }
            }
        }

        return learnedAbilities.ToArray();
    }

    public int[] NewItemStats(int slot, EquipItem item)
    {
        Character compCharacter = new Character(this);
        
        compCharacter.equipItem(item, slot, true);
        
        return compCharacter.FullStats(-1);

    }

    public void equipItem(EquipItem item, int slotIndex, bool temp = false)
    {
        EquipItem newItem = new EquipItem(item);
        int holdQuant = item.quantity;
        Dictionary<int, EquipItem> holdItems = new Dictionary<int, EquipItem>();
        bool ammoSafe = false;
        bool safeToEquip = false;

        //first, we check the slots to see if we need to remove anything
        //weapon slots are the most complicated
        if (slotIndex == 0 || slotIndex == 1)
        {
            int clear = 0;
            int otherSlot = (slotIndex == 0) ? 1 : 0;
            //if the item is two-handed, we need to clear out both hands
            if (item.TwoHanded)
            {

                //we need to check if the slots are clear, so keep track in a var;


                //check first slot
                if ((Inventory[slotIndex].id < 5000))
                {
                    //if it's not empty there's a possibility that we're equipping a bow or gun
                    if (item.equipType == EquipItem.EquipType.BOW || item.equipType == EquipItem.EquipType.GUN)
                    {
                        //are we replacing an older firearm?
                        if (item.equipType == (((EquipItem)Inventory[slotIndex])).equipType)
                        {
                            //then we leave anything in the otherslot alone
                            ammoSafe = true;
                        }
                        //if we're equipping a different weapon type, ammmosafe remains false
                    }
                    //regardless of any of this, we need to clear the slot we're equipping to
                    //hold that item
                    holdItems.Add(slotIndex, ((EquipItem)Inventory[slotIndex]));
                    //clear the slot
                    Inventory[slotIndex] = GameDatabase.Instance.ItemDatabase[0];
                    //flag first clear
                    clear++;

                }
                else
                {
                    //if the slot is empty then we just increase clear
                    clear++;
                }

                //check if we successfully cleared the first slot
                if (clear > 0)
                {
                    //now to check the other slot, but only if it wasn't ammoSafe
                    if (!ammoSafe)
                    {
                        if (Inventory[otherSlot].id < 5000)
                        {
                            //hold that item
                            holdItems.Add(otherSlot, ((EquipItem)Inventory[otherSlot]));
                            //clear the slot
                            Inventory[otherSlot] = GameDatabase.Instance.ItemDatabase[0];
                            //flag next clear
                            clear++;
                        }
                        else
                        {
                            clear++;
                        }
                    }
                    else
                    {
                        clear++;
                    }
                }

                //if we cleared both slots
                if (clear == 2)
                {
                    //we flag the slot as safe
                    safeToEquip = true;
                }
            }
            //if it's not 2-handed
            else
            {
                //we should check the otherslot and see if it is
                if ((Inventory[otherSlot]).id < 5000)
                {
                    if (((EquipItem)Inventory[otherSlot]).TwoHanded)
                    {
                        //is it a bow or gun?
                        if (((EquipItem)Inventory[otherSlot]).equipType == EquipItem.EquipType.BOW ||
                            ((EquipItem)Inventory[otherSlot]).equipType == EquipItem.EquipType.GUN)
                        {
                            //then we're equipping ammo
                            clear++;
                        }
                        else {
                            //hold that item
                            holdItems.Add(otherSlot, ((EquipItem)Inventory[otherSlot]));
                            //clear the slot
                            Inventory[otherSlot] = GameDatabase.Instance.ItemDatabase[0];
                            //flag first clear
                            clear++;
                        }
                    }
                    else
                    {
                        //if it's not two handed, then it's clear
                        clear++;
                    }
                }
                else
                {
                    //if it's empty, it's clear
                    clear++;
                }

                //now check the equip slot
                if (Inventory[slotIndex].id < 5000)
                {
                    //if the item is stackable
                    if (item.isStackable())
                    {
                        //check to see if the thing we're replacing is the same stuff
                        if (item.id == Inventory[slotIndex].id)
                        {
                            //then leave it there for now
                            clear++;
                        }
                        //if not, then just treat it the same
                        else {
                            //hold that item
                            holdItems.Add(slotIndex, ((EquipItem)Inventory[slotIndex]));
                            //clear the slot
                            this.Inventory[slotIndex] = GameDatabase.Instance.ItemDatabase[0];
                            //flag second clear
                            clear++;
                        }
                    }
                    else {
                        //hold that item
                        holdItems.Add(slotIndex, ((EquipItem)Inventory[slotIndex]));
                        //clear the slot
                        Inventory[slotIndex] = GameDatabase.Instance.ItemDatabase[0];
                        //flag second clear
                        clear++;
                    }
                }
                else
                {
                    clear++;
                }

                if (clear == 2)
                {
                    safeToEquip = true;
                }
            }
        }
        //if it's any other slot
        else
        {
            if (this.Inventory[slotIndex].id < 5000)
            {
                //hold that item
                holdItems.Add(slotIndex, ((EquipItem)Inventory[slotIndex]));
                //clear the slot
                Inventory[slotIndex] = GameDatabase.Instance.ItemDatabase[0];
                //flag safe to equip
                safeToEquip = true;
            }
            else
            {
                safeToEquip = true;
            }
        }
        //if we can't put them back in, we reverse the entire process
        if (safeToEquip)
        {
            bool fail = false;
            if (!temp)
            {
                foreach (KeyValuePair<int, EquipItem> x in holdItems)
                {
                    if (!GameDatabase.Instance.FindSpace(x.Value, x.Value.quantity))
                    {
                        fail = true;
                        break;
                    }
                }
            }

            if (!fail)
            {
                if (!temp)
                {
                    foreach (KeyValuePair<int, EquipItem> x in holdItems)
                    {
                        GameDatabase.Instance.AddItem(x.Value, x.Value.quantity);
                    }
                }
                holdItems.Clear();

                if (item.isStackable())
                {
                    if (item.equipType == EquipItem.EquipType.ARROW || item.equipType == EquipItem.EquipType.BULLET || item.equipType == EquipItem.EquipType.THROWING)
                    {
                        if (Inventory[slotIndex].id == item.id)
                        {
                            int toNines = 99 - Inventory[slotIndex].quantity;
                            Inventory[slotIndex].quantity += Mathf.Min(item.quantity, toNines);
                            holdQuant -= Mathf.Min(item.quantity, toNines);

                            if (holdQuant == 0)
                            {
                                newItem.quantity = holdQuant;
                                if(!temp)
                                GameDatabase.Instance.Inventory[GameDatabase.Instance.Inventory.IndexOf(item)] = GameDatabase.Instance.ItemDatabase[0];
                            }
                        }
                        else
                        {
                            newItem.quantity = holdQuant;
                            Inventory[slotIndex] = newItem;
                            if(!temp)
                            GameDatabase.Instance.Inventory[GameDatabase.Instance.Inventory.IndexOf(item)] = GameDatabase.Instance.ItemDatabase[0];
                        }
                    }
                    else
                    {
                       holdQuant--;
                        if (holdQuant == 0)
                        {
                            if(!temp)
                            GameDatabase.Instance.Inventory[GameDatabase.Instance.Inventory.IndexOf(item)] = GameDatabase.Instance.ItemDatabase[0];
                        }
                        newItem.quantity = 1;
                        Inventory[slotIndex] = newItem;
                    }
                }
                else if (item.TwoHanded)
                {
                    if (hand == Character.Handedness.LEFT)
                    {
                        Inventory[1] = newItem;
                    }
                    else
                    {
                        Inventory[0] = newItem;
                    }

                    if(!temp && item is EquipItem)
                        GameDatabase.Instance.Inventory[GameDatabase.Instance.Inventory.IndexOf(item)] = GameDatabase.Instance.ItemDatabase[0];
                }
                else
                {
                    Inventory[slotIndex] = newItem;
                    if(!temp && item is EquipItem)
                        GameDatabase.Instance.Inventory[GameDatabase.Instance.Inventory.IndexOf(item)] = GameDatabase.Instance.ItemDatabase[0];
                }

            }
            else
            {
                foreach (KeyValuePair<int, EquipItem> x in holdItems)
                {
                    Inventory[x.Key] = x.Value;
                }
                holdItems.Clear();
            }
        }
        else
        {
            foreach (KeyValuePair<int, EquipItem> x in holdItems)
            {
                Inventory[x.Key] = x.Value;
            }
            holdItems.Clear();
        }

    }

    public int[] FullStats(int exempt = -1)
    {
        int[] returnArray = new int[7];

        for (int i = 0; i < returnArray.Length; i++)
        {
            returnArray[i] = rawStats[i];
            for (int j = 0; j < Inventory.Count; j++)
                if (j != exempt && (Inventory[j] is EquipItem))
                {
                    EquipItem x = Inventory[j] as EquipItem;
                    int bonus = x.stats[i];

                    if (j == 0)
                    {
                        if (hand == Handedness.LEFT)
                        {
                            bool success = false;
                            if (x.equipType == EquipItem.EquipType.ARROW || x.equipType == EquipItem.EquipType.BULLET)
                            {
                                success = true;
                            }
                            if (!success)
                            {
                                if (!x.TwoHanded)
                                    bonus = Mathf.FloorToInt(bonus * .65f);
                            }
                        }
                        if (x.equipType == WeaponTypes[0])
                        {
                            bonus = Mathf.FloorToInt(bonus * Primary);
                        }
                        else if (x.equipType == WeaponTypes[1])
                        {
                            bonus = Mathf.FloorToInt(bonus * Secondary);
                        }
                        else if (x.equipType == WeaponTypes[2])
                        {
                            bonus = Mathf.FloorToInt(bonus * Tertiary);
                        }
                    }
                    else if (j == 1)
                    {
                        if (hand == Handedness.RIGHT)
                        {
                            bool success = false;
                            if (x.equipType == EquipItem.EquipType.ARROW || x.equipType == EquipItem.EquipType.BULLET)
                            {
                                success = true;
                            }
                            if (!success)
                            {
                                if (!x.TwoHanded)   
                                    bonus = Mathf.FloorToInt(bonus * .65f);
                            }
                        }
                        if (x.equipType == WeaponTypes[0])
                        {
                            bonus = Mathf.FloorToInt(bonus * Primary);
                        }
                        else if (x.equipType == WeaponTypes[1])
                        {
                            bonus = Mathf.FloorToInt(bonus * Secondary);
                        }
                        else if (x.equipType == WeaponTypes[2])
                        {
                            bonus = Mathf.FloorToInt(bonus * Tertiary);
                        }
                    }
                    returnArray[i] += bonus;
                }
        }

        return returnArray;
    }


    public override int[] getBattleStats()
    {
        return FullStats(-1);
    }

    public Character instantiate()
    {
        return new Character(this);
    }
}