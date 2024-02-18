using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2 : MonoBehaviour
{
    //Source to video tutorial which helped me implement this grid: https://www.youtube.com/watch?v=nhiFx28e7JY&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW&index=2
    //various parts of the code have been altered to fit the 2D environment. Most Vector3 variables have been swapped to Vector2 and Vector 2 

   
//it's important to note that this script was created by following a tutorial but it has been adapted to fit the 2D space to save resources and help with performance.
    float Diameter;// null value which will change in awake.
    public LayerMask unPassableMask;//public variable which take checks for objects which with the unWalkable Layer.
    public Vector2 gridSize; //size of each cell within the grid;
    public float Radius; //radius of the node (size of the Cell);
    public bool displayGizmos;//toggle to turn debug on and off for testing.
    Node2[,] grid;//empty Node array which passes 2 values (gridX, gridY)
    

    int gridX, gridY;//null values which will take values of the Vector2 GridSize.


   
     void OnDrawGizmos()
     {
       // Gizmos.DrawWireCube(transform.position, new Vector2(gridSize.x, gridSize.y));
        Gizmos.DrawWireCube(transform.position, new Vector2(gridSize.x, gridSize.y));


        if (grid != null && displayGizmos == true)
        {
            
            foreach (Node2 n in grid)//for each value in grid array
            {
                Gizmos.color = (n.Passable) ? Color.grey : Color.red;
                Gizmos.DrawWireCube(n.worldPos, Vector3.one * (Diameter - 0.1f));
            }
        }
                    
     }


     void Awake()/*in the awake function, we calculate the overall size of the grid based on the numbers we've entered in the inspector.
                  * these values are then taken and divided by the Diamter which are sent to the MakeGrid method. */
    {
        Diameter = Radius * 2;
        gridX = Mathf.RoundToInt(gridSize.x / Diameter);
        gridY = Mathf.RoundToInt(gridSize.y / Diameter);
        MakeGrid();

        
    }

    void MakeGrid()//this function is used to create the grid and applying which nodes(cells) are passable or impassable
    {
        grid = new Node2[gridX, gridY];//the grid is converted into a Node grid which is 
        Vector2 worldPosBLeft = (Vector2)transform.position - Vector2.right * gridSize.x / 2 - Vector2.up * gridSize.y / 2;
       

        for (int x = 0; x < gridX; x++)//for loop to create nodes based on values entered in gridX and gridY in inspector.
        {
            for (int y = 0; y < gridY; y++)
            {
                Vector2 point = worldPosBLeft + Vector2.right * (x * Diameter + Radius) + Vector2.up * (y * Diameter + Radius);
                bool passable = (Physics2D.OverlapCircle(point, Radius, unPassableMask) == null);// this will check all objects within the grid and check it's point, radius and layermask
                grid[x, y] = new Node2(passable, point, x,y);//the Node2 grid passes in the passable bool, the enemy's position and the x and y coordinates
            }

        }
    }

    //this function calulate the point of certain gameobjects within the grid. This could be the player or the enemy
    public Node2 WorldPoint(Vector2 worldPosition)
    {
        float x = (worldPosition.x + gridSize.x / 2) / gridSize.x;/*the sum of both x and y passes in the worldPosition of a certain object, 
                                                                    and adds that to the grids overall size and divided by 2 and divided by the grid size again*/
        float y = (worldPosition.y + gridSize.y / 2) / gridSize.y;
        x = Mathf.Clamp01(x);/*"clamp is used to clamp the given value between a range defined by the minimum integer and maximum integer values (0 or 1 since clamp01 is used)
                              * a value is returned when the value is within the min and max"(Unity, 2021)." Source: 
                              * https://docs.unity3d.com/ScriptReference/Mathf.Clamp.html#:~:text=Clamps%20the%20given%20value%20between%20a%20range%20defined%20by%20the,less%20than%20the%20min%20value. */ 
        y = Mathf.Clamp01(y); //we use clamp in this scenario to keep 

        //rounding the sum of arrayX and arrayY to the nearest whole number to keep values specific.
        int arrayX = Mathf.RoundToInt((gridX -1) * x);//the sum of the x coordinate is rounded to the nearest whole number
        int arrayY = Mathf.RoundToInt((gridY -1) * y);

        return grid[arrayX, arrayY];//returns the player's/enemys world position within the grid


    }

    //input handler to toggle debug Gizmos.
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            displayGizmos = true;
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            displayGizmos = false;
        }


        
    }

    //This public List is used to check for neighbouring nodes arounf the current node the player/ enemy is on.
    //with the use of this list, it allows us to check the most appropriate node is to find the shortest path.
    //This Code was taken from sebastian Lagues A* pathfinding video in which he explains and showcases how to implement the A* pathfinding algorithim from scratch.
    //Source https://www.youtube.com/watch?v=mZfyt03LDH4&t=738s
    public List<Node2> NeighbourNode(Node2 node)
    {
        List<Node2> nextToNode = new List< Node2 > ();
        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y ==0)
                {
                    continue;
                }
                int check = node.gX + x;
                int check2 = node.gY + y;

                if(check >= 0 && check < gridX && check2 >= 0 && check2 < gridY)
                {
                    nextToNode.Add(grid[check, check2]);
                }
            }

        }
        return nextToNode;
    }




}
