using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSys : SystemRoot<BattleSys>
{

    

    public void InitBattle(int mapId)
    {
        base.Init();

        GameObject go = new GameObject
        {
            name = "BattleRoot",
        };

        go.transform.SetParent(mGameRoot.transform);

        BattleMgr bMgr = go.AddComponent<BattleMgr>();
        bMgr.Init(mapId);
    }


    public void OpenPlayerCtrlWnd() {
        mGameRoot.mPlayerCtrlWnd.SetWndState();
    }

    public void SetPlayerDir(Vector2 dir) {

    }

}
