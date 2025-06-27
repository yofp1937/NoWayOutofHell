using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.InGame
{
    public class PoolManager : Singleton<PoolManager>
    {
        [Header("# Main Data")]
        public GameObject DebugObj;
        public Transform DebugT;
        public GameObject Zombie;

        InGamePool poolManager = new();

        public GameObject Get(GameObject prefab)
        {
            return poolManager.Get(prefab);
        }

        class InGamePool : Game.System.PoolManager
        {
            protected override GameObject CreateObject(string key, GameObject prefab)
            {
                GameObject obj = Instantiate(prefab);
                obj.name = key;
                m_poolDict[key].Add(obj);
                return obj;
            }
        }
    }
}