using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ScrollView 挂载
/// </summary>
public class LooopScroll : MonoBehaviour
{
    #region 字段
    public GameObject childitemPrefab;//子物体的预制体
    private GridLayoutGroup contGridLayoutGroup;//排序组件
    private ContentSizeFitter sizeFitter;//自动适应
    private RectTransform content;
    private DataApp<LoopDataItem> loopdata;
    #endregion




    #region Unity回调
    private void Awake()
    {
        Init();
    }
    void Start()
    {
        contGridLayoutGroup.enabled = true;
        sizeFitter.enabled = true;
        //添加一个子节点
        OnAddHead();
        //关掉排序组件
        Invoke("Close_Sort_Com", 0.1f);
    }
    #endregion





    #region Unity方法
    public void Init()
    {
        content = transform.Find("Viewport/Content").GetComponentInChildren<RectTransform>();
        contGridLayoutGroup = content.GetComponentInChildren<GridLayoutGroup>();
        sizeFitter = content.GetComponentInChildren<ContentSizeFitter>();
        //*****************************************//
        //数据模拟
        loopdata = new DataApp<LoopDataItem>();
        List<LoopDataItem> loopitenList = new List<LoopDataItem>();
        for (int i = 0; i < 100; i++)
        {
            loopitenList.Add(new LoopDataItem(i + 1));
        }
        loopdata.InitData(loopitenList);
    }
    /// <summary>
    /// 创建新的item
    /// </summary>
    /// <returns></returns>
    public GameObject GetChildItem()
    {
        //先查找有没有被回收的子节点(对象池缓存)
        for (int i = 0; i < content.childCount; i++)
        {
            GameObject m = content.GetChild(i).gameObject;
            if (!m.activeSelf)
            {
                m.SetActive(true);
                return m;
            }
        }
        //如果没有创建一个
        GameObject childItem = GameObject.Instantiate(childitemPrefab, content, false);
        NewItemSet(childItem);
        return childItem;
    }
    /// <summary>
    /// 对新生成的item进行初始设置
    /// </summary>
    /// <param name="childItem"></param>
    private void NewItemSet(GameObject childItem)
    {
        childItem.transform.localScale = Vector3.one;
        childItem.transform.localPosition = Vector3.zero;
        //设置锚点（左上角）
        childItem.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        childItem.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        //设置宽高
        childItem.GetComponent<RectTransform>().sizeDelta = contGridLayoutGroup.cellSize;
        LoopItem loopitem = childItem.AddComponent<LoopItem>();
        loopitem.onAddHead += this.OnAddHead;
        loopitem.onRemoveHead += this.OnRemoveHead;
        loopitem.onAddLast += this.OnAddLast;
        loopitem.onRemoveLast += this.OnRemoveLast;
    }

    /// <summary>
    /// 在最上边添加一个物体
    /// </summary>
    private void OnAddHead()
    {
        LoopDataItem loopitemdata = loopdata.GetHeaderData();

        if (loopitemdata != null)
        {
         //   Debug.Log("、上一个数据" + loopitemdata.id);
            Transform first = FindFirst();
            GameObject obj = GetChildItem();
            obj.transform.SetAsFirstSibling();//设置下标
            //设置数据
            SetData(obj, loopitemdata);
            //动态设置位置
            if (first != null)
            {                                                        //保持和排序组件一样的size+间距
                obj.transform.localPosition = first.localPosition + new Vector3(0, contGridLayoutGroup.cellSize.y + contGridLayoutGroup.spacing.y);
            }
        }

    }
    //在最上边移除一个物体
    private void OnRemoveHead()
    {
        if (loopdata.RemoveHeadData())//数据移除成功
        {
            //隐藏
            Transform first = FindFirst();
            if (first != null)
            {
                first.gameObject.SetActive(false);
            }
        }

    }
    //在最后添加一个物体
    private void OnAddLast()
    {
        LoopDataItem loopitemdata = loopdata.GetLastData();
        if (loopitemdata != null)
        {
            Transform last = FindLast();
            GameObject obj = GetChildItem();
            obj.transform.SetAsLastSibling();//设置下标
                                             //设置数据
        //   Debug.Log("下一个数据" + loopitemdata.id);
            SetData(obj, loopitemdata);
            //动态设置位置
            if (last != null)
            {
                obj.transform.localPosition = last.localPosition - new Vector3(0, contGridLayoutGroup.cellSize.y + contGridLayoutGroup.spacing.y);
            }
            //要不要增加高度
            if (IsNeedAddContentHight(obj.transform))
            {
                //对高度增加
                content.sizeDelta += new Vector2(0, contGridLayoutGroup.cellSize.y + contGridLayoutGroup.spacing.y);
            }
        }
    }
    //在最下后移除一个物体
    private void OnRemoveLast()
    {
        if (loopdata.RemoveLastData())//数据移除成功
        {
            Transform last = FindLast();
            if (last != null)
            {
                last.gameObject.SetActive(false);
            }
        }
    }

    //找到最后一个item 位置返回
    public Transform FindFirst()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i).gameObject.activeSelf)
            {
                return content.GetChild(i).transform;
            }

        }
        return null;
    }
    //找到最后一个item 位置返回
    private Transform FindLast()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            if (content.GetChild(i).gameObject.activeSelf)
            {
                return content.GetChild(i).transform;
            }
        }
        return null;
    }
    /// <summary>
    /// 内容的适配
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public bool IsNeedAddContentHight(Transform trans)
    {
        Vector3[] rectCorners = new Vector3[4];
        Vector3[] contentCorners = new Vector3[4];
        trans.GetComponent<RectTransform>().GetWorldCorners(rectCorners);
        content.GetWorldCorners(contentCorners);
        if (rectCorners[0].y < contentCorners[0].y)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 关闭排序组件
    /// </summary>
    public void Close_Sort_Com()
    {
        contGridLayoutGroup.enabled = false;
        sizeFitter.enabled = false;
    }

    public void SetData(GameObject childitem, LoopDataItem data)
    {
        childitem.transform.Find("Text").GetComponent<Text>().text = data.id.ToString();
    }
    #endregion

}
