using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MapDeveloper 
{
    public static List<FutureHexagonParameters> Develop(int width, int height, float cellSize, int countZones, int countCastles)
    {
        List<FutureHexagonParameters> map = new List<FutureHexagonParameters>();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                map.Add(new FutureHexagonParameters()
                {
                    gridPosition = new GridPosition(x, z, 0),
                    position = GetHexWorldPosition(x, z, cellSize)
                });
            }
        }
        
        List<Vector3> zonesPosition = new List<Vector3>();
        Vector3 center = map.First(x => x.gridPosition.x == width/2 && x.gridPosition.z == height/2).position;
        float mapRadiusFromCenter = map.First(x => x.gridPosition.x == width - 1 && x.gridPosition.z == height - 1).position.magnitude / 2f;
        while (zonesPosition.Count != countZones)
        {
            int randX = Random.Range(0, width);
            int randZ = Random.Range(0, height);
            Vector3 randomPosition = map.First(x => x.gridPosition.x == randX && x.gridPosition.z == randZ).position;
            if (!zonesPosition.Any(x => (x - randomPosition).magnitude < mapRadiusFromCenter / countZones)&& (center - randomPosition).magnitude > mapRadiusFromCenter / 5f)
                zonesPosition.Add(randomPosition);
        }

        foreach (var hex in map)
        {
            float minmagnitude = float.MaxValue;
            for (int z = 0; z < zonesPosition.Count; z++)
            {
                float currentmagnitude = (zonesPosition[z] - hex.position).magnitude;
                if (minmagnitude > currentmagnitude)
                {
                    minmagnitude = currentmagnitude;
                    hex.zoneId = z;
                }
            }
        }
        
        List<Vector3> castlesPosition = new List<Vector3>();
        while (castlesPosition.Count != countCastles)
        {
            int randX = Random.Range(0, width);
            int randZ = Random.Range(0, height);
            FutureHexagonParameters randomHexagon = map.First(x => x.gridPosition.x == randX && x.gridPosition.z == randZ);
            Vector3 randomPosition = randomHexagon.position;
            if (!castlesPosition.Any(x => (x - randomPosition).magnitude < mapRadiusFromCenter / countCastles))
            {
                castlesPosition.Add(randomPosition);   
                randomHexagon.isCastle = true;
            }
        }
        
        return map;
    }
    
    static Vector3 GetHexWorldPosition(int x, int z, float cellSize)
    {
        float offsetX = (z % 2 == 0) ? 0 : cellSize * 0.5f;
        float offsetY = -0.1f;
        float offsetZ = Mathf.Sqrt(3) / 2 * cellSize;
        return new Vector3(x * cellSize + offsetX, offsetY, z * offsetZ);
    }
}
