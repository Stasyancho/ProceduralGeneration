using System;
using UnityEngine;

public class Line : IComparable<Line>
{
    public Vector2 Point1;
    public Vector2 Point2;
    public Line(Vector2 point1, Vector2 point2)
    {
        Point1 = point1;
        Point2 = point2;
    }
    public float Distance
    {
        get
        {
            return (Point1-Point2).magnitude;
        }
    }

    public bool Contains(Vector2 point)
    {
        return Point1 == point || Point2 == point;
    }

    public bool Contains(Vector3 point)
    {
        return Contains(new Vector2(point.x, point.z));
    }

    public int CompareTo(Line other)
    {
        return this.Distance.CompareTo(other.Distance);
    }
}
