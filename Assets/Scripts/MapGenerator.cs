using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [Header("Seed")]
    [SerializeField] private bool useSeed;
    [SerializeField] private int seed;
    
    [Header("MapSettings")]
    [SerializeField] private Transform gridParent; 
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private float hexSize = 2f;
    [SerializeField] private List<GameObject> prefabs;
    
    [Header("CastleSettings")]
    [SerializeField] private Transform pathsParent; 
    [SerializeField] private int castlesCount;
    [SerializeField] private GameObject castlePrefab;
    [SerializeField] private float minimalAngleForTriangle;
    
    [Header("Result")]
    public List<Hexagon> hexList;

    private int iteration = 0;
    private void Awake()
    {
        if (useSeed)
        {
            Random.InitState(seed);
            Debug.Log("Seed: " + seed);
        }
        else
        {
            int randomSeed = Random.Range(int.MinValue, int.MaxValue);
            Random.InitState(randomSeed);
            Debug.Log("Seed: " + randomSeed);
        }
    }

    public void GenerateMap()
    {
        Debug.Log("Iteration: " + ++iteration);
        hexList.ForEach(x => Destroy(x.gameObject));
        hexList.Clear();
        for (int x = pathsParent.childCount - 1; x >= 0; x--)
            Destroy(pathsParent.transform.GetChild(x).gameObject);
        List<FutureHexagonParameters> map = MapDeveloper.Develop(gridWidth, gridHeight, hexSize, prefabs.Count, castlesCount);
        hexList = HexGridSystem.GenerateGrid(map, gridParent, prefabs, castlePrefab);
        List<Line> lines = RoadsDeveloper.Develop(map.Where(x => x.isCastle).ToList(), minimalAngleForTriangle);
        RoadsDrawer.DrawRoads(pathsParent, lines);
    }
}
