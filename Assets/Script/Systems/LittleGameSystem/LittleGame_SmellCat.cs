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
        // cam.orthographicSize -= 0.001f * zoomSpeed;
        //
        // App.system.soundEffect.PlayUntilEnd("Button");
        //
        // //FillAmount
        // float distance = originSize - zoomSize;
        // fillCircle.fillAmount += 0.001f * zoomSpeed / distance;
        //
        // if (cam.orthographicSize <= zoomSize)
        // {
        //     cam.orthographicSize = originSize;
        //     Close();
        //     Success();
        //     cat.catHeartEffect.Play();
        //     OpenLobby();
        //         
        //     //ExitAnim
        //     anim.SetBool(CatAnimTable.IsCanExit.ToString(), true);
        // }

        cam.orthographicSize -= zoomSpeed;
        fillCircle.fillAmount += fillSpeed;
        
        App.system.soundEffect.PlayUntilEnd("Button");

        if (fillCircle.fillAmount >= 1)
        {
            Close();
            cam.orthographicSize = originSize;
            Success();
            cat.catHeartEffect.Play();
            OpenLobby();
            
            //ExitAnim
            anim.SetBool(CatAnimTable.IsCanExit.ToString(), true);
        }
    }
}
