/*
 * 스크립트 이름: Singleton.cs
 * 설명: Singleton 패턴을 사용하고 싶을 때 상속받는 class
 *       DontDestroyOnload를 할 때는 MonoSingleton을 상속받고
 *       이 씬에서만 Singleton 패턴을 유지하고 싶다면 Singleton을 상속받는다.
 * 
 * 생성일: 2025-05-22
 * 생성자: 김상훈
 * 
 * 변경 내역:
 * - 2025-05-22, 김상훈, 초기 버전 작성.
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    /// <summary>
    /// Instance property를 통해 MonoSingleton에 접근
    /// </summary>
    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[MonoSingleton] Instance '" + typeof(T) +
                    "'이(가) 이미 파괴됐습니다. null을 반환합니다.");
                return null;
            }

            // 멀티스레드에서 동시 접근 시도시 하나씩 접근하게 잠금 설정
            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // 기존 Instance 검색
                    m_Instance = (T)FindObjectOfType(typeof(T));

                    // Instance가 존재하지 않는 경우 새로운 Instance를 생성
                    if (m_Instance == null)
                    {
                        // 객체 생성 후 Singleton 부착
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + "(MonoSingleton)";

                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return m_Instance;
            }
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }
    private void OnDestroy()
    {
        m_ShuttingDown = false;
    }
}


public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    /// <summary>
    /// Instance property를 통해 Singleton에 접근
    /// </summary>
    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "'이(가) 이미 파괴됐습니다. null을 반환합니다.");
                return null;
            }

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // 기존 Instance 검색
                    m_Instance = (T)FindObjectOfType(typeof(T));

                    // Instance가 존재하지 않는 경우 새로운 Instance를 생성
                    if (m_Instance == null)
                    {
                        // 객체 생성 후 Singleton 부착
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + "(Singleton)";
                    }
                }

                return m_Instance;
            }
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }
    private void OnDestroy()
    {
        m_ShuttingDown = false;
    }
}