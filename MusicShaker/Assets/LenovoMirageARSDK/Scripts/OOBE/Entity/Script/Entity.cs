//using UniRx;
using UnityEngine;
using System;

namespace LenovoMirageARSDK.OOBE
{

    public abstract class Entity : IEntity
    {
        private FSMLayer                            m_FsmLayer;
        private IDisposable                         m_Disposable;

        public Entity()
        {
            initEntity();
        }

        public Entity(object user)
        {
            this.user = user;
            initEntity();
        }     

        protected GameObject                        self;
        protected object                            user;
        private Action<string, string>              m_ValueChnageFunc;
        private Action                              m_DestroyFunc;

        void initEntity()
        {
            m_FsmLayer = new LenovoMirageARSDK.OOBE.FSMLayer();
            m_DestroyFunc = null;
            InitState();

            if (m_Disposable != null)
                m_Disposable.Dispose();
        }

        /// <summary>
        /// 添加状态
        /// </summary>
        protected void AddState(State m_State)
        {
            if (m_FsmLayer == null)
                return;

            m_FsmLayer.AddState(m_State);
        }

        /// <summary>
        /// 开始状态机
        /// </summary>
        public virtual IEntity Begin(GameObject SelfObj = null,string Layer = "Normal")
        {
            if (m_FsmLayer == null)
                return null;

            m_FsmLayer.Start(Layer);

            if (SelfObj != null)
            {
                //self = SelfObj;
                //m_Disposable = Observable.EveryUpdate()
                //                         .Do(x => m_FsmLayer.update())
                //                         .DoOnCancel(() => Destroy())
                //                         .Subscribe()
                //                         .AddTo(SelfObj);
            }
            else
            {
                //m_Disposable = Observable.EveryUpdate()
                //    .Do(x => m_FsmLayer.update())
                //    .Subscribe();
            }

            return this;
        }

        public void Updata()
        {
            if (m_FsmLayer != null)
                m_FsmLayer.update();
        }

        public Action DestroyEvent(Action action = null)
        {
            if (action != null)
                m_DestroyFunc += action;
            return m_DestroyFunc;
        }

        public object GetUser()
        {
            return user;
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        public void RestFsm()
        {
            m_FsmLayer.Rest();
        }

        /// <summary>
        /// 状态切换回调
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public IEntity OnValueChange(Action<string, string> func)
        {            
            m_ValueChnageFunc = func;
            return this;
        }

        /// <summary>
        /// 初始化状态
        /// </summary>
        protected abstract void InitState();

        /// <summary>
        /// 手动卸载或传递Transform跟随Transform周期
        /// </summary>
        public virtual void Destroy()
        {
            if (m_Disposable != null)
            {
                m_Disposable.Dispose();
                m_Disposable = null;
            }

            if (m_FsmLayer != null)
            {
                m_FsmLayer.Close();
                m_FsmLayer = null;
            }

            (this as IEntity).RemoveEntity();

            OnDestroy();

            if (m_DestroyFunc != null)
            {
                m_DestroyFunc();
                m_DestroyFunc = null;
            }
        }

        public string GetCurrentState()
        {
            if (m_FsmLayer == null)
                return "";

            return m_FsmLayer.CurrentState("Normal").ID;
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Layer"></param>
        public void SetState(string id, string Layer = "Normal")
        {
            if (m_FsmLayer == null)
                return;

            m_FsmLayer.SetTransition(id, Layer);

            if (m_ValueChnageFunc != null)
                m_ValueChnageFunc(id, m_FsmLayer.CurrentID(Layer));
        }

        public void SetState<T>(T id) where T : System.IConvertible
        {
            if (m_FsmLayer == null)
                return;

            m_FsmLayer.SetTransition(id.ToString(), "Normal");

            if (m_ValueChnageFunc != null)
                m_ValueChnageFunc(id.ToString(), m_FsmLayer.CurrentID("Normal"));
        }
        /// <summary>
        /// 卸载回调
        /// </summary>
        protected abstract void OnDestroy();
    }

}
