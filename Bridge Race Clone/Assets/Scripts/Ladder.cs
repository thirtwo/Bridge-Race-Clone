using System.Collections.Generic;
using Thirtwo.Character;
using UnityEngine;
public class Ladder : MonoBehaviour
{
    [SerializeField] private int ladderLength;
    [SerializeField] private GameObject ladderBrick;
    public List<LadderStair> stairs = new List<LadderStair>();
    [SerializeField] private List<Material> brickColors;
    public List<Collider> colliderPoints;
    private Floor floor;
    private void Awake()
    {
        floor = GetComponentInParent<Floor>();
        var controllers = FindObjectsOfType<BrickController>();
        for (int i = 0; i < colliderPoints.Count; i++)
        {
            for (int k = 0; k < controllers.Length; k++)
            {
                if ((Brick.BrickColor)i == controllers[k].color) continue;
                var collider = controllers[k].GetComponent<Collider>();
                Physics.IgnoreCollision(collider, colliderPoints[i]);
            }
        }
    }
    public void GetBrick(CharController controller, Brick brick)
    {
        if (stairs.Count == 0)
        {
            AddLadderStair(controller, brick);
        }
        else
        {
            for (int i = 0; i < stairs.Count; i++)
            {
                if (stairs[i].color != brick.color)
                {
                    ChangeLadderStair(controller, brick, i);
                    return;
                }
            }
            if (stairs.Count >= ladderLength) return;
            AddLadderStair(controller, brick);
        }
    }
    public void CheckStairColors(Brick.BrickColor color)
    {
        if (stairs.Count == 0) return;
        for (int i = 0; i < stairs.Count; i++)
        {
            if (stairs[i].color != color)
            {
                colliderPoints[(int)color].transform.position = stairs[i].obj.transform.position;
                return;
            }
        }
    }
    private void ChangeLadderStair(CharController controller, Brick brick, int i)
    {
        var clone = Instantiate(ladderBrick, transform.parent);
        clone.transform.SetPositionAndRotation(stairs[i].obj.transform.position, Quaternion.Euler(135, 0, 0));
        clone.GetComponent<LadderBrick>().AnimateBrick(brickColors[(int)brick.color].color);
        brick.gameObject.SetActive(false);
        var startPoint = colliderPoints[(int)brick.color].transform;
        startPoint.localPosition += Vector3.up / 2;
        stairs[i].obj.SetActive(false);
        var stair = new LadderStair(brick.color, clone);
        stairs[i] = stair;
        if (i >= ladderLength)
        {
            if (floor.floorNo == controller.floorNo)
            {
                for (int k = 0; k < stairs.Count; k++)
                {
                    if (stairs[k].color != brick.color)
                    {
                        return;
                    }
                }
                var collider = controller.GetComponent<Collider>();
                Physics.IgnoreCollision(colliderPoints[(int)brick.color], collider);
                controller.floorNo++;
                controller.GetActiveFloor();
            }
        }
    }

    private void AddLadderStair(CharController controller, Brick brick)
    {
        var startPoint = colliderPoints[(int)brick.color].transform;
        var clone = Instantiate(ladderBrick, transform.parent);
        clone.transform.SetPositionAndRotation(startPoint.position, Quaternion.Euler(135, 0, 0));
        clone.GetComponent<LadderBrick>().AnimateBrick(brickColors[(int)brick.color].color);
        brick.gameObject.SetActive(false);
        startPoint.localPosition += Vector3.up / 2;
        var stair = new LadderStair(brick.color, clone);
        stairs.Add(stair);
        if (stairs.Count >= ladderLength)
        {
            if (floor.floorNo == controller.floorNo)
            {
                var collider = controller.GetComponent<Collider>();
                Physics.IgnoreCollision(colliderPoints[(int)brick.color], collider);
                controller.floorNo++;
                controller.GetActiveFloor();
            }
        }
    }

}
[System.Serializable]
public class LadderStair
{
    public Brick.BrickColor color;
    public GameObject obj;
    public LadderStair(Brick.BrickColor color, GameObject obj)
    {
        this.color = color;
        this.obj = obj;
    }
}