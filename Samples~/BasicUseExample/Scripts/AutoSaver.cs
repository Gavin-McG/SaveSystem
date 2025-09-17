using System;
using System.Collections;
using UnityEngine;

namespace WolverineSoft.SaveSystem.Samples.BasicUseExample
{
    public class AutoSaver : MonoBehaviour
    {
        [SerializeField] private bool autoSave = true;
        [SerializeField, Min(0.2f)] private float saveDelay = 5f;

        private Coroutine autoSaveCoRoutine;
        
        private void OnEnable()
        {
            autoSaveCoRoutine = StartCoroutine(AutoSaveRoutine());
        }

        private void OnValidate()
        {
            if (autoSave && autoSaveCoRoutine == null)
            {
                autoSaveCoRoutine = StartCoroutine(AutoSaveRoutine());
            }
            else if (!autoSave && autoSaveCoRoutine != null)
            {
                StopCoroutine(autoSaveCoRoutine);
                autoSaveCoRoutine = null;
            }
        }

        IEnumerator AutoSaveRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(saveDelay);

                SaveManager.Instance.SaveData();
            }
        }
    }
}

