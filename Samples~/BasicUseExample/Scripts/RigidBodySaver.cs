using UnityEngine;
using WolverineSoft.SaveSystem;

namespace WolverineSoft.SaveSystem.Samples.BasicUseExample
{

    public class SaveTester : MonoBehaviour, ISaveData<SaveTester.SaveData>
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private string identifier;

        public string Identifier => identifier;

        public class SaveData
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 velocity;
            public Vector3 angularVelocity;
        }

        public SaveData GetSaveData()
        {
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
        }

        public void RestoreFromSaveData(SaveData data)
        {
            transform.position = data.Position;
            transform.rotation = data.Rotation;
            rb.linearVelocity = data.velocity;
            rb.angularVelocity = data.angularVelocity;
        }
    }
}
