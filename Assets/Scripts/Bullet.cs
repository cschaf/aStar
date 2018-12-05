using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	Rigidbody rb;
	public float BulletSpeed;
	
	void Start() {		
		rb.GetComponent<Rigidbody>();
		rb.AddRelativeForce(0,0,BulletSpeed, ForceMode.Impulse);
	}
	
}

