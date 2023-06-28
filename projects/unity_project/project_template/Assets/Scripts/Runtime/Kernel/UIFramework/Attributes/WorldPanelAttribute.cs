using System;

namespace LT_UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class WorldPanelAttribute : Attribute
    {
        /// <summary>
        /// 预设资源相对路径
        /// </summary>
        public string PrefabPath { get; private set; }

        /// <summary>
        /// 是否是Resources目录下的资源
        /// </summary>
        public bool IsResources { get; private set; }

        public WorldPanelAttribute(string prefabPath, bool isResources = false)
        {
            PrefabPath = prefabPath;
            IsResources = isResources;
        }
    }
}
