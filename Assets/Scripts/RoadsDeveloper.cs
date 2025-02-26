using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RoadsDeveloper 
{
    public static List<Line> Develop(List<FutureHexagonParameters> points)
    {
        List<Line> lines = CreateLines(points).ToList();
        return FilterLines(lines);
    }
    
    static IEnumerable<Line> CreateLines(List<FutureHexagonParameters> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                yield return new Line(new Vector2(points[i].position.x, points[i].position.z), new Vector2(points[j].position.x, points[j].position.z));
            }
        }
    }
    static List<Line> FilterLines(List<Line> lines)
    {
        lines.Sort();
        List<Line> result = new List<Line>();
        while (lines.Count > 0)
        {
            List<Line> intersectedLines = lines.Where(x => IsLineIntersect(x, lines[0])).ToList();
            result.Add(lines[0]);
            foreach (var item in intersectedLines)
            {
                lines.Remove(item);
            }
            lines.Remove(lines[0]);
        }
        return result;
    }
    static bool IsLineIntersect(Line line1, Line line2)
    {
        if (IsLineConnect(line1, line2))
            return false;

        Vector2 p1 = line1.Point1;
        Vector2 p2 = line1.Point2;
        Vector2 p3 = line2.Point1;
        Vector2 p4 = line2.Point2;

        float o1 = Orientation(p1, p2, p3);
        float o2 = Orientation(p1, p2, p4);
        float o3 = Orientation(p3, p4, p1);
        float o4 = Orientation(p3, p4, p2);

        if (o1 * o2 < 0 && o3 * o4 < 0)
            return true;

        if (o1 == 0 && IsPointOnSegnent(p1, p2, p3))
            return true;
        if (o2 == 0 && IsPointOnSegnent(p1, p2, p4))
            return true;
        if (o3 == 0 && IsPointOnSegnent(p3, p4, p1))
            return true;
        if (o4 == 0 && IsPointOnSegnent(p3, p4, p2))
            return true;
        return false;
    }
    static float CrossProduct(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }

    static float Orientation(Vector2 a, Vector2 b, Vector2 c)
    {
        return CrossProduct(b - a, c - a);
    }
    static bool IsPointOnSegnent(Vector2 a, Vector2 b, Vector2 p)
    {
        return Mathf.Min(a.x, b.x) <= p.x && p.x <= MathF.Max(a.x, b.x) && MathF.Min(a.y, b.y) <= p.y && p.y <= MathF.Max(a.y, b.y);
    }

    static bool IsLineConnect(Line line1, Line line2)
    {
        return
            line1.Point1 == line2.Point1 ||
            line1.Point1 == line2.Point2 ||
            line1.Point2 == line2.Point1 ||
            line1.Point2 == line2.Point2;
    }
}
