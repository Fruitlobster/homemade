using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//These depict the different states the player can stay in. 
//They are not all relevant at the moment and only affect the possible movement of the player
enum state {
    FREE,       //FREE means that the player can move freely. Like moving on the ground and jumping
    HANGING,    //HANGING means that the player is hanging on a wall or off a cliff. He can then climb up or jump away
    STUNNED,    //A STUNNED player can not move or do anything 
    FLYING      //FLYING means the player is in the air. So no movement or jumping is available
} ;      

/******************************************************************
//The PlayerController manages the possible movement of the player.
//Here we write the typicall movements like walking and jumping.
//At the moment we only have the normal movement and some kind of jumping. 
*******************************************************************/


public class PlayerController : MonoBehaviour {

    

    public float speed;                     //Floating point variable to store the player's movement speed.
    public float moveHorizontal = 0.0f;     //Stores the force that affects the player on the horizontal line per frame
    public float moveVertical = 0.0f;       //Stores the force that affects the player on the vertical line per frame
    public GameObject player;               //The Player gameobject that we use in the Unity editor
    public GameObject GameManager;          //The GameManager gameobject that we use in the Unity editor

    state playerState;                      //Stores the current state that the player is located in
    private Rigidbody2D rb2d;               //The Rigidbody2D component of the player gameobject. It represents the physical form of the object and stores data like transformations for moving or rotations for..rotations
    private Animator animator;              //The Animator component of the player gameobject. It handles the different animations the player can perform.
    private float origGrav;                 //Stores the original gravity that influences the player. Because we change the gravity at certain events.
    private BoxCollider2D myBox;            //The BoxCollider of the player. Is basically a box arount the player that we use for the calculation of collisions, i.e. if the player hits something.
    private float maxSpeed;                 //Stores the maximal possible speed the player can move with.

    // Use this for initialization
    void Start () {
        
        
        rb2d = GetComponent<Rigidbody2D>();                             //Get and store a reference to the Rigidbody2D component so that we can access it. 
        animator = GetComponent<Animator>();                            //Get and store a reference to the Animator component so that we can access it. 
        myBox = GetComponent<BoxCollider2D>();                          //Get and store a reference to the Animator component so that we can access it.
        origGrav = rb2d.gravityScale * GameLogic.instance.gravity;      //We multiply the gravityScale of the player with the variable gravity of the GameLogic object, 
                                                                        //because we want an overall gravity that can change under certain circumstances and an individual gravity.
        rb2d.gravityScale = origGrav;                                   //store the calculated gravity, so that it affects the player.
        maxSpeed = 100;                                                 //initialize maxSpeed
        

        playerState = state.FREE;                                       //set the playerState to FREE
       
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    private void FixedUpdate()
    {

        switch (playerState)
        {
            case state.FREE:
                //Store the current horizontal input in the float moveHorizontal.
                moveHorizontal = Input.GetAxis("Horizontal");

                //If the input indicates that we want to go to the right, we turn the player to the right and...
                if (moveHorizontal > 0)
                {
                    animator.SetTrigger("Turn-Right");
                }
                //...if it indicates that we want to go to the left, we turn the player to the left
                else if(moveHorizontal < 0)
                {
                    animator.SetTrigger("Turn-Left");
                }
                //if the player object moves to the right we start the MoveRight animation and deactivate StandStill
                if (rb2d.velocity.x > 0)
                {
                    animator.SetBool("MoveRight", true);
                    animator.SetBool("StandStill", false);
                }
                //if the player object moves to the left we start the MoveLeft animation and deactivate StandStill
                else if (rb2d.velocity.x < 0)
                {
                    animator.SetBool("MoveRight", false);
                    animator.SetBool("StandStill", false);
                }
                //If the player does not move we start the PlayerIdle animation
                if (Mathf.Abs(rb2d.velocity.x) == 0 )
                {
                    animator.SetBool("StandStill", true);
                }
                break;
            case state.HANGING:
                //If we hang on a cliff and press Jump then we should get the normal jumping behavior (NOT WORKING)
                if (Jump())
                {
                    moveVertical = 50.0f;
                    rb2d.gravityScale = origGrav;
                    playerState = state.FLYING;
                }
               
                break;
                //not implemented yet. 
            case state.FLYING:

                break;
            case state.STUNNED:
                break;
                
        }
        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement to move our player.
        rb2d.AddForce(movement);
        //set moveVertical to 0. Otherwise the player flies of into eternity after a jump.
        moveVertical = 0.0f;
    }

    //Determines the climb behaviour of the player (NOT WORKING)
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

    //What happens on Collision
    private void OnCollisionEnter2D(Collision2D other)
    {
        //If we jump on a floor we want get a hold of it
       if(other.collider.tag == "Floor" && other.collider.transform.position.y >= transform.position.y)
        {
            //so we set the player in the HANGING state and set his gravity to 0.
            playerState = state.HANGING;
            rb2d.gravityScale = 0f;
            //StartCoroutine(Climb(new Vector3(transform.position.x,other.collider.transform.position.y + myBox.size.y/2f + other.collider.GetComponent<BoxCollider2D>().size.y/2f,transform.position.z)));
           // rb2d.MovePosition(new Vector3(transform.position.x, other.collider.transform.position.y + myBox.size.y / 2f + other.collider.GetComponent<BoxCollider2D>().size.y / 2f, transform.position.z));
        }
       // rb2d.gravityScale = origGrav;
    }

    //what happens on trigger collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        //If we hit an Exit, we want to get to another area
        if (other.gameObject.CompareTag("Exit"))
        {
            //The information of the destination should be known by the exit itself
            other.gameObject.GetComponent<Transition>().transition(player, GameManager);
                         
        }
    }

    //returns true if the jump button gets pressed and the player is not in the air
    private bool Jump()
    {
        return (Input.GetButtonDown("Jump") && Mathf.Abs(rb2d.velocity.y) <= 0.01f);
    }

	// Update is called once per frame
	void Update () {
        //When we were flying and landed on the ground, we want to get back to the player state FREE, to move freely again
        if (playerState == state.FLYING && Mathf.Abs(rb2d.velocity.y) == 0.0f)
        {
            playerState = state.FREE;
            Debug.Log(playerState);
        }
        //If we press jump, we want to get in the air and enter the state FLYING
        if (Jump())
        {
            moveVertical = 50.0f;
            playerState = state.FLYING;
            Debug.Log(playerState);
        }

	}
}
