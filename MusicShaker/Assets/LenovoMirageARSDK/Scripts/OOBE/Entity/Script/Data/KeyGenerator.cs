using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace LenovoMirageARSDK.OOBE
{
    public class KeyGenerator : MonoBehaviour
    {

        public static KeyData GetData()
        {
            if (File.Exists(KeyData.path))
            {
                string tempJson = File.ReadAllText(KeyData.path);
                return JsonUtility.FromJson<KeyData>(tempJson);
            }
            else
            {
                return null;
            }

        }

    }
}
