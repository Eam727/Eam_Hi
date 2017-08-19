//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

///// <summary>
///// AdaptScrollUI.
///// </summary>
//public class AdaptScrollUI : MonoBehaviour
//{
//    public int LineSpac = 0;                //�м��
//    public GameObject ModuleObj = null;     //�ж���
    
//    Vector3 m_StartPos = Vector3.zero;                  //�����ڻ������е���ʼ����
//    int m_iDefaultLine = 1;                             //��ʼ�е��к� Ĭ��Ϊ���� 
//    int m_iAfterLine = 0;                               //�ƶ��� �ƶ�����к�
//    int m_iBeforeLine = 0;                              //�ƶ��� �ƶ�ǰ���к�
//    int m_iScrollLineCount = 0;                         //�������е�������
//    int m_iShowLineCount = 0;                           //������ʾ��������

//    LuaInterface.LuaFunction m_CritFunc = null;         //�������ٽ��ص�
//    LuaInterface.LuaFunction m_EndFunc = null;          //������β���ص�

//    List<GameObject> m_LineObjs = new List<GameObject>();  //���滬������� ���ڳ�ʼ��

//    public LuaInterface.LuaFunction RegisterCritFunc { set { m_CritFunc = value; } }
//    public LuaInterface.LuaFunction RegisterEndFunc { set { m_EndFunc = value; } }

    
//    private System.Action<Object, int> m_CritCallBack = null;
//    public System.Action<Object, int> RegisterCritCallBack { set { m_CritCallBack = value; } }


//    /// <summary>
//    /// ��ȡ��������ʼλ��
//    /// </summary>
//    public Vector3 StartPos
//    {
//        get { return m_StartPos; }
//        set
//        {
//            m_StartPos = value;

//            float scrollheight = transform.gameObject.GetComponent<UIPanel>().height;
//            float centerheight = transform.gameObject.GetComponent<UIPanel>().finalClipRegion.y;
//            //�����
//            GameObject showend = new GameObject("showend");
//            showend.transform.parent = transform;
//            showend.transform.gameObject.AddComponent<UIWidget>();
//            showend.transform.gameObject.GetComponent<UIWidget>().height = 2;
//            showend.transform.gameObject.GetComponent<UIWidget>().pivot = UIWidget.Pivot.Bottom;
//            showend.transform.localScale = new Vector3(1, 1, 1);
//            showend.transform.localPosition = new Vector3(m_StartPos.x, -((scrollheight + centerheight) / 2 - 4), 1);

//            //�����
//            GameObject showstart = new GameObject("showstart");
//            showstart.transform.parent = transform;
//            showstart.transform.gameObject.AddComponent<UIWidget>();
//            showstart.transform.gameObject.GetComponent<UIWidget>().height = 2;
//            showstart.transform.gameObject.GetComponent<UIWidget>().pivot = UIWidget.Pivot.Top;
//            showstart.transform.localScale = new Vector3(1, 1, 1);
//            showstart.transform.localPosition = new Vector3(m_StartPos.x, ((scrollheight + centerheight) / 2 - 4), 1);
//        }
//    }

//    /// <summary>
//    /// ���û���������������ˢ��
//    /// </summary>
//    public int ScrollLineNum
//    {
//        set
//        {
//            if (m_iDefaultLine > value && 0 != value) return;

//            if (m_iDefaultLine + m_iShowLineCount > value)
//            {
//                ResetScroll(value - m_iShowLineCount + 1);
//            }

//            if (m_LineObjs.Count == 0)
//                InitScrollView(LineSpac, ModuleObj);

//            m_iScrollLineCount = value;
//            ExpScrollRange(m_iScrollLineCount);

//            for (int i = m_iDefaultLine; i < m_iDefaultLine + m_iShowLineCount; i++)
//            {
//                if (i > value)
//                {
//                    GameObject cellobj = transform.FindChild("row_item" + i).gameObject;
//                    cellobj.SetActive(false);
//                }
//            }

//            InitScrollLines(Mathf.Min(value, m_iShowLineCount));
//        }
//    }

//    void Awake()
//    {
//        if (m_LineObjs.Count == 0)
//        {
//            InitScrollView(LineSpac, ModuleObj);
//        }
//    }

//    void Update()
//    {
//        if (0 == m_iScrollLineCount) return;

//        if (0 == transform.localPosition.y || transform.localPosition.y == StartPos.y - (m_iScrollLineCount - 1) * LineSpac)
//        {
//            return;
//        }

//        int startNow = Mathf.FloorToInt(transform.localPosition.y / LineSpac) + 1;

//        //��������ʼ��β����
//        if (startNow < 0) return;
//        if (startNow == 0) startNow = 1;
//        if (startNow + m_iShowLineCount - 2 == m_iScrollLineCount) startNow = m_iScrollLineCount - m_iShowLineCount + 1;

//        if (startNow + m_iShowLineCount - 2 > m_iScrollLineCount)
//        {
//            //�ﵽ��β���ص�
//            if (m_EndFunc != null) m_EndFunc.Call();
//            else return;

