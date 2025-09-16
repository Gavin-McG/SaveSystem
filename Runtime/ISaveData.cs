using UnityEngine;

namespace WolverineSoft.SaveSystem
{

    /// <summary>
    /// Base class for the <see cref="ISaveData{T}"/> generic.
    /// </summary>
    /// <remarks>DO NOT USE FOR YOUR OWN CLASSES</remarks>
    public interface ISaveData
    {
        /// <summary>
        /// Unique identifier for identifying what data to restore
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Id for what manager to regiger itself with. Default to ""
        /// </summary>
        public string ManagerId => "";

        /// <summary>
        /// Returns all the necessary save data for an Object as a System.Object
        /// </summary>
        public object GetDataObject();

        /// <summary>
        /// Method used to restore the state of the Object from data as an Object value
        /// </summary>
        public void RestoreFromObjectData(object data);

        /// <summary>
        /// Called when so save data is found for the given object.
        /// </summary>
        public void RestoreToDefault();
    }


    /// <summary>
    /// Interface used for Objects which should save data with the save system. 
    /// Any object which uses this interface should *Never be Disabled*
    /// </summary>
    /// <remarks>
    /// If you have objects which should be disabled and require some state, create some form of manager script
    /// </remarks>
    public interface ISaveData<T> : ISaveData
    {
        object ISaveData.GetDataObject()
        {
            return GetSaveData();
        }

        void ISaveData.RestoreFromObjectData(object data)
        {
            if (data is T tData)
            {
                RestoreFromSaveData(tData);
            }
            else
            {
                RestoreToDefault();
            }
        }

        /// <summary>
        /// Returns all the necessary save data for an Object.
        /// </summary>
        public T GetSaveData();

        /// <summary>
        /// Method used to restore the state of the Object from data.
        /// </summary>
        public void RestoreFromSaveData(T data);
    }
}
