using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/******************************************************************
//This was my idea to build every level with a script instead of building it by hand.
//But we still need the scene objects at the beginning (I have not found another way to make it work)
//So we want to build our levels with this script. Build in random secrets, connections to other areas and spawn boxes for enemys.
*******************************************************************/

public class LevelBuilder : MonoBehaviour {

    public int rows = 10;       //Height of the level part
    public int columns = 50;    //Width of the level part
    private int[] levelParts = new int[2];  //Array of number of scenes that exist
    public GameObject wfloor;    //white floor
    public GameObject cfloor;    //cyan floor
    public GameObject exit;     //exit to new level part 

    //The awake function gets called only once when this game object gets instantiated.
    private void Awake()
    {
        
        buildLevelOne(SceneManager.GetSceneByName("Dschungel1"));   //Builds scene Dschungel1
        buildLevelTwo(SceneManager.GetSceneByName("Dschungel2"));   //Builds scene Dschungel2
    }

    private void Update()
    {
    }

    //Build level one
    public void buildLevelOne(Scene buildIn)
    {
        
        BoxCollider2D floorCollider = wfloor.GetComponent<BoxCollider2D>();             //Get reference to the white floor boxCollider component (we need it to get its width and height)
        //Iterate over the x and y axis of the level to build it according tot these coordinates
        for (int x = 0; x <= columns; x++)
        {
            for(int y = 0; y <= rows; y++)
            {
                //We build white floor at the ground and as walls on the left and the right side
                if (y == 0 || x == 0|| x == columns) {
                    //This instantiats a white floor object ( wfloor) at the position of the "new Vector3" position rotated by "Quaternion.identity". So in this case it is not rotated at all.
                    GameObject floorInstance = Instantiate(wfloor, new Vector3(x * floorCollider.size.x, y * floorCollider.size.y, 0f), Quaternion.identity);
                    //We move the object to the scene we want to build
                    SceneManager.MoveGameObjectToScene(floorInstance, buildIn);
                }
            }
        }
        //We also build in an exit at the position (30,0,0)
        GameObject exitInstance = Instantiate(exit, new Vector3(30f, 5f, 0f), Quaternion.identity);
        //And set its destination to the scene Dschungel2
        exitInstance.GetComponent<Transition>().setDest("Dschungel2");
        SceneManager.MoveGameObjectToScene(exitInstance, buildIn);
    }

    //Exactly the same as the one above except that we use cyan floor tiles 
    public void buildLevelTwo(Scene buildIn)
    {
        Scene oldScene = SceneManager.GetActiveScene();
        
        Debug.Log(SceneManager.SetActiveScene(buildIn));
        
        BoxCollider2D floorCollider = cfloor.GetComponent<BoxCollider2D>();
        
        Debug.Log("Build in: " + SceneManager.GetActiveScene().name);
        for (int x = 0; x <= columns; x++)
        {
            for (int y = 0; y <= rows; y++)
            {

                if (y == 0 || x == 0 || x == columns)
                {

                    GameObject floorInstance = Instantiate(cfloor, new Vector3(x * floorCollider.size.x, y * floorCollider.size.y, 0f), Quaternion.identity);
                   // SceneManager.MoveGameObjectToScene(floorInstance, buildIn);
                }
            }
        }
        //GameObject exitInstance = Instantiate(exit, new Vector3(20f, 5f, 0f), Quaternion.identity);
        //exitInstance.GetComponent<Transition>().setDest("Dschungel1");
        //SceneManager.MoveGameObjectToScene(exitInstance, buildIn);
        SceneManager.SetActiveScene(oldScene);
    }

}
