using DeviceManager;
using LenovoMirageARSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LenovoMirageARSDK.Example
{
    /// <summary>
    /// The Example of LenovoMirageARSDK
    /// Example of The Android Persmission
    /// </summary>
    public class BasicExample_7_AndroidPermission_Ctrl : MonoBehaviour
    {
        public Text text;

        // Calendar Permission
        private string targetPermissionName = "android.permission.READ_CALENDAR";

        // Camera and Call_Phone permission
        private string[] targetPermissionNames = {
        "android.permission.CAMERA",
        "android.permission.CALL_PHONE"
        };


        #region Permission Request

        /// <summary>
        /// Request Single Permission Once
        /// </summary>
        private void RequestPermission(string permission)
        {
            DeviceUtils.RequestPermission(permission).ThenAction((result) =>
            {
                if (result.GrantResults[0])
                {
                    Debug.Log(result.PermissionNames[0] + " is Granted");
                }
                else
                {
                    if (DeviceUtils.IsRefuseShowRequestDialog(result.PermissionNames[0]))
                    {
                        text.text= result.PermissionNames[0] + " Denied and Refuse Show Request Dialog";
                        Debug.Log(result.PermissionNames[0] + " Denied and Refuse Show Request Dialog");
                    }
                    else
                    {
                        Debug.Log(result.PermissionNames[0] + " is Denied");
                    }
                }

            });
        }

        /// <summary>
        /// Request Multi Permissions Once
        /// </summary>
        private void RequestMultiPermissions(string[] permissions)
        {
            DeviceUtils.RequestPermissions(permissions).ThenAction((result) =>
            {
                if (result.IsAllGranted)
                {
                    Debug.Log("All Permission is Granted");
                }
                else
                {
                    for (int i = 0; i < result.PermissionNames.Length; i++)
                    {
                        if (result.GrantResults[i])
                        {
                            Debug.Log(result.PermissionNames[i] + " is Granted");
                        }
                        else
                        {
                            if (DeviceUtils.IsRefuseShowRequestDialog(result.PermissionNames[i]))
                            {
                                text.text += result.PermissionNames[i] + " Denied and Refuse Show Request Dialog";
                                Debug.Log(result.PermissionNames[i] + " Denied and Refuse Show Request Dialog");
                            }
                            else
                            {
                                Debug.Log(result.PermissionNames[i] + " is Denied");
                            }
                        }
                    }
                }
            });
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Request Single Permission Once
        /// </summary>
        public void CallRequestPermission()
        {
            text.text = "";

            if (DeviceUtils.CheckPermission(targetPermissionName))
            {
                text.text = targetPermissionName + " Is Granted";
            }
            else
            {
                RequestPermission(targetPermissionName);
            }            
        }

        /// <summary>
        /// Request Multi Permissions Once
        /// </summary>
        public void CallRequestMultiPermissions()
        {
            text.text = "";

            foreach (var item in targetPermissionNames)
            {
                if (DeviceUtils.CheckPermission(item))
                {
                    text.text += item + " Is Granted; ";
                }
            }

            RequestMultiPermissions(targetPermissionNames);
        }

        #endregion


    }
}

