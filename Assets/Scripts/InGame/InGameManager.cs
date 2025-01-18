using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    protected override void Init()
    {
        m_IsDestroyOnLoad = true;
        base.Init();
    }
}

