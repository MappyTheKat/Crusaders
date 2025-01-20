using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 Velocity;
    public DirectionController ChildDirectionController;
    public ZController ChildZController;

    public Direction Direction;
    private int multiplier = 1;

    void Start()
    {
        if (!ChildDirectionController)
            ChildDirectionController = gameObject.GetComponentInChildren<DirectionController>();
        if (!ChildZController)
            ChildZController = GetComponentInChildren<ZController>();

        multiplier = Direction == Direction.Right ? 1 : -1;
        ChildZController.ZVelocity = Velocity.z;
        ChildDirectionController.SetDirection(Direction);
    }

    void Update()
    {
        transform.Translate(new Vector2(Velocity.x * multiplier, Velocity.y));
    }

    public void SetVelocity(Vector3 velocity)
    {
        Velocity = velocity;
    }

    public void SetDirection(Direction direction)
    {
        Direction = direction;
        multiplier = Direction == Direction.Right ? 1 : -1;
        ChildDirectionController.SetDirection(Direction);
    }
}
