using System;
using UnityEngine;
using WolverineSoft.SaveSystem;

namespace WolverineSoft.SaveSystem.Samples.BasicUseExample
{
    /// <summary>
    /// Example class for saving an object with a RigidBody
    /// </summary>
    public class RigidBodySaver : MonoBehaviour, ISaveData<RigidBodySaver.SaveData>
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private string identifier;

        private SaveData defaultSave;

        //identifies save Data by individual assigned identifier
        public string Identifier => identifier;
        
        public class SaveData
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 velocity;
            public Vector3 angularVelocity;
        }

        private void OnEnable()
        {
            //assign default data on first init
            if (defaultSave == null) defaultSave = GetSaveData();
        }

        public SaveData GetSaveData()
        {
            //return current data
            return new SaveData()
            {
                Position = transform.position,
                Rotation = transform.rotation,
                velocity = rb.linearVelocity,
                angularVelocity = rb.angularVelocity,
            };
        }

        public void RestoreToDefault()
        {
            //restore default data
            if (defaultSave != null)
                RestoreFromSaveData(defaultSave);
        }

        public void RestoreFromSaveData(SaveData data)
        {
            //restore from saved data
            transform.position = data.Position;
            transform.rotation = data.Rotation;
            rb.linearVelocity = data.velocity;
            rb.angularVelocity = data.angularVelocity;
        }
    }
}
