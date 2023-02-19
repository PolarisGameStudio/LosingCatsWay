using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ScreenshotSystem : MvcBehaviour
{
    [SerializeField] private UIView view;

    private bool isShot;
    
    public Callback OnScreenshotComplete;
    public Callback OnScreenshotCancel;

    public void Open()
    {
        view.Show();
    }

    public void Close()
    {
        view.InstantHide();
    }

    public void Cancel()
    {
        Close();
        OnScreenshotCancel?.Invoke();
    }

    public void TakeScreenshot()
    {
        if (isShot) return;
        isShot = true;
        App.system.confirm.CloseOpenSoundEffect();
        App.system.soundEffect.Play("ED00020");
        StartCoroutine(Screenshot());
    }

    IEnumerator Screenshot()
    {
        Close();

        yield return new WaitForEndOfFrame();

        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        var texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

        yield return new WaitForEndOfFrame();

        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        yield return texture;

        byte[] _byte = texture.EncodeToPNG();

        string dateTime = App.system.myTime.MyTimeNow.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = "LosingCatWay_" + dateTime + ".png";

        NativeGallery.SaveImageToGallery(_byte, "Screenshots", fileName);

        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_PhotoTaken);

        isShot = false;
        OnScreenshotComplete?.Invoke();
    }
}
