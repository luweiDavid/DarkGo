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
    private CfgSvc mCfgSvc;
    private ResSvc mResSvc;
    private AudioSvc mAudioSvc;
    private NetSvc mNetSvc;
    private TimerSvc mTimerSvc;

    //业务层
    private LoginSys mLoginSys;
    private MainCitySys mMainCitySys;
    private FuBenSys mFuBenSys;
    private BattleSys mBattleSys;


    #region    //UI层
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
    [HideInInspector]
    public GuideWnd mGuideWnd;
    [HideInInspector]
    public StrongWnd mStrongWnd;
    [HideInInspector]
    public ChatWnd mChatWnd; 
    [HideInInspector]
    public CommonBuyWnd mCommonBuyWnd;
    [HideInInspector]
    public FuBenWnd mFuBenWnd;
    #endregion

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
        mGuideWnd = transform.Find("UIRoot/GuideWnd").GetComponent<GuideWnd>();
        mStrongWnd = transform.Find("UIRoot/StrongWnd").GetComponent<StrongWnd>();
        mChatWnd = transform.Find("UIRoot/ChatWnd").GetComponent<ChatWnd>();
        mCommonBuyWnd = transform.Find("UIRoot/CommonBuyWnd").GetComponent<CommonBuyWnd>();
        mFuBenWnd = transform.Find("UIRoot/FuBenWnd").GetComponent<FuBenWnd>();
         
        #endregion

        #region  服务层初始化
        mCfgSvc = GetComponent<CfgSvc>();
        mCfgSvc.Init();
        mResSvc = GetComponent<ResSvc>();
        mResSvc.Init(); 
        mAudioSvc = GetComponent<AudioSvc>();
        mAudioSvc.Init();
        mNetSvc = GetComponent<NetSvc>();
        mNetSvc.Init();
        mTimerSvc = GetComponent<TimerSvc>();
        mTimerSvc.Init();
        #endregion

        #region  业务层初始化
        mLoginSys = GetComponent<LoginSys>();
        mLoginSys.Init();
        mMainCitySys = GetComponent<MainCitySys>();
        mMainCitySys.Init();
        mFuBenSys = GetComponent<FuBenSys>();
        mFuBenSys.Init();
        mBattleSys = GetComponent<BattleSys>();
        mBattleSys.Init();
        #endregion

        InitUIRoot(); 
        mLoginSys.EnterLogin();
         
    }

    public PlayerData GetPlayerData() {
        return mPlayerData;
    }

    public void SetPlayerData(PlayerData data) { 
        mPlayerData = data;

        MainCitySys.Instance.UpdateMainWnd(data);
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