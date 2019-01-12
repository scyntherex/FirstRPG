using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockChar : MonoBehaviour {

    private bool canUnlock;
    public int charNumber;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(canUnlock && Input.GetButtonDown("Fire1")
            && PlayerController.instance.canMove)
        {
            GameManager.instance.UnlockChar(charNumber);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            canUnlock = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            canUnlock = false;
    }
}
