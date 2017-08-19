using UnityEngine;
using System;
using System.Collections;
using HIEngine;

public class UILabelHelper : MonoBehaviour
{
    // UILabel 文本显示方式
    public enum ShowMode
    {
        Verbatim,   // 逐字显示
        Full,       // 全部显示
    }
    private ShowMode m_eShowMode = ShowMode.Full;

    public bool IgnoreTimeScale = false;
    public Action VerbatimEndCallback = null;   // 逐字显示结束的回调函数

    private UILabel m_cLabel = null;
    private string m_strText = null;

    private float m_fShowSpeedTimer = 0.0f;
    private float m_fShowSpeed = 1.0f;  // = (1 / 每秒显示的文字数量)
    private int m_nCurShowTextLength = 0;
    private bool m_bNeedUpdateShowText = false;

    public int TextShowMode
    {
        get
        {
            return (int)m_eShowMode;
        }

        set
        {
            m_eShowMode = (ShowMode)value;
            switch (m_eShowMode)
            {
                case ShowMode.Verbatim:
                    {
                        ResetVerbatimData();
                    }
                    break;

                case ShowMode.Full:
                    {
                        m_cLabel.text = m_strText;
                    }
                    break;
            }
        }
    }

    public int ShowSpeed
    {
        get
        {
            return ((int)(1.0f / m_fShowSpeed));
        }

        set
        {
            m_fShowSpeed = (1.0f / value);
        }
    }

    public void Reset()
    {
        if (m_eShowMode == ShowMode.Verbatim)
        {
            ResetVerbatimData();
        }
    }

    public void ShowFullText()
    {
        if (m_eShowMode == ShowMode.Verbatim)
        {
            m_cLabel.text = m_strText;
            VerbatimEnd();
        }
    }

    private void Awake()
    {
        m_cLabel = GetComponent<UILabel>();
        if (null == m_cLabel)
        {
            Log.Info("[UILabelHelper::Awake]: Can not find UILabel component");
            DestroyImmediate(this);
            return;
        }
    }

    private void Update()
    {
        if (ShowMode.Verbatim == m_eShowMode)
        {
            UpdateShowText();
        }
    }

    private void UpdateShowText()
    {
        if (!m_bNeedUpdateShowText)
        {
            return;
        }

        // 更新显示文本
        m_fShowSpeedTimer += (IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
        if (m_fShowSpeedTimer > m_fShowSpeed)
        {
            m_fShowSpeedTimer -= m_fShowSpeed;

            if (m_cLabel.supportEncoding)
            {
                NGUIText.ParseSymbol(m_strText, ref m_nCurShowTextLength);
            }

            ++m_nCurShowTextLength;
            if (m_nCurShowTextLength > m_strText.Length)
            {
                VerbatimEnd();
            }
        }

        if (m_bNeedUpdateShowText)
        {
            m_cLabel.text = m_strText.Substring(0, m_nCurShowTextLength);
        }
    }

    private void ResetVerbatimData()
    {
        m_strText = m_cLabel.text;
        m_fShowSpeedTimer = 0.0f;
        m_nCurShowTextLength = 0;
        m_bNeedUpdateShowText = true;
        m_cLabel.text = string.Empty;
    }

    private void VerbatimEnd()
    {
        m_bNeedUpdateShowText = false;

        if (null != VerbatimEndCallback)
        {
            VerbatimEndCallback();
        }
    }
}