using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace  WolverineSoft.SaveSystem.Samples.BasicUseExample
{
    /// <summary>
    /// Example class for a Manager which manages a set of objects with the save system
    /// </summary>
    public class ObjectManagerSaver : MonoBehaviour, ISaveData<List<ObjectManagerSaver.InstanceData>>
    {
        [SerializeField] private GameObject prefab;
        private List<GameObject> objects = new List<GameObject>();

        //identifies save Data by "ObjectManager"
        public string Identifier => "ObjectManager";

        private void OnEnable()
        {
            StartCoroutine(SpawnObjectRoutine());
        }

        private void CreateNewObject(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            GameObject newObject = Instantiate(prefab, position, rotation, gameObject.transform);
            newObject.transform.localScale = scale;
            
            objects.Add(newObject);
        }

        IEnumerator SpawnObjectRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                
                //position
                Vector3 position = transform.position + Random.insideUnitSphere*20f;
                
                //rotation
                Vector3 upDir = Random.onUnitSphere;
                Vector3 forwardDir = Vector3.Cross(upDir, Random.onUnitSphere).normalized;
                Quaternion rotation = Quaternion.LookRotation(forwardDir, upDir);
                
                //scale
                Vector3 scale = new Vector3(1,1,1) * Random.Range(0.2f, 1.0f);
                
                CreateNewObject(position, rotation, scale);
            }
        }

        //data class for individual items
        public class InstanceData
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;
        }

        public List<InstanceData> GetSaveData()
        {
            //get list of all object data
            return objects.Select(x => new InstanceData()
            {
                position = x.transform.position,
                rotation = x.transform.rotation,
                scale = x.transform.localScale
            }).ToList();
        }

        public void RestoreToDefault()
        {
            //default is no objects created
            foreach (var item in objects)
            {
                Destroy(item);
            }
            objects.Clear();
        }

        public void RestoreFromSaveData(List<InstanceData> data)
        {
            //clear current objects
            RestoreToDefault();
            
            //restore saved objects
            foreach (var item in data)
            {
                CreateNewObject(item.position, item.rotation, item.scale);
            }
        }
    }
}


