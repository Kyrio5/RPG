using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFunctions {

	public static void Heal(Combatant target, int power)
    {
        target.HP += power;
        if(target.HP > target.MaxHP) { target.HP = target.MaxHP; }
    }

    public static void CureEffect(Combatant target, Character.Statuses effect)
    {
        target.StatusEffects[(int)effect] = false;
        
    }

    public static void MPHeal(Combatant target, int power)
    {
        target.MP += power;
        if (target.MP > target.MaxMP) { target.MP = target.MaxMP; }
    }

    public static int Revive(Combatant target)
    {
        target.StatusEffects[0] = false;
        int value = Mathf.FloorToInt(target.MaxHP * Random.Range(.01f, .05f));
        target.HP += value;
        return value;
    }
}
