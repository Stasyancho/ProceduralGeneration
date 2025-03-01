using System.Collections.Generic;
using UnityEngine;

public class EventHexagon : Hexagon
{
    public override void HexagonEventStart()
    {
        Debug.Log("Some event");
    }
}

