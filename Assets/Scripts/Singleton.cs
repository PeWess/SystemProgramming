using UnityEngine;
using Object = UnityEngine.Object;

namespace SolarSystemKritskiy_Main
{
    public class Singleton<T> : MonoBehaviour where T: Object
    {
        public static T Instance;

        protected virtual void Awake()
        {
            Instance = this as T;
        }
    }
}
