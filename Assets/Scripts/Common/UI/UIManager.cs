using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Buffers.Text;

public class UIManager : Singleton<UIManager>
{
    public Transform UICanvasTrs;
    public Transform m_ClosedUITrs;
    //public Transform ClosedUITrs;
    public BaseUI m_FrontUI;
    //페이드 이미지 컴포넌트
    public Image m_Fade;
    //현재 열려있는 UI 화면을 담고 있는 변수
    Dictionary<System.Type, GameObject> m_OpenUIPool = new Dictionary<System.Type, GameObject>();
    //닫혀있는 UI 화면을 담고 있는 변수
    Dictionary<System.Type, GameObject> m_ClosedUIPool = new Dictionary<System.Type, GameObject>();

    protected override void Init()
    {
        base.Init();
    }

    //열기를 원하는 UI 화면의 실제 인스턴스를 가져오는 함수
    //out 함수에는 한가지 값이나 참조만 반환할 수 있기 때문에
    //여러가지 값이나 참조를 반환하고 싶을 때 이렇게 out 매게변수를 사용
    //이 함수는 BaseUI, isAlreadyOpen 두가지 값을 반환
    //매니저에 있는 UI를 가져오고 가져올게 없으면 생성까지 시켜주는 ui를 반환해주는 함수
    BaseUI GetUI<T>(out bool isAlreadyOpen)
    {
        //여기서 T는 이제 열고자 하는 화면 UI 클래스 타입. 이것을 uiType으로 받아온다.
        System.Type uiType = typeof(T);

        BaseUI ui = null;
        isAlreadyOpen = false;

        if (m_OpenUIPool.ContainsKey(uiType))
        {
            //풀에 있는 해당 오브젝트의 BaseUI 컴포넌트를 ui변수에 대입
            ui = m_OpenUIPool[uiType].GetComponent<BaseUI>();
            isAlreadyOpen = true;
        }
        //그렇지 않고 m_ClosedUIPool에 존재한다면
        else if (m_ClosedUIPool.ContainsKey(uiType))
        {
            //해당 풀에 있는 BaseUI 컴포넌트를 ui변수에 대입
            ui = m_ClosedUIPool[uiType].GetComponent<BaseUI>();
            m_ClosedUIPool.Remove(uiType);
        }
        //둘다 아니고 아예 한번도 생성된 적이 없는 인스턴스라면
        else
        {
            //생성을 시켜줌
            var uiObj = Instantiate(Resources.Load($"UI/{uiType}", typeof(GameObject))) as GameObject;
            //프리팹의 이름이 반드시 UI 클래스의 이름과 동일해야함
            //왜냐하면 UI클래스의 이름으로 경로를 만들어서 리소스 폴더에서 로드해오라고 요청한거기 때문에
            ui = uiObj.GetComponent<BaseUI>();
        }
        return ui;
    }

    //특정 UI화면이 열려있는지 확인하고 그 열려있는 UI화면을 가져오는 함수
    public BaseUI GetActiveUI<T>()
    {
        var uiType = typeof(T);
        //m_OpenUIPool에 특정 화면 인스턴스가 존재한다면 그 화면 인스턴스를 리턴해 주고 그렇지 않으면 널 리턴
        return m_OpenUIPool.ContainsKey(uiType) ? m_OpenUIPool[uiType].GetComponent<BaseUI>() : null;
    }

