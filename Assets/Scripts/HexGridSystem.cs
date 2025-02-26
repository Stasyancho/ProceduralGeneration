using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HexGridSystem
{

    public static List<Hexagon> GenerateGrid(IEnumerable<FutureHexagonParameters> map, Transform parent, List<GameObject> prefabs, GameObject castlePrefab)
    {
        List<Hexagon> hexObjects = new List<Hexagon>();

        foreach (var hexagonParameters in map)
        {
            GameObject newHex;
            if (hexagonParameters.isCastle)
                newHex = Object.Instantiate(castlePrefab, hexagonParameters.position, Quaternion.identity, parent);
            else
                newHex = Object.Instantiate(prefabs[hexagonParameters.zoneId], hexagonParameters.position, Quaternion.identity, parent);
            Hexagon hex = newHex.GetComponent<Hexagon>();
            hex.gridPosition = hexagonParameters.gridPosition;
            hexObjects.Add(hex);
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
