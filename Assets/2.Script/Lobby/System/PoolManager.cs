using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Lobby
{
    public class PoolManager : Singleton<PoolManager>
    {
        [Header("# Main Data")]
        public GameObject RoomInfoGroup;

        LobbyPool poolManager = new();

        public GameObject Get(GameObject prefab)
        {
            return poolManager.Get(prefab);
        }

        class LobbyPool : Game.System.PoolManager
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