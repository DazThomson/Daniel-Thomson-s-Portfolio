using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;


public class PathFinding : MonoBehaviour
{
    // Start is called before the first frame update
    //this script will be attached to the enemy and will always have a lock on the player.
    //I implemented this pathfinding script through following a tutoiral created by Sebastian Lague which goes over implementing the algorithm in the game
    //In order to fit my needs, i needed to convert all Vector3 types in his video to Vector2 because i'm working in a 2D space.
    //Source to tutorial: https://www.youtube.com/watch?v=mZfyt03LDH4&t=1281s (Sebastian L, Dec 2014), Date accessed: 28th June 2022
    
    
    RequestPathManager manager;    
    Grid2 aStarGrid;//need a reference to the Grid.
    void Awake()
    {
        manager = GetComponent<RequestPathManager>();
        aStarGrid = GetComponent<Grid2>();
       

    }

    //every frame, we want to call the pathfinder function to ensure the enemy is always keeping track of the players where abouts.
    public void StartPath(Vector2 start, Vector2 endPos)
    {
        StartCoroutine(Pathfinder(start, endPos));
    }
    //Code adapted to work in 2D space
    IEnumerator Pathfinder(Vector2 Position, Vector2 Target)
    {
        Vector2[] waypoint = new Vector2[0];
        bool pathSuccessful = false;
        Node2 startNode = aStarGrid.WorldPoint(Position);//we need to reference our starting Cell and targetNode and assign it to the worldPosition inside the grid
        //This will convert the enemy's position in the world to the grid and the Enemy's target too.
        Node2 targetNode = aStarGrid.WorldPoint(Target);
        List<Node2> open = new List<Node2>();
        HashSet<Node2> closed = new HashSet<Node2>();/*a hashset is used to handle all unique data the pathfinder is recieving. This includes all data which the pathfinder has already 
                                                      * processed. this is useful because a Hashset doesn't have a capacity limit which allows an infinite flow of data to be stored inside */                                                     
        open = new List<Node2>();
        closed = new HashSet<Node2>();
        if (startNode.Passable && targetNode.Passable)
        {
            open.Add(startNode);  //startNode is added to the node list open;
            while (open.Count > 0)
            {
                Node2 currentNode = open[0];
                for (int i = 1; i < open.Count; i++)
                {//for loop will go through the list open.
                    if (open[i].FCost < currentNode.FCost || open[i].FCost == currentNode.FCost)
                    {//
                        if (open[i].h < currentNode.h)
                            currentNode = open[i];
                    }
                }

                open.Remove(currentNode);//currentNode is removed from the list and added into the closed HashSet becasue data has already been handled.
                closed.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccessful = true;
                    break;
                }
                foreach (Node2 neighbour in aStarGrid.NeighbourNode(currentNode))/*in this for each loop, it checks for every neighbour node (nodes next to the current node we're on and
		checks the distance which one is the optimal node to take for the shortest distance. */
		
                {//
                    if (!neighbour.Passable || closed.Contains(neighbour))/*if the one of the neighbouring nodes isn't passable or if one of the closed has set data contains the neiagbour node,
		    then we continue to check for new nodes teh enemy could take. */
                    {
                        continue;
                    }

                    int newCostToNeighbour = currentNode.g + Distance(currentNode, neighbour);
                    if (newCostToNeighbour < neighbour.g || !open.Contains(neighbour))
                    {
                        neighbour.g = newCostToNeighbour;
                        neighbour.h = Distance(neighbour, targetNode);
                        neighbour.parent = currentNode;
                        if (!open.Contains(neighbour))
                            open.Add(neighbour);
                       
                    }
                }
            }
        }
            yield return null;
            if (pathSuccessful)//when the path is completed the waypoint variable will call the RecallPathing Function 
            {
                waypoint = RecallPathing(startNode, targetNode);
            }
            manager.FinishedPathProcess(waypoint, pathSuccessful);// this information is sent to the function in Request pathmanager which takes the data from Vector2 waypoint and 
        //pathSuccessful bool to this function.
	}   

   	//the path is recalled to check for whether or not the user's position is updated. if it is it will set a new path taking in the current node2.
        Vector2[] RecallPathing(Node2 startNode, Node2 end)
        {
            List<Node2> setPath = new List<Node2>();
            Node2 current = end;

            while (current != startNode)
            {
                setPath.Add(current);
                current = current.parent;
            }
            Vector2[] waypoints = SimplerPath(setPath);
            Array.Reverse(waypoints);     
            return waypoints;
        }
        
    Vector2[] SimplerPath(List<Node2> pathfind)//Vector2 function which passes a list of nodes in the array.
    {
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 oldDir = Vector2.zero;

        for (int i = 1; i < pathfind.Count; i++)
        {
            Vector2 newDir = new Vector2(pathfind[i - 1].gX - pathfind[i].gX, pathfind[i - 1].gY - pathfind[i].gY);
            if (newDir != oldDir)
            {
                waypoints.Add(pathfind[i].worldPos);
            }
            oldDir = newDir;
        }
        return waypoints.ToArray();
    }
        int Distance(Node2 node1, Node2 node2)// an integer function which takes the first and second node and calculates the distance between the two.
        {
            int DistanceX;       
            int DistanceY;
            DistanceX = Mathf.Abs(node1.gX - node2.gX);
            DistanceY = Mathf.Abs(node1.gY - node2.gY);

           if(DistanceX < DistanceY)
                return 14 * DistanceY + 10 * (DistanceX - DistanceY);
        
           return 14 * DistanceX + 10 * (DistanceY - DistanceX);
        }
}

