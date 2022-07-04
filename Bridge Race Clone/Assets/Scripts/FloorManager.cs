using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    #region Singleton
    private static FloorManager instance = null;
    public static FloorManager Instance => instance;
    #endregion

    [SerializeField] private Floor[] floors;

    private void Awake()
    {
        instance = this;
        if(floors == null)
        {
            floors = FindObjectsOfType<Floor>();
        }
    }
    public Floor GetFloor(int no)
    {
        for (int i = 0; i < floors.Length; i++)
        {
            if (floors[i].floorNo == no) return floors[i];
        }
        return null;
    }
    public void SpawnBlocks()
    {
        //
    }
}
