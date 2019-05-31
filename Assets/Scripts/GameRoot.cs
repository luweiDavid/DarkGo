/****************************************************
    文件：GameRoot.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 14:37:24
	功能：游戏管理
*****************************************************/

using Protocol;
using UnityEngine;

public class GameRoot : MonoBehaviour 
{
    public static GameRoot Instance;

    //服务层
    private ResSvc mResSvc;
    private AudioSvc mAudioSvc;
    private NetSvc mNetSvc;

    //业务层
    private LoginSys mLoginSys;
    private MainCitySys mMainCitySys;
     
    //UI层
    [HideInInspector]
    public Transform mUIRootTr;
    [HideInInspector]
    public LoadingWnd mLoadingWnd;
    [HideInInspector]
    public LoginWnd mLoginWnd;
    [HideInInspector]
    public DynamicTipsWnd mDynamicTipsWnd;
    [HideInInspector]
    public CreateWnd mCreateWnd;
    [HideInInspector]
    public MainWnd mMainWnd;
    [HideInInspector]
    public ActorInfoWnd mActorInfoWnd;


    //数据层
    private PlayerData mPlayerData;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        Init();
    }

    private void Init() {
        #region  所有UI界面的获取， 本项目暂时没有做资源加载的框架
        mUIRootTr = transform.Find("UIRoot");
        mLoadingWnd = transform.Find("UIRoot/LoadingWnd").GetComponent<LoadingWnd>();
        mLoginWnd = transform.Find("UIRoot/LoginWnd").GetComponent<LoginWnd>();
        mDynamicTipsWnd = transform.Find("UIRoot/DynamicTips").GetComponent<DynamicTipsWnd>();
        mCreateWnd = transform.Find("UIRoot/CreateWnd").GetComponent<CreateWnd>();
        mMainWnd = transform.Find("UIRoot/MainWnd").GetComponent<MainWnd>();
        mActorInfoWnd = transform.Find("UIRoot/ActorInfoWnd").GetComponent<ActorInfoWnd>();
        #endregion

        #region  服务层初始化
        mResSvc = GetComponent<ResSvc>();
        mResSvc.Init(); 
        mAudioSvc = GetComponent<AudioSvc>();
        mAudioSvc.Init();
        mNetSvc = GetComponent<NetSvc>();
        mNetSvc.Init();
        #endregion

        #region  业务层初始化
        mLoginSys = GetComponent<LoginSys>();
        mLoginSys.Init();
        mMainCitySys = GetComponent<MainCitySys>();
        mMainCitySys.Init();

        #endregion

        InitUIRoot(); 
        mLoginSys.EnterLogin();
         
    }

    public PlayerData GetPlayerData() {
        return mPlayerData;
    }

    public void SetPlayerData(PlayerData data) { 
        mPlayerData = data;
    }


    public void InitUIRoot() {
        for (int i = 0; i < mUIRootTr.childCount; i++)
        {
            mUIRootTr.GetChild(i).gameObject.SetActive(false);
        }

        mDynamicTipsWnd.SetWndState();
    }

    public void AddTips(string tips) {
        mDynamicTipsWnd.AddTips(tips);
    }

}