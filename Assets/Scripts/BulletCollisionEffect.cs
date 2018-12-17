using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionEffect : MonoBehaviour {
	public CollisionEffect currentEffect;
	private GameObject seeker;
	
	void Start(){
		currentEffect = CollisionEffect.PUSHBACK;
		seeker = GameObject.FindWithTag("Seeker");
	}
	void Awake(){
		

	}
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Seeker"){
			Debug.Log("Bullet collided with Seeker");
			StartCoroutine("ActivateCollisionEffect");
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Seeker"){
			StopCoroutine("ActivateCollisionEffect");
		}
	}
	
	IEnumerator ActivateCollisionEffect(){
		//PlayerController player = (PlayerController)seeker.GetComponent(typeOf(PlayerController));
		//player.activateEffect(currentEffect);
		yield return null;
	}
	
}
