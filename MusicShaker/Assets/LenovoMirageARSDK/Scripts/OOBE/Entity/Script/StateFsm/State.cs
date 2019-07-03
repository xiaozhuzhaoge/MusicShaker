using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary> State
/// by lzh 2017.08.10
/// </summary>

namespace LenovoMirageARSDK.OOBE
{
    public abstract class State
    {
        protected IEntity                                                               parent;
        protected Dictionary<string, string>                                            map = new Dictionary<string, string>();
        protected string                                                                stateID;
        public string                                                                   ID { get { return stateID; } }

        public State() { }
        public State(IEntity AI)
        {
            parent = AI;
        }

        /// <summary>
        /// 为状态加入执行条件
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="id"></param>
        public void AddTransition(string key, string state)
        {
            if (key == null || state == null)
            {
                return;
            }

            if (map.ContainsKey(key))
            {
                return;
            }

            map.Add(key, state);
        }

        public IEntity GetEntity()
        {
            return parent;
        }
        /// <summary>
        /// 删除执行状态的条件
        /// </summary>
        /// <param name="myTransition"></param>
        public void DeleteTransition(string key)
        {
            if (key == null)
            {
                return;
            }

            if (map.ContainsKey(key))
            {
                map.Remove(key);
                return;
            }
        }
        /// <summary>
        /// 根据条件获得状态
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public string GetOutputState(string trans)
        {
            if (trans == null)
            {
                Debug.Log(trans);
                return null;
            }
            if (map.ContainsKey(trans))
            {
                return map[trans];
            }

            return null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="player"></param>
        /// <param name="npc"></param>
        public abstract void update();

        /// <summary>
        /// 进入状态
        /// </summary>
        public abstract void EnterState();

        /// <summary>
        /// 离开状态
        /// </summary>
        public abstract void ExitState();

    }
}
