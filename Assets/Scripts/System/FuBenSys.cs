
using Protocol;
using UnityEngine;


public class FuBenSys : SystemRoot<FuBenSys>
{



    public void OpenFuBenWnd() {
        mGameRoot.mFuBenWnd.SetWndState();
    }

    public void ReqFuBen(int fbid)
    {
        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.ReqFuBen,
        };
        newMsg.ReqFuBen = new ReqFuBen
        {
            fubenID = fbid,
        };

        mNetSvc.SendMsg(newMsg);
        Debug.Log("ReqFuBen");
    }
    public void RspFuBen(NetMsg msg) {
        RspFuBen data = msg.RspFuBen;

        mGameRoot.SetPlayerData(data.pd);
        mGameRoot.mMainWnd.SetWndState(false);
        mGameRoot.mFuBenWnd.SetWndState(false);

        BattleSys.Instance.InitBattle(data.fubenID);
    }


}
