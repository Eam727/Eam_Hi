using UnityEngine;
using System.Collections;

public class Panel1 : MonoBehaviour {

	void Start () 
    {
        EventTriggerListener.GetListener(transform.Find("Button").gameObject).onPointerClick +=
            (go) => { gameObject.SetActive(false); };
        EventTriggerListener.GetListener(transform.Find("Button (1)").gameObject).onPointerClick +=
            (go) => { Debug.Log("装上"); };
        EventTriggerListener.GetListener(transform.Find("Button (2)").gameObject).onPointerClick +=
            (go) => { Debug.Log("卸下"); };
        EventTriggerListener.GetListener(transform.Find("Image/Button").gameObject).onPointerClick +=
            (go) => { Debug.Log("查看属性"); };
	}
	
}
