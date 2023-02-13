
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 每一个Item 挂载的脚本
/// </summary>
public class LoopItem : MonoBehaviour
{
    #region  字段
    private RectTransform rect;
    private RectTransform viewRect;
    public Vector3[] rectConrners;//每一个item显示的区域大小 上 下 左 右 四个点
    public Vector3[] viewConrners;
    #endregion



    #region 事件
    public Action onAddHead;
    public Action onRemoveHead;
    public Action onAddLast;
    public Action onRemoveLast;
    #endregion



    #region 方法
    private void Awake()
    {
        rectConrners = new Vector3[4];
        viewConrners = new Vector3[4];
    }
    void Start()
    {
        rect = GetComponent<RectTransform>();
        viewRect = GetComponentInParent<ScrollRect>().GetComponent<RectTransform>();
    }

    private float m =0.1f;
    void Update()
    {
        m -= Time.deltaTime;
        if (m <= 0f)
        {
            m = 0.1f;
            //类似于分帧加载
            LinstenerCorners();
        }

    }
    /// <summary>
    /// 监听显示区域的变化
    /// </summary>
    public void LinstenerCorners()
    {
        //获取自身的边界
        rect.GetWorldCorners(rectConrners);
        //获取显示区域
        viewRect.GetWorldCorners(viewConrners);
        //可以根据rectConrners[0] y轴判断
        if (IsFirst())
        {
            if (rectConrners[0].y > viewConrners[1].y)//不在显示范围内
            {
                //头节点隐藏
                if (onRemoveHead != null) { onRemoveHead(); };
            }

            if (rectConrners[1].y < viewConrners[1].y)//在显示范围内
            {
                //添加头节点
                if (onAddHead != null) { onAddHead(); };
            }
        }

        if (IsLast())
        {
            //添加尾部
            if (rectConrners[0].y > viewConrners[0].y)
            {
                if (onAddLast != null) { onAddLast(); };
            }
            //回收尾部
            if (rectConrners[1].y < viewConrners[0].y)
            {
                if (onRemoveLast != null) { onRemoveLast(); };
                //尾节点隐藏
            }
        }

    }
    /// <summary>
    /// 判断头节点
    /// </summary>
    /// <returns></returns>
    public bool IsFirst()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).gameObject.activeSelf)
            {
                if (transform.parent.GetChild(i) == transform)
                {
                    return true;
                }
                break;
            }
        }
        return false;
    }
    /// <summary>
    /// 判断尾节点
    /// </summary>
    /// <returns></returns>
    public bool IsLast()
    {
        for (int i = transform.parent.childCount - 1; i >= 0; i--)
        {
            if (transform.parent.GetChild(i).gameObject.activeSelf)
            {
                if (transform.parent.GetChild(i) == transform)
                {
                    return true;
                }
                break;
            }
        }
        return false;
    }
    #endregion
}
