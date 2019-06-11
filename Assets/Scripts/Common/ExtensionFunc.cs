/****************************************************
    文件：ExtensionFunc.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/6/1 15:18:33
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;  

public static class ExtensionFunc 
{ 
    public static void OnValueChgedExt(this Toggle toggle, UnityAction<string> cb2) {
        toggle.onValueChanged.AddListener((bool b)=> { 
            if (b) {
                cb2(toggle.name);
            } 
        });
    }


    public static void OnClickExt(this Button btn,UnityAction cb1, UnityAction<string> cb2) {
        btn.onClick.AddListener(() =>
        {
            cb1();
            cb2(btn.name);
        });
    }


    public static void OnClickExt(this Button btn, UnityAction<string> cb2)
    {
        btn.onClick.AddListener(() =>
        { 
            cb2(btn.name);
        });
    }
}

