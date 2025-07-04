using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    public abstract class PoolManager
    {
        protected Dictionary<string, List<GameObject>> m_poolDict = new Dictionary<string, List<GameObject>>();

        /// <summary>
        /// Pool에 존재하는 prefab 반환(없으면 생성하여 반환)
        /// </summary>
        public GameObject Get(GameObject prefab)
        {
            GameObject obj = null;
            string key = prefab.name;

            // _poolDict에 key가 존재하지않으면 객체 생성 후 등록
            if (!m_poolDict.ContainsKey(key))
            {
                m_poolDict[key] = new List<GameObject>();
                obj = CreateObject(key, prefab);
                // Debug.Log($"[PoolManager]: pool 미존재로 생성");
            }

            // _poolDict에 비활성화된 객체 검사하여 등록
            foreach (var pooledObj in m_poolDict[key])
            {
                if (!pooledObj.activeInHierarchy)
                {
                    obj = pooledObj;
                    // Debug.Log($"[PoolManager]: 발견된 obj 반환");
                    break;
                }
            }

            // 비활성화된 객체가 존재하지않으면 객체 생성 후 등록
            if (obj == null)
            {
                obj = CreateObject(key, prefab);
                // Debug.Log($"[PoolManager]: obj 생성 후 반환");
            }

            // obj.transform.parent = transform;
            // obj.transform.position = transform.position;
            obj.SetActive(true);
            return obj;
        }

        /// <summary>
        /// 새로운 객체 생성 후 _poolDict에 등록
        /// </summary>
        protected abstract GameObject CreateObject(string key, GameObject prefab);
    }
}
