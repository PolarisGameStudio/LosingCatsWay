using UnityEngine;

public class MyGrid
{
    public int Value;

    public int X;
    public int Y;
    
    public GameObject Content;

    public MyGrid(int value, GameObject content)
    {
        Value = value;
        Content = content;
    }
}