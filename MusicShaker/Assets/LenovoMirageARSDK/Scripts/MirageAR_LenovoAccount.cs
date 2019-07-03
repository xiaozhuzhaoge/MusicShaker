using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LenovoMirageARSDK
{
    /// <summary>
    /// Lenovo Account
    /// </summary>
    public class MirageAR_LenovoAccount
    {
        #region Private Properties 

        /// <summary>
        /// UserName
        /// </summary>
        private string m_UserName;

        /// <summary>
        /// UserID
        /// </summary>
        private string m_UserID;

        #endregion

        #region Attribute

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName
        {
            get
            {
                return m_UserName;
            }

            set
            {
                m_UserName = value;
            }
        }

        /// <summary>
        /// UserID
        /// </summary>
        public string UserID
        {
            get
            {
                return m_UserID;
            }

            set
            {
                m_UserID = value;
            }
        }

        #endregion


    }

}
