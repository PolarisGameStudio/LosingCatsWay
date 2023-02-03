using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LittleGame_SmellCat : LittleGame
{
    [Title("Game")]
    // [SerializeField] private float zoomSize;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private Image fillCircle;
    [SerializeField] private float fillSpeed;
    
    private Camera cam;
    private float originSize;

    public override void StartGame(Cat cat)
    {
        base.StartGame(cat);

        cam = Camera.main;
        originSize = cam.orthographicSize;
        fillCircle.fillAmount = 0;
    }

    public void SmellCat()
    {
        cam.orthographicSize -= zoomSpeed;
        fillCircle.fillAmount += fillSpeed;
        
        App.system.soundEffect.PlayUntilEnd("Button");

        if (fillCircle.fillAmount >= 1)
        {
            Close();
            cam.orthographicSize = originSize;
            cat.catHeartEffect.Play();
            // Success();
            // OpenLobby();
            SuccessToLobby();
            
            //ExitAnim
            anim.SetBool(CatAnimTable.IsCanExit.ToString(), true);
        }
    }
}
