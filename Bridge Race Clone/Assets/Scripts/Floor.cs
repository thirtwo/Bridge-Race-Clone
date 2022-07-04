using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class Floor : MonoBehaviour
{
    public bool isFloorActive;
    public int floorNo;
    public List<Brick> bricks;
    public List<Ladder> ladders;
    private List<SpawnPoints> spawnPoints = new List<SpawnPoints>();
    [SerializeField] private List<Brick> brickPrefabs;
    private WaitForSeconds wait;
    private void Awake()
    {
        for (int i = 0; i < bricks.Count; i++)
        {
            spawnPoints.Add(new SpawnPoints(bricks[i], bricks[i].transform.position));
        }
        wait = new WaitForSeconds(0.3f);
        StartCoroutine(RandomSpawnBlocks());
    }
    private IEnumerator RandomSpawnBlocks()
    {
        while (isFloorActive)
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                if (bricks[i].IsCollected)
                {
                    var pos = spawnPoints[i].position;
                    var brick = Instantiate(brickPrefabs[Random.Range(0, brickPrefabs.Count)], pos, Quaternion.identity);
                    spawnPoints[i].holdedBrick = brick;
                    bricks[i] = brick;
                    yield return wait;
                }
                yield return null;
            }
                yield return null;
        } 
    }

    public Ladder GetLadder()
    {
        return ladders[Random.Range(0, ladders.Count)];
    }
}
public class SpawnPoints
{
    public Brick holdedBrick;
    public Vector3 position;

    public SpawnPoints(Brick holdedBrick, Vector3 position)
    {
        this.holdedBrick = holdedBrick;
        this.position = position;
    }
}
