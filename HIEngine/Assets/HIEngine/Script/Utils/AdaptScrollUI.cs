//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

///// <summary>
///// AdaptScrollUI.
///// </summary>
//public class AdaptScrollUI : MonoBehaviour
//{
//    public int LineSpac = 0;                //行间距
//    public GameObject ModuleObj = null;     //行对象
    
//    Vector3 m_StartPos = Vector3.zero;                  //首行在滑动块中的起始坐标
//    int m_iDefaultLine = 1;                             //起始行的行号 默认为首行 
//    int m_iAfterLine = 0;                               //移动行 移动后的行号
//    int m_iBeforeLine = 0;                              //移动行 移动前的行号
//    int m_iScrollLineCount = 0;                         //滑动块中的总行数
//    int m_iShowLineCount = 0;                           //可以显示的总行数

//    LuaInterface.LuaFunction m_CritFunc = null;         //滑动到临界点回调
//    LuaInterface.LuaFunction m_EndFunc = null;          //滑动到尾部回调

//    List<GameObject> m_LineObjs = new List<GameObject>();  //保存滑动块对象 用于初始化

//    public LuaInterface.LuaFunction RegisterCritFunc { set { m_CritFunc = value; } }
//    public LuaInterface.LuaFunction RegisterEndFunc { set { m_EndFunc = value; } }

    
//    private System.Action<Object, int> m_CritCallBack = null;
//    public System.Action<Object, int> RegisterCritCallBack { set { m_CritCallBack = value; } }


//    /// <summary>
//    /// 获取滑动块起始位置
//    /// </summary>
//    public Vector3 StartPos
//    {
//        get { return m_StartPos; }
//        set
//        {
//            m_StartPos = value;

//            float scrollheight = transform.gameObject.GetComponent<UIPanel>().height;
//            float centerheight = transform.gameObject.GetComponent<UIPanel>().finalClipRegion.y;
//            //上填充
//            GameObject showend = new GameObject("showend");
//            showend.transform.parent = transform;
//            showend.transform.gameObject.AddComponent<UIWidget>();
//            showend.transform.gameObject.GetComponent<UIWidget>().height = 2;
//            showend.transform.gameObject.GetComponent<UIWidget>().pivot = UIWidget.Pivot.Bottom;
//            showend.transform.localScale = new Vector3(1, 1, 1);
//            showend.transform.localPosition = new Vector3(m_StartPos.x, -((scrollheight + centerheight) / 2 - 4), 1);

//            //下填充
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
//    /// 设置滑动块总行数，并刷新
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

//        //滑动到起始结尾处理
//        if (startNow < 0) return;
//        if (startNow == 0) startNow = 1;
//        if (startNow + m_iShowLineCount - 2 == m_iScrollLineCount) startNow = m_iScrollLineCount - m_iShowLineCount + 1;

//        if (startNow + m_iShowLineCount - 2 > m_iScrollLineCount)
//        {
//            //达到结尾处回调
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
//    /// 设置行高，计算滑动块最多显示行数
//    /// </summary>
//    /// <param name="spac">好高</param>
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
//    /// 初始化滑动块，填满
//    /// </summary>
//    /// <param name="LineHeight">行高</param>
//    /// <param name="Template">行模板</param>
//    public void InitScrollView(int LineHeight, GameObject Template)
//    {
//        //填满滑动行
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
//    /// 刷新当前的滑动块
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
//    /// 隐藏某行 并刷新
//    /// </summary>
//    /// <param name="deleteLine">隐藏的行号</param>
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
//    /// 重置滑动块，默认首行开始
//    /// </summary>
//    /// <param name="firstline">默认显示位置</param>
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
//    /// 设置滑动块区域
//    /// </summary>
//    /// <param name="limitNum">滑动块最大行</param>
//    void ExpScrollRange(int limitNum)
//    {

//        //Util.Log("limit = " + limitNum + "    spac = " + LineSpac);

//        //新增添加位置
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
//    /// 刷新滑动块
//    /// </summary>
//    /// <param name="lineNum">刷新行数</param>
//    void InitScrollLines(int lineNum)
//    {
//        for (int i = m_iDefaultLine; i < m_iDefaultLine + lineNum; i++)
//        {
//            InitLine(i);
//        }
//    }

//    /// <summary>
//    /// 滑动到临界点，刷新临界行 update中使用
//    /// </summary>
//    /// <param name="olds">临界行 之前位置行</param>
//    /// <param name="news">临界行 之后位置行</param>
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
//    /// 刷新单行
//    /// </summary>
//    /// <param name="lineNum">行号</param>
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
