/****************************************************
    文件：GameRoot.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 14:37:24
	功能：游戏管理
*****************************************************/

using UnityEngine;

public class GameRoot : MonoBehaviour 
{
    public static GameRoot Instance;

    public LoginSys mLoginSys;
    public ResSvc mResSvc;

    public LoadingWnd mLoadingWnd;
    public LoginWnd mLoginWnd;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        Init();
    }

    private void Init() {
        mLoadingWnd = transform.Find("UIRoot/LoadingWnd").GetComponent<LoadingWnd>();
        mLoginWnd = transform.Find("UIRoot/LoginWnd").GetComponent<LoginWnd>();

        mResSvc = GetComponent<ResSvc>();
        mResSvc.Init();
        mLoginSys = GetComponent<LoginSys>();
        mLoginSys.Init(); 

        mLoginSys.EnterLogin();
    }

}