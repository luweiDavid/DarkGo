/****************************************************
    文件：CfgSvc.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/6/1 13:59:17
	功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CfgSvc : ServiceRoot<CfgSvc>
{

    public override void Init()
    {
        base.Init();
        InitRandNameCfg(PathDefine.RdName);
        InitMapCfg(PathDefine.Map_V1);
        InitGuideCfg(PathDefine.Guide);
        InitStrongCfg(PathDefine.Strong);
    }


    #region  初始化配置表-- 强化系统配置  
    /// <summary>
    /// 获取对应位置的前starLv等级(包括starLv)的属性加成总和
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="starLv"></param>
    /// <param name="proType">属性类型</param>
    /// <returns></returns>
    public int GetStrongProTotalAddition(int pos, int starLv,int proType) { 
        int _value = 0;
        Dictionary<int, CfgStrongData> dataDic = null;
        if (strongDataDic.TryGetValue(pos, out dataDic)) {
            for (int i = 1; i <= starLv; i++)
            {
                if (!dataDic.ContainsKey(i)) {
                    continue;
                }
                switch (proType)
                {
                    case (int)PropertyType.Hp:
                        _value += dataDic[i].addHp;
                        break;
                    case (int)PropertyType.Hurt:
                        _value += dataDic[i].addHurt;
                        break;
                    case (int)PropertyType.Def:
                        _value += dataDic[i].addDef;
                        break;
                }
            }
        }
        return _value;
    }

    public CfgStrongData GetStrongData(int pos,int starLv) {
        CfgStrongData data = null;
        Dictionary<int, CfgStrongData> dataDic = null;
        if(strongDataDic.TryGetValue(pos,out dataDic))
        {
            if (dataDic != null && dataDic.TryGetValue(starLv, out data)) {
                return data;
            } 
        }
        return null;
    }

    private Dictionary<int, Dictionary<int, CfgStrongData>> strongDataDic = new Dictionary<int, Dictionary<int, CfgStrongData>>();
    private void InitStrongCfg(string path)
    {
        TextAsset ta = Resources.Load<TextAsset>(path);
        if (!ta)
        {
            Debug.LogError(path + " 路径错误");
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(ta.text);

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
        Dictionary<int, CfgStrongData> dataDic = null;
        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            if (ele.GetAttribute("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttribute("ID"));
            CfgStrongData cfg = new CfgStrongData
            {
                ID = ID,
            };
             
            foreach (XmlElement subEle in nodeList[i].ChildNodes)
            {
                string str = subEle.Name;
                switch (str)
                {
                    case "pos":
                        cfg.pos = int.Parse(subEle.InnerText);
                        break;
                    case "starlv":
                        cfg.starLv = int.Parse(subEle.InnerText);
                        break;
                    case "addhp":
                        cfg.addHp = int.Parse(subEle.InnerText);
                        break;
                    case "addhurt":
                        cfg.addHurt = int.Parse(subEle.InnerText);
                        break;
                    case "adddef":
                        cfg.addDef = int.Parse(subEle.InnerText);
                        break;
                    case "minlv":
                        cfg.minLv = int.Parse(subEle.InnerText);
                        break;
                    case "coin":
                        cfg.coin = int.Parse(subEle.InnerText);
                        break;
                    case "crystal":
                        cfg.crystal = int.Parse(subEle.InnerText);
                        break; 
                }
            }
            
            if (strongDataDic.ContainsKey(cfg.pos))
            {
                dataDic.Add(cfg.starLv, cfg);
            }
            else {
                dataDic = new Dictionary<int, CfgStrongData>();
                dataDic.Add(cfg.starLv, cfg);
                strongDataDic.Add(cfg.pos, dataDic);
            } 
        }
    }

    #endregion

    #region  初始化配置表 -- 地图配置
    public CfgMapData GetMapData(int mapID)
    {
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
                        ; mapData.playerBornRote = new Vector3(float.Parse(pos4[0]), float.Parse(pos4[1]), float.Parse(pos4[2]));
                        break;
                }
            }
            mapDataDic.Add(ID, mapData);
        }
    }

    #endregion


    #region 初始化配置表 -- 引导配置
    public CfgGuideData GetGuideData(int id)
    {
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
    public string GetRandName(bool man = true)
    {
        string name = "";
        name = surNameList[CommonTool.RandomInt(0, surNameList.Count - 1)];
        if (man)
        {
            name += manNameList[CommonTool.RandomInt(0, manNameList.Count - 1)];
        }
        else
        {
            name += womanNameList[CommonTool.RandomInt(0, womanNameList.Count - 1)];
        }

        return name;
    }

    private List<string> surNameList = new List<string>();
    private List<string> manNameList = new List<string>();
    private List<string> womanNameList = new List<string>();
    private void InitRandNameCfg(string path)
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
            foreach (XmlElement subEle in nodeList[i].ChildNodes)
            {
                string str = subEle.Name;
                switch (str)
                {
                    case "surname":
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