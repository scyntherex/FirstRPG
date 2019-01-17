using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItem : MonoBehaviour {

    private bool canDestroy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (canDestroy && Input.GetButtonDown("Fire1")
            && PlayerController.instance.canMove)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            canDestroy = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            canDestroy = false;
    }
}
