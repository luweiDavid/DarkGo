/****************************************************
    文件：PETools.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/22 10:51:39
	功能：Nothing
*****************************************************/

using System;
public class PETools  
{
    public static int RandomInt(int min, int max, Random rd = null) {
        if (rd == null) {
            rd = new Random();
        }
        int value = rd.Next(min, max + 1);
        return value;
    }
}