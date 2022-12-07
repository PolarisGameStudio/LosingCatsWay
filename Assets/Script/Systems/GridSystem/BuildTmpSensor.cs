using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTmpSensor : MonoBehaviour
{
    private Vector3 lastPosition = Vector3.one;

    private void Update()
    {
        Vector3 nowPosition = transform.position;

        if (!lastPosition.Equals(nowPosition))
        {
            lastPosition = nowPosition;
            buildTmpPositionChange?.Invoke(lastPosition);
        }
    }

    public delegate void BuildTmpPositionChange(Vector3 vector3);

    public BuildTmpPositionChange buildTmpPositionChange;
}