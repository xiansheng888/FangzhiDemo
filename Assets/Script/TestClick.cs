using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClick : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            type01 t01= System.Activator.CreateInstance(typeof(type01))as type01;
            t01.Show_Info();
            t01.names = "666";
            Debug.LogError(t01.names);
        }
    }


}
public enum ClassType
{
    type01,type02,
}
public class type01
{
    public string names = "0";
    public void  Show_Info()
    {
        Debug.Log("我是type01");
    }
}
public class type02
{
    public string names = "02";
    public void Show_Info()
    {
        Debug.Log("我是type02");
    }
}

