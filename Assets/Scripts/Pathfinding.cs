using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Die Pathfinding Klasse.
/// Sie ist für das Ausführen des A* Algolithmus zuständig
/// </summary>
public class Pathfinding : MonoBehaviour {
	PathRequestManager requestManager;
	StarGrid grid;
	
	void Awake(){
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<StarGrid>();
	}
  /// <summary>
  /// Startet die Coroutin für das berechnen des kürzesten wegen mittels A*
  /// </summary>
  /// <param name="startPos">Startposition</param>
  /// <param name="targetPos">Zielposition</param>
	public void StartFindPath(Vector3 startPos, Vector3 targetPos){
		StartCoroutine(FindPath(startPos, targetPos));
	}

  /// <summary>
  /// Coroutine die asynchron ausgeführt wird und den kürzesten wegen mittels A* berechnet
  /// </summary>
  /// <param name="startPos">Startposition</param>
  /// <param name="targetPos">Zielposition</param>
  /// <returns></returns>
	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos){
		Vector3[] waypoints = new Vector3[0]; // alle Position/Punkte des Pfades
		bool pathSuccess = false;
		
		Node startNode = grid.NodeFromWorldPoint(startPos); // liefert den StartNode anhand von der Position im Raum
		Node targetNode = grid.NodeFromWorldPoint(targetPos);// liefert den ZielNode anhand von der Position im Raum


    if(startNode.walkable && targetNode.walkable){
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize); // Offene Liste mit Nodes die noch nicht besucht wurden und für den Pfad in Frage kommen
			HashSet<Node> closeSet = new HashSet<Node>(); // Geschlossene Liste mit Nodes die schon besucht worden sind von Algorithmus
			
			openSet.Add(startNode); // füge den StartNode zur offenen Liste hinzu
			
			while(openSet.Count > 0){ // Loop durch die offene Liste
				Node currentNode = openSet.RemoveFirst();
				closeSet.Add(currentNode); // füge den aktuellen Node der geschlossenen Liste hinzu
				// Sollte der aktuelle Node gleich dem ZielNode sein, ist der kürzeste Pfad gefunden
				if(currentNode == targetNode){
					pathSuccess = true;
					break;
				}
				
				foreach(Node neighbour in grid.GetNeighbours(currentNode)){
          // Prüfe ob Nachbar in der geschlossenen Liste oder ein Hindernis ist 
					if(!neighbour.walkable || closeSet.Contains(neighbour)){
						continue;
					}
          // Bestimme den geeignesten Nachbar um weiter zu gehen
					int newMovementCosToNeighbour = currentNode.gCost+ GetDistance(currentNode, neighbour);
          // ist der neue Pfad zum Nachbarn kürzer oder der Nachbar ist nicht in der offenen Liste
					if(newMovementCosToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)){
						neighbour.gCost = newMovementCosToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;
            // Ist der Nachbar nicht in der offenen Liste
						if(!openSet.Contains(neighbour)){
							openSet.Add(neighbour);
						}
						else{
							openSet.UpdateItem(neighbour);
						}
					}
				}
			}
		}
		yield return null;
		if(pathSuccess){
			waypoints = RetracePath(startNode, targetNode);
		}
		requestManager.FinishedProcessingPath(waypoints, pathSuccess);
	}
	/// <summary>
  /// Rückverfolgung des kürzesten Pfades anhand der ParentNodes
  /// </summary>
  /// <param name="startNode">Start Node</param>
  /// <param name="endNode">Ziel Node</param>
  /// <returns></returns>
	Vector3[] RetracePath(Node startNode, Node endNode){
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		
		while(currentNode != startNode){
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		//Simplify the path 
		//Vector3[] waypoints = SimplifyPath(path);
		
		//Use all path nodes
		Vector3[] waypoints = GetNodesPositions(path);
		Array.Reverse(waypoints);
		return waypoints;
	}
	/// <summary>
  /// Vereinfacht den gefundenen Pfad indem er nur die Node von einem Richtungswechsel behält.
  /// </summary>
  /// <param name="path">Liste der Nodes für den Pfad</param>
  /// <returns>Liste mit Nodes nach der Vereinfachung</returns>
	Vector3[] SimplifyPath(List<Node> path){
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;
		
		for(int i=1; i < path.Count; i++){
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
			if(directionNew != directionOld){
				waypoints.Add(path[i].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}
	/// <summary>
  /// Gibt die Positionen der Wegpunkte des Pfades zurück
  /// </summary>
  /// <param name="path"></param>
  /// <returns></returns>
	Vector3[] GetNodesPositions(List<Node> path){
		List<Vector3> waypoints = new List<Vector3>();
		
		for(int i=1; i < path.Count; i++){
			waypoints.Add(path[i].worldPosition);
		}
		return waypoints.ToArray();
	}	
	/// <summary>
  /// Berechnet die Distanz zwischen zwei Nodes
  /// </summary>
  /// <param name="nodeA">Node A</param>
  /// <param name="nodeB">Node B</param>
  /// <returns></returns>
	int GetDistance(Node nodeA, Node nodeB){
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		
		if(dstX > dstY){
			return 14 * dstY + 10 * (dstX-dstY);
		}
		return 14 * dstX + 10 * (dstY - dstX);
	}
}
