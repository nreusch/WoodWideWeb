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
	private List<Edge> listEdges = new List<Edge>();


	GameObject currentEdge;
	private bool drawing = false;
	private TreeNode nodeA;
	private TreeNode nodeB;
	private Edge edgeAB;

	
	[SerializeField] private float rangeX = 10;
    [SerializeField] private float rangeY = 5;
 	[SerializeField] private float amountCirles = 4;
	[SerializeField] private float spaceInBetweenCircles = 5;
	[SerializeField] private bool initConnection = false;
	[SerializeField] GameObject tradeInitiateView;




    // Start is called before the first frame update
    void Start()
    {
		initWorld();
		tradeInitiateView.SetActive(false);

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
			nodeA = node;
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
		else if (Input.GetMouseButtonDown(0) && drawing && !GameObject.ReferenceEquals(nodeA, node))
		{
			nodeB = node;

			// If node is clicked and drawing line -> end drawing line
			if(nodeA.currentConnections < nodeA.maxConnections && node.currentConnections < node.maxConnections)
			{
				// if connection already exists, update the connection.
				if(!connectionExist(nodeA, node)){
					Edge newEdge = new Edge();
					newEdge.makeConnection(nodeA, node);
					listEdges.Add(newEdge);
					edgeAB = newEdge;
					tradeInitiateView.SetActive(true);

					nodeA.addConnectionTo(node);
					node.addConnectionFrom(nodeA);
				} else{
					cancelCurrentConnection(); // stop drawing the connection
					Edge edge = getConnection(nodeA, node); // update the current connection
				}
			}
			else
			{
				cancelCurrentConnection(); 
			}
			currentEdge = null;
			
			drawing = false;
		}
	}

	public void finishMakingConnections(int W, int N, int P, int K){
		tradeInitiateView.SetActive(false);
		tradeResourceFromAToB(W,N,P,K);
		Debug.Log("traded resources");
		edgeAB = null;
		nodeA = null;
		nodeB = null;
		cancelCurrentConnection();
	}

	public void tradeResourceFromAToB(int W, int N, int P, int K){
		edgeAB.setTradeResources(W,N,P,K);
		Dictionary<Enums.EResource,int> resources = edgeAB.getTradeResources();


		// reset to original state
        nodeA.decreaseCurrentStateResources(resources[Enums.EResource.Water],
                                            resources[Enums.EResource.Nitrogen],
                                            resources[Enums.EResource.Phosphorus],
                                            resources[Enums.EResource.Potassium]);
		nodeB.increaseCurrentStateResources(resources[Enums.EResource.Water],
                                            resources[Enums.EResource.Nitrogen],
                                            resources[Enums.EResource.Phosphorus],
                                            resources[Enums.EResource.Potassium]);
        // update layout
		nodeA.updateLayout();
		nodeB.updateLayout();
    }


	private void cancelCurrentConnection(){
		LineRenderer _lineRenderer = currentEdge.GetComponent<LineRenderer>();
		Destroy(_lineRenderer);
	}

	private bool connectionExist(TreeNode a, TreeNode b){
		foreach (Edge edge in listEdges) {
			if (edge.isConnectrionFromAToB(a, b)) return true;
		}
		return false;
	}

	private Edge getConnection(TreeNode a, TreeNode b){
		foreach (Edge edge in listEdges) {
			if (edge.isConnectrionFromAToB(a, b)) return edge;
		}
		return null;
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
