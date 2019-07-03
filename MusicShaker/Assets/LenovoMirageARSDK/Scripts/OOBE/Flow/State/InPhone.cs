namespace LenovoMirageARSDK.OOBE
{
    using UnityEngine;
    using LenovoMirageARSDK;
    using Ximmerse.Vision;

    public class InPhone : State
    {
        #region Private Properties

        private Transform tsf;
        private Transform rawimage;

        /// <summary>
        /// Max Time
        /// </summary>
        private float m_MaxTime=10;

        /// <summary>
        /// Record Pass Time
        /// </summary>
        private float m_RecordTime;

        /// <summary>
        /// Symbol Whether Has Jumped Scene
        /// </summary>
        private bool m_IsJumpedScene=false;

        #endregion //Private Properties

        public InPhone(IEntity parentEntity) : base(parentEntity)
        {
            stateID = "InPhone";
        }

        public InPhone() : base()
        {
            stateID = "InPhone";
        }

        public override void EnterState()
        {
            tsf = UICtr.Instance.Open(UIType.Adaptive);

            InitUI();            

            // Bind Any Button Down Event
            VisionSDK.Instance.Input.OnButtonDown+= OnButtonDown;
        }

        public override void ExitState()
        {
        }

        public override void update()
        {
            if (m_RecordTime>= m_MaxTime)
            {
                // Load Next Level
                UseGuide.EnterNextScene();
                m_RecordTime = 0;
            }
            else
            {
                m_RecordTime += Time.deltaTime;
            }
        }

        #region Private Method      

        /// <summary>
        /// On Any Button Down,Enter The Game Scene
        /// </summary>
        private void OnButtonDown(object sender, ButtonEventArgs eventArguments)
        {
            // Load Next Level
            UseGuide.EnterNextScene();
            m_RecordTime = 0;
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Init UI
        /// </summary>
        private void InitUI()
        {
            rawimage = tsf.Find("RawImage");
            rawimage.gameObject.SetActive(true);
            rawimage.GetComponent<RectTransform>().sizeDelta = (rawimage.transform.parent as RectTransform).sizeDelta;
            tsf.Find("Left").gameObject.SetActive(false);
            tsf.Find("Right").gameObject.SetActive(false);
            tsf.Find("Button").gameObject.SetActive(false);
            tsf.Find("Text").gameObject.SetActive(false);
            tsf.Find("ModelName").gameObject.SetActive(false);
            tsf.Find("Image").gameObject.SetActive(false);
        }

        #endregion //Private Method

    }
}