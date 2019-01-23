using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Die StarGrid Klasse.
/// Sie repräsentiert das Grid in dem sich die Nodes befinden, also das Spielfeld
/// </summary>
public class StarGrid : MonoBehaviour {
    public LayerMask unwalkableMask; // Layer auf dem die Hindernisse liegen 
    public Vector2 gridWorldSize; // Göße des Grids
    public float nodeRadius; // Radius eines Nodes innerhalb des Grids
	  public bool displayGridGizmos; // Debug Variable
	  public float collisionBuffer; // Wert für einen Puffer der um ein Hinderniss angezeigt wird
	  public float seekerHeight; // Größe des Seeker
    private Node[,] grid; // Spielfeld bzw. das eigentliche Grid
	  private LineRenderer lineRenderer;
	  private List<GameObject> nodes; // Liste aller Nodes
	  float nodeDiameter; // Durchmesser eines Nodes
	  int gridSizeX, gridSizeY; // Größe x und Größe y des Spielfeldes

  /// <summary>
  /// Maximale Größe des Spielfeldes
  /// </summary>
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
		  ClearNodes();
          CreateGrid();

      }
    /// <summary>
    /// Debug Visualisierungen
    /// </summary>
    void OnDrawGizmos(){
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
		    if(grid != null && displayGridGizmos){
			    foreach(Node n in grid){
				    Gizmos.color = (n.walkable)?Color.white:Color.red;
				    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
			    }
		    }
     }
	
  /// <summary>
  /// Iteriert durch das Grid und findet alle umliegenden Nachbarn zu einem Node
  /// </summary>
  /// <param name="node">Node dessen Nachbarn ermittelt werden sollen</param>
  /// <returns> Liste aller Nachbarn</returns>
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
	
    /// <summary>
    /// Erstellt Grid und befüllt es mit Nodes. Dabei wird entschieden welcher Node begehbar und welcher nicht begehbar ist.
    /// </summary>
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
	
    /// <summary>
    /// Gibt den Node einer gewünschten Position um Unity Raum wieder.
    /// </summary>
    /// <param name="worldPoint"> Position im Raum von Unity</param>
    /// <returns>Node der auf dem worldPoint liegt</returns>
	  public Node NodeFromWorldPoint(Vector3 worldPoint){
		  float percentX = (worldPoint.x + gridWorldSize.x/2) / gridWorldSize.x;
		  float percentY = (worldPoint.z + gridWorldSize.y/2) / gridWorldSize.y;
		
		  percentX = Mathf.Clamp01(percentX);
		  percentY = Mathf.Clamp01(percentY);
		
		  int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		  int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		
		  return grid[x,y];
	  }
	
	  /// <summary>
    /// Löscht alle Nodes, die nicht mehr benötigt werden für die Visualisierung der Hindernisse
    /// </summary>
	  private void ClearNodes() {
		  for(int i = 0; i < nodes.Count; i++) {
			  if(nodes[i] != null || !ReferenceEquals(nodes[i], null)) {
			    GameObject current = nodes[i];
			    nodes.Remove(nodes[i]);
			    Destroy(current);
			  }
		  }
	  }
	  /// <summary>
    /// Zeichnet einen Cube dort wo ein Hinderniss ist
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
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
