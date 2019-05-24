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

    //服务层
    public ResSvc mResSvc;
    public AudioSvc mAudioSvc;
    public NetSvc mNetSvc;

    //业务层
    public LoginSys mLoginSys;


    public Transform mUIRootTr;
    public LoadingWnd mLoadingWnd;
    public LoginWnd mLoginWnd;
    public DynamicTipsWnd mDynamicTipsWnd;
    public CreateWnd mCreateWnd;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        Init();
    }

    private void Init() {
        mUIRootTr = transform.Find("UIRoot");
        mLoadingWnd = transform.Find("UIRoot/LoadingWnd").GetComponent<LoadingWnd>();
        mLoginWnd = transform.Find("UIRoot/LoginWnd").GetComponent<LoginWnd>();
        mDynamicTipsWnd = transform.Find("UIRoot/DynamicTips").GetComponent<DynamicTipsWnd>();
        mCreateWnd = transform.Find("UIRoot/CreateWnd").GetComponent<CreateWnd>();

        mResSvc = GetComponent<ResSvc>();
        mResSvc.Init(); 
        mAudioSvc = GetComponent<AudioSvc>();
        mAudioSvc.Init();
        mNetSvc = GetComponent<NetSvc>();
        mNetSvc.Init();

        mLoginSys = GetComponent<LoginSys>();
        mLoginSys.Init();
        InitUIRoot();

        mLoginSys.EnterLogin();
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