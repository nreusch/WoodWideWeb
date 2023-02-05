using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Edge
{
    TreeNode nodeA;
    TreeNode nodeB;
    public Dictionary<Enums.EResource,int> resources = new Dictionary<Enums.EResource, int>();

    public void makeConnection(TreeNode nodeA, TreeNode nodeB){
        this.nodeA = nodeA;
        this.nodeB = nodeB;
    }

    public void setTradeResources(int W, int N, int P, int K){
        setResource(Enums.EResource.Water, W);
		setResource(Enums.EResource.Nitrogen, N);
		setResource(Enums.EResource.Phosphorus, P);
		setResource(Enums.EResource.Potassium, K);
    }

    public Dictionary<Enums.EResource,int> getTradeResources(){
        return resources;
    }

    public void setResource(Enums.EResource res, int amount)
	{
		if(!resources.ContainsKey(res))
		{
			resources.Add(res, amount);
		}
		else
		{
			resources[res] = amount;
		}
	}


    public bool isConnectrionFromAToB(TreeNode nodeA, TreeNode nodeB){
        if (this.nodeA == nodeA && this.nodeB == nodeB)
            return true;
        return false;
    }
}
