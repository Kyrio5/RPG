using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : Item {

    public enum TargetGroups
    {
        MENUUSABLE, MENUUSABLE_ALLABLE, ALLIESFIRST, ENEMIESFIRST, SINGLEALLY, SINGLEENEMY
    };

    public int MPCost;
    public int LevelLearned;
    public bool Learned = false;
    public TargetGroups group;

    public Character myCharacter;

    public bool isUsable()
    {
        return myCharacter.MP >= MPCost;
    }

    public Ability(int id, string name, string description, int sprite, int cost, int level, TargetGroups groups) : 
        base(id, name,description,sprite,false)
    {
        MPCost = cost;
        LevelLearned = level;
        group = groups;
        ListType = ItemType.ABILITY;
    }

    public bool LifeSpell(int target, List<Combatant> targetGroup)
    {
        int basePower;
        int bonusPower;

        if (target >= 0)
        {
            basePower = 12;
            bonusPower = Mathf.FloorToInt(myCharacter.FullStats(-1)[3] + Random.Range(1, 256) % Mathf.FloorToInt((((myCharacter.Level + myCharacter.FullStats(-1)[3]) / 8f) + 1)));
            return Revive(targetGroup[target], basePower * bonusPower);
        }
        else
        {
            basePower = 10;
            bonusPower = Mathf.FloorToInt(myCharacter.FullStats(-1)[3] + Random.Range(1, 256) % Mathf.FloorToInt((((myCharacter.Level + myCharacter.FullStats(-1)[3]) / 8f) + 1)));
            int successes = 0;
            for(int i = 0; i < targetGroup.Count; i++)
            {
                if(Revive(targetGroup[i], basePower * bonusPower))
                {
                    successes++;
                }
            }
            return successes > 0;
        }
    }
    public bool FullLifeSpell(int target, List<Combatant> targetGroup)
    {
        if (target >= 0)
        {
            return Revive(targetGroup[target], 9999);
        }
        else
        {
            int successes = 0;
            for (int i = 0; i < targetGroup.Count; i++)
            {
                if (Revive(targetGroup[i], 9999))
                {
                    successes++;
                }
            }
            return successes > 0;
        }
    }
    public bool CureSpell(int target, List<Combatant> targetGroup)
    {
        int basePower;
        int bonusPower;

        if (target >= 0)
        {
            basePower = 16;
            bonusPower = Mathf.FloorToInt(myCharacter.FullStats(-1)[3] + Random.Range(1, 256) % Mathf.FloorToInt((((myCharacter.Level + myCharacter.FullStats(-1)[3]) / 8f) + 1)));
            return Heal(targetGroup[target], basePower * bonusPower);
        }
        else
        {
            basePower = 8;
            bonusPower = Mathf.FloorToInt(myCharacter.FullStats(-1)[3] + Random.Range(1, 256) % Mathf.FloorToInt((((myCharacter.Level + myCharacter.FullStats(-1)[3]) / 8f) + 1)));
            int successes = 0;
            for (int i = 0; i < targetGroup.Count; i++)
            {
                if (Heal(targetGroup[i], basePower * bonusPower))
                {
                    successes++;
                }
            }
            return successes > 0;
        }
    }
    public bool CuraSpell(int target, List<Combatant> targetGroup)
    {
        int basePower;
        int bonusPower;

        if (target >= 0)
        {
            basePower = 38;
            bonusPower = Mathf.FloorToInt(myCharacter.FullStats(-1)[3] + Random.Range(1, 256) % Mathf.FloorToInt((((myCharacter.Level + myCharacter.FullStats(-1)[3]) / 8f) + 1)));
            return Heal(targetGroup[target], basePower * bonusPower);
        }
        else
        {
            basePower = 25;
            bonusPower = Mathf.FloorToInt(myCharacter.FullStats(-1)[3] + Random.Range(1, 256) % Mathf.FloorToInt((((myCharacter.Level + myCharacter.FullStats(-1)[3]) / 8f) + 1)));
            int successes = 0;
            for (int i = 0; i < targetGroup.Count; i++)
            {
                if (Heal(targetGroup[i], basePower * bonusPower))
                {
                    successes++;
                }
            }
            return successes > 0;
        }
    }
    public bool CuragaSpell(int target, List<Combatant> targetGroup)
    {
        int basePower;
        int bonusPower;

        if (target >= 0)
        {
            basePower = 107;
            bonusPower = Mathf.FloorToInt(myCharacter.FullStats(-1)[3] + Random.Range(1, 256) % Mathf.FloorToInt((((myCharacter.Level + myCharacter.FullStats(-1)[3]) / 8f) + 1)));
            return Heal(targetGroup[target], basePower * bonusPower);
        }
        else
        {
            basePower = 100;
            bonusPower = Mathf.FloorToInt(myCharacter.FullStats(-1)[3] + Random.Range(1, 256) % Mathf.FloorToInt((((myCharacter.Level + myCharacter.FullStats(-1)[3]) / 8f) + 1)));
            int successes = 0;
            for (int i = 0; i < targetGroup.Count; i++)
            {
                if (Heal(targetGroup[i], basePower * bonusPower))
                {
                    successes++;
                }
            }
            return successes > 0;
        }
    }

    public bool Stona(int target, List<Combatant> targetGroup)
    {
        
        if (target >= 0)
        {
            return UseItem.Soft(targetGroup[target]);
        }
        else
        {
            int successes = 0;
            for (int i = 0; i < targetGroup.Count; i++)
            {
                if (UseItem.Soft(targetGroup[target]))
                {
                    successes++;
                }
            }
            return successes > 0;
        }
    }
    public bool Esuna(int target, List<Combatant> targetGroup)
    {

        if (target >= 0)
        {
            return UseItem.Panacea(targetGroup[target]);
        }
        else
        {
            int successes = 0;
            for (int i = 0; i < targetGroup.Count; i++)
            {
                if (UseItem.Panacea(targetGroup[target]))
                {
                    successes++;
                }
            }
            return successes > 0;
        }
    }

    public override bool Activate(int target, List<Combatant> targetGroup)
    {
        bool success = false;
        switch (itemName)
        {
            //WhiteMagic
            case "":
                success = false;
                break;
            case "Cure":
                success = CureSpell(target, targetGroup);
                break;
            case "Cura":
                success = CuraSpell(target, targetGroup);
                break;
            case "Curaga":
                success = CuragaSpell(target, targetGroup);
                break;
            case "Life":
                success = LifeSpell(target, targetGroup);
                break;
            case "Full-Life":
                success = FullLifeSpell(target, targetGroup);
                break;
            case "Stona":
                success = Stona(target, targetGroup);
                break;
            case "Esuna":
                success = Esuna(target, targetGroup);
                break;
        }
        if (success)
        {
            myCharacter.MP -= MPCost;
        }
        return success;
    }

    public bool Revive(Combatant target, int power)
    {
        if (target != null)
        {
            if (target.StatusEffects[0])
            {
                EffectFunctions.Revive(target);
                EffectFunctions.Heal(target, power);
                return true;
            }
            return false;
        }
        return false;
    }

    public bool Heal(Combatant target, int power)
    {
        if (target != null)
        {
            if (target.HP < target.MaxHP)
            {
                EffectFunctions.Heal(target, power);
                return true;
            }
            return false;
        }
        return false;
    }
}
