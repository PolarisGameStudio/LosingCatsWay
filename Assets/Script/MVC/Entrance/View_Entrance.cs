using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using Doozy.Runtime.Common.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class View_Entrance : ViewBehaviour
{
    [Title("Entrance")]
    [SerializeField] private Cat_Entrance[] frontCats;
    [SerializeField] private Cat_Entrance[] backCats;
    [SerializeField] private Cat_Entrance deadCat;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private Button maskButton;

    [Title("Bg")] [SerializeField] private Image bg;
    [SerializeField] private Sprite normalBg;
    [SerializeField] private Sprite naturalDeadSprite;
    [SerializeField] private Sprite sickDeadSprite;
    [SerializeField] private GameObject[] normalBgObjects;

    [Title("Fog")] [SerializeField] private Image fog;
    [SerializeField] private Sprite naturalFog;
    [SerializeField] private Sprite sickFog;

    [Title("Effects")] [SerializeField] private UIParticle naturalEffect;
    [SerializeField] private UIParticle sickEffect;

    [Title("Title")] [SerializeField] private GameObject normalTitle;
    [SerializeField] private GameObject deadTitle;

    [Title("ChooseDiary")] [SerializeField]
    private View_EntranceDiary chooseDiary;
    
    public override void Init()
    {
        base.Init();
        App.model.entrance.OnCatsChange += OnCatsChange;
        App.model.entrance.OnDeadCatChange += OnDeadCatChange;
        App.model.entrance.OnOpenTypeChange += OnOpenTypeChange;
    }

    public override void Close()
    {
        base.Close();
        HideAllCats();
    }

    private void OnCatsChange(object value)
    {
        List<Cat> cats = (List<Cat>)value;
        backCats.Shuffle();

        closeButton.SetActive(true);
        HideAllCats();

        for (int i = 0; i < cats.Count; i++)
        {
            var catData = cats[i].cloudCatData;

            if (i < 8)
            {
                frontCats[i].SetCatData(catData);
                frontCats[i].SetActive(true);
            }
            else
            {
                backCats[i].SetCatData(catData);
                backCats[i].SetActive(true);
            }
        }
    }

    private void OnDeadCatChange(object value)
    {
        if (value == null)
            return;
        
        HideAllCats();
        
        Cat cat = (Cat)value;
        deadCat.SetCatData(cat.cloudCatData);
        deadCat.SetActive(true);
        deadCat.StartDead();
        
        print(cat.cloudCatData.CatHealthData.SickId);

        if (cat.cloudCatData.CatHealthData.SickId.IsNullOrEmpty())
        {
            bg.sprite = naturalDeadSprite;
            fog.sprite = naturalFog;
            naturalEffect.gameObject.SetActive(true);
            naturalEffect.Play();
        }
        else
        {
            bg.sprite = sickDeadSprite;
            fog.sprite = sickFog;
            sickEffect.gameObject.SetActive(false);
            sickEffect.Play();
        }

        closeButton.SetActive(false);
    }

    private void OnOpenTypeChange(object value)
    {
        int index = Convert.ToInt32(value);

        if (index == 0)
        {
            //正常
            closeButton.SetActive(true);
            maskButton.interactable = true;
            bg.sprite = normalBg;

            for (int i = 0; i < normalBgObjects.Length; i++)
                normalBgObjects[i].SetActive(true);

            fog.gameObject.SetActive(false);

            normalTitle.SetActive(true);
            deadTitle.SetActive(false);

            naturalEffect.gameObject.SetActive(false);
            sickEffect.gameObject.SetActive(false);
            
            return;
        }

        if (index == 1)
        {
            //死
            closeButton.SetActive(false);
            maskButton.interactable = false;
            
            for (int i = 0; i < normalBgObjects.Length; i++)
                normalBgObjects[i].SetActive(false);

            fog.gameObject.SetActive(true);
            
            normalTitle.SetActive(false);
            deadTitle.SetActive(true);
        }
    }

    #region Method

    private void HideAllCats()
    {
        for (int i = 0; i < frontCats.Length; i++)
        {
            frontCats[i].SetActive(false);
        }

        for (int i = 0; i < backCats.Length; i++)
        {
            backCats[i].SetActive(false);
        }

        deadCat.SetActive(false);
    }

    #endregion

    public void OpenChooseDiary()
    {
        chooseDiary.Open();
    }

    public void CloseChooseDiary()
    {
        chooseDiary.Close();
    }
}
