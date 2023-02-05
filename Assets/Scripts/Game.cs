using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

	public GameObject nodePrefab;
	public GameObject edgePrefab;
	Vector3 mousePos;

	private List<GameObject> nodes = new List<GameObject>();
	private List<GameObject> edges = new List<GameObject>();
	GameObject currentEdge;
	private bool drawing = false;
	private TreeNode originNode;
	
	[SerializeField] private float rangeX = 10;
    [SerializeField] private float rangeY = 5;
 	[SerializeField] private float amountCirles = 4;

    // Start is called before the first frame update
    void Start()
    {
		initWorld();
    }

	void initWorld()
	{
		GameObject node = Instantiate(nodePrefab, new Vector3(0,0,0), Quaternion.identity, transform);
		nodes.Add(node);

		for (int i = 0; i < amountNodes; i++){
			Vector3 nodeLocation = new Vector3(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeY), 0);
			// GameObject node = Instantiate(nodePrefab, nodeLocation, Quaternion.identity, transform);
			SpawnPrefabOnCircle2D(nodePrefab, 3f);
			nodes.Add(node);
		}
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
		
		GameObject node = Instantiate(nodePrefab, randomPos, Quaternion.identity);
		node.transform.position = randomPos;
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
			GameObject node = Instantiate(nodePrefab, _mousePos, Quaternion.identity, transform);
			nodes.Add(node);
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
				node.addConnectionTo(originNode);
				
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
