using System;
using UnityEngine;

namespace LenovoMirageARSDK.OOBE
{
    public interface IEntity
    {
        IEntity         OnValueChange(Action<string, string> func);
        IEntity         Begin(GameObject Obj = null, string Layer = "Normal");
        void            SetState(string id, string Layer = "Normal");
        void            SetState<T>(T id) where T : System.IConvertible;
        void            Destroy();
        Action          DestroyEvent(Action t);
        object          GetUser();
        string          GetCurrentState();
        void            Updata();
    }
}
