using TMPro;
using UnityEngine;

public class ShowFrame : MonoBehaviour
{
    //GUI显示的帧率
    private float m_FrameCount = 0;

    //一个m_ProcessInterval时长单元下逻辑更新帧数累计
    private float m_FrameCountTikc = 0;

    //一个m_ProcessInterval时长单元下逻辑更新时长累计
    private float m_LogicUpdateTimeTick = 0;

    //帧率计算间隔时长
    private float m_ProcessInterval = 0.5f;

    //真实时间流逝时长(不受时间缩放大小影响)
    private float m_TimeTick = 0;

    private TextMeshProUGUI m_FPSText;

    private void Start()
    {
        m_FPSText = GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        m_TimeTick += Time.fixedUnscaledDeltaTime;
    }

    // Update is called once per frame
    private void Update()
    {
        m_LogicUpdateTimeTick += Time.deltaTime;
        m_FrameCountTikc += 1;
        if (m_TimeTick >= m_ProcessInterval)
        {
            m_FrameCount = m_FrameCountTikc / (m_LogicUpdateTimeTick);
            m_TimeTick = 0;
            m_FrameCountTikc = 0;
            m_LogicUpdateTimeTick = 0;
            m_FPSText.text = "FPS:\t" + m_FrameCount.ToString("f1");
        }
    }
}