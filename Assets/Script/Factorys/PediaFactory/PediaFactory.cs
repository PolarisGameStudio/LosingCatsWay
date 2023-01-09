using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PediaFactory : SerializedMonoBehaviour
{
    [Title("Id")]
    [SerializeField] private List<string> catPediaIds = new List<string>();
    [SerializeField] private List<string> itemPediaIds = new List<string>();
    [SerializeField] private List<string> hobbyPediaIds = new List<string>();
    [SerializeField] private List<string> sickPediaIds = new List<string>();

    [Title("Sprites")] [SerializeField]
    private Dictionary<string, Sprite> pediaSprites = new Dictionary<string, Sprite>();
    [SerializeField] private Dictionary<string, bool> pediaUnlocks = new Dictionary<string, bool>();

    public List<string> GetPediaIds(int type)
    {
        if (type == 0)
            return catPediaIds;
        if (type == 1)
            return itemPediaIds;
        if (type == 2)
            return hobbyPediaIds;
        return sickPediaIds;
    }

    public Sprite GetPediaSprite(string id)
    {
        if (pediaSprites.ContainsKey(id))
            return pediaSprites[id];
        return null;
    }

    public bool GetPediaUnlock(string id)
    {
        return pediaUnlocks[id];
    }
}
