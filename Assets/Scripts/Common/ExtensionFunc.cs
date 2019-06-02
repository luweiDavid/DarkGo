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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="toggle"></param>
    /// <param name="cb2"></param>
    public static void OnValueChgedExt(this Toggle toggle, UnityAction<string> cb2) {
        toggle.onValueChanged.AddListener((bool b)=> { 
            if (b) {
                cb2(toggle.name);
            } 
        });
    }
}

