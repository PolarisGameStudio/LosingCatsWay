using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PolyNav;
using Sirenix.OdinInspector;
using UnityEngine;

public class JustTestRoom : MonoBehaviour
{
    public PolyNavMap polyNavMap;
    public PolygonCollider2D polygonCollider2D;
    public Room room;

    public List<MyEdge> CreateEdges(Vector2 originPoint, int roomSize)
    {
        List<Vector2> points = new List<Vector2>();
        points.Add(originPoint);

        var maxRight = originPoint + new Vector2(roomSize * 5.12f, 0);
        var maxUp = originPoint + new Vector2(0, roomSize * 5.12f);
        var maxRightUp = new Vector2(maxRight.x, maxUp.y);

        // right
        for (int i = 1; i <= roomSize; i++)
            points.Add(originPoint + new Vector2(i * 5.12f, 0));

        // up
        for (int i = 1; i <= roomSize; i++)
            points.Add(maxRight + new Vector2(0, i * 5.12f));

        // left
        for (int i = 1; i <= roomSize; i++)
            points.Add(maxRightUp - new Vector2(i * 5.12f, 0));

        // down
        for (int i = 1; i < roomSize; i++)
            points.Add(maxUp - new Vector2(0, i * 5.12f));

        List<MyEdge> result = new List<MyEdge>();

        for (int i = 0; i < points.Count; i++)
        {
            int a = i;
            int b = i + 1;

            if (b == points.Count)
                b = 0;

            result.Add(new MyEdge(points[a], points[b]));
        }

        return result;
    }

    public List<Vector2> CreateMapPoints(List<List<MyEdge>> allEdges)
    {
        List<MyEdge> myEdges = new List<MyEdge>();

        for (int i = 0; i < allEdges.Count; i++)
        {
            List<MyEdge> tmp = allEdges[i];

            for (int j = 0; j < tmp.Count; j++)
                myEdges.Add(tmp[j]);
        }

        Dictionary<string, MyEdge> dictionary = new Dictionary<string, MyEdge>();

        for (int i = 0; i < myEdges.Count; i++)
        {
            var myEdge = myEdges[i];
            var myEdgeId = myEdge.Id;

            if (dictionary.ContainsKey(myEdgeId))
                dictionary[myEdgeId].Count++;
            else
                dictionary.Add(myEdgeId, myEdge);
        }

        myEdges.Clear();
        var list = dictionary.ToList();

        for (int i = 0; i < list.Count; i++)
        {
            MyEdge myEdge = list[i].Value;

            if (myEdge.Count > 1)
                continue;

            myEdges.Add(myEdge);
        }

        // 偵測多餘點
        List<Vector2> deletePoints = new List<Vector2>();

        for (int i = 0; i < myEdges.Count; i++)
        {
            MyEdge myEdge = myEdges[i];

            Vector2? v = null;

            if (myEdge.IsVertical())
            {
                if (myEdges.Find(x => x.HasA(myEdge.B)) != null)
                    v = myEdge.B;

                if (myEdges.Find(x => x.HasB(myEdge.A)) != null)
                    v = myEdge.A;
            }
            else
            {
                if (myEdges.Find(x => x.HasA(myEdge.B)) != null)
                    v = myEdge.B;

                if (myEdges.Find(x => x.HasB(myEdge.A)) != null)
                    v = myEdge.A;
            }

            if (!deletePoints.Exists(x => x == v))
                if (v != null)
                    deletePoints.Add((Vector2)v);
        }

        List<Vector2> result = new List<Vector2>();

        result.Add(myEdges[0].A);
        result.Add(myEdges[0].B);

        Vector2 prevVector2 = myEdges[0].B;

        myEdges.RemoveAt(0);

        while (myEdges.Count != 0)
        {
            if (myEdges.Count == 1)
                break;

            int index = myEdges.FindIndex(x => x.HasVector2(prevVector2));

            if (index != -1)
            {
                var myEdge = myEdges[index];
                Vector2 anotherPoint = myEdge.GetAnotherVector2(prevVector2);
                result.Add(anotherPoint);
                prevVector2 = anotherPoint;
                myEdges.RemoveAt(index);
            }
        }

        print(deletePoints.Count);

        // 刪除多餘點
        for (int i = 0; i < deletePoints.Count; i++)
        {
            Vector2 deletePoint = deletePoints[i];
            int index = result.FindIndex(x => x == deletePoint);

            if (index != -1)
                result.RemoveAt(index);
        }

        return result;
    }

    [Button]
    public void T()
    {
        List<List<MyEdge>> allEdges = new List<List<MyEdge>>();

        List<MyEdge> edgesA = CreateEdges(new Vector2(-5.12f, -5.12f), 2);
        allEdges.Add(edgesA);

        List<Vector2> mapPoints = CreateMapPoints(allEdges);

        polygonCollider2D.SetPath(0, mapPoints);
        polyNavMap.GenerateMap();
    }
}

public class MyEdge
{
    public MyEdge(Vector2 v1, Vector2 v2)
    {
        Count = 1;

        if (v1.x < v2.x)
        {
            A = v1;
            B = v2;
            Id = (int)(A.x / 5.12f) + "." + (int)(A.y / 5.12f) + "-" + (int)(B.x / 5.12f) + "." + (int)(B.y / 5.12f);
            return;
        }

        if (v1.x > v2.x)
        {
            A = v2;
            B = v1;
            Id = (int)(A.x / 5.12f) + "." + (int)(A.y / 5.12f) + "-" + (int)(B.x / 5.12f) + "." + (int)(B.y / 5.12f);
            return;
        }

        if (v1.y < v2.y)
        {
            A = v1;
            B = v2;
        }
        else
        {
            A = v2;
            B = v1;
        }

        Id = (int)(A.x / 5.12f) + "." + (int)(A.y / 5.12f) + "-" + (int)(B.x / 5.12f) + "." + (int)(B.y / 5.12f);
    }

    public Vector2 A;
    public Vector2 B;
    public string Id;

    // 特規 為了方便
    public int Count;

    public bool HasVector2(Vector2 vector2)
    {
        if (vector2 == A)
            return true;

        if (vector2 == B)
            return true;

        return false;
    }

    public bool HasA(Vector2 vector2)
    {
        if (vector2 == A)
            return true;

        return false;
    }

    public bool HasB(Vector2 vector2)
    {
        if (vector2 == B)
            return true;

        return false;
    }

    public Vector2 GetAnotherVector2(Vector2 vector2)
    {
        if (vector2 == A)
            return B;

        return A;
    }

    public bool IsVertical()
    {
        Vector2 tmp = B - A;

        if (tmp.y.Equals(5.12f))
            return true;

        return false;
    }
}