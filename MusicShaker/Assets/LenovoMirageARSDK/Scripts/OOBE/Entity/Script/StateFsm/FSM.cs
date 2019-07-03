using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 执行状态的条件 
/// </summary> 

namespace LenovoMirageARSDK.OOBE
{
    public class FSM
    {
        private List<State>                             myList;
        public bool                                     Active;

        public FSM()
        {
            myList = new List<State>();
        }
        private string                                  oldFsmID;
        public State                                    CurrentState { get { return currentState; } }
        [SerializeField]
        private State                                   currentState;
        private State                                   m_Firstate;
        /// <summary>
        /// 加入状态
        /// </summary>
        /// <param name="State"></param>
        public void AddState(State State)
        {
            if (State == null)
            {
                Debug.Log("传递状态为空");
            }

            if (myList.Count == 0)
            {
                myList.Add(State);
                currentState = State;
                m_Firstate = currentState;
                return;
            }

            foreach (State state in myList)
            {
                if (state.ID == State.ID)
                {
                    return;
                }
            }
            myList.Add(State);
        }

        /// <summary>
        /// 查找状态
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        public bool FindState(string trans)
        {
            var id = currentState.GetOutputState(trans);

            if (id == null)
                return false;

            return true;
        }

        /// <summary>
        /// 删除状态
        /// </summary>
        /// <param name="State"></param>
        public void DeleteState(string state)
        {
            if (state == null)
            {
                return;
            }

            foreach (State item in myList)
            {
                if (item.ID == state)
                {
                    myList.Remove(item);
                    return;
                }
            }
        }

        /// <summary>
        /// 清除所有状态
        /// </summary>
        public void ClearList()
        {
            if (currentState != null)
            {
                currentState.ExitState();
            }
            myList.Clear();
        }

        public void EnforceState(State m_state)
        {
            currentState.ExitState();
            currentState = m_state;
            currentState.EnterState();
        }

        /// <summary>
        /// 执行状态
        /// </summary>
        /// <param name="trans"></param>
        public void SetTransition(string trans)
        {
            if (trans == null)
            {
                return;
            }

            var id = currentState.GetOutputState(trans);

            if (id == null)
            {
                //Debug.LogError("想切换的状态为" + trans.ToString());
                return;
            }

            currentState.ExitState();

            foreach (State state in myList)
            {
                if (state.ID == id)
                {
                    currentState = state;
                    currentState.EnterState();
                    return;
                }
            }

            Debug.Log("当前层级不存在该状态" + id);
        }

        public void Start()
        {
            if (currentState == null)
            {
                Debug.Log("当前状态为空");
                return;
            }
            Active = true;
            currentState = m_Firstate;
            currentState.EnterState();
        }

        public void Exit()
        {
            Active = false;
            currentState.ExitState();
        }
    }
}
