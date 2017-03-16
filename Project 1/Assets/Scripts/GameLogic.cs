using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************
//The GameLogic manages every part of the overall game like the gravity, the gameSpeed etc.
*******************************************************************/
public class GameLogic : MonoBehaviour {

    public float gameSpeed = 1f;                    //The game speed determines how fast everything moves.
    public float gravity = 0.5f;                    //The gravity determines how much everything gets pulled to the earth
    public static GameLogic instance = null;        

    private void Awake()
    {
        
        //Check if instance already exists
        if (instance == null)
        {

            //if not, set instance to this
            instance = this;

        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
