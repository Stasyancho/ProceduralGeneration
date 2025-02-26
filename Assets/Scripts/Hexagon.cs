using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    public GridPosition gridPosition;
    public List<Hexagon> Neighbors;
    public HexagonType Type;
    public float StepCost;
    public bool Explored = false;
}

public enum HexagonType
{
    None,
    Impassable
}
