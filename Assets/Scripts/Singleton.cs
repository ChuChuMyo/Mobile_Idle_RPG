using UnityEngine;

//Singleton Design Pattern
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    //씬 전환 시 삭제할지 여부
    protected bool m_IsDestroyOnLoad = false;
    //이 클래스의 스태틱 인스턴스 변수
    protected static T m_Instance;

    public static T Instance
    { get { return m_Instance; } }

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (m_Instance == null)
        {
            m_Instance = (T)this;

            if (!m_IsDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //삭제 시 실행 되는 함수
    protected virtual void OnDestroy()
    {
        Dispose();
    }

    //삭제 시 추가로 처리해 주어야할 작업을 함수로 만들어 처리
    protected virtual void Dispose()
    {
        m_Instance = null;
    }
}