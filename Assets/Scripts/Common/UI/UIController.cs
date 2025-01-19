using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    
    private void OnClickInventoryBtn()
    {
        //일단 열려있는 UI를 닫음
        UIManager.Instance.CloseAllOpenUI();
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<InventoryUI>(uiData);
    }
}
