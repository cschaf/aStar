using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
	public float ShootingSpeed;
	public Transform seeker;
	public Transform bulletExitArea;
		
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
	
	IEnumerator Shooting(){
		 while(true){
			GameObject bullet = Instantiate(Resources.Load("Bullet", typeof(GameObject))) as GameObject;
			Rigidbody rb = bullet.GetComponent<Rigidbody>();
			bullet.transform.rotation = bulletExitArea.transform.rotation;
			bullet.transform.position = bulletExitArea.transform.position;
			rb.AddRelativeForce(0,0,ShootingSpeed, ForceMode.Impulse);
			Destroy(bullet,2);
			yield return new WaitForSeconds(1);
		 }
		
		
	}
}
