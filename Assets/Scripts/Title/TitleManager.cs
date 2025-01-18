using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
public class TitleManager : Singleton<TitleManager>
{
    [SerializeField]
    private TextMeshProUGUI titleText;
    private float timer;
    private bool touchToStart = false;
    protected override void Init()
    {
        m_IsDestroyOnLoad = true; // Ÿ��Ʋ �Ŵ��� �� ��ȯ�� ����
        base.Init();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 2)
        {
            titleText.text = "Touch To Start";
            touchToStart = true;
        }
        if (touchToStart)
        {
            if(Input.GetMouseButtonDown(0))
            {
                GameStart();
            }
        }

    }

    protected void GameStart()
    {
        // ���̵� ��
        UIManager.Instance.Fade(Color.black, 0f, 1f, 1f, 0f, false, () =>
        {
            //UIManager.Instance.CloseAllOpenUI();
            Utils.LoadScene(SceneNames.InGame);
        });
    }
}
