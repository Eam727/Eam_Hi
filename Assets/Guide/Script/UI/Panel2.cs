using UnityEngine;
using System.Collections;

public class Panel2 : MonoBehaviour {

	void Start () 
    {
        EventTriggerListener.GetListener(transform.Find("Button").gameObject).onPointerClick +=
            (go) => { gameObject.SetActive(false); };
        EventTriggerListener.GetListener(transform.Find("Button (1)").gameObject).onPointerClick +=
            (go) => { Debug.Log("整理"); };
	}

}
