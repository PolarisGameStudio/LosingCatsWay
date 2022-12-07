using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Mall_Item_", menuName = "Mall/Create Mall_Item")]
public class Mall_Item : ScriptableObject
{
    [Title("Information")] 
    public string id;
    
    [Title("Reward")] 
    public Reward[] rewards;

    [Title("Buy")] [EnumPaging] 
    public ItemBoughtType itemBoughtType;
    
    [HideIf("@itemBoughtType == ItemBoughtType.Free")] 
    public int price;

    [Title("BuyRule")] [EnumPaging] 
    public MallItemRefreshType refreshType;
    
    [HideIf("@refreshType == MallItemRefreshType.Infinity")] 
    public int limitCount;
    
    [HideIf("@refreshType == MallItemRefreshType.Infinity")] 
    public bool isSesson;
}