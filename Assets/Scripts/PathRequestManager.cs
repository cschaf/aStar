using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Die Klasse PathRequestManager.
/// Sie dient dazu mehrere Pfadanfragen zu organizieren.
/// Im Fall das mehrere Einheiten ihren kürzesten Weg zum Ziel benötigen.
/// Um die Performanz zu erhöhen werden damit alle Anfragen in eine Queue gesammelt und nacheinander angearbeitet.
/// Der PathRequestManager ist als Singleton umgesätzt
/// </summary>
public class PathRequestManager : MonoBehaviour {
	static PathRequestManager instance;
	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	PathRequest currentPathRequest;
	bool isProcessingPath;
	Pathfinding pathFinding;


	void Awake(){
		instance = this;
		pathFinding = GetComponent<Pathfinding>();
	}
	
  /// <summary>
  /// Stellt eine neue Pfadanfrage zur Brechnung und versucht sie zu bearbeitet
  /// </summary>
  /// <param name="pathStart"></param>
  /// <param name="pathEnd"></param>
  /// <param name="callback"></param>
	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback){
		PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
		instance.pathRequestQueue.Enqueue(newRequest);
		instance.TryProcessNext();
	}
  /// <summary>
  /// Führ eine Anfrage aus, solange gerade keine andere Anfrage verarbeitet wird
  /// </summary>  
	void TryProcessNext(){
		if(!isProcessingPath && pathRequestQueue.Count > 0){
			currentPathRequest = pathRequestQueue.Dequeue();
			isProcessingPath = true;
			pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
		}
	}
	/// <summary>
  /// Schließt eine Anfrage ab und gibt dessen Ergebnisse an die Callback Funktion
  /// </summary>
  /// <param name="path"></param>
  /// <param name="success"></param>
	public void FinishedProcessingPath(Vector3[] path, bool success){
		currentPathRequest.callback(path, success);
		isProcessingPath = false;
		TryProcessNext();
	}
	/// <summary>
  /// Datenstruktur die eine Anfrage auf einen Pfad darstellt
  /// </summary>
	struct PathRequest{
		public Vector3 pathStart; // Startposition
		public Vector3 pathEnd; // Zielposition
		public Action<Vector3[], bool> callback; //Callback Funktion, die mit dem Ergbnis arbeitet 
		
		public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback){
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}
	}
}
