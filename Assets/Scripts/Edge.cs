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

    public bool isConnectrionFromAToB(TreeNode nodeA, TreeNode nodeB, Enums.EResource res){
        if (this.nodeA == nodeA && this.nodeB == nodeB && this.resource == res)
            return true;
        return false;
    }

    public void updateAmount(int amount){
        // reset to original state
        nodeA.increaseCurrentStateResource(this.resource, this.amount);
		nodeB.decreaseCurrentStateResource(this.resource, this.amount);

        // update to new state
        this.amount = amount;
        nodeA.decreaseCurrentStateResource(this.resource, amount);
		nodeB.increaseCurrentStateResource(this.resource, amount);

        // update layout
		nodeA.updateLayout();
		nodeB.updateLayout();
    }

}
