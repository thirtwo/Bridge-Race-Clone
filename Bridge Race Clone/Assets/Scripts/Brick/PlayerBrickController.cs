using UnityEngine;
public class PlayerBrickController : BrickController
{
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
                if (hit.gameObject.TryGetComponent<Ladder>(out var ladder))
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
}
