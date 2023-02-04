using UnityEngine;
using System.Collections;

public class SortLayer : MonoBehaviour {

    public int OrderInLayer = 0;

	// Use this for initialization
	void Start () {
	   MeshRenderer renderer = GetComponent<MeshRenderer>();
       renderer.sortingLayerName = "TextLayer";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}