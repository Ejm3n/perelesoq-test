using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SmartHome.Utils
{
    public class SceneAutoLoader : MonoBehaviour
    {
        void Awake()
        {
            Utils.LoadAllScenesFromBuildSettings();
        }
    }
}
