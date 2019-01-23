using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Die Klasse Turret.
/// Sie repräsentiert eine schießende Einheit, dabei kann definiert
/// werden was geschossen wird und aus welcher Position des Schießenden das
/// Projektil austritt
/// </summary>
public class Turret : MonoBehaviour {
	public float ShootingSpeed; // Schussgeschwindigkeit
	public Transform seeker; // Referenz auf den Sucher
	public Transform bulletExitArea; // Bereich aus dem das Projektil geschossen werden soll
	public GameObject bulletObject; // Pojektil, das geschossenw werden soll
		
	void Update () {
		transform.LookAt(seeker);
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Seeker"){
			StartCoroutine("Shooting");
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Seeker"){
			StopCoroutine("Shooting");
		}
	}
  /// <summary>
  /// Schießt ein Projektil in Richtung des Seekers
  /// </summary>
  /// <returns></returns>
  IEnumerator Shooting(){
		 while(true){
			GameObject bullet = getBulletObject();
			Rigidbody rb = bullet.GetComponent<Rigidbody>();
			bullet.transform.rotation = bulletExitArea.transform.rotation;
			bullet.transform.position = bulletExitArea.transform.position;
			rb.AddRelativeForce(0,0,ShootingSpeed, ForceMode.Impulse);
			Destroy(bullet,5);
			yield return new WaitForSeconds(1);
		 }
		
		
	}
	/// <summary>
  /// Instaziert das zugewiesene Projektil sofern vorhanden
  /// sonst erstelle ein Delault Projektil
  /// </summary>
  /// <returns></returns>
	GameObject getBulletObject(){
		if(bulletObject == null){
			return Instantiate(Resources.Load("Bullet", typeof(GameObject))) as GameObject;
		}
		else{
			return Instantiate(bulletObject);
		}
		
	}
}
