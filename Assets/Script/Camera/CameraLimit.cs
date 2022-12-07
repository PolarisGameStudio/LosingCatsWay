using Lean.Common;
using UnityEngine;

public class CameraLimit : MonoBehaviour
{
    public MyGridSystem myGridSystem;

    private Camera mainCamera;
    private LeanConstrainToBox leanConstrainToBox;

    private int lastZoom;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        leanConstrainToBox = GetComponent<LeanConstrainToBox>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int zoom = (int) mainCamera.orthographicSize;

        if (!lastZoom.Equals(zoom))
        {
            lastZoom = zoom;
            SetCameraLimit();
        }
    }

    private void SetCameraLimit()
    {
        int bufferX = lastZoom / 3;
        int bufferY = lastZoom / 5;

        if (bufferY >= 1)
            bufferY -= 1;

        leanConstrainToBox.Size = new Vector3((myGridSystem.width - bufferX * 2) * 5,
            (myGridSystem.height - bufferY * 2) * 5, 1);
    }
}