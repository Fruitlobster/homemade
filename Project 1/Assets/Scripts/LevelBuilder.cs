using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {

    public int rows = 10;
    public int columns = 50;

    public GameObject floor;


    private void Awake()
    {
        buildLevelOne();
    }
    void buildLevelOne()
    {
        BoxCollider2D floorCollider = floor.GetComponent<BoxCollider2D>();
        for (int x = 0; x <= columns; x++)
        {
            for(int y = 0; y <= rows; y++)
            {

                if (y == 0 || x == 0|| x == columns) {
                    GameObject instance = Instantiate(floor, new Vector3(x * floorCollider.size.x, y * floorCollider.size.y, 0f), Quaternion.identity);
                }
            }
        }
    }

}
