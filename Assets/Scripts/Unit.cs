using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Die Unit Klasse.
/// Sie repräsentiert den Sucher/Seeker, der zum Ziel gehen muss
/// </summary>
public class Unit : MonoBehaviour {

	public Transform target;
	public float speed; // Geschwindigkeit mit der die Unit läuft
	public Text winnerText; // Anzeigetext der dem Spieler zu Ende des Spiel angezeigt wird
	private LineRenderer lineRenderer;
	private List<GameObject> connectors; //  Liste mit Verbindungselementen für den Simplyfied Pfad
	Vector3[] path;
	int targetIndex;
	
	void Start(){
		connectors = new List<GameObject>();
		// Get the line renderer component
		lineRenderer = gameObject.GetComponent<LineRenderer>();
		lineRenderer.startColor = Color.black;
	}
	
	void Update(){
		//ClearConnectors();
		PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

	}
  /// <summary>
  /// Zeichnet den Pfad, den die Unit läuft entlang der Nodes
  /// </summary>
  /// <returns></returns>
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
          //Draw connector sphere
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
  /// <summary>
  /// Zeichnet eine Sphere als Connector zwischen den Nodes
  /// </summary>
  /// <param name="pos">Position</param>
  private void DrawSphere(Vector3 pos) {
    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    sphere.transform.localScale = new Vector3(1, 1, 1);
    sphere.transform.position = pos;
    sphere.GetComponent<Renderer>().material.color = Color.black;
    connectors.Add(sphere);
  }

  /// <summary>
  /// Bereinigt die Connector wenn sie nicht mehr benötigt werden
  /// </summary>
  private void ClearConnectors() {
    for(int i = 0; i < connectors.Count; i++) {
		if(connectors[i] != null || !ReferenceEquals(connectors[i], null)) {
          GameObject current = connectors[i];
          connectors.Remove(connectors[i]);
          Destroy(current);
        }
    }
  }

  /// <summary>
  /// Löst Coroutine zum Bewegen der Unit zum Ziel und Zeichnen des Pfades aus sobald er gefunden wurde
  /// </summary>
  /// <param name="newPath"></param>
  /// <param name="pathSuccessful"></param>
  public void OnPathFound(Vector3[] newPath, bool pathSuccessful){
		if(pathSuccessful){
			path = newPath;
			lineRenderer.positionCount = path.Length;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	/// <summary>
  /// Löst das Bewegen der Unit zum Ziel aus
  /// </summary>
  /// <returns></returns>
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
