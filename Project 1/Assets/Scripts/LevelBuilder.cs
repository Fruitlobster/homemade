using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBuilder : MonoBehaviour {

    public int rows = 10;       //Height of the level part
    public int columns = 50;    //Width of the level part
    private int[] levelParts = new int[2];
    public GameObject wfloor;    //white floor
    public GameObject cfloor;    //cyan floor
    public GameObject exit;     //exit to new level part 
    private void Awake()
    {
        buildLevelOne(SceneManager.GetActiveScene());
        buildLevelTwo(SceneManager.CreateScene("Dschungel2"));
    }

    private void Update()
    {
    }

    //Build level one
    public void buildLevelOne(Scene buildIn)
    {
        
        BoxCollider2D floorCollider = wfloor.GetComponent<BoxCollider2D>();
        for (int x = 0; x <= columns; x++)
        {
            for(int y = 0; y <= rows; y++)
            {

                if (y == 0 || x == 0|| x == columns) {
                    GameObject floorInstance = Instantiate(wfloor, new Vector3(x * floorCollider.size.x, y * floorCollider.size.y, 0f), Quaternion.identity);
                    SceneManager.MoveGameObjectToScene(floorInstance, buildIn);
                }
            }
        }
        GameObject exitInstance = Instantiate(exit, new Vector3(30f, 5f, 0f), Quaternion.identity);
        exitInstance.GetComponent<Transition>().setDest("Dschungel2");
        SceneManager.MoveGameObjectToScene(exitInstance, buildIn);
    }

    public void buildLevelTwo(Scene buildIn)
    {
        BoxCollider2D floorCollider = cfloor.GetComponent<BoxCollider2D>();
        for (int x = 0; x <= columns; x++)
        {
            for (int y = 0; y <= rows; y++)
            {

                if (y == 0 || x == 0 || x == columns)
                {
                    GameObject floorInstance = Instantiate(cfloor, new Vector3(x * floorCollider.size.x, y * floorCollider.size.y, 0f), Quaternion.identity);
                    SceneManager.MoveGameObjectToScene(floorInstance, buildIn);
                }
            }
        }
        GameObject exitInstance = Instantiate(exit, new Vector3(20f, 5f, 0f), Quaternion.identity);
        exitInstance.GetComponent<Transition>().setDest("Dschungel1");
        SceneManager.MoveGameObjectToScene(exitInstance, buildIn);
    }

}
