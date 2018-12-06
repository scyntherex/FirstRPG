using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;

	// Use this for initialization
	void Start () {
        target = PlayerController.instance.transform;
	}

    // LateUpdate is called once per frame after Update
    void LateUpdate () {
        transform.position = 
            new Vector3(target.position.x, 
            target.position.y, 
            transform.position.z);
	}
}
