using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public Transform target;
	public float speed;
  public Text winnerText;
  private LineRenderer lineRenderer;
  private List<GameObject> connectors;
	Vector3[] path;
	int targetIndex;
	
	void Start(){
    connectors = new List<GameObject>();
    // Get the line renderer component
    lineRenderer = gameObject.GetComponent<LineRenderer>();
    lineRenderer.startColor = Color.black;
  }
	
	void Update(){
		PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    UpdateSphereConnectors();
	}

  IEnumerator DrawUnitPath() {
    if(path != null){
      //List<GameObject> spheres = new List<GameObject>();
      if(path.Length == 1) {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, path[0]);
        lineRenderer.SetPosition(1, transform.position);
      }
      else if (path.Length > 1) {
        for(int i = targetIndex; i < path.Length; i++) {
          // Draw connector sphere
          //DrawSphere(path[i]);
          if(i == targetIndex) {
            lineRenderer.SetPosition(i, path[i]);
            lineRenderer.SetPosition(i, transform.position);
          }
          else {
            lineRenderer.SetPosition(i, path[i - 1]);
            lineRenderer.SetPosition(i, path[i]);
          }
        }
      }
    }
    yield return null;

  }

  private void DrawSphere(Vector3 pos) {
    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    sphere.transform.localScale = new Vector3(5, 5, 5);
    sphere.transform.position = pos;
    sphere.GetComponent<Renderer>().material.color = Color.black;
    connectors.Add(sphere);
  }

  private void UpdateSphereConnectors() {
    for(int i = 0; i < connectors.Count; i++) {
      if(connectors[i].transform.position.z < gameObject.transform.position.z) {
        if(connectors[i] != null || !ReferenceEquals(connectors[i], null)) {
          GameObject current = connectors[i];
          connectors.Remove(connectors[i]);
          Destroy(current);
        }
      }
    }
  }

  public void OnPathFound(Vector3[] newPath, bool pathSuccessful){
		if(pathSuccessful){
        path = newPath;
        lineRenderer.positionCount = path.Length;
			  StopCoroutine("FollowPath");
		  	StartCoroutine("FollowPath");
		}
	}
	
	public void OnDrawGizmos(){
		//if(path != null){
		//	for(int i=targetIndex; i<path.Length; i++){
		//		Gizmos.color = Color.black;
		//		Gizmos.DrawCube(path[i], Vector3.one);
		//		if(i == targetIndex){
		//			Gizmos.DrawLine(transform.position, path[i]);
		//		}
		//		else{
		//			Gizmos.DrawLine(path[i-1], path[i]);
		//		}
		//	}
		//}
	}
	
	IEnumerator FollowPath(){
		   if(winnerText.text == "Verloren" || winnerText.text == "Gewonnen"){
			   // do nothing or add a restart game button
		   } else {
			   if (path.Length > 0)
			   {
          StopCoroutine("DrawUnitPath");
          StartCoroutine("DrawUnitPath");
          Vector3 currentWaypoint = path[0];

				   while (true)
				   {
					   if (transform.position == currentWaypoint)
					   {
						   targetIndex++;
						   if (targetIndex >= path.Length)
						   {
							   yield break;
						   }
						   currentWaypoint = path[targetIndex];
					   }
					   transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
					   yield return null;
				   }
			   }
			   else
			   {
				   if (!(winnerText.text == "Verloren"))
				   {
					   winnerText.text = "Gewonnen";
				   }
			   }
		   }
		}
}
