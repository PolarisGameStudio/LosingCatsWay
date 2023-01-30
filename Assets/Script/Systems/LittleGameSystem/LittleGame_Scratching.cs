using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LittleGame_Scratching : LittleGame
{
    [Title("Game")]
    public Image fillCircle;
    public float fillSpeed;

    public override void StartGame(Cat cat)
    {
        base.StartGame(cat);
        fillCircle.fillAmount = 0;
    }
    
    public void Drag()
    {
        fillCircle.fillAmount += fillSpeed;
        
        App.system.soundEffect.PlayUntilEnd("Button");

        if (fillCircle.fillAmount >= 1)
        {
            Close();

            //ExitAnim
            anim.SetBool(CatAnimTable.IsCanExit.ToString(), true);
            Success();
            OpenLobby();
        }
    }
}
