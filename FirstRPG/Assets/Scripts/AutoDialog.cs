using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDialog : MonoBehaviour {

    private bool autoActive;
    public string[] dialogLines;
    public bool isPerson;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (autoActive)
        {
            StartCoroutine(StartDialog());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            autoActive = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            autoActive = false;
        }
    }

    public IEnumerator StartDialog()
    {
        PlayerController.instance.canMove = false;
        yield return new WaitForSeconds(1.0f);
        DialogManager.instance.showDialog(dialogLines, isPerson);
        PlayerController.instance.canMove = true;
        Destroy(gameObject);
    }
}
