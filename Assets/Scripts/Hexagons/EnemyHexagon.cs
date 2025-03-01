using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHexagon : Hexagon
{
    public override void HexagonEventStart()
    {
        Debug.Log("Start battle");
    }
}

