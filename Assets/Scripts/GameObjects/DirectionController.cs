using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour
{
    public Direction Direction;

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetDirection(Direction direction)
    {
        this.Direction = direction;

        if (this.Direction == Direction.Right)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}

public enum Direction
{
    Right,
    Left,
}
