namespace LenovoMirageARSDK.OOBE
{
    using System.Collections.Generic;
    using System;
    using System.Reflection;

    public static class EntityExtention
    {
        public static IEntity StateMachine<B>(this object self) where B : IEntity
        {
            var ctors = typeof(B).GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            var ctor = Array.Find(ctors, c => c.GetParameters().Length == 1);

            if (ctor == null)
            {
                throw new Exception("Public Constructor() not found! in " + typeof(B));
            }

            object[] list = new object[1];

            if (self is State)
            {
                list[0] = ((State)self).GetEntity();
            }
            else
            {
                list[0] = self;
            }

            var retInstance = ctor.Invoke(list);

            if (m_AllIEntity == null)
            {
                m_AllIEntity = new List<LenovoMirageARSDK.OOBE.IEntity>();
            }
            m_AllIEntity.Add(retInstance as IEntity);

            return retInstance as IEntity;
        }

        private static List<IEntity> m_AllIEntity;

        public static void RemoveEntity(this IEntity self)
        {
            if (m_AllIEntity.Contains(self))
            {
                m_AllIEntity.Remove(self);
            }
        }

        public static void StateMachineEvent<T>(this object self, T eventid) where T : System.IConvertible
        {
            if (m_AllIEntity == null)
                return;

            m_AllIEntity.ForEach(x => { x.SetState(eventid); });
        }
    }
}