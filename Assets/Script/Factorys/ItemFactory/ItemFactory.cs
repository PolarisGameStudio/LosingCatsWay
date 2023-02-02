using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemFactory : SerializedMonoBehaviour
{
    [Searchable, SerializeField] private List<Item> items;
    public Dictionary<int, Reward[]> LevelRewards = new Dictionary<int, Reward[]>();
    public Dictionary<string, GameObject> avatarEffects;

    #region MVC

    private MyApplication app = null;

    protected MyApplication myApp
    {
        get
        {
            if (app == null)
                app = FindObjectOfType<MyApplication>();
            
            return app;
        }
    }

    #endregion

    public Item GetItem(string id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == id)
                return items[i];
        }

        return null;
    }

    // Based on Count
    public List<Item> GetHoldItems(ItemType targetType) // 0 = 全部
    {
        List<Item> result = new List<Item>();
        
        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];

            if (targetType == ItemType.All) // 全部 要去除不用的
            {
                if (item.itemType == ItemType.Icon)
                    continue;
                if (item.itemType == ItemType.Coin)
                    continue;
                if (item.itemType == ItemType.Diamond)
                    continue;
                if (item.itemType == ItemType.Play)
                    continue;
                if (item.itemType == ItemType.Unlock)
                    continue;
            }
            else if (item.itemType != targetType)
                continue;

            if (item.Count > 0)
                result.Add(item);
        }

        return result;
    }

    // Ignore Count
    public List<Item> GetItemByType(int type)
    {
        List<Item> result = new List<Item>();

        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];

            if (type != 0 && item.itemType != (ItemType)type)
                continue;

            if (item.itemType == ItemType.Unlock)
                continue;

            result.Add(item);
        }

        return result;
    }

    // Get current level rewards from 0 to 10
    public List<Reward[]> GetRewardsByTenLevel(int level)
    {
        List<Reward[]> result = new List<Reward[]>();
        MathfExtension.GetNumberRangeByTen(level, out int startIndex, out int endIndex);

        for (int i = startIndex; i <= endIndex; i++)
        {
            if (LevelRewards.ContainsKey(i))
                result.Add(LevelRewards[i]);
            else
                result.Add(null);
        }

        return result;
    }

    public Reward[] GetRewardsByLevel(int level)
    {
        return LevelRewards.ContainsKey(level) ? LevelRewards[level] : null;
    }

    public List<Item> GetUnlockItemsByLevel(int level) // 取等級解鎖的Items // todo LevelUpSystem, CatGuideCard的解鎖項
    {
        List<Item> result = new List<Item>();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].unlockLevel != level)
                continue;
            result.Add(items[i]);
        }

        return result;
    }
}