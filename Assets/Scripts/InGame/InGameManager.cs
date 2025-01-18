using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    protected override void Init()
    {
        m_IsDestroyOnLoad = true;
        base.Init();
        UIManager.Instance.Fade(Color.black, 1f, 0f, 1f, 0f, true); // 페이드 아웃
    }
}

