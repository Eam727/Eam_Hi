using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Canvas1 : MonoBehaviour {

    void Awake()
    {
        EventTriggerListener.GetListener(UIManager.Instance.Find("Button")).onPointerClick +=
            (go) => { UIManager.Instance.Show("EquipPanel"); };
        EventTriggerListener.GetListener(UIManager.Instance.Find("Button (1)")).onPointerClick +=
            (go) => { UIManager.Instance.Show("BagPanel"); };
        EventTriggerListener.GetListener(UIManager.Instance.Find("Button (2)")).onPointerClick +=
            (go) => { GuideManager.Instance.Next(); };
        EventTriggerListener.GetListener(UIManager.Instance.Find("Button (3)")).onPointerClick +=
            (go) => { GuideManager.Instance.Next(); };

        GuideManager.Instance.Init(); 
    }

}
