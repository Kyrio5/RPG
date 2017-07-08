using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipItem : Item {
    public static Dictionary<EquipType, int> TypeSprite = new Dictionary<EquipType, int>()
    {
        {EquipType.SWORD, 0 },
        {EquipType.AXE, 1 },
        {EquipType.GSWORD, 2 },
        {EquipType.HAMMER, 3 },
        {EquipType.SCYTHE, 4 },
        {EquipType.STAFF, 5 },
        {EquipType.ROD, 6 },
        {EquipType.BOW, 7 },
        {EquipType.SHIELD, 8 },
        {EquipType.QSTAFF, 9 },
        {EquipType.SPEAR, 10 },
        {EquipType.GUN, 11 },
        {EquipType.TOOL, 12 },
        {EquipType.KNIFE, 13 },
        {EquipType.BOMB, 14 },
        {EquipType.ARROW, 15 },
        {EquipType.BOOT, 16 },
        {EquipType.GLOVE, 17 },
        {EquipType.RAPIER, 18 },
        {EquipType.BULLET, 19 },
        {EquipType.HARMOR, 20 },
        {EquipType.MARMOR, 21 },
        {EquipType.LARMOR, 22 },
        {EquipType.HELM, 26 },
        {EquipType.HAT, 27 },
        {EquipType.BANDANA, 28 },
        {EquipType.THROWING, 29 },
        {EquipType.ACCESSORY, 30 },

    };

    public enum EquipType
    {
        SWORD, GSWORD, AXE,
        HAMMER, SCYTHE, STAFF,
        ROD, BOW, SHIELD, QSTAFF,
        SPEAR, GUN, TOOL,
        KNIFE, BOMB, ARROW,
        BOOT, GLOVE, RAPIER,
        BULLET, HARMOR, MARMOR,
        LARMOR, HELM, HAT, BANDANA,
        THROWING, ACCESSORY
    }

    public EquipType equipType;
    //ATT, DEF, SPR, WIS, MAG, VIT, SPD
    public int[] stats;
    public int FinesseCharge;

    public bool TwoHanded;
    public bool RequiresAmmo;


	public EquipItem(int id, string name, string desc, bool stack, EquipType type,
        int[] stats, bool twoHand, bool ammo, int price = 0) :
        base(id,name,desc,TypeSprite[type],stack, price)
    {
        equipType = type;
        this.stats = stats;
        TwoHanded = twoHand;
        RequiresAmmo = ammo;
        ListType = ItemType.EQUIP;
    }

    public EquipItem(EquipItem copy):
        base(copy)
    {
        equipType = copy.equipType;
        this.stats = copy.stats;
        TwoHanded = copy.TwoHanded;
        RequiresAmmo = copy.RequiresAmmo;
    }

    public float FinessePercent()
    {
        return FinesseCharge / 100f;
    }

    
}
