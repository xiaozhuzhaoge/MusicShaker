namespace DeviceManager
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;

    /// <summary>
    /// A class used for monitoring the status of an asynchronous task.
    /// </summary>
    /// <typeparam name="T">The resultant type of the task.</typeparam>
    public class AsyncTask<T>
    {
        /// <summary>
        /// A collection of actons to perform on the main Unity thread after the task is complete.
        /// </summary>
        private List<Action<T>> m_ActionsUponTaskCompletion;

        /// @cond EXCLUDE_FROM_DOXYGEN
        /// <summary>
        /// Constructor for AsyncTask.
        /// </summary>
        /// <param name="asyncOperationComplete">A callback that, when invoked, changes the status of the task to
        /// complete and sets the result based on the argument supplied.</param>
        public AsyncTask(out Action<T> asyncOperationComplete)
        {
            IsComplete = false;
            asyncOperationComplete = delegate(T result)
            {
                this.Result = result;
                IsComplete = true;
                if (m_ActionsUponTaskCompletion != null)
                {
                    AsyncTask.PerformActionInUpdate(() =>
                    {
                        for (int i = 0; i < m_ActionsUponTaskCompletion.Count; i++)
                        {
                            m_ActionsUponTaskCompletion[i](result);
                        }
                    });
                }
            };
        }

        /// @endcond

        /// @cond EXCLUDE_FROM_DOXYGEN
        /// <summary>
        /// Constructor for AsyncTask that creates a completed task.
        /// </summary>
        /// <param name="result">The result of the completed task.</param>
        public AsyncTask(T result)
        {
            Result = result;
            IsComplete = true;
        }

        /// @endcond

        /// <summary>
        /// Gets a value indicating whether the task is complete.
        /// </summary>
        /// <value><c>true</c> if the task is complete, otherwise <c>false</c>.</value>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// Gets the result of a completed task.
        /// </summary>
        /// <value>The result of the completed task.</value>
        public T Result { get; private set; }

        /// <summary>
        /// Returns a yield instruction that monitors this task for completion within a coroutine.
        /// </summary>
        /// <returns>A yield instruction that monitors this task for completion.</returns>
        public CustomYieldInstruction WaitForCompletion()
        {
            return new WaitForTaskCompletionYieldInstruction<T>(this);
        }

        /// <summary>
        /// Performs an action (callback) in the first Unity Update() call after task completion.
        /// </summary>
        /// <param name="doAfterTaskComplete">The action to invoke when task is complete.  The result of the task will
        /// be passed as an argument to the action.</param>
        /// <returns>The invoking asynchronous task.</returns>
        public AsyncTask<T> ThenAction(Action<T> doAfterTaskComplete)
        {
            // Perform action now if task is already complete.
            if (IsComplete)
            {
                doAfterTaskComplete(Result);
                return this;
            }

            // Allocate list if needed (avoids allocation if then is not used).
            if (m_ActionsUponTaskCompletion == null)
            {
                m_ActionsUponTaskCompletion = new List<Action<T>>();
            }

            m_ActionsUponTaskCompletion.Add(doAfterTaskComplete);
            return this;
        }
    }

    /// @cond EXCLUDE_FROM_DOXYGEN
    /// <summary>
    /// Helper methods for dealing with asynchronous tasks.
    /// </summary>
    public class AsyncTask
    {
        private static Queue<Action> s_UpdateActionQueue = new Queue<Action>();
        private static object s_LockObject = new object();

        /// <summary>
        /// Queues an action to be performed on Unity thread in Update().  This method can be called by any thread.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        public static void PerformActionInUpdate(Action action)
        {
            lock (s_LockObject)
            {
                s_UpdateActionQueue.Enqueue(action);
            }
        }

        /// <summary>
        /// An Update handler called each frame.
        /// </summary>
        public static void OnUpdate()
        {
            lock (s_LockObject)
            {
                while (s_UpdateActionQueue.Count > 0)
                {
                    Action action = s_UpdateActionQueue.Dequeue();
                    action();
                }
            }
        }
    }

    /// @endcond
}