    //이제 UI를 가져왔거나 생성시켜준 UI를 오픈시켜주는 함수
    public void OpenUI<T>(BaseUIData uiData)
    {
        System.Type uiType = typeof(T);
        //이미 열려있는지 알 수 있는 변수
        bool isAlreadyOpen = false;
        var ui = GetUI<T>(out isAlreadyOpen);
        //근데 이제 ui가 없으면 에러로그 띄워주고 종료 시킴
        if (!ui)
        {
            return;
        }

        //진행중에 이미 열려있다면 이것 또한 비정상적인 요청이라고 판단
        if (isAlreadyOpen)
        {
            return;
        }

        //위의 유효성 검사를 통과해서 정상적으로 UI화면이 열릴 수 있다면
        //이제 실제로 UI화면을 열고 데이터를 세팅해 준다.

        //childCount 하위에 있는 게임 오브젝트 갯수
        var siblingIdx = UICanvasTrs.childCount - 2;

        //UI화면 초기화
        ui.Init(UICanvasTrs);

        //하이라키 순위 변경 SetsiblingIndex : 매개변수를 넣어서 순위를 지정
        //siblingIdx는 0부터 시작하는데 0, 1, 2, 3 ...
        //이렇게 정수값 1단위로 늘어난다.
        //생성하고자 하는 UI화면을 이미 생성되어있는 UI화면들 보다
        //상단에 위치시켜 줘야하기 때문에
        //현재 존재하는 UICanvasTrs 하위 오브젝트들의 개수를 받아와서
        //siblingIdx 값으로 넘겨주는 것임.
        //siblingIdx가 0부터 시작하기 때문에 childCount가 새로운 UI화면을 추가할 시
        //가장 큰 sublingIdx값이 되기 때문
        //예를 들어 자식이 2개 -> 0, 다음에 생성되면 3개가 되는데 그게 1이되고 그다음에 샹성되면 2가 되는 식으로
        //매번 캔바스 상에서 가장 아래에 위치하게 되고 화면에서는 가장 최상단에 노출
        ui.transform.SetSiblingIndex(siblingIdx);

        //컴포넌트가 연동된 게임오브젝트 활성화
        ui.gameObject.SetActive(true);

        //UI 화면에 보이는 UI요소의 데이터를 세팅해줌
        ui.SetInfo(uiData);
        ui.ShowUI();

        //현재 열고자하는 화면 UI가 가장 상단에 있는 UI가 될것이기 때문에 이렇게 설정
        m_FrontUI = ui;

        //m_OpenUIPool에 생성한 UI인스턴스를 넣어준다.
        m_OpenUIPool[uiType] = ui.gameObject;
    }

    //화면을 닫는 함수
    public void CloseUI(BaseUI ui)
    {
        System.Type uiType = ui.GetType();

        ui.gameObject.SetActive(false);

        //오브젝트 풀에서 제거
        m_OpenUIPool.Remove(uiType);

        //클로즈 풀에서 추가
        m_ClosedUIPool[uiType] = ui.gameObject;

        //ClosedUITrs하위로 위치
        ui.transform.SetParent(m_ClosedUITrs);

        //최상단 UI널로 초기화
        m_FrontUI = null;

        //현재 최상단에 있는 UI화면 오브젝트를 가져온다.
        var lastChild = UICanvasTrs.GetChild(UICanvasTrs.childCount - 1);

        //만약 UI가 존재한다면 이 UI화면 인스턴스를 최상단 UI로 대입
        if (lastChild)
        {
            m_FrontUI = lastChild.gameObject.GetComponent<BaseUI>();
        }
    }


    //UI화면이 열린것이 하나라도 있는지 확인하는 함수
    public bool ExistsOpenUI()
    {
        //m_FrontUI가 null인지 아닌지 확인해서 그 불값을 반환
        return m_FrontUI != null;
    }

    public BaseUI GetCurrentFrontUI()
    {
        return m_FrontUI;
    }

    //가장 최상단에 있는 UI화면 인스턴스를 닫는 함수
    public void CloseCurrentFrontUI()
    {
        m_FrontUI.CloseUI();
    }

    //결론적으로 열려있는 모든 UI화면을 닫으라는 함수
    public void CloseAllOpenUI()
    {
        while (m_FrontUI)
        {
            m_FrontUI.CloseUI(true);
        }
    }

    //페이드 함수
    public void Fade(Color color, float startAlpha, float endAlpha, float duration, float startDelay, bool deactiveOnFisish, Action onFinish = null)
    {
        StartCoroutine(FadeCo(color, startAlpha, endAlpha, duration, startDelay, deactiveOnFisish, onFinish));
    }

    //페이드 처리 코루틴
    private IEnumerator FadeCo(Color color, float startAlpha, float endAlpha, float duration, float startDelay, bool deactiveOnFisish, Action onFinish = null)
    {
        yield return new WaitForSeconds(startDelay);

        m_Fade.transform.localScale = Vector3.one;
        m_Fade.color = new Color(color.r, color.g, color.b, startAlpha);

        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < duration)
        {
            m_Fade.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startAlpha, endAlpha, (Time.realtimeSinceStartup - startTime) / duration));
            yield return null;
        }

        m_Fade.color = new Color(color.r, color.g, color.b, endAlpha);

        if (deactiveOnFisish)
        {
            m_Fade.transform.localScale = Vector3.zero;
        }

        onFinish?.Invoke(); //페이드 처리가 끝났을 때 수행되길 원하는 로직이 있다면 처리
    }

    public void CancelFade()
    {
        m_Fade.transform.localScale = Vector3.zero;
    }
}
