using System;
using System.Collections.Generic;
using UnityEngine;

public class AIBrickController : BrickController
{
    public event Action OnCollect;
    public event Action OnDrop;
    private void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.layer == 6)//brick layer
        {
            if (hit.gameObject.TryGetComponent<Brick>(out var brick))
            {
                if (brick.color == color && !brick.IsCollected)
                {
                    Collect(brick);
                }
            }
        }
        if (hit.gameObject.layer == 7) //ladder layer
        {
            if (bricks.Count > 0)
            {
                var ladder = hit.gameObject.GetComponentInParent<Ladder>();
                if (ladder != null)
                {
                    Drop(ladder, bricks[^1]);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            var ladder = other.GetComponentInParent<Ladder>();
            if (ladder != null)
            {
                ladder.CheckStairColors(color);
            }
        }
    }

    protected override void Collect(Brick brick)
    {
        base.Collect(brick);
        OnCollect?.Invoke();
    }
    protected override void Drop(Ladder ladder, Brick brick)
    {
        base.Drop(ladder, brick);
        OnDrop?.Invoke();
    }
}
