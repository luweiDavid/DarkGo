/****************************************************
    文件：CfgDataBase.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/28 18:12:34
	功能：配置文件信息基类
*****************************************************/

using UnityEngine;

public class BaseCfg<T>
{
    public int ID;
}

public class CfgStrong : BaseCfg<CfgStrong>
{
    public int pos;
    public int starLv;
    public int addHp;
    public int addHurt;
    public int addDef;
    public int minLv;
    public int coin;
    public int crystal;

}

public class CfgMap : BaseCfg<CfgMap> {

    public string mapName;
    public string sceneName;
    public int power;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;

}


public class CfgGuide : BaseCfg<CfgGuide>
{
    public int npcID;
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;

}

















