using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UseItem : Item {
    
    public enum UseType
    {
        Potion, HiPotion, XPotion, Ether, FullEther, Elixer, Megalixer, PhoenixDown, MegaPhoenix, Panacea,
        EyeDrops, EchoScreen, Antidote, Soft, Tranquilizer, MagicTag
    };
    
    public enum Target
    {
        ALLY, ENEMY, ALLALLY, ALLENEMY, ALL
    };

    public UseType ID;
    public Target targets;

    public UseItem(int itemID, string name, string description, int sprite, bool stack, UseType ID, Target Targets, int price = 0, int quant = 0) 
        : base(itemID, name, description, sprite, stack, price)
    {
        this.ID = ID;
        this.targets = Targets;
        this.quantity = quant;
        this.ListType = ItemType.USE;
    }

    public UseItem(UseItem copy)
        : base(copy)
    {
        this.ID = copy.ID;
        this.targets = copy.targets;
    }

    public override bool Usable()
    {
        return true;
    }

    public bool Activate(List<BattleActor> targs)
    {
        List<Combatant> list = new List<Combatant>();
        foreach(BattleActor x in targs)
        {
            list.Add(x.myCombatant);
        }
        switch (ID)
        {
            case UseType.Potion:
                targs[0].ShowDamageText(-150);
                return Item.Potion(targs[0].myCombatant);
            case UseType.HiPotion:
                targs[0].ShowDamageText(-500);
                return Item.Hi_Potion(targs[0].myCombatant);
            case UseType.XPotion:
                targs[0].ShowDamageText(-9999);
                return Item.XPotion(targs[0].myCombatant);
            case UseType.Ether:
                return Item.Ether(targs[0].myCombatant);
                targs[0].ShowMPDamageText(-50);
            case UseType.FullEther:
                return Item.FullEther(targs[0].myCombatant);
                targs[0].ShowMPDamageText(-999);
            case UseType.Elixer:
                targs[0].ShowDamageText(-9999);
                targs[0].ShowMPDamageText(-999);
                return Item.Elixer(targs[0].myCombatant);
            case UseType.Megalixer:
                foreach(BattleActor x in targs)
                {
                    x.ShowDamageText(-9999);
                    x.ShowMPDamageText(-999);
                }
                return Item.Megalixer(list);
            case UseType.PhoenixDown:
                return Item.PhoenixDown(targs[0].myCombatant);
            case UseType.MegaPhoenix:
                return Item.MegaPhoenix(list);
            case UseType.EyeDrops:
                return Item.EyeDrops(targs[0].myCombatant);
            case UseType.Antidote:
                return Item.Antidote(targs[0].myCombatant);
            case UseType.EchoScreen:
                return Item.EchoScreen(targs[0].myCombatant);
            case UseType.Soft:
                return Item.Soft(targs[0].myCombatant);
            case UseType.Tranquilizer:
                return Item.Tranquilizer(targs[0].myCombatant);
            case UseType.MagicTag:
                return Item.MagicTag(targs[0].myCombatant);
            case UseType.Panacea:
                return Item.Panacea(targs[0].myCombatant);
        }
        return false;
    }

    public override bool Activate(int target, List<Combatant> targetGroup)
    {

            switch (ID)
            {
                case UseType.Potion:
                    return Item.Potion(targetGroup[target]);
                case UseType.HiPotion:
                    return Item.Hi_Potion(targetGroup[target]);
                case UseType.XPotion:
                    return Item.XPotion(targetGroup[target]);
                case UseType.Ether:
                    return Item.Ether(targetGroup[target]);
                case UseType.FullEther:
                    return Item.FullEther(targetGroup[target]);
                case UseType.Elixer:
                    return Item.Elixer(targetGroup[target]);
                case UseType.Megalixer:
                    return Item.Megalixer(targetGroup);
                case UseType.PhoenixDown:
                    return Item.PhoenixDown(targetGroup[target]);
                case UseType.MegaPhoenix:
                    return Item.MegaPhoenix(targetGroup);
                case UseType.EyeDrops:
                    return Item.EyeDrops(targetGroup[target]);
                case UseType.Antidote:
                    return Item.Antidote(targetGroup[target]);
                case UseType.EchoScreen:
                    return Item.EchoScreen(targetGroup[target]);
                case UseType.Soft:
                    return Item.Soft(targetGroup[target]);
                case UseType.Tranquilizer:
                    return Item.Tranquilizer(targetGroup[target]);
                case UseType.MagicTag:
                    return Item.MagicTag(targetGroup[target]);
                case UseType.Panacea:
                    return Item.Panacea(targetGroup[target]);
        }
        return false;
    }

    public new UseItem InstantiateItem()
    {
        return new UseItem(this);
    }
}
