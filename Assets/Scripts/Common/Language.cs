/****************************************************
    文件：Language.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/24 18:26:48
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class Language 
{
    static Dictionary<int, string> strDic = new Dictionary<int, string>()
    {
        [1] = "账号或密码为空！",
        [2] = "服务器失联",
        [3] = "玩家已上线",
        [4] = "密码错误",
        [5] = "",
        [6] = "",
        [7] = "",
        [8] = "",
        [9] = "",
        [10] = "",
        [11] = "",
        [12] = "",
        [13] = "",
        [14] = "",
        [15] = "",
        [16] = "",
        [17] = "",
        [18] = "",



        [1000] = "功能正在开发中.....",
    };

    public static string GetString(int index) {
        string str = "";
        strDic.TryGetValue(index, out str);
        return str;
    }
}