using LT_Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LT_UI
{
    public class WindowMgr : AbsMgr
    {
        /// <summary>
        /// 正在显示的Window
        /// </summary>
        private Stack<WindowWrap> m_ActiveWindows = new Stack<WindowWrap>();

        /// <summary>
        /// 所有Windows的缓存
        /// </summary>
        private Dictionary<Type, WindowWrap> m_CacheWindows = new Dictionary<Type, WindowWrap>();

        /// <summary>
        /// 正在显示的导航条
        /// </summary>
        private Stack<WindowWrap> m_ActiveActionBars = new Stack<WindowWrap>();

        /// <summary>
        /// 所有ActionBar的缓存
        /// </summary>
        private Dictionary<Type, WindowWrap> m_CacheActionBars = new Dictionary<Type, WindowWrap>();

        public override void OnInit(ITuple tuple = null)
        {
            IsUpdate = true;
        }

        public override void OnRemove()
        {
            m_UpdateTemp.Clear();
            m_UpdateTemp.AddRange(m_ActiveWindows);
            m_UpdateTemp.AddRange(m_ActiveActionBars);
            var windowsEnumerator = m_UpdateTemp.GetEnumerator();
            while (windowsEnumerator.MoveNext())
            {
                var temp = windowsEnumerator.Current;
                if (temp != null && temp.Window != null)
                {
                    temp.Window.OnRemove();
                }
            }
            m_CacheWindows.Clear();
            m_CacheActionBars.Clear();
        }

        private List<WindowWrap> m_UpdateTemp = new List<WindowWrap>();

        public override void OnUpdate(float delta)
        {
            if (!IsUpdate)
            {
                return;
            }

            m_UpdateTemp.Clear();
            m_UpdateTemp.AddRange(m_ActiveWindows);
            m_UpdateTemp.AddRange(m_ActiveActionBars);
            var windowsEnumerator = m_UpdateTemp.GetEnumerator();
            while (windowsEnumerator.MoveNext())
            {
                var temp = windowsEnumerator.Current;
                if (temp != null && temp.Window != null && temp.Window.IsActive && temp.Window.IsUpdate)
                {
                    temp.Window.OnUpdate(delta);
                }
            }
        }

        public virtual void ShowWindow<T>(ITuple tuple = null) where T : AbsWindow
        {
            Type windowType = typeof(T);

            if (m_CacheWindows.TryGetValue(windowType, out WindowWrap windowWrap))
            {
                if (windowWrap != null && windowWrap.Window != null && !windowWrap.Window.IsActive)
                {
                    if (!m_ActiveWindows.Contains(windowWrap))
                    {
                        m_ActiveWindows.Push(windowWrap);
                        windowWrap.Window.OnShow(tuple);
                        windowWrap.Window.IsActive = true;
                    }
                }
            }
            else if (m_CacheActionBars.TryGetValue(windowType, out windowWrap))
            {
                if (windowWrap != null && windowWrap.Window != null && !windowWrap.Window.IsActive)
                {
                    if (!m_ActiveActionBars.Contains(windowWrap))
                    {
                        m_ActiveActionBars.Push(windowWrap);
                        windowWrap.Window.OnShow(tuple);
                        windowWrap.Window.IsActive = true;
                    }
                }
            }
            else
            {
                WindowAttribute attribute = windowType.GetCustomAttribute<WindowAttribute>();
                AbsWindow absWindow = Activator.CreateInstance(windowType, new object[] { attribute.IsResident }) as AbsWindow;
                windowWrap = new WindowWrap(absWindow, attribute);
                if (attribute.IsActionBar)
                {
                    m_CacheActionBars.Add(windowType, windowWrap);
                    m_ActiveActionBars.Push(windowWrap);
                }
                else
                {
                    m_CacheWindows.Add(windowType, windowWrap);
                    m_ActiveWindows.Push(windowWrap);
                }
                absWindow.OnInit();
                absWindow.OnShow(tuple);
                absWindow.IsActive = true;
            }
        }

        private List<WindowWrap> m_HideTempList = new List<WindowWrap>();

        /// <summary>
        /// 关闭一个指定的window
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tuple"></param>
        public virtual void HideWindow<T>(ITuple tuple = null) where T : AbsWindow
        {
            HideWindow(typeof(T), m_CacheWindows, m_ActiveWindows, tuple);
        }

        private void HideWindow(Type windowType, Dictionary<Type, WindowWrap> cacheWindows, Stack<WindowWrap> activeWindows, ITuple tuple = null)
        {
            if (cacheWindows.TryGetValue(windowType, out WindowWrap windowWrap))
            {
                if (activeWindows.Contains(windowWrap))
                {
                    m_HideTempList.Clear();
                    while (activeWindows.Count > 0)
                    {
                        var temp = activeWindows.Peek();
                        if (temp != windowWrap)
                        {
                            m_HideTempList.Add(temp);
                            activeWindows.Pop();
                        }
                        else
                        {
                            windowWrap.Window.OnHide(tuple);
                            windowWrap.Window.IsActive = false;
                            if (!windowWrap.WindowAttribute.IsResident)
                            {
                                cacheWindows.Remove(windowType);
                                windowWrap.Window.OnRemove();
                            }
                            activeWindows.Pop();
                            break;
                        }
                    }
                    for (int i = m_HideTempList.Count - 1; i >= 0; i--)
                    {
                        activeWindows.Push(m_HideTempList[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 隐藏导航条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tuple"></param>
        public void HideActionBarWindow<T>(ITuple tuple = null) where T : AbsWindow
        {
            HideWindow(typeof(T), m_CacheActionBars, m_ActiveActionBars, tuple);
        }

        /// <summary>
        /// 关闭栈顶界面
        /// </summary>
        public void BackAny(ITuple tuple = null)
        {
            if (m_ActiveWindows.Count > 1)
            {
                var absWindow = m_ActiveWindows.Peek();
                HideWindow(absWindow.Window.GetType(), m_CacheWindows, m_ActiveWindows, tuple);
            }
        }

        /// <summary>
        /// 关闭该window和它上面的所有window
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tuple"></param>
        public void BackAt<T>(ITuple tuple = null) where T : AbsWindow
        {
            if (m_ActiveWindows.Count > 0)
            {
                Type type = typeof(T);
                if (m_CacheWindows.TryGetValue(type, out var windowWrap) && windowWrap.Window.IsActive)
                {
                    while (m_ActiveWindows.Count > 0)
                    {
                        var curWindow = m_ActiveWindows.Pop();
                        curWindow.Window.IsActive = false;
                        if (curWindow.Window.IsResident)
                        {
                            curWindow.Window.OnHide();
                        }
                        else
                        {
                            curWindow.Window.OnRemove();
                            m_CacheWindows.Remove(type);
                        }
                        if (curWindow == windowWrap)
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 该窗口界面是否正在显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool IsShowing<T>() where T : AbsWindow
        {
            bool ret = false;
            var enumerator = m_ActiveWindows.GetEnumerator();
            while (enumerator.MoveNext())
            {
                WindowWrap windowWrap = enumerator.Current;
                if (windowWrap.Window is T)
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取一个Window
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetWindow<T>() where T : AbsWindow
        {
            Type type = typeof(T);
            T window = null;
            if (m_CacheWindows.TryGetValue(type, out var windowWrap))
            {
                window = windowWrap.Window as T;
            }
            return window;
        }

        private sealed class WindowWrap
        {
            public readonly AbsWindow Window;
            public readonly WindowAttribute WindowAttribute;

            public WindowWrap(AbsWindow window, WindowAttribute attribute)
            {
                Window = window;
                WindowAttribute = attribute;
            }
        }
    }
}