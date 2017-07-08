using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Combatant {

    public Combatant(string name, int[] baseStats, int LV)
    {
        Name = name;
        if (Name != "")
        {
            this.baseStats = baseStats;
            Level = LV;
            StatusEffects = new bool[15];
            rawStats = new int[7];
            SetRawStats();
            SetHPfromVIT();
            SetMPfromMAG();

            HP = MaxHP;
            MP = MaxMP;
            
        }


    }

    public void Die()
    {
        StatusEffects = new bool[15];
        StatusEffects[0] = true;
    }
    
    public Combatant(Combatant copy)
    {
        Name = copy.Name;
        this.baseStats = copy.baseStats;
        Level = copy.Level;
        StatusEffects = copy.StatusEffects;
    }
    
    public string Name;
    public int[] baseStats;
    public int[] rawStats;
    public int HP, MaxHP, MP, MaxMP, Level;

    public bool[] StatusEffects = new bool[15];
    
    public float Fire = 1f;
    public float Lightning = 1f, Ice = 1f, Earth = 1f, Wind = 1f, Water = 1f, Holy = 1f, Darkness = 1f;

    public virtual int[] getBattleStats()
    {
        return rawStats;
    }

    public virtual bool RangedWeapon()
    {
        return false;
    }

    public void SetRawStats()
    {
        rawStats[0] = Mathf.FloorToInt(baseStats[0] + (Level * .3f)); //ATT
        rawStats[1] = Mathf.FloorToInt(baseStats[1] + (Level * .4f)); //DEF
        rawStats[2] = Mathf.FloorToInt(baseStats[2] + (Level * .3f)); //SPR
        rawStats[3] = Mathf.FloorToInt(baseStats[3] + (Level * .15f)); //WIS
        rawStats[4] = Mathf.FloorToInt(baseStats[4] + (Level * .12f)); //MAG
        rawStats[5] = Mathf.FloorToInt(baseStats[5] + (Level * .2f)); //VIT
        rawStats[6] = Mathf.FloorToInt(baseStats[6] + (Level * .1f)); //SPD
    }

    public void SetHPfromVIT()
    {
        MaxHP = Mathf.FloorToInt(rawStats[5] * 250 * (Level / 50f));
    }

    public void SetMPfromMAG()
    {
        MaxMP = Mathf.FloorToInt(rawStats[4] * 200 * (Level / 100f) / 10);
    }

    public bool isKO()
    {
        return StatusEffects[0];
    }

    public bool isBlind()
    {
        return StatusEffects[1];
    }
    public bool isPoisoned()
    {
        return StatusEffects[2];
    }
    public bool isSilenced()
    {
        return StatusEffects[3];
    }
    public bool isPetrified()
    {
        return StatusEffects[4];
    }
    public bool isSlowed()
    {
        return StatusEffects[5];
    }
    public bool isHasted()
    {
        return StatusEffects[6];
    }
    public bool isStopped()
    {
        return StatusEffects[7];
    }
    public bool isShelled()
    {
        return StatusEffects[8];
    }
    public bool isProtected()
    {
        return StatusEffects[9];
    }
    public bool isBerserk()
    {
        return StatusEffects[10];
    }
    public bool isConfused()
    {
        return StatusEffects[11];
    }
    public bool isAutoLifed()
    {
        return StatusEffects[12];
    }
    public bool isReflected()
    {
        return StatusEffects[13];
    }
    public bool isZombied()
    {
        return StatusEffects[14];
    }
}
