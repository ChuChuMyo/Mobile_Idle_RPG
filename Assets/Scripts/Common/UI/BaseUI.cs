using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BaseUIData
{
    //UI화면을 열었을 때 해주고 싶은 행위를 정의
    public Action OnShow;
    //UI화면을 닫으면서 실행해야 되는 기능 정의
    public Action OnClose;
}
public class BaseUI : MonoBehaviour
{
    public Action m_OnShow;
    public Action m_OnClose;

    public virtual void Init(Transform anchor)
    {
        m_OnShow = null;
        m_OnClose = null;
        transform.SetParent(anchor);
        
        var rectTransform = GetComponent<RectTransform>();
        if (!rectTransform)
        {
            return;
        }

        rectTransform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 1100*-1);
        
    }


    public virtual void SetInfo(BaseUIData uiData)
    {
        m_OnShow = uiData.OnShow;
        m_OnClose = uiData.OnClose;
    }

    //UI 화면을 실제로 열어서 화면에 표시해 주는 함수
    public virtual void ShowUI()
    {
        m_OnShow?.Invoke();//널검사랑 동일함
        //? 키워드를 사용한 예외 처리
        //_action?.Invoke(3); //if(action != null)를 ?키워드를 사용하여 null임을 체크

        m_OnShow = null;//실행 후에는 널로 초기화
    }

    //UI화면을 닫는 함수
    public virtual void CloseUI(bool isCloseAll = false)
    {
        if (!isCloseAll)
        {
            m_OnClose?.Invoke();
        }
        m_OnClose = null;

        UIManager.Instance.CloseUI(this);
    }

    public virtual void OnClickCloseButton()
    {
        CloseUI();
    }

}
