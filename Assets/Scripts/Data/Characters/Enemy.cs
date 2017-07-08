using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Combatant
{

    bool[] StatusImmunities;
    public int gilReward;
    public int expReward;
    Item[] itemDrops;


    public Enemy(string name, int[] baseStats, int Level, float[] resistences, bool[] immunities,
        int gilReward, int expReward, Item[] drops, int HP = 0, int MP = 0) :base(name, baseStats, Level)
    {
        this.gilReward = gilReward;
        this.expReward = expReward;
        itemDrops = drops;


        Fire = resistences[0];
        Lightning = resistences[1];
        Ice = resistences[2];
        Earth = resistences[3];
        Wind = resistences[4];
        Water = resistences[5];
        Holy = resistences[6];
        Darkness = resistences[7];



        if (immunities == null)
        {
            StatusImmunities = new bool[15];
        }
        else
        StatusImmunities = immunities;
        
        if(HP > 0)
        {
            MaxHP = HP;
        }
        if(MP > 0)
        {
            MaxMP = MP;
        }

    }

    public Enemy(Enemy copy) : base(copy)
    {
        Fire = copy.Fire;
        Lightning = copy.Lightning;
        Ice = copy.Ice;
        Earth = copy.Earth;
        Wind = copy.Wind;
        Water = copy.Water;
        Holy = copy.Holy;
        Darkness = copy.Darkness;

        StatusImmunities = copy.StatusImmunities;

    }




}
