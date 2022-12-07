using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class ChooseUI : MonoBehaviour
{
    #region Sub-Class

    [System.Serializable]
    public class ImgData
    {
        [FoldoutGroup("Data")] public string name;
        [FoldoutGroup("Data")] public Image img;

        [FoldoutGroup("Data")] public Sprite defaultSprite;
        [FoldoutGroup("Data")] public Sprite chooseSprite;

        [FoldoutGroup("Data")] public Color32 defaultImgColor = Color.white;
        [FoldoutGroup("Data")] public Color32 chooseImgColor = Color.white;
    }

    [System.Serializable]
    public class TMPData
    {
        [FoldoutGroup("Data")] public string name;
        [FoldoutGroup("Data")] public TextMeshProUGUI tmp;

        [FoldoutGroup("Data")] public Color32 defaultTmpColor = Color.white;
        [FoldoutGroup("Data")] public Color32 chooseTmpColor = Color.white;
    }

    #endregion

    public Image icon;
    [Space(10)]

    public ImgData[] imgDatas;
    public TMPData[] tmpDatas;

    public void Choose()
    {
        //Image
        for (int i = 0; i < imgDatas.Length; i++)
        {
            if (imgDatas[i].chooseSprite != null) imgDatas[i].img.sprite = imgDatas[i].chooseSprite;
            imgDatas[i].img.color = imgDatas[i].chooseImgColor;
        }

        //TMP
        for (int i = 0; i < tmpDatas.Length; i++)
        {
            tmpDatas[i].tmp.color = tmpDatas[i].chooseTmpColor;
        }
    }

    public void Cancel()
    {
        //Image
        for (int i = 0; i < imgDatas.Length; i++)
        {
            if (imgDatas[i].defaultSprite != null) imgDatas[i].img.sprite = imgDatas[i].defaultSprite;
            imgDatas[i].img.color = imgDatas[i].defaultImgColor;
        }

        //TMP
        for (int i = 0; i < tmpDatas.Length; i++)
        {
            tmpDatas[i].tmp.color = tmpDatas[i].defaultTmpColor;
        }
    }

    #region GetSet

    public Image GetImage(string name)
    {
        for (int i = 0; i < imgDatas.Length; i++)
        {
            //如果有則回傳
            if (imgDatas[i].name == name) return imgDatas[i].img;
        }

        //如果沒有則回傳空值
        return null;
    }

    public TextMeshProUGUI GetTMP(string name)
    {
        for (int i = 0; i < tmpDatas.Length; i++)
        {
            //如果有則回傳
            if (tmpDatas[i].name == name) return tmpDatas[i].tmp;
        }

        //如果沒有則回傳空值
        return null;
    }

    #endregion
}