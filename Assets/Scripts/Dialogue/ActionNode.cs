using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode
{
    public bool exit;
    public int[] branchIndices;

    public ActionNode (bool exit, int[] indices)
    {
        branchIndices = indices;
        this.exit = exit;
    }
    
}

