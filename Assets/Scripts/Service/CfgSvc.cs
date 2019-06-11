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
        Dictionary<int, CfgStrong> cfgDic = null;
        if (strongCfgDic.TryGetValue(pos, out cfgDic)) {
            for (int i = 1; i <= starLv; i++)
            {
                if (!cfgDic.ContainsKey(i)) {
                    continue;
                }
                switch (proType)
                {
                    case (int)PropertyType.Hp:
                        _value += cfgDic[i].addHp;
                        break;
                    case (int)PropertyType.Hurt:
                        _value += cfgDic[i].addHurt;
                        break;
                    case (int)PropertyType.Def:
                        _value += cfgDic[i].addDef;
                        break;
                }
            }
        }
        return _value;
    }

    public CfgStrong GetCfgStrong(int pos,int starLv) {
        CfgStrong data = null;
        Dictionary<int, CfgStrong> dataDic = null;
        if(strongCfgDic.TryGetValue(pos,out dataDic))
        {
            if (dataDic != null && dataDic.TryGetValue(starLv, out data)) {
                return data;
            } 
        }
        return null;
    }

    private Dictionary<int, Dictionary<int, CfgStrong>> strongCfgDic = new Dictionary<int, Dictionary<int, CfgStrong>>();
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
        Dictionary<int, CfgStrong> cfgDic = null;
        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            if (ele.GetAttribute("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttribute("ID"));
            CfgStrong cfg = new CfgStrong
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
            
            if (strongCfgDic.ContainsKey(cfg.pos))
            {
                cfgDic.Add(cfg.starLv, cfg);
            }
            else {
                cfgDic = new Dictionary<int, CfgStrong>();
                cfgDic.Add(cfg.starLv, cfg);
                strongCfgDic.Add(cfg.pos, cfgDic);
            } 
        }
    }

    #endregion

    #region  初始化配置表 -- 地图配置
    public CfgMap GetCfgMap(int mapID)
    {
        CfgMap cfg = null;
        mapCfgDic.TryGetValue(mapID, out cfg);
        return cfg;
    }

    private Dictionary<int, CfgMap> mapCfgDic = new Dictionary<int, CfgMap>();

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

            CfgMap cfg = new CfgMap
            {
                ID = ID,
            };

            foreach (XmlElement subEle in nodeList[i].ChildNodes)
            {
                string str = subEle.Name;
                switch (str)
                {
                    case "mapName":
                        cfg.mapName = subEle.InnerText;
                        break;
                    case "sceneName":
                        cfg.sceneName = subEle.InnerText;
                        break;
                    case "power":
                        cfg.power = int.Parse(subEle.InnerText);
                        break;
                    case "mainCamPos":
                        string[] pos1 = subEle.InnerText.Split(',');
                        cfg.mainCamPos = new Vector3(float.Parse(pos1[0]), float.Parse(pos1[1]), float.Parse(pos1[2]));
                        break;
                    case "mainCamRote":
                        string[] pos2 = subEle.InnerText.Split(',');
                        cfg.mainCamRote = new Vector3(float.Parse(pos2[0]), float.Parse(pos2[1]), float.Parse(pos2[2]));
                        break;
                    case "playerBornPos":
                        string[] pos3 = subEle.InnerText.Split(',');
                        cfg.playerBornPos = new Vector3(float.Parse(pos3[0]), float.Parse(pos3[1]), float.Parse(pos3[2]));
                        break;
                    case "playerBornRote":
                        string[] pos4 = subEle.InnerText.Split(',');
                        cfg.playerBornRote = new Vector3(float.Parse(pos4[0]), float.Parse(pos4[1]), float.Parse(pos4[2]));
                        break;
                }
            }
            mapCfgDic.Add(ID, cfg);
        }
    }

    #endregion


    #region 初始化配置表 -- 引导配置
    public CfgGuide GetCfgGuide(int id)
    {
        CfgGuide cfg = null;
        guideCfgDic.TryGetValue(id, out cfg);
        return cfg;
    }

    private Dictionary<int, CfgGuide> guideCfgDic = new Dictionary<int, CfgGuide>();
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
            CfgGuide cfg = new CfgGuide();
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

            guideCfgDic.Add(ID, cfg);
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