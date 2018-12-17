using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
	public float ShootingSpeed;
	public Transform seeker;
	public Transform bulletExitArea;
	public GameObject bulletObject;
		
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
			GameObject bullet = getBulletObject();
			Rigidbody rb = bullet.GetComponent<Rigidbody>();
			bullet.transform.rotation = bulletExitArea.transform.rotation;
			bullet.transform.position = bulletExitArea.transform.position;
			rb.AddRelativeForce(0,0,ShootingSpeed, ForceMode.Impulse);
			Destroy(bullet,5);
			yield return new WaitForSeconds(1);
		 }
		
		
	}
	
	GameObject getBulletObject(){
		if(bulletObject == null){
			return Instantiate(Resources.Load("Bullet", typeof(GameObject))) as GameObject;
		}
		else{
			return Instantiate(bulletObject);
		}
		
	}
}
