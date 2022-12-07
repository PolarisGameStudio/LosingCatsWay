using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ChancePool<T>
{
    private List<Node> nodes;

    public ChancePool()
    {
        nodes = new List<Node>();
    }

    public void AddItem(T t, float value)
    {
        Node node = new Node(t, value);
        nodes.Add(node);
    }

    public void RemoveItem(T t)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (t.Equals(nodes[i].Item))
            {
                nodes.RemoveAt(i);
                break;
            }
        }
    }

    public void UpdateItem(T t, float value)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (t.Equals(nodes[i].Item))
            {
                nodes[i].Value = value;
                break;
            }
        }
    }

    public object GetItem()
    {
        float totalValue = 0;

        for (int i = 0; i < nodes.Count; i++)
            totalValue += nodes[i].Value;

        float randomValue = Random.Range(0, totalValue);

        float minValue = 0;
        float maxValue = 0;

        for (int i = 0; i < nodes.Count; i++)
        {
            minValue = maxValue;
            maxValue += nodes[i].Value;

            if (randomValue <= maxValue && randomValue >= minValue)
                return nodes[i].Item;
        }

        return null;
    }

    public class Node
    {
        public Node(T t, float value)
        {
            Item = t;
            Value = value;
        }

        public T Item;
        public float Value;
    }
}