using System;
using System.Linq;
using UnityEngine;

public static class LTUtility
{
    public static void String2Vector3(out Vector3 p, string value)
    {
        string[] separators = new string[] { nameof(Vector3), "(", ")", "," };
        var strs = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        var vector = new Vector3(float.Parse(strs[0]), float.Parse(strs[1]), float.Parse(strs[2]));
        p = vector;
    }

    public static void String2Quaternion(out Quaternion p, string value)
    {
        string[] separators = new string[] { nameof(Quaternion), "(", ")", "," };
        var strs = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        var vector = new Quaternion(float.Parse(strs[0]), float.Parse(strs[1]), float.Parse(strs[2]), float.Parse(strs[3]));
        p = vector;
    }

    /// <summary>
    /// 给定的匹配字符串是否包含在路径中
    /// </summary>
    /// <param name="path"></param>
    /// <param name="matchParams"></param>
    /// <returns></returns>
    public static bool StringIsMatchIn(string path, params string[] matchParams)
    {
        bool ret = true;
        foreach (var item in matchParams)
        {
            if (!path.Contains(item))
            {
                ret = false;
                break;
            }
        }
        return ret;
    }

    /// <summary>
    /// 给定的路径中是否包含指定的文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <param name="vs"></param>
    /// <returns></returns>
    public static bool DirectoryIsMatchInPath(string path, params string[] matchParams)
    {
        var directorys = path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
        var bb = from e in directorys where matchParams.Contains(e) select e;
        return bb.Count() == matchParams.Length;
    }

    /// <summary>
    /// 脚本是否是在Editor文件夹中
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool IsEditorScript(string path)
    {
        return DirectoryIsMatchInPath(path, "Editor");
    }

    /// <summary>
    /// 计算一个点是否在指定位置所成的扇形范围内
    /// </summary>
    public static bool PointIsInVisualRange(Vector3 point, Transform cricleTrans, float visualAngle, float cricleRadius)
    {
        //计算圆心点左右两个边界方向
        Quaternion originRot = cricleTrans.rotation;
        Quaternion leftRot = Quaternion.AngleAxis(-visualAngle * 0.5f, cricleTrans.up);
        cricleTrans.rotation = leftRot * cricleTrans.rotation;
        Vector3 leftDir = cricleTrans.forward;
        leftDir.Set(leftDir.x, 0, leftDir.z);
        cricleTrans.rotation = originRot;

        Quaternion rightRot = Quaternion.AngleAxis(visualAngle * 0.5f, cricleTrans.up);
        cricleTrans.rotation = rightRot * cricleTrans.rotation;
        Vector3 rightDir = cricleTrans.forward;
        rightDir.Set(rightDir.x, 0, rightDir.z);
        cricleTrans.rotation = originRot;

        point.Set(point.x, 0, point.z);
        Vector3 criclePos = cricleTrans.position;
        criclePos.Set(criclePos.x, 0, criclePos.z);

        //首先判断该点是否在角度范围内
        Vector3 pointDir = point - criclePos;
        float leftAngle = Vector3.Angle(pointDir, leftDir);
        float rightAngle = VerctorAngle(pointDir, rightDir, cricleTrans.forward);
        bool ret = false;
        //在扇形内，扇形的两个边界不考虑
        if (leftAngle < visualAngle && rightAngle < visualAngle)
        {
            //判断是否在圆内,原边界不考虑
            if (pointDir.magnitude < cricleRadius)
            {
                ret = true;
            }
        }
        return ret;
    }

    public static float VerctorAngle(Vector3 vector1, Vector3 vector2, Vector3 forward)
    {
        //首先判断两个向量所在的象限
        float v1 = Vector3.Dot(vector1, forward);
        float angle = Vector3.Angle(vector1, vector2);
        if (v1 < 0)//在第三四象限
        {
            angle = 360f - angle;
        }
        return angle;
    }
}