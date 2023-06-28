using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

public static class MonoExtention
{
    public static void SetHide(this Transform transform)
    {
        if (transform == null)
        {
            throw new System.Exception($"{transform.GetType().FullName} is null");
        }
        else
        {
            transform.localScale = Vector3.zero;
        }
    }

    public static void SetHide(this GameObject gameObject)
    {
        if (gameObject == null)
        {
            throw new System.Exception($"{gameObject.GetType().FullName} is null");
        }
        else
        {
            SetHide(gameObject.transform);
        }
    }

    /// <summary>
    /// 存在拆箱操作,如果频繁调用该方法,请用原生方法代替
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="scale">目标大小</param>
    public static void SetShow(this Transform transform, Vector3? scale = null)
    {
        if (transform == null)
        {
            throw new System.Exception($"{transform.GetType().FullName} is null");
        }
        else
        {
            transform.localScale = scale == null ? Vector3.one : (Vector3)scale;
        }
    }

    /// <summary>
    /// 存在拆箱操作,如果频繁调用该方法,请用原生方法代替
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="scale">目标大小</param>
    public static void SetShow(this GameObject gameObject, Vector3? scale = null)
    {
        if (gameObject == null)
        {
            throw new System.Exception($"{gameObject.GetType().FullName} is null");
        }
        else
        {
            SetShow(gameObject.transform, scale);
        }
    }

    public static void SetShowState(this GameObject gameObject, bool state)
    {
        if (state)
        {
            gameObject.SetShow();
        }
        else
        {
            gameObject.SetHide();
        }
    }

    public static void SetShowState(this Transform transform, bool stata)
    {
        transform.gameObject.SetShowState(stata);
    }

    public static T GetCom<T>(this Transform transform, string path = null) where T : UnityEngine.Component
    {
        T ret = null;
        if (string.IsNullOrEmpty(path))
        {
            ret = transform.GetComponent<T>();
        }
        else
        {
            Transform child = transform.Find(path);
            if (child != null)
            {
                ret = child.GetComponent<T>();
            }
        }
        if (ret == null)
        {
            throw new System.Exception($"{transform.name} can't find {typeof(T).FullName} component");
        }
        return ret;
    }

    public static T GetCom<T>(this GameObject gameObject, string path = null) where T : UnityEngine.Component
    {
        return gameObject.transform.GetCom<T>(path);
    }

    public static T GetFristCom<T>(this Transform transform) where T : Component
    {
        T ret = default;
        if (transform.TryGetComponent<T>(out ret))
        {
            return ret;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var com = child.GetFristCom<T>();
            if (com != null)
            {
                ret = com;
                break;
            }
            continue;
        }
        return ret;
    }

    public static T GetComInChild<T>(this Transform transform, string childShortName) where T : Component
    {
        T ret = default;
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.name == childShortName)
            {
                ret = child.GetCom<T>();
                break;
            }
            else
            {
                ret = child.GetComInChild<T>(childShortName);
            }
            if (ret != null)
            {
                break;
            }
        }
        return ret;
    }

    public static void BindEvent(this Transform transform, UnityAction action)
    {
        Assert.IsNotNull(transform, "transform is null!");
        Button button = transform.GetCom<Button>();
        if (button == null)
        {
            if (transform.GetCom<Image>() == null)
            {
                transform.gameObject.AddComponent<EmptyRaycast>();
            }
            transform.gameObject.AddComponent<Button>().BindEvent(action);
        }
        else
        {
            button.BindEvent(action);
        }
    }

    public static void BindEvent(this Button button, UnityAction action)
    {
        Assert.IsNotNull(button, "button is null");
        button.onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }

    /// <summary>
    /// 设置自身和所有子节点的层
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="layer"></param>
    public static void SetLayer(this Transform transform, int layer)
    {
        if (transform != null)
        {
            transform.gameObject.layer = layer;
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                child.layer = layer;
                child.SetLayer(layer);
            }
        }
    }

    public static void SetLayer(this GameObject gameObject, int layer)
    {
        gameObject.transform.SetLayer(layer);
    }

    public static void SetTag(this Transform transform, string tag)
    {
        Assert.IsNotNull(transform, "transform is null!");
        transform.tag = tag;
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).SetTag(tag);
        }
    }

    public static void SetTag(this GameObject gameObject, string tag)
    {
        Assert.IsNotNull(gameObject, "gameObject is null!");
        gameObject.transform.SetTag(tag);
    }

    public static void SetIsKinematic(this Rigidbody rigidbody, bool state)
    {
        if (state)
        {
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        else
        {
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
        rigidbody.isKinematic = state;
    }

    /// <summary>
    /// 计算两点同高度的距离
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static float DistanceOfSameHeight(this Vector3 vector, Vector3 target)
    {
        target.Set(target.x, vector.y, target.z);
        return Vector3.Distance(vector, target);
    }

    /// <summary>
    /// 目标层级是否在匹配层级中
    /// </summary>
    /// <param name="layerMask"></param>
    /// <param name="matchLayer"></param>
    /// <returns></returns>
    public static bool LayerMaskIsMatchIn(this LayerMask matchLayer, int targetLayer)
    {
        Int32 targetValue = (Int32)Mathf.Pow(2, targetLayer);
        return (matchLayer.value & targetValue) == targetValue;
    }

    /// <summary>
    /// 目标层级是否完全匹配该层级
    /// </summary>
    /// <param name="matchLayer"></param>
    /// <param name="targetLayer"></param>
    /// <returns></returns>
    public static bool LayerMaskIsFullMatch(this LayerMask matchLayer, int targetLayer)
    {
        return LayerMask.GetMask(LayerMask.LayerToName(targetLayer)) == matchLayer;
    }
}