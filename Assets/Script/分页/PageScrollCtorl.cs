using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PageScrollCtorl : MonoBehaviour, IEndDragHandler, IDragHandler
{
    #region 变量
    private ScrollRect currscrollRect;
    private float[] arr;
    private Transform getAllChildCount;
    private bool isMove = false;
    private float timer = 1.3f;
    private int currindex;
    private float startindex;
    private float autoTimer = 2;
    private float autojs = 2;
    private bool isdragEnter = false;
    public ScrollType scrolltype = ScrollType.Horizontal;
    public float currScale = 1f;
    public float otherScale = 0.5f;
    public int lastpage;
    public int nexpage;
    private List<Transform> Scales = new List<Transform>();
    #endregion
    void Start()
    {
        Initd();
    }
    private void Initd()
    {
        currscrollRect = GetComponent<ScrollRect>();
        getAllChildCount = GameObject.Find("Viewport/Content").GetComponent<Transform>();
        arr = new float[getAllChildCount.childCount];
        Get_page_Count();
        for (int i = 0; i < getAllChildCount.childCount; i++)
        {
            Scales.Add(getAllChildCount.GetChild(i));
        }

    }
    private void Get_page_Count()
    {
        for (int i = 0; i < arr.Length; i++)
        {
            switch (scrolltype)
            {
                case ScrollType.Horizontal:
                    arr[i] = i * (1.0f / (arr.Length - 1.0f));
                    break;
                case ScrollType.vertical:
                    arr[i] = 1 - i * (1.0f / (arr.Length - 1.0f));
                    break;
                default:
                    break;
            }

        }
    }
    /// <summary>
    /// 设置当前分页的位置
    /// </summary>
    /// <param name="index"></param>
    private void Set_Page_Pos(int index)
    {
        currindex = index;
        timer = 1.3f;
        switch (scrolltype)
        {
            case ScrollType.Horizontal:
                startindex = currscrollRect.horizontalNormalizedPosition;
                break;
            case ScrollType.vertical:
                startindex = currscrollRect.verticalNormalizedPosition;
                break;

        }
        isMove = true;

    }
    void Update()
    {
            AutoMove();
        ListerScale();
        if (isMove)
        {
            Debug.Log("走了");
            timer -= Time.deltaTime;
            switch (scrolltype)
            {
                case ScrollType.Horizontal:
                    currscrollRect.horizontalNormalizedPosition = Mathf.Lerp(startindex, arr[currindex], 1.3f);
                    break;
                case ScrollType.vertical:
                    currscrollRect.verticalNormalizedPosition = Mathf.Lerp(startindex, arr[currindex], 1.3f);
                    break;
                default:
                    break;
            }
            if (timer <= 0)
            {
                isMove = false;
                flag = true;
            }
        }
    }
    public void ListerScale()
    {
        //找到上一页，和下一页
        for (int i = 0; i < arr.Length; i++)
        {//  1  i=2   
            if (arr[i] < currscrollRect.horizontalNormalizedPosition)
            {
                lastpage = i;
            }
        }

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] > currscrollRect.horizontalNormalizedPosition)
            {
                nexpage = i;
                break;
            }
        }
        float percent = (currscrollRect.horizontalNormalizedPosition - arr[lastpage]) / (arr[nexpage] - arr[lastpage]);
        Scales[lastpage].transform.localScale = Vector3.Lerp(Vector3.one * currScale, Vector3.one * otherScale,  percent);
        Scales[nexpage].transform.localScale = Vector3.Lerp(Vector3.one * currScale, Vector3.one * otherScale,1 - percent);
        for (int i = 0; i < Scales.Count; i++)
        {
            if (i != lastpage && i != nexpage)
            {
                Scales[i].transform.localScale = Vector3.one * otherScale;
            }
        }
    }
    private int n = 1;
    private void AutoMove()
    {
        autojs -= Time.deltaTime;
        if (autojs <= 0)
        {
            autojs = 2;
            if (n >= arr.Length)
            {
                n = 0;
            }
            else
            {
                Set_Page_Pos(n++);
            }

        }

    }
    bool flag = true;
    public void OnEndDrag(PointerEventData eventData)
    {
        if (true)
        {
            flag = false;
            Debug.Log("拖拽结束");
            int minvalue = 0;
            for (int i = 1; i < arr.Length; i++)
            {
                switch (scrolltype)
                {
                    case ScrollType.Horizontal:
                        if (Mathf.Abs(arr[minvalue] - currscrollRect.horizontalNormalizedPosition) > Mathf.Abs(arr[i] - currscrollRect.horizontalNormalizedPosition))
                        {
                            minvalue = i;
                        }
                        break;
                    case ScrollType.vertical:
                        if (Mathf.Abs(arr[minvalue] - currscrollRect.verticalNormalizedPosition) > Mathf.Abs(arr[i] - currscrollRect.verticalNormalizedPosition))
                        {
                            minvalue = i;
                        }
                        break;
                    default:
                        break;
                }

            }
            Set_Page_Pos(minvalue);
        }
        isdragEnter = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        isdragEnter = true;
    }
}
public enum ScrollType
{
    Horizontal, vertical,
}
