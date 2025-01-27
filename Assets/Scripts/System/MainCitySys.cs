/****************************************************
    文件：MainCitySys.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/27 10:59:41
	功能：主城业务系统
*****************************************************/

using Protocol;
using UnityEngine;
using UnityEngine.AI;

public class MainCitySys : SystemRoot<MainCitySys> {
    public PlayerController playerCtrl;
    private Transform playerCamTr;

    #region 引导相关属性
    private CfgGuide curGuideData;
    private NavMeshAgent navAgent; 
    private Transform[] npcPosTrArray;
    private bool isNavigation = false;
    #endregion
     

    public void EnterMainCity() {
        CfgMap mapData = mCfgSvc.GetCfgMap(Constant.Scene_MainCityID);
        mResSvc.AsyncLoadScene(Constant.Scene_MainCity, () =>
        {
            //加载角色
            LoadPlayer(mapData);
            //打开主面板
            mGameRoot.mMainWnd.SetWndState(true);

            //设置info摄像机 
            if (playerCamTr != null) {
                playerCamTr.gameObject.SetActive(false);
            }
            //播放主城背景音乐
            mAudioSvc.PlayBgAudio(Constant.Audio_BgMainCity, true);
             
            npcPosTrArray = GameObject.FindGameObjectWithTag("NpcPos").GetComponent<MainCityNpcPos>().npcPosTrArray;
        }); 
    }


    
    private void LoadPlayer(CfgMap cfg)
    {
        if (cfg == null) {
            Debug.LogError("地图配置信息为空");
            return;
        } 
        GameObject cityPlayerGo = mResSvc.GetInstantiateGo(PathDefine.PlayerAssassinCity);
        if (cityPlayerGo != null) { 
            cityPlayerGo.transform.position = cfg.playerBornPos;
            cityPlayerGo.transform.localEulerAngles = cfg.playerBornRote;

            Camera.main.transform.position = cfg.mainCamPos;
            Camera.main.transform.localEulerAngles = cfg.mainCamRote;
            cityPlayerGo.transform.localScale = new Vector3(1f, 1f, 1f);

            playerCtrl = cityPlayerGo.GetComponent<PlayerController>();
            playerCtrl.Init();
            navAgent = cityPlayerGo.GetComponent<NavMeshAgent>();
        }
    }

    public void SetPlayerMove(Vector3 dir) {
        //用摇杆控制角色移动时，必须终止navAgent导航
        StopNavigation();
        playerCtrl.MoveDir = dir;
    }

    public void SetPlayerCam() {
        if (playerCamTr == null) {
            playerCamTr = GameObject.FindGameObjectWithTag("PlayerCamera").transform;
        }

        playerCamTr.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 1.80423f
            + new Vector3(0, 0.81f, 0);
        playerCamTr.localEulerAngles = new Vector3(0, 180 + playerCtrl.transform.localEulerAngles.y, 0);
        playerCamTr.localScale = Vector3.one;
        playerCamTr.gameObject.SetActive(true);
    }

    public void SetPlayerCamDisable() {
        if (playerCamTr != null)
        {
            playerCamTr.gameObject.SetActive(false);
        }
    }
    private float startRot_Y;
    public void SetPlayerStartRotate() {
        startRot_Y = playerCtrl.transform.localEulerAngles.y;
    }

