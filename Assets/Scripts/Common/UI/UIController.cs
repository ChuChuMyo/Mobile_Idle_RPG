using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    
    public void OnClickInventoryBtn()
    {
        //�ϴ� �����ִ� UI�� ����
        UIManager.Instance.CloseAllOpenUI();
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<InventoryUI>(uiData);
    }

    public void OnClickSkillBtn()
    {
        UIManager.Instance.CloseAllOpenUI();
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<SkillUI>(uiData);
    }

    public void OnClickStatusBtn()
    {
        UIManager.Instance.CloseAllOpenUI();
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<StatusUI>(uiData);
    }
}
