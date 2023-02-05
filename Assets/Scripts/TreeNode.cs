using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreeNode : MonoBehaviour
{
	[SerializeField] public float maxConnections = 3;
	public int currentConnections = 0;

	public GameObject connectionTextObject;
	public GameObject resourceTextObject;

	public Dictionary<Enums.EResource,int> resourceBasis = new Dictionary<Enums.EResource, int>();
	public Dictionary<Enums.EResource,int> resourceCurrent = new Dictionary<Enums.EResource, int>();


	private List<TreeNode> outgoingConnections = new List<TreeNode>();
	private List<TreeNode> incomingConnections = new List<TreeNode>();

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Node start");
		connectionTextObject.GetComponent<TextMesh>().text = string.Format("{0}/{1}", currentConnections, maxConnections);
    }

	public void InitResources(int W, int N, int P, int K)
	{
		UpdateBasisResource(Enums.EResource.Water, W);
		UpdateBasisResource(Enums.EResource.Nitrogen, N);
		UpdateBasisResource(Enums.EResource.Phosphorus, P);
		UpdateBasisResource(Enums.EResource.Potassium, K);

		UpdateCurrentStateResource(Enums.EResource.Water, W);
		UpdateCurrentStateResource(Enums.EResource.Nitrogen, N);
		UpdateCurrentStateResource(Enums.EResource.Phosphorus, P);
		UpdateCurrentStateResource(Enums.EResource.Potassium, K);

		updateLayout();
	}

	public void tradeResourceToNodeB(TreeNode nodeB, Enums.EResource res, int amount){
		TreeNode nodeA = this;
		nodeA.decreaseCurrentStateResource(res, amount);
		nodeB.increaseCurrentStateResource(res, amount);
		nodeA.updateLayout();
		nodeB.updateLayout();
		// Debug.Log("trade done");
		// Debug.Log(nodeA.getAmountOfResource(Enums.EResource.Water));
	}

	public int getAmountOfResource(Enums.EResource res){
		return resourceCurrent[res];
	}

	public void updateLayout(){
		TextMeshPro tmp_object = resourceTextObject.GetComponent<TextMeshPro>();
		tmp_object.text = "";
		foreach(KeyValuePair<Enums.EResource,int> kvp in resourceCurrent)
		{
			tmp_object.text += string.Format("<sprite=\"R\" index=\"{0}\"> {1}\n", (int) kvp.Key, kvp.Value);
		}
	}

	public void decreaseCurrentStateResource(Enums.EResource res, int amount)
	{
		resourceCurrent[res] -= amount;	
	}

	public void increaseCurrentStateResource(Enums.EResource res, int amount)
	{
		resourceCurrent[res] += amount;	
	}

	public void UpdateCurrentStateResource(Enums.EResource res, int amount)
	{
		if(!resourceCurrent.ContainsKey(res))
		{
			resourceCurrent.Add(res, amount);
		}
		else
		{
			resourceCurrent[res] += amount;
		}
	}


	public void UpdateBasisResource(Enums.EResource res, int amount)
	{
		if(!resourceBasis.ContainsKey(res))
		{
			resourceBasis.Add(res, amount);
		}
		else
		{
			resourceBasis[res] += amount;
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnMouseDown()
	{
		SendMessageUpwards("NodeClicked", this.GetComponent<TreeNode>());
	}

	public bool addConnectionTo(TreeNode otherNode)
	{
		if(outgoingConnections.Contains(otherNode)) return false;

		if(currentConnections < maxConnections)
		{
			outgoingConnections.Add(otherNode);
			currentConnections += 1;
			connectionTextObject.GetComponent<TextMesh>().text = string.Format("{0}/{1}", currentConnections, maxConnections);
			return true;
		}

		return false;
	}

	public bool addConnectionFrom(TreeNode otherNode)
	{
		if(incomingConnections.Contains(otherNode)) return false;

		if(currentConnections < maxConnections)
		{
			incomingConnections.Add(otherNode);
			currentConnections += 1;
			connectionTextObject.GetComponent<TextMesh>().text = string.Format("{0}/{1}", currentConnections, maxConnections);
			return true;
		}

		return false;
	}
}
