
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

public class ResSvc : MonoBehaviour 
{
    public static ResSvc Instance;

    private Action pregressCB = null;

    public void Init()
    {
        Instance = this;

        InitRandNameCfg(); 
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

    private Dictionary<string, AudioClip> cachedClipDic = new Dictionary<string, AudioClip>();
    public AudioClip GetAudio(string path, bool isCache = false) {
        AudioClip clip = null;
        if (!cachedClipDic.TryGetValue(path, out clip)) {
            clip = Resources.Load<AudioClip>(path);
            if (clip != null) {
                cachedClipDic.Add(path, clip);
            } 
        }
        return clip;
    }








    #region  读取配置表
    public string GetRandName(bool man = true) {
        string name = "";
        name = surNameList[PETools.RandomInt(0, surNameList.Count - 1)];
        if (man)
        {
            name += manNameList[PETools.RandomInt(0, manNameList.Count - 1)];
        }
        else {
            name += womanNameList[PETools.RandomInt(0, womanNameList.Count - 1)];
        }

        return name;
    }



    private List<string> surNameList = new List<string>();
    private List<string> manNameList = new List<string>();
    private List<string> womanNameList = new List<string>(); 
    private void InitRandNameCfg() {
        TextAsset ta = Resources.Load<TextAsset>(PathDefine.RdName);
        if (!ta) {
            Debug.LogError(PathDefine.RdName + " 路径错误");
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(ta.text);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            if (ele.GetAttribute("ID") == null) {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttribute("ID"));
            foreach (XmlElement subEle in nodeList[i].ChildNodes)
            {
                string str = subEle.Name;
                switch (str)
                { 
                    case "surname" :
                        surNameList.Add(subEle.InnerText);
                        break;
                    case "man":
                        manNameList.Add(subEle.InnerText);
                        break;
                    case "woman":
                        womanNameList.Add(subEle.InnerText);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    #endregion



}