﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGrid : MonoBehaviour {
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
	public bool displayGridGizmos;
	public float collisionBuffer;
    private Node[,] grid;
	
	float nodeDiameter;
	int gridSizeX, gridSizeY;
	
	
	public int MaxSize{
		get{return gridSizeX * gridSizeY;}
	}
	
	void Awake(){
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
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
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius+collisionBuffer, unwalkableMask));
				grid[x,y] = new Node(walkable, worldPoint, x, y);
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
}
