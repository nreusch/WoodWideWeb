using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreeNode : MonoBehaviour
{
	public int maxConnections;
	public int currentConnections = 0;

	public GameObject textPrefab;

	private GameObject connectionsText;

	private List<TreeNode> connections = new List<TreeNode>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Node start");
		connectionsText = Instantiate(textPrefab, Camera.main.WorldToScreenPoint(transform.position), transform.rotation, GameObject.Find("Canvas").transform);
		connectionsText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}/{1}", currentConnections, maxConnections);
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
			connectionsText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}/{1}", currentConnections, maxConnections);
			return true;
		}

		return false;
	}
}
