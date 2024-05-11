using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Stage
{
    public Direction dir;
    public int Count;

    public Stage()
    {
        Count = 3;
    }

    public Vector3 GetDirectionVector()
    {
        Vector3 vector = Vector3.forward;
        switch (dir)
        {
            case Direction.Forward:
                vector = Vector3.forward;
                break;
            case Direction.Left:
                vector = Vector3.left;
                break;
            case Direction.Backward:
                vector = Vector3.back;
                break;
            case Direction.Right:
                vector = Vector3.right;
                break;
            default:
                break;
        }
        return vector;
    }
}

public enum Direction
{
    Forward,
    Left,
    Backward,
    Right
}
