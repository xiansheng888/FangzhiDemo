using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataApp<T>
{
    #region  缓存容器
    //缓存所有数据
    public List<T> allData = new List<T>();
    //当前显示的数据
    public LinkedList<T> CurrShowData = new LinkedList<T>();//列表
    #endregion


    #region 方法
    public T GetHeaderData()
    {
        if (allData.Count == 0)
        {
            return default(T);//null
        }
        //没有数据，显示第一个数据
        if (CurrShowData.Count == 0)
        {
            T header = allData[0];
            //添加到当前显示的数据里面
            CurrShowData.AddFirst(header);
            return header;
        }

        //获取CurrShowData 正在显示的数据的上一个
        T t = CurrShowData.First.Value;
        int index = allData.IndexOf(t);
        if (index != 0)
        {
            T header = allData[index - 1];
            //添加到当前显示的数据里面
            CurrShowData.AddFirst(header);
            return header;
        }
        return default(T);//null
    }
    /// <summary>
    /// ////////////////////////////////
    /// </summary>
    /// <returns></returns>
    public bool RemoveHeadData()
    {
        if (CurrShowData.Count == 0 || CurrShowData.Count == 1) { return false; };//移除失败
                                                                                  //CurrShowData 移除正在显示的数据的上一个
        LoopDataItem a = CurrShowData.First as LoopDataItem;
        CurrShowData.RemoveFirst();
        return true;//移除成功
    }
    /// <summary>
    /// ////////////////////////
    /// </summary>
    /// <returns></returns>
    public T GetLastData()
    {
        if (allData.Count == 0)
        {
            return default(T);//null
        }
        //没有数据，显示第一个数据
        if (CurrShowData.Count == 0)
        {
            T last = allData[0];
            //添加到当前显示的数据里面
            CurrShowData.AddLast(last);
            return last;
        }

        //获取CurrShowData 最后一个数据的下一个
        T t = CurrShowData.Last.Value;
        int index = allData.IndexOf(t);
        if (index != allData.Count - 1)
        {
            T now_last = allData[index + 1];
            CurrShowData.AddLast(now_last);

            return now_last;
        }
        return default(T);//null
    }
    /// <summary>
    /// /////////////////////////////
    /// </summary>
    public bool RemoveLastData()
    {
        //移除CurrShowData 获取正最后数据的下一个
        if (CurrShowData.Count == 0 || CurrShowData.Count == 1) { return false; };//移除失败
        CurrShowData.RemoveLast();
        return true;//移除成功
    }
    #endregion

    #region  数据管理
    public void InitData(T[] t)
    {
        allData.Clear();
        CurrShowData.Clear();
        allData.AddRange(t);
    }
    //重载
    public void InitData(List<T> t)
    {
        InitData(t.ToArray());
    }
    //数据添加
    public void AddData(T[] t)
    {
        allData.AddRange(t);

    }
    //重载
    public void AddData(List<T> t)
    {
        AddData(t.ToArray());

    }
    #endregion
}
