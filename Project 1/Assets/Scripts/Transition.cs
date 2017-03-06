using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour {

    public string destName;

    public void setDest(string name)
    {
        destName = name;
        
    }

    public string getDest()
    {
        return destName;
    }

	public void transition(GameObject player, GameObject GameManager)
    { 
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(destName));
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(destName));
        SceneManager.MoveGameObjectToScene(GameManager, SceneManager.GetSceneByName(destName));
        
    }
}
