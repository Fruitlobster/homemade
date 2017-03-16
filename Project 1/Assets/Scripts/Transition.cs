using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/******************************************************************
//Transistion is a script of an Exit object and determines where the player will go to, when he enters an exit.
*******************************************************************/
public class Transition : MonoBehaviour {

    public string destName;             //Scene name of the destination

    //Function to set the destination name
    public void setDest(string name)
    {
        destName = name;
        
    }
    //Function to get the destination name
    public string getDest()
    {
        return destName;
    }

    //The main reason for this script is this function.
    //We move the player and the GameManager to the destination scene and set it to be the active scene
    //(NOT WORKING)
	public void transition(GameObject player, GameObject GameManager)
    {
        Debug.Log("Active Scene: " + SceneManager.GetActiveScene().name);
        Debug.Log("Destination Name: " + destName);
        Debug.Log("Player name: " + player.name);
        Debug.Log("GameManager: " + GameManager.name);
        

        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(destName));
        SceneManager.MoveGameObjectToScene(GameManager, SceneManager.GetSceneByName(destName));
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(destName));
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        //SceneManager.LoadSceneAsync(destName, LoadSceneMode.Single);
        Debug.Log("New active Scene: " + SceneManager.GetActiveScene().name);
    }
}
