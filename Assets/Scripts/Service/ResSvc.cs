
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

        InitRandNameCfg(PathDefine.RdName);
        InitMapCfg(PathDefine.Map_V1);
        InitGuideCfg(PathDefine.Guide);
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


    #region  初始化配置表 -- 地图配置
    public CfgMapData GetMapData(int mapID) {
        CfgMapData data = null;
        mapDataDic.TryGetValue(mapID, out data);
        return data;
    } 

    private Dictionary<int, CfgMapData> mapDataDic = new Dictionary<int, CfgMapData>();

    private void InitMapCfg(string path)
    {
        TextAsset ta = Resources.Load<TextAsset>(path);
        if (!ta)
        {
            Debug.LogError(path + " 路径错误");
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(ta.text);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            if (ele.GetAttribute("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttribute("ID"));

            CfgMapData mapData = new CfgMapData
            {
                ID = ID,
            };

            foreach (XmlElement subEle in nodeList[i].ChildNodes)
            { 
                string str = subEle.Name;
                switch (str)
                {
                    case "mapName":
                        mapData.mapName = subEle.InnerText;
                        break;
                    case "sceneName":
                        mapData.sceneName = subEle.InnerText;
                        break;
                    case "power":
                        mapData.power = int.Parse(subEle.InnerText);
                        break;
                    case "mainCamPos":
                        string[] pos1 = subEle.InnerText.Split(',');
                        mapData.mainCamPos = new Vector3(float.Parse(pos1[0]), float.Parse(pos1[1]), float.Parse(pos1[2]));
                        break;
                    case "mainCamRote":
                        string[] pos2 = subEle.InnerText.Split(',');
                        mapData.mainCamRote = new Vector3(float.Parse(pos2[0]), float.Parse(pos2[1]), float.Parse(pos2[2]));
                        break;
                    case "playerBornPos":
                        string[] pos3 = subEle.InnerText.Split(','); 
                        mapData.playerBornPos = new Vector3(float.Parse(pos3[0]), float.Parse(pos3[1]), float.Parse(pos3[2]));
                        break;
                    case "playerBornRote":
                        string[] pos4 = subEle.InnerText.Split(','); 
;                        mapData.playerBornRote = new Vector3(float.Parse(pos4[0]), float.Parse(pos4[1]), float.Parse(pos4[2]));
                        break;  
                }
            }
            mapDataDic.Add(ID, mapData);
        }
    }

    #endregion


    #region 初始化配置表 -- 引导配置
    public CfgGuideData GetGuideData(int id) {
        CfgGuideData cfg = null;
        guideDataDic.TryGetValue(id, out cfg);
        return cfg;
    }

    private Dictionary<int, CfgGuideData> guideDataDic = new Dictionary<int, CfgGuideData>();
    private void InitGuideCfg(string path)
    {
        TextAsset ta = Resources.Load<TextAsset>(path);
        if (!ta)
        {
            Debug.LogError(path + " 路径错误");
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(ta.text);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            if (ele.GetAttribute("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttribute("ID"));
            CfgGuideData cfg = new CfgGuideData();
            cfg.ID = ID;

            foreach (XmlElement subEle in nodeList[i].ChildNodes)
            {
                string str = subEle.Name;
                switch (str)
                {
                    case "npcID":
                        cfg.npcID = int.Parse(subEle.InnerText);
                        break;
                    case "dilogArr":
                        cfg.dilogArr = subEle.InnerText;
                        break;
                    case "actID":
                        cfg.actID = int.Parse(subEle.InnerText);
                        break;
                    case "coin":
                        cfg.coin = int.Parse(subEle.InnerText);
                        break;
                    case "exp":
                        cfg.exp = int.Parse(subEle.InnerText);
                        break;
                }
            }

            guideDataDic.Add(ID, cfg);
        }
    }

    #endregion

    #region  初始化配置表 -- 随机名字配置
    public string GetRandName(bool man = true) {
        string name = "";
        name = surNameList[CommonTool.RandomInt(0, surNameList.Count - 1)];
        if (man)
        {
            name += manNameList[CommonTool.RandomInt(0, manNameList.Count - 1)];
        }
        else {
            name += womanNameList[CommonTool.RandomInt(0, womanNameList.Count - 1)];
        }

        return name;
    } 

    private List<string> surNameList = new List<string>();
    private List<string> manNameList = new List<string>();
    private List<string> womanNameList = new List<string>(); 
    private void InitRandNameCfg(string path) {
        TextAsset ta = Resources.Load<TextAsset>(path);
        if (!ta) {
            Debug.LogError(path + " 路径错误");
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