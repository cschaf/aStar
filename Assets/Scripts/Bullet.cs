using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Die Bullet Klasse.
/// Sie repräsentiert ein standard Projektil, was geschossen werden kann. 
/// </summary>
public class Bullet : MonoBehaviour {

  Rigidbody rb;
  public float BulletSpeed;

  void Start() {
    // Fügt ein Rigidbody zum Objekt hinzu und über eine relative Kraft darauf aus.
    rb.GetComponent<Rigidbody>();
    rb.AddRelativeForce(0, 0, BulletSpeed, ForceMode.Impulse);
  }

}

