using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

	public GameObject nodePrefab;
	public GameObject edgePrefab;
	Vector3 mousePos;

	private List<TreeNode> nodes = new List<TreeNode>();
	private List<GameObject> edges = new List<GameObject>();
	GameObject currentEdge;
	private bool drawing = false;
	private TreeNode originNode;
	
	[SerializeField] private float rangeX = 10;
    [SerializeField] private float rangeY = 5;
 	[SerializeField] private float amountCirles = 4;
	[SerializeField] private float spaceInBetweenCircles = 5;


    // Start is called before the first frame update
    void Start()
    {
		initWorld();
    }

	void initWorld()
	{
			InstantiateNode(new Vector3(5,0,0));
			InstantiateNode(new Vector3(0,0,0));
	}

	void InstantiateNode(Vector3 pos){
		TreeNode node = Instantiate(nodePrefab, pos, Quaternion.identity, transform).GetComponent<TreeNode>();
		node.InitResources(10,10,10,10);
		nodes.Add(node);
	}

    // Update is called once per frame
    void Update()
    {
		if (drawing)
		{
			// If drawing line -> update end position to mouse position
			Vector3 _currentPosition = GetCurrentMousePosition().GetValueOrDefault();
			LineRenderer _lineRenderer = currentEdge.GetComponent<LineRenderer>();
			_lineRenderer.SetPosition(1, _currentPosition);
		}

		if(Input.GetMouseButtonDown(1))
		{
			Vector3 _mousePos = GetCurrentMousePosition().GetValueOrDefault();
			InstantiateNode(_mousePos);
		}
    }


	void NodeClicked(TreeNode node)
	{
		// Debug.Log("Node clicked received");
		if (!drawing)
		{
			originNode = node;
			// If node is clicked and not drawing line -> start drawing line
			drawing = true;

			GameObject edge = Instantiate(edgePrefab);
			edges.Add(edge);
			currentEdge = edge;

			Vector3 _initialPosition = GetCurrentMousePosition().GetValueOrDefault();
			LineRenderer _lineRenderer = currentEdge.GetComponent<LineRenderer>();
			_lineRenderer.SetPosition(0, _initialPosition);
			_lineRenderer.SetPosition(1, _initialPosition);
			_lineRenderer.enabled = true;
		}
		else if (Input.GetMouseButtonDown(0) && drawing && !GameObject.ReferenceEquals(originNode, node))
		{
			// If node is clicked and drawing line -> end drawing line
			if(originNode.currentConnections < originNode.maxConnections && node.currentConnections < node.maxConnections)
			{
				originNode.addConnectionTo(node);
				node.addConnectionFrom(originNode);
				originNode.tradeResourceToNodeB(node, Enums.EResource.Water, 5); // HARDCODED
			}
			else
			{
				LineRenderer _lineRenderer = currentEdge.GetComponent<LineRenderer>();
				Destroy(_lineRenderer);
			}
			drawing = false;
			currentEdge = null;
			originNode = null;
		}
	}

	private Vector3? GetCurrentMousePosition()
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var plane = new Plane(Vector3.forward, Vector3.zero);

		float rayDistance;
		if (plane.Raycast(ray, out rayDistance))
		{
			return ray.GetPoint(rayDistance);
			
		}

		return null;
	}
}
