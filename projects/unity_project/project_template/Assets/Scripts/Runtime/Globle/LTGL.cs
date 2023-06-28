using cfg;
using LT_Event;
using LT_GL;
using LT_Kernel;
using LT_UI;
using System.Threading;

public sealed class LTGL : Singleton<LTGL>
{
    /// <summary>
    /// 框架是否初始化
    /// </summary>
    public bool FrameIsInit { get; set; } = false;

    /// <summary>
    /// 游戏启动脚本实例
    /// </summary>
    public LTMonoBehaviour MainBehaviour { get; set; }

    public CancellationTokenSource TokenSource { get; set; }

    /// <summary>
    /// 模块管理器
    /// </summary>
    public ModuleMgr ModuleMgr { get; set; }

    /// <summary>
    /// 携程管理器
    /// </summary>
    public CoroutineMgr CoroutineMgr { get; set; }

    /// <summary>
    /// 定时器管理器
    /// </summary>
    public LT_GL.TimerMgr TimerMgr { get; set; }

    /// <summary>
    /// 音频管理器
    /// </summary>
    public AudioMgr AudioMgr { get; set; }

    /// <summary>
    /// 资源加载管理器
    /// </summary>
    public ResMgr ResMgr { get; set; }

    /// <summary>
    /// 场景管理器
    /// </summary>
    public SceneMgr SceneMgr { get; set; }

    public UIRootMgr UIRootMgr { get; set; }

    /// <summary>
    /// Window界面管理器
    /// </summary>
    public WindowMgr WindowMgr { get; set; }

    /// <summary>
    /// 数据模型管理器
    /// </summary>
    public ModelMgr ModelMgr { get; set; }

    /// <summary>
    /// 业务逻辑事件
    /// </summary>
    public EventSys LogicEvent { get; } = new EventSys();

    /// <summary>
    /// 场景事件
    /// </summary>
    public EventSys SceneEvent { get; } = new EventSys();

    /// <summary>
    /// 游戏事件
    /// </summary>
    public EventSys GameEvent { get; } = new EventSys();

    public GameSettingMgr GameSettingMgr { get; set; }

    public Tables Tables { get; set; }
}