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

    // Start is called before the first frame update
    void Start()
    {
    }

    [Button]
    public void T()
    {
        Vector2 a = new Vector2(0, 0);
        Vector2 b = new Vector2(5.12f, 0);
        Vector2 c = new Vector2(0, 5.12f);
        Vector2 d = new Vector2(5.12f, 5.12f);
        
        Vector2 e = new Vector2(5.12f, 0);
        Vector2 f = new Vector2(10.24f, 0);
        Vector2 g = new Vector2(5.12f, 5.12f);
        Vector2 h = new Vector2(10.24f, 5.12f);

        List<MyEdge> myEdges = new List<MyEdge>();

        myEdges.Add(new MyEdge(a, b));
        myEdges.Add(new MyEdge(a, c));
        myEdges.Add(new MyEdge(c, d));
        myEdges.Add(new MyEdge(b, d));
        
        myEdges.Add(new MyEdge(e, f));
        myEdges.Add(new MyEdge(e, g));
        myEdges.Add(new MyEdge(g, h));
        myEdges.Add(new MyEdge(f, h));
        
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

        polygonCollider2D.SetPath(0, result);
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

    public Vector2 GetAnotherVector2(Vector2 vector2)
    {
        if (vector2 == A)
            return B;

        return A;
    }
}