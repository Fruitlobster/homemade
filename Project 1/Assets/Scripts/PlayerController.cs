using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;             //Floating point variable to store the player's movement speed.
    public float moveHorizontal;
    public float moveVertical = 0.0f;
    private Rigidbody2D rb2d;
    
	// Use this for initialization
	void Start () {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    private void FixedUpdate()
    {
        //Store the current horizontal input in the float moveHorizontal.
        moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        //float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        rb2d.AddForce(movement);

        moveVertical = 0.0f;
    }

    //Determines the jump behaviour of the player


	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb2d.velocity.y) <= 0.01f)
        {
            moveVertical = 50.0f;
        }
        
	}
}
