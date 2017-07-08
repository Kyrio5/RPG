using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {

    public enum ItemType
    {
      NULL, USE, ABILITY, EQUIP  
    };

    public int id;
    public ItemType ListType = ItemType.NULL;
    protected string itemName;
    int spriteIndex;


    string itemDescription;
    bool stackable;

    public int value;
    public int quantity = 1;

    public Item(int id, string name, string description, int sprite, bool stack, int price = 0)
    {
        this.id = id;
        itemName = name;
        itemDescription = description;
        spriteIndex = sprite;
        stackable = stack;
        value = price;
    }

    public Item(Item copy)
    {
        this.id = copy.id;
        itemName = copy.itemName;
        itemDescription = copy.itemDescription;
        spriteIndex = copy.spriteIndex;
        stackable = copy.stackable;
        ListType = copy.ListType;
        value = copy.value;
    }

    public bool isStackable()
    {
        return stackable;
    }

    public int CompareTo(Item item)
    {
        if (this.id < item.id)
        {
            return -1;
        }
        else if (this.id > item.id)
        {
            return 1;
        }
        else
        {
            if (this.quantity > item.quantity)
            {
                return -1;
            }
            else if (this.quantity < item.quantity)
            {
                return 1;
            }
            else
                return 0;
        }
    }

    public virtual bool Usable()
    {
        return false;
    }

    public virtual bool Activate(int target, List<Combatant> targetGroup)
    {
        return false;
    }


    public string getNameWithIcon()
    {
        if(itemName == "")
        {
            return itemName;
        }
        return "<sprite=" + spriteIndex + ">" + itemName;
    }

    public string getNameForBubble()
    {
        if(itemName == "")
        {
            return itemName;
        }
        return "*" + spriteIndex + itemName;
    }

    public string getName()
    {
        return itemName;
    }

    public string getDescription()
    {
        return itemDescription;
    }


    public static bool Potion(Combatant target)
    {
        if (target != null)
        {
            if (!target.StatusEffects[0])
            {
                if (target.HP < target.MaxHP)
                {
                    EffectFunctions.Heal(target, 150);
                    return true;
                }
            }
        }
        return false;
    }

    public static bool Hi_Potion(Combatant target)
    {
        if (target != null)
        {
            if (!target.StatusEffects[0])
            {
                if (target.HP < target.MaxHP)
                {
                    EffectFunctions.Heal(target, 500);
                    return true;
                }
            }
        }
        return false;
    }

    public static bool XPotion(Combatant target)
    {
        if (target != null)
        {
            if (!target.StatusEffects[0])
            {
                if (target.HP < target.MaxHP)
                {
                    EffectFunctions.Heal(target, 9999);
                    return true;
                }
            }
        }
        return false;
    } 

    public static bool Ether(Combatant target)
    {
        if (target != null)
        {
            if (!target.StatusEffects[0])
            {
                if (target.MP < target.MaxMP)
                {
                    EffectFunctions.MPHeal(target, 50);
                    return true;
                }
            }
        }
        return false;
    }

    public static bool FullEther(Combatant target)
    {
        if (target != null)
        {
            if (!target.StatusEffects[0])
            {
                if (target.MP < target.MaxMP)
                {
                    EffectFunctions.MPHeal(target, 9999);
                    return true;
                }
            }
        }
        return false;
    }

    public static bool Elixer(Combatant target)
    {
        if (target != null)
        {
            if (!target.StatusEffects[0])
            {
                if (target.HP < target.MaxHP || target.MP < target.MaxMP)
                {
                    EffectFunctions.Heal(target, 9999);
                    EffectFunctions.MPHeal(target, 9999);
                    return true;
                }
            }
        }
        return false;
    }

    public static bool Megalixer(List<Combatant> targetGroup)
    {
        int successes = 0;
        for (int i = 0; i < targetGroup.Count; i++)
        {
            if (Elixer(targetGroup[i]))
            {
                successes++;
            }
        }
        return successes > 0;
    }

    public static bool PhoenixDown(Combatant target)
    {
        if (target != null)
        {
            if (target.HP == 0 && target.StatusEffects[0])
            {
                EffectFunctions.Revive(target);
                return true;
            }
        }
        return false;

    }

    public static bool MegaPhoenix(List<Combatant> targetGroup)
    {

        int successes = 0;
        for (int i = 0; i < targetGroup.Count; i++)
        {
            if (PhoenixDown(targetGroup[i]))
            {
                successes++;
            }
        }
        return successes > 0;
    }

    public static bool EyeDrops(Combatant target)
    {
        if (target != null)
        {
            if (target.StatusEffects[(int)Character.Statuses.Blind])
            {
                EffectFunctions.CureEffect(target, Character.Statuses.Blind);
                return true;
            }
        }
        return false;

    }

    public static bool EchoScreen(Combatant target)
    {
        if (target != null)
        {
            if (target.StatusEffects[(int)Character.Statuses.Silence])
            {
                EffectFunctions.CureEffect(target, Character.Statuses.Silence);
                return true;
            }
        }
        return false;
    }

    public static bool Antidote(Combatant target)
    {
        if (target != null)
        {
            if (target.StatusEffects[(int)Character.Statuses.Poison])
            {
                EffectFunctions.CureEffect(target, Character.Statuses.Poison);
                return true;
            }
        }
        return false;
    }

    public static bool Soft(Combatant target)
    {
        if (target != null)
        {
            if (target.StatusEffects[(int)Character.Statuses.Petrified])
            {
                EffectFunctions.CureEffect(target, Character.Statuses.Petrified);
                return true;
            }
        }
        return false;
    }

    public static bool Tranquilizer(Combatant target)
    {
        if (target != null)
        {
            if (target.StatusEffects[(int)Character.Statuses.Berserk])
            {
                EffectFunctions.CureEffect(target, Character.Statuses.Berserk);
                return true;
            }
        }
        return false;
    }

    public static bool MagicTag(Combatant target)
    {
        if (target != null)
        {
            if (target.StatusEffects[(int)Character.Statuses.Zombie])
            {
                EffectFunctions.CureEffect(target, Character.Statuses.Zombie);
                return true;
            }
        }
        return false;
    }
    public static bool Panacea(Combatant target)
    {
        bool success = EyeDrops(target);
        bool success2 = EchoScreen(target);
        bool success3 = Antidote(target);
        bool success4 = Soft(target);
        bool success5 = Tranquilizer(target);

        return success || success2 || success3 || success4 || success5;
    }

    public Item InstantiateItem()
    {
        return new Item(this);
    }

}
