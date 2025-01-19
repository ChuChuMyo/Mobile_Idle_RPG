using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    
    private void OnClickInventoryBtn()
    {
        //�ϴ� �����ִ� UI�� ����
        UIManager.Instance.CloseAllOpenUI();
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<InventoryUI>(uiData);
    }
}