//        }

//        if (startNow != m_iDefaultLine)
//        {
//            int dis = startNow - m_iDefaultLine;
//            if (dis >= m_iShowLineCount)
//            {
//                for (int i = m_iDefaultLine; i < m_iDefaultLine + m_iShowLineCount; i++)
//                {
//                    ChangePos(i, dis + i);
//                }
//            }
//            else
//            {
//                if (dis > 0)
//                {
//                    for (int i = 0; i < dis; i++)
//                    {
//                        ChangePos(m_iDefaultLine + i, m_iDefaultLine + i + m_iShowLineCount);
//                    }
//                }
//                else
//                {
//                    for (int i = dis; i < 0; i++)
//                    {
//                        if (m_iDefaultLine + i == 0)
//                        {
//                            return;
//                        }
//                        ChangePos(m_iDefaultLine + i + m_iShowLineCount, m_iDefaultLine + i);
//                    }
//                }
//            }

//            m_iDefaultLine = startNow;
//        }
//    }

//    /// <summary>
//    /// �����иߣ����㻬���������ʾ����
//    /// </summary>
//    /// <param name="spac">�ø�</param>
//    public void SetLineSpac(int spac)
//    {
//        LineSpac = spac;
//        float scrollheight = transform.gameObject.GetComponent<UIPanel>().height;

//        if (0 == scrollheight % LineSpac)
//        {
//            m_iShowLineCount = Mathf.FloorToInt(scrollheight / LineSpac) + 1;
//        }
//        else
//        {
//            m_iShowLineCount = Mathf.FloorToInt(scrollheight / LineSpac) + 2;
//        }   
//    }

//    /// <summary>
//    /// ��ʼ�������飬����
//    /// </summary>
//    /// <param name="LineHeight">�и�</param>
//    /// <param name="Template">��ģ��</param>
//    public void InitScrollView(int LineHeight, GameObject Template)
//    {
//        //����������
//        if (LineHeight != 0 && Template != null)
//        {
//            LineSpac = LineHeight;
//            ModuleObj = Template;

//            ModuleObj.SetActive(false);
//            StartPos = ModuleObj.transform.localPosition;

//            float scrollheight = transform.gameObject.GetComponent<UIPanel>().height;
//            m_iShowLineCount = Mathf.FloorToInt(scrollheight / LineSpac) + 2;

//            int topline = 1;
//            if (ModuleObj.transform.parent = transform)
//            {
//                ModuleObj.transform.name = "row_item1";
//                topline = 2;
//                ModuleObj.transform.localPosition = StartPos;
//                ModuleObj.transform.localScale = new Vector3(1, 1, 1);
//                ModuleObj.SetActive(true);

//                m_LineObjs.Add(ModuleObj);
//            }
//            for (int i = topline; i <= m_iShowLineCount; i++)
//            {
//                GameObject cell = GameObject.Instantiate(ModuleObj) as GameObject;
//                cell.transform.parent = transform;
//                cell.transform.name = "row_item" + i;
//                Vector3 pos = StartPos;
//                pos.y = StartPos.y - (i - 1) * LineSpac;
//                cell.transform.localPosition = pos;
//                cell.transform.localScale = new Vector3(1, 1, 1);
//                cell.SetActive(true);

//                m_LineObjs.Add(cell);
//            }
//        }

//    }

//    /// <summary>
//    /// ˢ�µ�ǰ�Ļ�����
//    /// </summary>
//	public void RefreshScrollLines()
//	{
//		int LineNum = Mathf.Min(m_iShowLineCount, m_iScrollLineCount);
//		for (int i = 0; i < LineNum; i++)
//		{
//			InitLine(i + m_iDefaultLine);
//		}
//	}

//    /// <summary>
//    /// ����ĳ�� ��ˢ��
//    /// </summary>
//    /// <param name="deleteLine">���ص��к�</param>
//    public void DeleteScroll(int deleteLine)
//    {
//        m_iScrollLineCount = m_iScrollLineCount - 1;
//        ExpScrollRange(m_iScrollLineCount);

//        Transform trans = transform.FindChild("row_item" + (m_iScrollLineCount + 1));
//        if (trans != null)
//        {
//            trans.gameObject.SetActive(false);
//        }

//        int endline = m_iScrollLineCount;
//        if (endline > m_iDefaultLine + m_iShowLineCount - 1)
//        {
//            endline = m_iDefaultLine + m_iShowLineCount - 1;
//        }

//        for (int i = m_iDefaultLine; i <= endline; i++)
//        {
//            if (i >= deleteLine)
//            {
//                InitLine(i);
//            }
//        }
//    }

//    /// <summary>
//    /// ���û����飬Ĭ�����п�ʼ
//    /// </summary>
//    /// <param name="firstline">Ĭ����ʾλ��</param>
//    public void ResetScroll(int firstline = 1)
//    {
//        if (firstline < 1) firstline = 1;

//        m_iScrollLineCount = 0;
//        m_iDefaultLine = firstline;

