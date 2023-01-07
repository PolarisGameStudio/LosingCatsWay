using System;

[Serializable]
public class Reward
{
    public Item item;
    public int count;

    public Reward(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public Reward()
    {
    }
}
