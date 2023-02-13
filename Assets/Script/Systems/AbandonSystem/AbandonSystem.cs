using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbandonSystem : MvcBehaviour
{
    [SerializeField] private UIView view;
    
    [Title("ChooseCat")]
    [SerializeField] private Card_Abandon[] cards;
    [SerializeField] private Button chooseCatButton;

    [Title("Confirm")] [SerializeField] private UIView confirmView;
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private Button copyButton;
    [SerializeField] private GameObject copyMask;
    [SerializeField] private Button infoButton;
    [SerializeField] private GameObject infoMask;
    [SerializeField] private TextMeshProUGUI idText;

    [Title("FinalConfirm")] [SerializeField]
    private UIView finalConfirmView;

    [Title("Background")] [SerializeField] private Image confirmBg;

    private List<Cat> Cats;
    private Cat selectedCat;

    private string AbandonLocation;

    #region OpenClose
    
    private void Open()
    {
        view.Show();
    }

    public void Close()
    {
        view.InstantHide();
    }

    public void OpenConfirm()
    {
        confirmView.Show();
        catSkin.SetActive(true);
    }

    public void CloseConfirm()
    {
        confirmView.InstantHide();
        catSkin.SetActive(false);
    }

    public void OpenFinalConfirm()
    {
        finalConfirmView.Show();
    }

    public void CloseFinalConfirm()
    {
        finalConfirmView.InstantHide();
    }
    
    #endregion
    
    public void Active(string abandonLocation)
    {
        Open();

        AbandonLocation = abandonLocation;

        confirmBg.sprite = App.factory.catFactory.GetCatLocationSprite(abandonLocation);
        
        chooseCatButton.gameObject.SetActive(false);
        selectedCat = null;

        Cats = App.system.cat.GetCats();
        for (int i = 0; i < cards.Length; i++)
        {
            if (i >= Cats.Count)
                cards[i].SetActive(false);
            else
            {
                cards[i].SetActive(true);
                cards[i].SetSelect(false);
                cards[i].SetData(Cats[i].cloudCatData);
            }
        }
    }

    public void Select(int index)
    {
        chooseCatButton.gameObject.SetActive(true);
        selectedCat = Cats[index];

        for (int i = 0; i < cards.Length; i++)
        {
            if (i == index)
                cards[i].SetSelect(true);
            else
                cards[i].SetSelect(false);
        }
    }

    public void ConfirmSetData()
    {
        if (selectedCat == null)
            return;
        
        OpenConfirm();
        
        catSkin.ChangeSkin(selectedCat.cloudCatData);
        SkeletonGraphic skeletonGraphic = catSkin.skeletonGraphic;
        skeletonGraphic.AnimationState.SetAnimation(0, "Situation_Cat/Abandoned_1-1", false);
        skeletonGraphic.AnimationState.AddAnimation(0, "Situation_Cat/Abandoned_1-2", true, 0);

        bool isChip = selectedCat.cloudCatData.CatHealthData.IsChip;
        copyButton.interactable = isChip;
        copyMask.SetActive(!isChip);
        idText.text = isChip ? $"ID:{selectedCat.cloudCatData.CatData.CatId}" : "ID:-";
        
        infoButton.interactable = isChip;
        infoMask.SetActive(!isChip);
    }

    public void ConfirmAbandon()
    {
        Item item = App.factory.itemFactory.GetItem("ISL00002");

        if (item.Count <= 0)
        {
            CloseFinalConfirm();
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix);
            return;
        }
        
        App.system.confirm.Active(ConfirmTable.AbandonCatConfirm, okEvent: () => 
        {
            //Notify
            App.system.catNotify.Remove(selectedCat);
            App.system.cat.Remove(selectedCat);

            selectedCat.cloudCatData.CatData.Owner = AbandonLocation;
            
            selectedCat.cloudCatData.CatSurviveData.CleanLitterTimestamp = new Timestamp();
            selectedCat.cloudCatData.CatSurviveData.CleanLitterCount = 0;
            selectedCat.cloudCatData.CatSurviveData.UsingLitter = -1;

            if (!string.IsNullOrEmpty(selectedCat.cloudCatData.CatSkinData.UseSkinId))
            {
                Item skinItem = App.factory.itemFactory.GetItem(selectedCat.cloudCatData.CatSkinData.UseSkinId);
                skinItem.Count++;
                selectedCat.cloudCatData.CatSkinData.UseSkinId = string.Empty;
            }
            
            App.system.cloudSave.SaveCloudCatData(selectedCat.cloudCatData);
            item.Count -= 1;
            
            CloseFinalConfirm();
            CloseConfirm();
            Close();
        });
    }

    public void CopyId()
    {
        selectedCat.cloudCatData.CatData.CatId.CopyToClipboard();
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Copied);
    }
}
