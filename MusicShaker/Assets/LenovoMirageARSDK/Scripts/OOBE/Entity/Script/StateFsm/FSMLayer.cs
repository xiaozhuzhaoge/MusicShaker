using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

/// <summary>
/// by lzh 2017.10.30  多层状态机
/// </summary>

namespace LenovoMirageARSDK.OOBE
{
    public class FSMLayer
    {
        FSM m_CurFsm;
        FSM m_FirstFsm;

        public FSMLayer()
        {

        }
        
        /// <summary>
        /// 添加状态机
        /// </summary>
        /// <param name="t_Fsm"></param>
        public void AddFsm(FSM t_Fsm, string m_layer = "Normal")
        {
            if (m_HsmList.ContainsKey(m_layer))
            {
                Debug.Log("该层已经拥有状态机");
                return;
            }
            else
            {
                m_HsmList.Add(m_layer,t_Fsm);
            }
        }

        /// <summary>
        /// 直接添加状态
        /// </summary>
        /// <param name="t_State"></param>
        public void AddState(State t_State, string m_Layer = "Normal")
        {
            if (m_HsmList.ContainsKey(m_Layer))
            {
                m_HsmList[m_Layer].AddState(t_State);
            }
            else
            {
                FSM t_fsm = new FSM();
                if (m_HsmList.Count == 0)
                {
                    m_FirstFsm = t_fsm;
                    m_CurTag = m_Layer;
                }
                m_HsmList.Add(m_Layer, t_fsm);
                m_HsmList[m_Layer].AddState(t_State);
            }
        }


        /// <summary>
        /// 多层次状态机入口
        /// </summary>
        /// <param name="t_Fsm"></param>
        public void Start(string t_StartLayer = "Normal")
        {
            if (m_HsmList.ContainsKey(t_StartLayer))
            {
                m_HsmList[t_StartLayer].Start();
            }
        }

        /// <summary>
        /// 多层状态机重置
        /// </summary>
        /// <returns></returns>
        public bool Rest()
        {
            bool isActive = false;
            foreach (var item in m_HsmList)
            {
                if (item.Value.Active)
                {
                    item.Value.Exit();
                    item.Value.Start();
                    isActive = true;
                }
            }
            return isActive;
        }

        /// <summary>
        /// 改变当前状态机层次
        /// </summary>
        /// <param name="t_tag"></param>
        public void ChangeLayer(string t_tag)
        {
            if (t_tag.Equals(m_CurTag))
                return;

            if (m_HsmList.ContainsKey(t_tag))
            {
                m_CurTag = t_tag;
                m_CurFsm.Exit();
                m_CurFsm = m_HsmList[t_tag];
                m_CurFsm.Start();
            }
        }

        /// <summary>
        /// 状态机Update驱动
        /// </summary>
        public void update()
        {
            foreach (var item in m_HsmList)
            {
                if (item.Value.Active)
                {
                    item.Value.CurrentState.update();
                }
            }
        }

        /// <summary>
        /// 返回当前层次正在执行的状态ID
        /// </summary>
        /// <param name="hsmLayer"></param>
        /// <returns></returns>
        public string CurrentID(string hsmLayer)
        {
            if (m_HsmList.ContainsKey(hsmLayer))
            {
                return m_HsmList[hsmLayer].CurrentState.ID;
            }

            return string.Empty;
        }

        /// <summary>
        /// 返回正在运行的状态
        /// </summary>
        /// <returns></returns>
        public State CurrentState(string hsmLayer)
        {
            if (m_HsmList.ContainsKey(hsmLayer))
            {
                return m_HsmList[hsmLayer].CurrentState;
            }

            return null;
        }

        /// <summary>
        /// 关闭状态机
        /// </summary>
        public void Close()
        {
            foreach (var item in m_HsmList)
            {
                if (item.Value.Active)
                {
                    item.Value.Exit();
                }
            }
            m_HsmList.Clear();
        }

        /// <summary>
        /// 强制进入状态
        /// </summary>
        /// <param name="t_state"></param>
        public void EnforceState(State t_state, string m_Layer = "Normal")
        {
            if (m_HsmList.ContainsKey(m_Layer))
            {
                m_HsmList[m_Layer].EnforceState(t_state);
            }
        }

        /// <summary>
        /// 根据ID跳转状态，多层
        /// </summary>
        /// <param name="eventName"></param>
        public bool SetTransition(string eventName)
        {
            bool HasAcitveFsm = false;
            foreach (var item in m_HsmList)
            {
                if (item.Value.Active)
                {
                    item.Value.SetTransition(eventName);
                    HasAcitveFsm = true;
                }
            }
            return HasAcitveFsm;
        }

        /// <summary>
        /// 根据ID跳转状态，单层
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public bool SetTransition(string eventName, string layer = "Normal")
        {
            bool HasAcitveFsm = false;
            if (m_HsmList.ContainsKey(layer))
            {
                if (m_HsmList[layer].Active)
                {
                    HasAcitveFsm = true;
                    m_HsmList[layer].SetTransition(eventName);
                }
            }
            return HasAcitveFsm;
        }

        string m_CurTag;  //层次标记
        Dictionary<string, FSM> m_HsmList = new Dictionary<string, FSM> ();  //对应关系
    }
}
