/****************************************************
    文件：ServerRoot.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/6/1 14:0:21
	功能：Nothing
*****************************************************/

using UnityEngine;

public class ServiceRoot<T> : MonoBehaviour where T:MonoBehaviour
{
    public static T Instance;

    public virtual void Init() {
        Instance = this as T;
    }
}