using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagNode : ActionNode
{
    int Flag;
    bool truth;

    public FlagNode(int Flag, bool truth, int[] indices, bool exit = false) : base(exit, indices)
    {
        this.Flag = Flag;
        this.truth = truth;
    }

    public void ChangeFlag()
    {
        GameDatabase.Instance.Flags[Flag] = truth;
    }

}
