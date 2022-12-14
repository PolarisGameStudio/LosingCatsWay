using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LittleGame_SmellCat : LittleGame
{
    [Title("Game")]
    [SerializeField] private float zoomSize;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private Image fillCircle;

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
        cam.orthographicSize -= 0.001f * zoomSpeed;
        
        App.system.soundEffect.Play("Button");
        
        //FillAmount
        float distance = originSize - zoomSize;
        fillCircle.fillAmount += 0.001f * zoomSpeed / distance;

        if (cam.orthographicSize <= zoomSize)
        {
            cam.orthographicSize = originSize;
            Close();
            Success();
            cat.catHeartEffect.Play();
            OpenLobby();
                
            //ExitAnim
            anim.SetBool(CatAnimTable.IsCanExit.ToString(), true);
        }
    }
}
