using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Edge
{
    TreeNode nodeA;
    TreeNode nodeB;
    Enums.EResource resource;
    int amount;

    public void makeConnection(TreeNode nodeA, TreeNode nodeB, Enums.EResource res, int amount){
        this.nodeA = nodeA;
        this.nodeB = nodeB;
        this.resource = res;
        this.amount = amount;

        nodeA.decreaseCurrentStateResource(res, amount);
		nodeB.increaseCurrentStateResource(res, amount);
		nodeA.updateLayout();
		nodeB.updateLayout();
    }

    public bool isConnectrionFromAToB(TreeNode nodeA, TreeNode nodeB){
        if (this.nodeA == nodeA && this.nodeB == nodeB)
            return true;
        return false;
    }

}
