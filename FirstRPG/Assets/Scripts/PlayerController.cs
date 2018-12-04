using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D TestRigidBody;
    public float moveSpeed;

    public Animator myAnim;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        TestRigidBody.velocity =
        new Vector2(Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")) * moveSpeed;

        myAnim.SetFloat("moveX", TestRigidBody.velocity.x);
        myAnim.SetFloat("moveY", TestRigidBody.velocity.y);

        if(Input.GetAxisRaw("Horizontal") == 1 ||
            Input.GetAxisRaw("Horizontal") == -1 || 
            Input.GetAxisRaw("Vertical") == 1 ||
            Input.GetAxisRaw("Vertical") == -1){
                myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }
    }
}
