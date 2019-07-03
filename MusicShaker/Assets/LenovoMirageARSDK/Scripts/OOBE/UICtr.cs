namespace LenovoMirageARSDK
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using LenovoMirageARSDK.OOBE;

    public enum UIType
    {
        Splash,
        Welcome,
        Setting,
        Control,
        Usb,
        Beacon,
        DownView,
        CornerSelect,
        Eixt,
        Adaptive,
    }

    public class UICtr : MonoBehaviour
    {
        [SerializeField]
        List<TypeUI> m_uiList = new List<TypeUI>();

        private void Awake()
        {
            Instance = this;
            m_uiList.ForEach(x => { m_UserList.Add(x.Uitype, x.Tsf); x.Tsf.gameObject.SetActive(false); });
        }

        public Transform Open(UIType m_type, bool single = false)
        {
            if (single)
                m_uiList.ForEach(x => { x.Tsf.gameObject.SetActive(false); });

            if (m_UserList.ContainsKey(m_type))
            {
                m_UserList[m_type].gameObject.SetActive(true);
                return m_UserList[m_type];
            }

            return null;
        }

        public void Close(UIType m_type)
        {
            if (m_UserList.ContainsKey(m_type))
            {
                m_UserList[m_type].gameObject.SetActive(false);
            }
        }

        public static UICtr Instance;
        Dictionary<UIType, Transform> m_UserList = new Dictionary<UIType, Transform>();
    }

    [System.Serializable]
    public class TypeUI
    {
        public UIType Uitype;
        public Transform Tsf;
    }
}
