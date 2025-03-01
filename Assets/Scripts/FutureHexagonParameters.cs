using System.Collections.Generic;
using UnityEngine;

public class FutureHexagonParameters
{
    public int ZoneId = -1;
    public Vector3 Position;
    public GridPosition GridPosition;
    public bool IsCastle = false;
    public bool IsWater = false;
    public List<FutureHexagonParameters> Neighbors = new List<FutureHexagonParameters>();
}
