using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoadsDrawer
{
    public static void DrawRoads(Transform parent, List<Line> roads)
    {
        roads.ForEach(x => DrawRoad(parent, x));
    }

    static void DrawRoad(Transform parent, Line road)
    {
        LineRenderer lineRenderer;
        lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.gameObject.transform.SetParent(parent);
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(0, new Vector3(road.Point1.x,1,road.Point1.y));
        lineRenderer.SetPosition(1, new Vector3(road.Point2.x,1,road.Point2.y)); 
    }
}
