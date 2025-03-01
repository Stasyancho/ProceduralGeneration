using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MapDeveloper 
{
    public static List<FutureHexagonParameters> Develop(int width, int height, float cellsize, int countZones, int countCastles)
    {
        GridPosition center = new GridPosition(width/2, height/2, 0);
        float availableRadius = Mathf.Min(width, height) / 2f - 17;
        List<FutureHexagonParameters> map = new List<FutureHexagonParameters>();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                FutureHexagonParameters hex = new FutureHexagonParameters()
                {
                    GridPosition = new GridPosition(x, z, 0),
                    Position = GetHexWorldPosition(x, z, cellsize)
                };
                
                if (GetDistance(center, hex.GridPosition) >= (availableRadius + Random.Range(0, 15)))
                {
                    hex.IsWater = true;
                }
                map.Add(hex);
            }
        }
        
        foreach (var hexagon in map)
        {
            hexagon.Neighbors.Add(map.FirstOrDefault(x => x.GridPosition.x == hexagon.GridPosition.x - 1 && x.GridPosition.z == hexagon.GridPosition.z));
            hexagon.Neighbors.Add(map.FirstOrDefault(x => x.GridPosition.x == hexagon.GridPosition.x + 1 && x.GridPosition.z == hexagon.GridPosition.z));
            hexagon.Neighbors.Add(map.FirstOrDefault(x => x.GridPosition.x == hexagon.GridPosition.x && x.GridPosition.z == hexagon.GridPosition.z - 1));
            hexagon.Neighbors.Add(map.FirstOrDefault(x => x.GridPosition.x == hexagon.GridPosition.x && x.GridPosition.z == hexagon.GridPosition.z + 1));
            if (hexagon.GridPosition.z % 2 == 0)
            {
                hexagon.Neighbors.Add(map.FirstOrDefault(x => x.GridPosition.x == hexagon.GridPosition.x - 1 && x.GridPosition.z == hexagon.GridPosition.z - 1));
                hexagon.Neighbors.Add(map.FirstOrDefault(x => x.GridPosition.x == hexagon.GridPosition.x - 1 && x.GridPosition.z == hexagon.GridPosition.z + 1));
            }
            else
            {
                hexagon.Neighbors.Add(map.FirstOrDefault(x => x.GridPosition.x == hexagon.GridPosition.x + 1 && x.GridPosition.z == hexagon.GridPosition.z - 1));
                hexagon.Neighbors.Add(map.FirstOrDefault(x => x.GridPosition.x == hexagon.GridPosition.x + 1 && x.GridPosition.z == hexagon.GridPosition.z + 1));
            }
        }
        
        for (int i = 0; i < 5; i++)
        {
            foreach (var hexagon in map)
            {
                if (hexagon.IsWater)
                {
                    IEnumerable<FutureHexagonParameters> temp = hexagon.Neighbors.Where(x => x != null && !x.IsWater);
                    if (temp.Count() > 3)
                        hexagon.IsWater = false;
                }
                else
                {
                    IEnumerable<FutureHexagonParameters> temp = hexagon.Neighbors.Where(x => x.IsWater);
                    if (temp.Count() > 3)
                        hexagon.IsWater = true;
                }
            }
        }

        List<FutureHexagonParameters> availableMap;
        int countBorderZones = 5;
        int maxLengthBorder = 8;
        for (int i = 0; i < countBorderZones; i++)
        {
            availableMap = map.Where(p => !p.IsWater).ToList();
            int randIndex = Random.Range(0, availableMap.Count);
            FutureHexagonParameters current = availableMap[randIndex];
            for (int j = 0; j < Random.Range(4, maxLengthBorder + 1); j++)
            {
                current.IsWater = true;
                List<FutureHexagonParameters> temp = current.Neighbors.Where(x => x != null && !x.IsWater).ToList();
                if (!temp.Any())
                    break;
                current = temp[Random.Range(0, temp.Count())];
            }
        }
        availableMap = map.Where(p => !p.IsWater).ToList();
        List<GridPosition> zonesPosition = new List<GridPosition>();
        List<List<FutureHexagonParameters>> zones = new List<List<FutureHexagonParameters>>();
        while (zonesPosition.Count != countZones)
        {
            int randIndex = Random.Range(0, availableMap.Count);
            if (!zonesPosition.Any(x =>
                    GetDistance(x, availableMap[randIndex].GridPosition) < availableRadius / Mathf.Sqrt(countZones) &&
                    GetDistance(center, availableMap[randIndex].GridPosition) < availableRadius / 2f))
            {
                availableMap[randIndex].ZoneId = zonesPosition.Count;
                zones.Add(new List<FutureHexagonParameters>(){availableMap[randIndex]});
                zonesPosition.Add(availableMap[randIndex].GridPosition);   
            }
        }

        bool isContinue = true;
        while (isContinue)
        {
            isContinue = false;
            foreach (var zone in zones)
            {
                for (int i = zone.Count() - 1; i >= 0; i--)
                {
                    List<FutureHexagonParameters> temp = zone[i].Neighbors.Where(x => x != null && !x.IsWater && x.ZoneId == -1).ToList();
                    temp.ForEach(x => x.ZoneId = zone[i].ZoneId);
                    zone.AddRange(temp);
                    zone.Remove(zone[i]);
                }
                
                if (zone.Count > 0)
                    isContinue = true;
            }
        }
        
        availableMap.Where(x => x.ZoneId == -1).ToList().ForEach(x =>
        {
            x.IsWater = true;
        });
        
        availableMap = map.Where(p => !p.IsWater).ToList();
        
        
        List<GridPosition> castlesPosition = new List<GridPosition>();
        while (castlesPosition.Count != countCastles)
        {
            int randIndex = Random.Range(0, availableMap.Count);
            if (!castlesPosition.Any(x => GetDistance(x,availableMap[randIndex].GridPosition) < availableRadius / Mathf.Sqrt(countCastles)))
            {
                castlesPosition.Add(availableMap[randIndex].GridPosition);   
                availableMap[randIndex].IsCastle = true;
            }
        }
        
        return map;
    }
    static float GetDistance(GridPosition from, GridPosition to)
    {
        GridPosition dir = to - from;
        float zDistance = Mathf.Abs(dir.z);
        float xDistance = Mathf.Abs(dir.x);
        float dist = 0;

        if (to.z % 2 != from.z % 2 && ((Mathf.Abs(from.z % 2) == 1 && dir.x > 0) ||(from.z % 2 == 0 && dir.x < 0)))
        {
            dist++;
            zDistance--;
            xDistance--;
        }

        dist += zDistance / 2 > xDistance ? zDistance : zDistance / 2 + xDistance;
        return Mathf.Ceil(dist);
    }
    static Vector3 GetHexWorldPosition(float x, float z, float cellSize)
    {
        float offsetX = (z % 2 == 0) ? 0 : cellSize * 0.5f;
        float offsetY = -0.1f;
        float offsetZ = Mathf.Sqrt(3) / 2 * cellSize;
        return new Vector3(x * cellSize + offsetX, offsetY, z * offsetZ);
    }
}
