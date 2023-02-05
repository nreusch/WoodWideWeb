using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreeNode : MonoBehaviour
{
	public int maxConnections;
	public int currentConnections = 0;

	public GameObject textObject;

	public Dictionary<Enums.EResource,int> resourceStorage = new Dictionary<Enums.EResource, int>();

	private List<TreeNode> connections = new List<TreeNode>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Node start");
		textObject.GetComponent<TextMesh>().text = string.Format("{0}/{1}", currentConnections, maxConnections);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnMouseDown()
	{
		Debug.Log("Node clicked");
		SendMessageUpwards("NodeClicked", this.GetComponent<TreeNode>());
	}

	public bool addConnectionTo(TreeNode otherNode)
	{
		if(currentConnections < maxConnections)
		{
			connections.Add(otherNode);
			currentConnections += 1;
			textObject.GetComponent<TextMesh>().text = string.Format("{0}/{1}", currentConnections, maxConnections);
			return true;
		}

		return false;
	}
}
