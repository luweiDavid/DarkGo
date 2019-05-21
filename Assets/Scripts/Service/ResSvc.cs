
/****************************************************
    文件：ResSys.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 14:38:10
	功能：服务层系统 -- 资源服务
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour 
{
    public static ResSvc Instance;

    private Action pregressCB = null;

    public void Init()
    {
        Instance = this;
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="loadedCB">加载场景完成的回调</param>
    public void AsyncLoadScene(string name, Action loadedCB) {
        GameRoot.Instance.mLoadingWnd.SetWndState(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);

        pregressCB = () => {
            GameRoot.Instance.mLoadingWnd.SetProgress(operation.progress);
            if (operation.progress >= 1)
            {
                if (loadedCB != null) {
                    loadedCB();
                }
                GameRoot.Instance.mLoadingWnd.SetWndState(false);
                pregressCB = null;
                operation = null;
            }  
        };
    }

    private void Update()
    {
        if (pregressCB != null) {
            pregressCB();
        }
    }

}