using LT_Kernel;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LT_UI
{
    /// <summary>
    /// 页面挂载的Canvas类型
    /// </summary>
    public enum ECanvasType
    {
        BaseCanvas,
        MiddleCanvas,
        TopCanvas,
        TipsCanvas
    }

    /// <summary>
    /// 是否处于激活状态
    /// </summary>
    public interface IIsActive
    {
        bool IsActive { get; set; }
    }

    /// <summary>
    /// 页面行为接口
    /// </summary>
    public interface IPage
    {
        void OnShowBefore(ITuple tuple = null);

        void OnShow(ITuple tuple = null);

        void OnHideBefore(ITuple tuple = null);

        void OnHide(ITuple tuple = null, bool isCache = false);
    }

    public abstract class AbsPanel : AutoCancleToken, IPage, IIsActive, ILifecycle, IUpdate, IIsUpdate
    {
        /// <summary>
        /// 游戏界面根节点
        /// </summary>
        protected Transform RootTrans { get; private set; }

        /// <summary>
        /// 页面挂载Canvas类型
        /// </summary>
        public ECanvasType CanvasType { get; private set; }

        public bool IsUpdate { get; set; } = false;

        public bool IsActive { get; set; } = false;

        public AbsPanel(Transform transform, ECanvasType canvasType)
        {
            RootTrans = transform;
            CanvasType = canvasType;
        }

        public virtual void OnHide(ITuple tuple = null, bool isCache = false)
        {
            if (isCache)
            {
                if (RootTrans != null)
                {
                    RootTrans.SetHide();
                }
            }
            else
            {
                if (RootTrans != null)
                {
                    UnityEngine.Object.Destroy(RootTrans.gameObject);
                }
            }
        }

        public virtual void OnHideBefore(ITuple tuple = null)
        {
        }

        public virtual void OnShow(ITuple tuple = null)
        {
            if (RootTrans == null)
            {
                throw new Exception($"{GetType().FullName} RootTrans is null!");
            }
            else
            {
                RootTrans.SetShow();
            }
        }

        public virtual void OnShowBefore(ITuple tuple = null)
        {
        }

        public override void OnInit(ITuple tuple = null)
        {
            base.OnInit(tuple);
        }

        public override void OnRemove()
        {
            if (RootTrans != null)
            {
                UnityEngine.Object.Destroy(RootTrans.gameObject);
            }
            base.OnRemove();
        }

        public virtual void OnUpdate(float delta)
        {
        }
    }
}