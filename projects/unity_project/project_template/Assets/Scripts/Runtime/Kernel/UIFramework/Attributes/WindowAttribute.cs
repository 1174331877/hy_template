using System;

namespace LT_UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class WindowAttribute : Attribute
    {
        /// <summary>
        /// 是否时常驻Window
        /// </summary>
        public bool IsResident { get; private set; }

        /// <summary>
        /// 是否时导航条
        /// </summary>
        public bool IsActionBar { get; private set; }

        public WindowAttribute(bool isResident, bool isActionBar = false)
        {
            IsResident = isResident;
            IsActionBar = isActionBar;
        }
    }
}