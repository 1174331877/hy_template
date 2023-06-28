using LT_Kernel;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class LTMonoBehaviour : MonoBehaviour, ILifecycle, IUpdate, IFixedUpdate, ILateUpdate, IIsUpdate
{
    protected CancellationTokenSource TokenSource = new CancellationTokenSource();

    public bool IsUpdate { get; set; } = false;

    #region 引擎接口

    private void Awake()
    {
        OnInit();
    }

    private void FixedUpdate()
    {
        if (!IsUpdate) return;
        OnFixedUpdate(Time.deltaTime);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsUpdate) return;
        OnUpdate(Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (!IsUpdate) return;
        OnLateUpdate(Time.deltaTime);
    }

    private void OnDestroy()
    {
        TokenSource.Cancel();
        TokenSource.Dispose();
        OnRemove();
    }

    #endregion 引擎接口

    #region 逻辑接口

    public virtual void OnInit(ITuple tuple = null)
    {
    }

    public virtual void OnRemove()
    {
    }

    public virtual void OnUpdate(float delta)
    {
    }

    public virtual void OnFixedUpdate(float delta)
    {
    }

    public virtual void OnLateUpdate(float delta)
    {
    }

    #endregion 逻辑接口
}