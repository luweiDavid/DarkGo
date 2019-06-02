
/****************************************************
    文件：ResSys.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 14:38:10
	功能：服务层系统 -- 资源服务
*****************************************************/

using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : ServiceRoot<ResSvc> 
{ 
    private Action pregressCB = null; 

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
    #region 资源加载 --- prefab
    private Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
    public GameObject GetInstantiateGo(string path, bool isCache = false) {
        GameObject prefab = null;
        if (!prefabDic.TryGetValue(path, out prefab)) {
            prefab = Resources.Load<GameObject>(path);
            if (prefab != null) {
                if (isCache)
                    prefabDic.Add(path, prefab);
            }
        }

        GameObject go = null;
        if (prefab != null)
        {
            go = Instantiate(prefab);
        }
        else {
            Debug.LogError("prefab资源加载错误 ： " + path);
        }

        return go;
    }

    #endregion


    #region  资源加载 -- 图片
    private Dictionary<string, Sprite> cachedSpriteDic = new Dictionary<string, Sprite>();
    public Sprite GetSprite(string path, bool isCache = false) {
        Sprite spt = null;
        if (!cachedSpriteDic.TryGetValue(path, out spt)) {
            spt = Resources.Load<Sprite>(path);
            if (spt != null)
            {
                if(isCache)
                    cachedSpriteDic.Add(path, spt);
            }
            else {
                Debug.LogError("图片资源加载错误：" + path);
            }
        }
        
        return spt;
    }
    #endregion


    #region 资源加载 --- 音效
    private Dictionary<string, AudioClip> cachedClipDic = new Dictionary<string, AudioClip>();
    public AudioClip GetAudio(string path, bool isCache = false) {
        AudioClip clip = null;
        if (!cachedClipDic.TryGetValue(path, out clip)) {
            clip = Resources.Load<AudioClip>(path);
            if (clip != null)
            {
                if (isCache)
                    cachedClipDic.Add(path, clip);
            }
            else {
                Debug.LogError("音效资源加载错误：" + path);
            }
    }
        return clip;
    }
    #endregion


   



}