using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Die Node Klasse.
/// Sie repräsentiert einen Node auf dem Spielfeld.
/// </summary>

public class Node :IHeapItem<Node>{
    public bool walkable; // Gibt an ob der Node begehbar ist oder nicht
    public Vector3 worldPosition; // Position im Raum von Unity
	  public Node parent; // Eine Referenz auf den Parent Node
	  public int gridX; // Koordinate x im Grid
	  public int gridY; // Koordinate y im Grid
	  public int gCost; // Distanz zum StartNode
	  public int hCost; // Distanz zum EndNode
	  private int heapIndex; // Position im Heap
	

    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
		    gridX = _gridX;
		    gridY = _gridY;
    }
  /// <summary>
  /// Gesamtkosten des Nodes (gCost + hCost)
  /// </summary>
  public int fCost{
		get{
			return gCost + hCost;
		}
	}
	
	public int HeapIndex{
		get{
			return heapIndex;
		}
		set{
			heapIndex = value;
		}
	}
  /// <summary>
  /// Methode um zwei Nodes zu vergleichen
  /// </summary>
  public int CompareTo(Node nodeToCompare){
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if(compare==0){
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}
}
