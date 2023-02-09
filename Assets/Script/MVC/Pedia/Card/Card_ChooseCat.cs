using TMPro;

public class Card_ChooseCat : MvcBehaviour
{
    public TextMeshProUGUI catTypeText;
    public CatSkin catSkin;

    public void SetData(string variety)
    {
        catTypeText.text = App.factory.stringFactory.GetCatVariety(variety);
        catSkin.ChangeSkin(variety);
    }

    public void Click()
    {
        
    }
}
