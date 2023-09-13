using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistantGameobjectPrefab;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;
            SpawnPersistantsObject();

            hasSpawned = true;
        }
        private void SpawnPersistantsObject()
        {
            GameObject persistantObject = Instantiate(persistantGameobjectPrefab);
            DontDestroyOnLoad(persistantObject);
        }
    }
}

