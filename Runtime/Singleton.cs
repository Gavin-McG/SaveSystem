using System.Linq;
using UnityEngine;

namespace WolverineSoft.SaveSystem
{
    /// <summary>
    /// Base Component class for "Singleton" objects.
    /// Contains property for managing Instance references.
    /// IsPrimary field allows multiple instances where necessary
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour
        where T : Singleton<T>
    {
        [SerializeField] private bool IsPrimary = true;

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null || !_instance.IsPrimary)
                {
                    //get all instances currently in scene
                    var instances = FindObjectsByType<T>(FindObjectsSortMode.None)
                        .Where(x => x.IsPrimary)
                        .ToList();

                    switch (instances.Count)
                    {
                        case 0:
                            //No Instance currently exists - returning null
                            Debug.LogError("[Singleton] A Primary instance of " + typeof(T) +
                                           " is needed in the scene");
                            _instance = null;
                            break;
                        case 1:
                            //Only 1 instance found - ideal
                            _instance = instances[0];
                            break;
                        default:
                            //multiple instances found - default to first and show error
                            Debug.LogError("[Singleton] There should never be more than 1 Primary singleton of type " +
                                           typeof(T) + "!");
                            _instance = instances[0];
                            break;
                    }
                }

                return _instance;
            }
        }

        //automatically assign self to Instance on Awake
        protected virtual void Awake()
        {
            if (_instance == null) _instance = (T)this;
        }
    }
}
