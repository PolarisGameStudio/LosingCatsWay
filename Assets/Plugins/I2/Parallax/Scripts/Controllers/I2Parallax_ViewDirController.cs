using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Parallax
{
    [System.Serializable]
    public class I2Parallax_ViewDirController
    {
        [System.NonSerialized] public Vector2 ViewPosition;

        public virtual void Stop() { }
        public virtual void Start() { Reset(); }

        public virtual void Reset()
        {
            ViewPosition = Vector2.zero;
        }

        public virtual void Update() {}
    }
}