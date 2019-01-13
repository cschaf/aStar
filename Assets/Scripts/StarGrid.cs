using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGrid : MonoBehaviour {
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
	public bool displayGridGizmos;
	public float collisionBuffer;
	public float seekerHeight;
    private Node[,] grid;
	private LineRenderer lineRenderer;
	private List<GameObject> nodes;
	float nodeDiameter;
	int gridSizeX, gridSizeY;
	
	
	public int MaxSize{
		get{return gridSizeX * gridSizeY;}
	}
	
	void Awake(){
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		nodes = new List<GameObject>();
		LineRenderer lineRenderer = GetComponent<LineRenderer>();
		Color c1 = Color.black;
		lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.SetColors(c1, c1);
		
		CreateGrid();
	}

    void Update(){
		StopCoroutine("DrawNode");
		//ClearNodes();
        CreateGrid();

    }

    void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
		if(grid != null && displayGridGizmos){
			foreach(Node n in grid){
				Gizmos.color = (n.walkable)?Color.white:Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
			}
		}
    }
	
	public List<Node> GetNeighbours(Node node){
		List<Node> neighbours = new List<Node>();
		
		for(int x=-1; x <= 1; x++){
			for(int y=-1; y <= 1; y++){
				if(x==0 && y==0)continue; //übergebener Node, überspringen
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				
				if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY){
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}
		
		return neighbours;
	}		
	
	void CreateGrid(){
		grid = new Node[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
		for (int x=0; x < gridSizeX; x++){
			for (int y=0; y < gridSizeY; y++){
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius) + new Vector3(0,seekerHeight/2,0);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius+collisionBuffer, unwalkableMask));
				grid[x,y] = new Node(walkable, worldPoint, x, y);
				if(!walkable)StartCoroutine(DrawNode(grid[x,y]));
			}
		}
	}
	
	public Node NodeFromWorldPoint(Vector3 worldPoint){
		float percentX = (worldPoint.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPoint.z + gridWorldSize.y/2) / gridWorldSize.y;
		
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
		
		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		
		return grid[x,y];
	}
	
	
	private void ClearNodes() {
		for(int i = 0; i < nodes.Count; i++) {
			if(nodes[i] != null || !ReferenceEquals(nodes[i], null)) {
			  GameObject current = nodes[i];
			  nodes.Remove(nodes[i]);
			  Destroy(current);
			}
		}
	}
	
	IEnumerator DrawNode(Node node){
		Vector3 nodePos = node.worldPosition;
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.localScale = new Vector3(nodeDiameter, 0.0001F, nodeDiameter);
        cube.transform.position = new Vector3(nodePos.x, nodePos.y, nodePos.z);
		cube.GetComponent<Renderer>().material.color = Color.red;
		nodes.Add(cube);
		yield return null;
	}
}
