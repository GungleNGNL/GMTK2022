using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;
    private static object m_Lock = new object();

    public static T Instance
    {
        get
        {
            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    m_Instance = FindObjectOfType(typeof(T)) as T;
                    if (m_Instance == null)
                    {
                        var obj = new GameObject();
                        m_Instance = obj.AddComponent<T>();

                        DontDestroyOnLoad(obj);
                    }
                }

                return m_Instance;
            }
        }
    }
}