    public void SetPlayerRotate(float angle) {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRot_Y + angle, 0);
    }


    #region  引导相关 
    public void StartGuideTask(CfgGuide data) {
        if (data != null)
        {
            curGuideData = data;

            //有这样一种情况：就是点击自动任务按钮时，角色已经在目的地了，这时就会走dis<0.5f的逻辑
            //因为一开始navAgent是被禁用的，所以需要打开，不然在调用isStopped属性时就会报错
            navAgent.enabled = true;
            if (data.npcID != -1)
            {
                //有目标npc时候，要启用导航组件进行寻路
                float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrArray[data.npcID].position);
                if (dis < 0.5f)
                {
                    isNavigation = false;
                    navAgent.isStopped = true;
                    navAgent.enabled = false;
                    playerCtrl.SetBlend(Constant.IdleBlend);
                    OpenGuideWnd();
                }
                else
                {
                    isNavigation = true;
                    navAgent.enabled = true;
                    navAgent.speed = Constant.PlayerMoveSpeed;
                    navAgent.SetDestination(npcPosTrArray[data.npcID].position);
                    playerCtrl.SetBlend(Constant.MoveBlend);
                }
            }
            else
            {
                OpenGuideWnd();
            }
        }
        else {
            mGameRoot.AddTips(Language.GetString(15));
        }
    }

    public void StopNavigation() { 
        if (navAgent.enabled)
        {
            isNavigation = false;
            navAgent.isStopped = true;
            navAgent.enabled = false; 
            playerCtrl.SetBlend(Constant.IdleBlend);
        } 
    }

    private void Update()
    {
        if (isNavigation) {
            playerCtrl.SetMainCamFollowPlayer(); 
            float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrArray[curGuideData.npcID].position);
            if (dis < 0.5f)
            {
                StopNavigation();
                OpenGuideWnd();
            }
        }
    } 


    private void ExecuteGuideTask()
    { 
        int actId = curGuideData.actID;
        switch (actId)
        {
            case 0:
                //与智者对话
                mGameRoot.AddTips(string.Format(Language.GetString(14), curGuideData.coin, curGuideData.exp));
                break;
            case 1: 
                OpenCommonBuyWnd(CommonBuyType.Coin);
                break;
            case 2:
                OpenCommonBuyWnd(CommonBuyType.Power);
                break;
            case 3:
                OpenStrongWnd();
                break;
            case 4:
                FuBenSys.Instance.OpenFuBenWnd();
                break;
            case 5:
                break; 
        }
    }


    #endregion


    #region 面板操作
    private void OpenGuideWnd()
    {
        mGameRoot.mGuideWnd.SetWndState(true);
    }

    private void OpenCommonBuyWnd(CommonBuyType _type) {
        mGameRoot.mCommonBuyWnd.SetWndState(true, new object[1] { _type });
    }

    private void OpenStrongWnd()
    {
        mGameRoot.mStrongWnd.SetWndState();
    }

    public void UpdateMainWnd(PlayerData data) {
        mGameRoot.mMainWnd.UpdateData(data);
    }

    public void UpdateStrongWnd(PlayerData data) {
        mGameRoot.mStrongWnd.UpdateData(data);
    }

    public void AddChatMsg(string str) {
        mGameRoot.mChatWnd.AddChatMsg(str);
    }   

    #endregion











    #region  网络请求相关
    public void ReqGuide(int id) {
        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.ReqGuide,
        };
        newMsg.ReqGuide = new ReqGuide
        {
            guideId = id,
        };

        mNetSvc.SendMsg(newMsg);
        Debug.Log("ReqGuide"); 
    }

    public void RspGuide(NetMsg msg) {
        RspGuide rspData = msg.RspGuide;
        
        mGameRoot.SetPlayerData(rspData.data); 

        //执行引导任务
        ExecuteGuideTask();
    }


    public void ReqStrong(int _pos)
    {
        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.ReqStrong,
        };
        newMsg.ReqStrong = new ReqStrong
        {
            pos = _pos,
        };

        mNetSvc.SendMsg(newMsg);
        Debug.Log("ReqStrong");
    }

    public void RspStrong(NetMsg msg)
    {
        int preFight = PECommonTool.GetFight(mGameRoot.GetPlayerData()); 
        RspStrong rspData = msg.RspStrong; 
        mGameRoot.SetPlayerData(rspData.data);
        int nextFight = PECommonTool.GetFight(rspData.data);

        //更新强化面板
        UpdateStrongWnd(rspData.data);

        mGameRoot.AddTips("<color=#FF0000>升星成功</color>, 战力提升：" + (nextFight - preFight));
    }

    public void SendChatMsg(string _str)
    {
        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.SendChatMsg,
        };
        newMsg.SendChatMsg = new SendChatMsg
        {
            str = _str,
        };

        mNetSvc.SendMsg(newMsg);
        Debug.Log("SendChatMsg  : " + _str);
    }

    public void PushChatMsg(NetMsg msg) {
        PushChatMsg rspData = msg.PushChatMsg;

       string chatStr = string.Format(Language.GetString(120), rspData.name, rspData.str);

        AddChatMsg(chatStr);
    }
     

    public void ReqBuy(int _buyType)
    {
        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.ReqBuy,
            ReqBuy = new ReqBuy
            {
                buyType = _buyType,
            },
        };

        mNetSvc.SendMsg(newMsg);
        Debug.Log("ReqBuy");
    }

    public void RspBuy(NetMsg msg) {
        RspBuy rspData = msg.RspBuy;
        mGameRoot.SetPlayerData(rspData.data);

        switch (rspData.buyType)
        {
            case (int)CommonBuyType.Coin:
                mGameRoot.AddTips(Language.GetString(50));
                break;
            case (int)CommonBuyType.Power:
                mGameRoot.AddTips(Language.GetString(49));
                break;
        }
    }

    public void NtfPowerChg(NetMsg msg) {
        NtfPowerChg rspData = msg.NtfPowerChg;
        PlayerData pre = mGameRoot.GetPlayerData(); 

        mGameRoot.SetPlayerData(rspData.data);

        mGameRoot.AddTips("增加体力： " + (rspData.data.Power - pre.Power));
    }

    #endregion


}