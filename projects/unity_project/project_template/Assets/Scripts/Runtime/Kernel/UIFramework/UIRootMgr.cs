using LT_Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace LT_UI
{
    public sealed class UIRootMgr : AbsMgr
    {
        public Transform UIRoot { get; private set; }
        public Camera UICamera { get; private set; }

        private Dictionary<ECanvasType, Transform> uiCanvas = new Dictionary<ECanvasType, Transform>();

        public override void OnInit(ITuple tuple = null)
        {
            base.OnInit(tuple);
            InitUIRoot();
        }

        private void InitUIRoot()
        {
            GameObject uiRootPrefab = LTGL.Ins.ResMgr.LoadFromResources<GameObject>("UIRoot");
            UIRoot = GameObject.Instantiate(uiRootPrefab).transform;
            UIRoot.localPosition = Vector3.zero;
            UIRoot.localRotation = Quaternion.identity;
            UIRoot.localScale = Vector3.one;
            UICamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            var canvasNames = Enum.GetNames(typeof(ECanvasType)).ToList();
            var enumerator = canvasNames.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Transform canvasTrans = UIRoot.Find(enumerator.Current).transform;
                Canvas canvas = canvasTrans.GetCom<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = UICamera;
                var canvasScaler = canvasTrans.GetCom<CanvasScaler>();
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = LTGL.Ins.GameSettingMgr.ScreenRatio;
                Enum.TryParse<ECanvasType>(enumerator.Current, out ECanvasType canvasType);
                uiCanvas.Add(canvasType, canvasTrans);
            }
            UnityEngine.Object.DontDestroyOnLoad(UIRoot);
        }

        public Transform GetCanvasByType(ECanvasType canvasType)
        {
            uiCanvas.TryGetValue(canvasType, out var transform);
            return transform;
        }
    }
}