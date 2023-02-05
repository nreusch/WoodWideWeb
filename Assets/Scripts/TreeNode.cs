using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreeNode : MonoBehaviour
{
	public int maxConnections;
	public int currentConnections = 0;

	public GameObject connectionTextObject;
	public GameObject resourceTextObject;

	public Dictionary<Enums.EResource,int> resourceStorage = new Dictionary<Enums.EResource, int>();

	private List<TreeNode> connections = new List<TreeNode>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Node start");
		connectionTextObject.GetComponent<TextMesh>().text = string.Format("{0}/{1}", currentConnections, maxConnections);

		
    }

	public void StoreResource(Enums.EResource res, int amount)
	{
		if(resourceStorage.ContainsKey(res))
		{
			resourceStorage.Add(res, amount);
		}
		else
		{
			resourceStorage[res] += amount;
		}

		TextMeshPro tmp_object = resourceTextObject.GetComponent<TextMeshPro>();
		tmp_object.text = "";
		foreach(KeyValuePair<Enums.EResource,int> kvp in resourceStorage)
		{
			tmp_object.text += string.Format("<sprite=\"R\" index=\"{0}\"> {1}\n", (int) kvp.Key, kvp.Value);
		}
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
			connectionTextObject.GetComponent<TextMesh>().text = string.Format("{0}/{1}", currentConnections, maxConnections);
			return true;
		}

		return false;
	}
}
