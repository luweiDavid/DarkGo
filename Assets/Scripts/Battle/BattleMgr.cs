using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : MgrRoot<BattleMgr>
{

    private ResSvc mResSvc;
    private AudioSvc mAudioSvc;
    private CfgSvc mCfgSvc;

    
    private SkillMgr mSkillMgr;
    private StateMgr mStateMgr;
    private MapMgr mMapMgr;


    public void Init(int mapId) {
        mResSvc = ResSvc.Instance;
        mAudioSvc = AudioSvc.Instance;
        mCfgSvc = CfgSvc.Instance;

        mSkillMgr = gameObject.AddComponent<SkillMgr>();
        mSkillMgr.Init();
        mStateMgr = gameObject.AddComponent<StateMgr>();
        mStateMgr.Init();


        mAudioSvc.PlayBgAudio(Constant.Audio_BgHuangYe);

        CfgMap cfg = mCfgSvc.GetCfgMap(mapId);
        if (cfg == null) {
            Debug.LogError("¼ÓÔØµØÍ¼Ê§°Ü:  " + mapId);
            return;
        } 

        mResSvc.AsyncLoadScene(cfg.sceneName, () =>
        {
            GameObject mapGo = GameObject.FindGameObjectWithTag("MapRoot");

            mMapMgr = mapGo.AddComponent<MapMgr>();
            mMapMgr.Init();
            InitScene(cfg);
        });
         
    }

    private void InitScene(CfgMap cfg) {
        Camera.main.transform.position = cfg.mainCamPos;
        Camera.main.transform.eulerAngles = cfg.mainCamRote;
        
        GameObject playerGo = mResSvc.GetInstantiateGo(PathDefine.PlayerAssassin);
        playerGo.transform.position = cfg.playerBornPos;
        playerGo.transform.eulerAngles = cfg.playerBornRote; 

    }

}
