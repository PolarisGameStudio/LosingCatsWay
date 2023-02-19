using UnityEngine;
using System.Collections;

public class Screenshot : MvcBehaviour
{
    public GameObject[] captureUI;

    public void CaptureScreen()
    {
        StartCoroutine(captureScreen());
    }

    IEnumerator captureScreen()
    {
        for (int i = 0; i < captureUI.Length; i++)
        {
            captureUI[i].SetActive(false);
        }

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

        //ES3.SaveImage(texture, fileName);

#if UNITY_ANDROID || UNITY_IPHONE
        NativeGallery.SaveImageToGallery(_byte, "Screenshots", fileName);
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE
        //todo 電腦版存自拍
#endif

        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_PhotoTaken);
    }
}
