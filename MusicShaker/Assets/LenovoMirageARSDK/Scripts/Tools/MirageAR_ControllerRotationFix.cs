using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LenovoMirageARSDK
{
    /// <summary>
    /// Controller Should Rotate 180 Around Z Axis If Not In Editor 
    /// </summary>
    public class MirageAR_ControllerRotationFix : MonoBehaviour
    {
        /// <summary>
        /// The Controller Model Root
        /// </summary>
        private GameObject controllerModel;

        private void Awake()
        {
            controllerModel = gameObject.transform.Find("Root").gameObject;
            if (controllerModel == null) return;


            if (Application.isEditor)
            {
                controllerModel.transform.localEulerAngles=new Vector3(0,0,0);
            }
            else
            {
                controllerModel.transform.localEulerAngles = new Vector3(0, 0, 180);                
            }
        }

    }

}

