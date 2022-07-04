using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Thirtwo.Character;

public abstract class BrickController : MonoBehaviour
{
    public Brick.BrickColor color;
    [SerializeField] protected float brickLength = 1f;
    [HideInInspector] public List<Brick> bricks = new List<Brick>();
    protected CharController charController;
    [SerializeField] protected GameObject collectParticle;

    protected virtual void Start()
    {
        Init();
        GameManager.OnGameFinished += GameManager_OnGameFinished;
    }
    protected virtual void OnDestroy()
    {
        GameManager.OnGameFinished -= GameManager_OnGameFinished;
    }

    protected virtual void GameManager_OnGameFinished(bool obj)
    {
        ExplodeBricks();
    }

    protected virtual void Collect(Brick brick)
    {
        brick.OnCollect();
        brick.GetComponent<BoxCollider>().enabled = false;
        bricks.Add(brick);
        var particle = Instantiate(collectParticle, transform);
        particle.transform.localPosition = new Vector3(0, 0.5f + (bricks.Count * brickLength), -1);
        brick.transform.SetParent(transform);
        brick.transform.position = transform.position;
        brick.transform.DOLocalMove(new Vector3(0, 0.5f + (bricks.Count * brickLength), -1), 0.5f);
        brick.transform.DOLocalRotateQuaternion(Quaternion.identity, 0.5f);
    }
    protected virtual void Init()
    {
        charController = GetComponent<CharController>();
    }
    protected virtual void Drop(Ladder ladder, Brick brick)
    {
        ladder.GetBrick(charController, brick);
        bricks.Remove(bricks[^1]);
    }
    protected virtual void ExplodeBricks()
    {
        for (int i = 0; i < bricks.Count; i++)
        {
            bricks[i].Explode(transform.position);
        }
    }
}
