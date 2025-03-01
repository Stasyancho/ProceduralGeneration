using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class HexGridSystem
{

    public static List<Hexagon> GenerateGrid(List<FutureHexagonParameters> map, Transform parent, List<GameObject> prefabs, GameObject borderPrefab, GameObject castlePrefab)
    {
        List<Hexagon> hexObjects = new List<Hexagon>();
        
        foreach (var hexagon in map)
        {
            GameObject prefab = null;
            if (hexagon.IsCastle)
                prefab = castlePrefab;
            if (hexagon.IsWater)
                prefab = borderPrefab;
            if (prefab == null)
                prefab = prefabs[hexagon.ZoneId];
            GameObject newObject = Object.Instantiate(prefab, hexagon.Position, Quaternion.identity, parent);
            var newHexagon = newObject.GetComponent<Hexagon>();
            newHexagon.gridPosition = hexagon.GridPosition;
            
            hexObjects.Add(newHexagon);
        }

        foreach (var hexagon in hexObjects)
        {
            hexagon.Neighbors.Add(hexObjects.FirstOrDefault(x => x.gridPosition.x == hexagon.gridPosition.x - 1 && x.gridPosition.z == hexagon.gridPosition.z));
            hexagon.Neighbors.Add(hexObjects.FirstOrDefault(x => x.gridPosition.x == hexagon.gridPosition.x + 1 && x.gridPosition.z == hexagon.gridPosition.z));
            hexagon.Neighbors.Add(hexObjects.FirstOrDefault(x => x.gridPosition.x == hexagon.gridPosition.x && x.gridPosition.z == hexagon.gridPosition.z - 1));
            hexagon.Neighbors.Add(hexObjects.FirstOrDefault(x => x.gridPosition.x == hexagon.gridPosition.x && x.gridPosition.z == hexagon.gridPosition.z + 1));
            if (hexagon.gridPosition.z % 2 == 0)
            {
                hexagon.Neighbors.Add(hexObjects.FirstOrDefault(x => x.gridPosition.x == hexagon.gridPosition.x - 1 && x.gridPosition.z == hexagon.gridPosition.z - 1));
                hexagon.Neighbors.Add(hexObjects.FirstOrDefault(x => x.gridPosition.x == hexagon.gridPosition.x - 1 && x.gridPosition.z == hexagon.gridPosition.z + 1));
            }
            else
            {
                hexagon.Neighbors.Add(hexObjects.FirstOrDefault(x => x.gridPosition.x == hexagon.gridPosition.x + 1 && x.gridPosition.z == hexagon.gridPosition.z - 1));
                hexagon.Neighbors.Add(hexObjects.FirstOrDefault(x => x.gridPosition.x == hexagon.gridPosition.x + 1 && x.gridPosition.z == hexagon.gridPosition.z + 1));
            }
        }
        
        return hexObjects;
    }
}
