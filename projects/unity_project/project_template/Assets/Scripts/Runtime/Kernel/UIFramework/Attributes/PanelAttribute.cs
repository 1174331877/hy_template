using System;

namespace LT_UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PanelAttribute : Attribute
    {
        /// <summary>
        /// 预设资源相对路径
        /// </summary>
        public string PrefabPath { get; private set; }

        /// <summary>
        /// 面板挂载Canvas类型
        /// </summary>
        public ECanvasType CanvasType { get; private set; }

        /// <summary>
        /// 是否是Resources目录下的资源
        /// </summary>
        public bool IsResources { get; private set; }

        public PanelAttribute(string prefabPath , ECanvasType canvasType , bool isResources = false)
        {
            PrefabPath = prefabPath;
            CanvasType = canvasType;
            IsResources = isResources;
        }
    }
}
