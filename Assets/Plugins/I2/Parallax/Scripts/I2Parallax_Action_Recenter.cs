using UnityEngine;
using System.Collections;

namespace I2.Parallax
{
    public class I2Parallax_Action_Recenter : MonoBehaviour
    {
        public void Recenter()
        {
            I2Parallax_Manager.singleton.Recenter();
        }
    }
}
