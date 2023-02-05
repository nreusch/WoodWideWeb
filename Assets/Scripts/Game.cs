using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

	public GameObject nodePrefab;
	public GameObject edgePrefab;
	Vector3 mousePos;
	Vector3 initialPos;

	private List<TreeNode> nodes = new List<TreeNode>();
	private List<GameObject> edges = new List<GameObject>();
	private List<Edge> listEdges = new List<Edge>();


	GameObject currentEdge;
	private bool drawing = false;
	private TreeNode nodeA;
	private TreeNode nodeB;
	private Edge edgeAB;

	
	[SerializeField] private List<int> nodeAmountsPerCircle = new List<int>();
	[SerializeField] private float spaceInBetweenCircles = 5f;
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
		Vector3 point = new Vector3(0,0,0);
		InstantiateNode(point);

		for(int j = 0; j < nodeAmountsPerCircle.Count; j++)
		{
			float radius = spaceInBetweenCircles * (j+1);
			for (int i = 0; i < nodeAmountsPerCircle[j]; i++){
				/* Distance around the circle */  
				var radians = 2 * Mathf.PI / nodeAmountsPerCircle[j] * i;
				
				/* Get the vector direction */ 
				var vertical = Mathf.Sin(radians);
				var horizontal = Mathf.Cos(radians); 
				
				var spawnDir = new Vector3 (horizontal, vertical, 0);
				
				/* Get the spawn position */ 
				var spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point
				
				InstantiateNode(spawnPos);
			}
		}
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

			_lineRenderer.SetPosition(0, initialPos);
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

			initialPos = GetCurrentMousePosition().GetValueOrDefault();
			LineRenderer _lineRenderer = currentEdge.GetComponent<LineRenderer>();
			_lineRenderer.SetPosition(0, initialPos);
			_lineRenderer.SetPosition(1, initialPos);
			_lineRenderer.enabled = true;
			_lineRenderer.material.mainTextureScale = new Vector2(2f,1f);
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
