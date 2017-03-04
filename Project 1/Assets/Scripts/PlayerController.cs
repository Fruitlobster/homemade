using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum state { FREE, HANGING, STUNNED,FLYING } ;

public class PlayerController : MonoBehaviour {

    

    public float speed;             //Floating point variable to store the player's movement speed.
    public float moveHorizontal = 0.0f;
    public float moveVertical = 0.0f;

    state playerState;
    private Rigidbody2D rb2d;
    private Animator animator;
    private float origGrav;
    private BoxCollider2D myBox;
    private float maxSpeed;

    // Use this for initialization
    void Start () {
        
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myBox = GetComponent<BoxCollider2D>();
        origGrav = rb2d.gravityScale * GameLogic.instance.gravity;
        rb2d.gravityScale = origGrav;
        maxSpeed = 100;
        

        playerState = state.FREE;
        Debug.Log(playerState);
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    private void FixedUpdate()
    {

        switch (playerState)
        {
            case state.FREE:
                //Store the current horizontal input in the float moveHorizontal.
                moveHorizontal = Input.GetAxis("Horizontal");

                //Store the current vertical input in the float moveVertical.
                //float moveVertical = Input.GetAxis("Vertical");

                break;
            case state.HANGING:
                if (Jump())
                {
                    moveVertical = 50.0f;
                    rb2d.gravityScale = origGrav;
                    playerState = state.FLYING;
                }
               
                break;
            case state.FLYING:

                break;
            case state.STUNNED:
                break;
                
        }
        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        rb2d.AddForce(movement);

        moveVertical = 0.0f;
    }

    //Determines the climb behaviour of the player
    protected IEnumerator Climb(Vector3 target)
    {
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        float sqrRemainingDistance = (transform.position - target).sqrMagnitude;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(rb2d.position, target, 10* Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2d.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - target).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
       if(other.collider.tag == "Floor" && other.collider.transform.position.y >= transform.position.y)
        {
            playerState = state.HANGING;
            rb2d.gravityScale = 0f;
            myBox.enabled = false;
            //StartCoroutine(Climb(new Vector3(transform.position.x,other.collider.transform.position.y + myBox.size.y/2f + other.collider.GetComponent<BoxCollider2D>().size.y/2f,transform.position.z)));
           // rb2d.MovePosition(new Vector3(transform.position.x, other.collider.transform.position.y + myBox.size.y / 2f + other.collider.GetComponent<BoxCollider2D>().size.y / 2f, transform.position.z));
        }
       // rb2d.gravityScale = origGrav;
        myBox.enabled = true;
    }
    private bool Jump()
    {
        return (Input.GetButtonDown("Jump") && Mathf.Abs(rb2d.velocity.y) <= 0.01f);
    }
	// Update is called once per frame
	void Update () {
        if (playerState == state.FLYING && Mathf.Abs(rb2d.velocity.y) == 0.0f)
        {
            playerState = state.FREE;
            Debug.Log(playerState);
        }
        if (Jump())
        {
            moveVertical = 50.0f;
            playerState = state.FLYING;
            Debug.Log(playerState);
        }

	}
}
