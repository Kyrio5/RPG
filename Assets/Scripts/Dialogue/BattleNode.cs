using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleNode : ActionNode {
    public string myBattle;

    public BattleNode(string battleScene, int[] indices, bool exit = false) :
        base(exit, indices)
    {
        myBattle = battleScene;
    }

    
}
