/****************************************************
    文件：Constant.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 14:46:42
	功能：公共模块，定义常量
*****************************************************/

public class Constant  
{
    //场景名称
    public const string Scene_Login = "Login";
    public const string Scene_GameRoot = "GameRoot";
    public const string Scene_MainCity = "MainCity";

    //场景id
    public const int Scene_MainCityID = 10000;

    //音乐名称
    public const string Audio_AssassinHit = "assassin_Hit";
    public const string Audio_BgHuangYe = "bgHuangYe"; 
    public const string Audio_BgLogin = "bgLogin"; 
    public const string Audio_BgMainCity = "bgMainCity";
    public const string Audio_FbItem = "fbitem";
    public const string Audio_FbClose = "fblose";
    public const string Audio_FbWin = "fbwin";  
    public const string Audio_BtnClick = "uiClickBtn";
    public const string Audio_BtnClose = "uiCloseBtn";
    public const string Audio_BtnExten = "uiExtenBtn";
    public const string Audio_BtnLogin = "uiLoginBtn";
    public const string Audio_OpenPage = "uiOpenPage";


    public const int RefScreenWidth = 1334;
    public const int RefScreenHeight = 750;
    public const int RockingBarDis = 90;

    public const float PlayerMoveSpeed = 6;
    public const float MonsterMoveSpeed = 3;


    public const int MoveBlend = 1;
    public const int IdleBlend = 0;
    public const float AccelerSpeed = 4;
}


public enum GuideNpcIDType {
    NpcWiseman = 0,
    NpcGeneral,
    NpcArtisan,
    NpcTrader,
}