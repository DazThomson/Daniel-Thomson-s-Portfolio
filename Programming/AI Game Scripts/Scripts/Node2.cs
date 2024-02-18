using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node2 
{
    public bool Passable;//we need a bool which determines whether an position inside the grid is passable or impassable.
    public Vector2 worldPos;//we take the world position of the object (in this case the enemy)
    public int g, h;
    public int gX;
    public int gY;

    public Node2 parent;

    public Node2(bool _passable, Vector2 _worldPosition, int _X, int _Y)//a public function which passes both a bool, int and Vector2
    {
        Passable = _passable;
        worldPos = _worldPosition;
        gX = _X;
        gY = _Y;
    }
    //f(n) = g(n) + h(n);
    //"g is the cost of the path from the start node to n and h is a heuristic function which estimates the cost of the cheapest path from n to the goal"(Wikipeida, Unknown), 
    //Source: https://en.wikipedia.org/wiki/A*_search_algorithm#Description

    //this will be referenced in PathFinding script to calculate the  shortest path based on where the player is located.
    public int FCost
    {
        get
        {
            return g + h;//calculate the G and H cost;
        }
    }
}
