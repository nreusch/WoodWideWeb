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
		// GameObject node = Instantiate(nodePrefab, new Vector3(0,0,0), Quaternion.identity, transform);
		// nodes.Add(node.GetComponent<TreeNode>());

		for (int circleI = 0; circleI < amountCirles; circleI++){
			int nodesOnCircle = 10+ 4*(2^circleI);
			float radius = circleI * spaceInBetweenCircles;
			for (int i = 0; i < nodesOnCircle; i++){
				Vector3 nodeLocation = new Vector3(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeY), 0);
				SpawnPrefabOnCircle2D(nodePrefab, radius);
			}
		}
	}

	void InstantiateNode(Vector3 pos){
		TreeNode node = Instantiate(nodePrefab, pos, Quaternion.identity, transform).GetComponent<TreeNode>();
		node.StoreResource(Enums.EResource.Water, 10);
		node.StoreResource(Enums.EResource.Nitrogen, 10);
		node.StoreResource(Enums.EResource.Phosphorus, 10);
		node.StoreResource(Enums.EResource.Potassium, 10);
		nodes.Add(node);
	}

	void SpawnPrefabOnCircle2D(GameObject nodePrefab, float radius)
	{
		Vector3 randomPos = Random.insideUnitSphere * radius;
		randomPos += transform.position;
		randomPos.y = 0f;
		Vector3 centerPoint = new Vector3(0,0,0);
		
		Vector3 direction = randomPos - transform.position;
		direction.Normalize();
		
		float dotProduct = Vector3.Dot(transform.forward, direction);
		float dotProductAngle = Mathf.Acos(dotProduct / transform.forward.magnitude * direction.magnitude);
		
		randomPos.x = Mathf.Cos(dotProductAngle) * radius + centerPoint.x;
		randomPos.y = Mathf.Sin(dotProductAngle * (Random.value > 0.5f ? 1f : -1f)) * radius + centerPoint.y;
		randomPos.z = transform.position.z;
		
		InstantiateNode(randomPos);
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
