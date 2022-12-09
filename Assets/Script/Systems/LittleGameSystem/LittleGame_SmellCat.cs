using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LittleGame_SmellCat : LittleGame
{
    public float zoomSize;
    public float zoomSpeed;
    [SerializeField] private Image fillImage;

    private Camera cam;
    private float originSize;

    public override void StartGame(Cat cat)
    {
        base.StartGame(cat);

        cam = Camera.main;
        originSize = cam.orthographicSize;
        fillImage.fillAmount = 0;
    }

    public void SmellCat()
    {
        cam.orthographicSize -= 0.01f * zoomSpeed;
        
        App.system.soundEffect.Play("Button");
        
        //FillAmount
        float distance = originSize - zoomSize;
        fillImage.fillAmount += 0.01f * zoomSpeed / distance;

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
