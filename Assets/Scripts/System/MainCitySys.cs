/****************************************************
    文件：MainCitySys.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/27 10:59:41
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.AI;

public class MainCitySys : SystemRoot
{
    public static MainCitySys Instance;

    public PlayerController playerCtrl;
    private Transform playerCamTr;

    private CfgGuideData curGuideData;
    private NavMeshAgent navAgent;

    private Transform[] npcPosTrArray;
 
    public override void Init()
    {
        base.Init();

        Instance = this;
    }

    public void EnterMainCity() {
        CfgMapData mapData = mResSvc.GetMapData(Constant.Scene_MainCityID);
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


    
    private void LoadPlayer(CfgMapData data)
    {
        if (data == null) {
            Debug.LogError("地图配置信息为空");
            return;
        } 
        GameObject cityPlayerGo = mResSvc.GetInstantiateGo(PathDefine.PlayerAssassinCity);
        if (cityPlayerGo != null) { 
            cityPlayerGo.transform.position = data.playerBornPos;
            cityPlayerGo.transform.localEulerAngles = data.playerBornRote;

            Camera.main.transform.position = data.mainCamPos;
            Camera.main.transform.localEulerAngles = data.mainCamRote;
            cityPlayerGo.transform.localScale = new Vector3(1f, 1f, 1f);

            playerCtrl = cityPlayerGo.GetComponent<PlayerController>();
            playerCtrl.Init();
            navAgent = cityPlayerGo.GetComponent<NavMeshAgent>();
        }
    }

    public void SetPlayerMove(Vector3 dir) { 
        playerCtrl.MoveDir = dir;
    }

    public void SetPlayerCam() {
        if (playerCamTr == null) {
            playerCamTr = GameObject.FindGameObjectWithTag("PlayerCamera").transform;
        }

        playerCamTr.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 3.8f
            + new Vector3(0, 1.2f, 0);
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
    private bool isNavigation = false;
    public void ExecuteGuideTask(CfgGuideData data) {
        if (data!=null){
            curGuideData = data;

            if (data.npcID != -1)
            {
                //有目标npc时候，要启用导航组件进行寻路
                float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrArray[data.npcID].position);
                if (dis < 0.5f)
                { 
                    isNavigation = false;
                    navAgent.enabled = false;
                    navAgent.isStopped = true; 
                    playerCtrl.SetBlend(Constant.IdleBlend);
                    OpenGuideWnd();
                }
                else {
                    isNavigation = true;
                    navAgent.enabled = true;
                    navAgent.speed = Constant.PlayerMoveSpeed; 
                    navAgent.SetDestination(npcPosTrArray[data.npcID].position);
                    playerCtrl.SetBlend(Constant.MoveBlend);
                }
            }
            else {
                OpenGuideWnd();
            }
        }
    }

    private void Update()
    {
        if (isNavigation) {
            playerCtrl.SetMainCamFollowPlayer();
        }
    }

    private void OpenGuideWnd() {


    }
    #endregion




}