//        for (int i = 0; i < m_LineObjs.Count; i++)
//        {
//            GameObject obj = m_LineObjs[i];
//            obj.transform.name = "row_item" + (firstline + i);
//            Vector3 pos = StartPos;
//            pos.y = StartPos.y - (firstline + i - 1) * LineSpac;
//            obj.transform.localPosition = pos;
//            obj.SetActive(true);
//        }

//        transform.GetComponent<UIScrollView>().ResetPosition();
//        transform.localPosition = new Vector3(0, (firstline - 1) * LineSpac, 0); //Vector3.zero;
//        transform.gameObject.GetComponent<UIPanel>().clipOffset = new Vector2(0, (firstline - 1) * LineSpac); //Vector2.zero;
//    }

//    /// <summary>
//    /// ���û���������
//    /// </summary>
//    /// <param name="limitNum">�����������</param>
//    void ExpScrollRange(int limitNum)
//    {

//        //Util.Log("limit = " + limitNum + "    spac = " + LineSpac);

//        //�������λ��
//        if (limitNum == 0) limitNum = 1;

//        Transform starttra = transform.FindChild("showstart");
//        if (starttra == null)
//        {
//            GameObject startobj = new GameObject("showstart");
//            starttra = startobj.transform;
//            starttra.parent = transform;
//            starttra.gameObject.AddComponent<UIWidget>();
//            starttra.gameObject.GetComponent<UIWidget>().height = 2;
//            starttra.gameObject.GetComponent<UIWidget>().pivot = UIWidget.Pivot.Top;
//            starttra.localScale = new Vector3(1, 1, 1);
//            starttra.localPosition = new Vector3(StartPos.x, StartPos.y, 1);
//        }

//        Transform endtra = transform.FindChild("scrollend");
//        if (endtra == null)
//        {
//            GameObject endobj = new GameObject("scrollend");
//            endtra = endobj.transform;
//            endtra.transform.parent = transform;
//            //        endtra.gameObject.AddComponent<UISprite>();
//            //       endtra.gameObject.GetComponent<UISprite>().height = spriteHeight;
//            endtra.gameObject.AddComponent<UIWidget>();
//            endtra.gameObject.GetComponent<UIWidget>().height = 2;
//            endtra.gameObject.GetComponent<UIWidget>().pivot = UIWidget.Pivot.Bottom;
//            endtra.localScale = new Vector3(1, 1, 1);
//        }
//        endtra.localPosition = new Vector3(StartPos.x, StartPos.y - (limitNum - 1) * LineSpac, 1);
//    }

//    /// <summary>
//    /// ˢ�»�����
//    /// </summary>
//    /// <param name="lineNum">ˢ������</param>
//    void InitScrollLines(int lineNum)
//    {
//        for (int i = m_iDefaultLine; i < m_iDefaultLine + lineNum; i++)
//        {
//            InitLine(i);
//        }
//    }

//    /// <summary>
//    /// �������ٽ�㣬ˢ���ٽ��� update��ʹ��
//    /// </summary>
//    /// <param name="olds">�ٽ��� ֮ǰλ����</param>
//    /// <param name="news">�ٽ��� ֮��λ����</param>
//    void ChangePos(int olds, int news)
//    {
    
//        //Util.Log("old = " + olds + "    news = " + news);

//        Transform trans = transform.FindChild("row_item" + olds);
//        if (trans != null)
//        {
//            GameObject module = trans.gameObject;
//            Vector3 objPos = module.transform.localPosition;
//            objPos.y = StartPos.y - (news - 1) * LineSpac;
//            module.transform.name = "row_item" + news;
//            module.transform.localPosition = objPos;

//            m_iBeforeLine = olds;
//            m_iAfterLine = news;

//            if (m_CritFunc != null)
//            {
//                //CritFunc.Call(module, lateActivePos);
//#if !DES_DEVELOP
//                Util.CallMethod(m_CritFunc, module, m_iAfterLine);
//#endif
//            }



//            if (m_CritCallBack != null)
//            {
//                m_CritCallBack.Invoke(module, m_iAfterLine);
//                //m_CritCallBack(module, m_iAfterLine);
//            }
//        }
//    }
	
//    /// <summary>
//    /// ˢ�µ���
//    /// </summary>
//    /// <param name="lineNum">�к�</param>
//	void InitLine(int lineNum)
//	{
//		Transform trans = transform.FindChild("row_item" + lineNum);
//		if (trans != null)
//		{
//			GameObject module = trans.gameObject;
//			trans.gameObject.SetActive(true);
//            if (m_CritFunc != null)
//            {
//                //CritFunc.Call(module, lineNum);
//#if !DES_DEVELOP
//                Util.CallMethod(m_CritFunc, module, lineNum);
//#endif
//            }
//            if (m_CritCallBack != null)
//            {
//                m_CritCallBack.Invoke(module, lineNum);
//                //m_CritCallBack(module, lineNum);
//            }
//		}
//	}
//}
