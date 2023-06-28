using LT_GL;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Main : LTMonoBehaviour
{
    private readonly ModuleMgr m_ModuleMgr = new();

    public override void OnInit(ITuple tuple = null)
    {
        LTGL.Ins.MainBehaviour = this;
        m_ModuleMgr.OnInit();
        print(LTGL.Ins.Tables.TBHeroInfo.Get(1).ToString());
    }

    public override void OnFixedUpdate(float delta)
    {
        m_ModuleMgr.OnFixedUpdate(Time.fixedDeltaTime);
    }

    public override void OnUpdate(float delta)
    {
        m_ModuleMgr.OnUpdate(Time.deltaTime);
    }

    public override void OnLateUpdate(float delta)
    {
        m_ModuleMgr.OnLateUpdate(Time.deltaTime);
    }

    public override void OnRemove()
    {
        m_ModuleMgr.OnRemove();
    }
}