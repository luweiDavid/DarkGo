/****************************************************
    文件：LoginSys.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 14:37:6
	功能：业务系统 - 登陆系统
*****************************************************/

using Protocol;
using UnityEngine;

public class LoginSys : SystemRoot<LoginSys>
{ 
    public void EnterLogin() {

        mResSvc.AsyncLoadScene(Constant.Scene_Login, () =>
        {
            //打开登陆面板 
            mGameRoot.mLoginWnd.SetWndState(true);
            mAudioSvc.PlayBgAudio(Constant.Audio_BgLogin, true); 
        }); 

    }

    public void ReqLogin(string _acct, string _password)
    {
        NetMsg newMsg = new NetMsg
        {
            cmd = (int)MsgType.ReqLogin,
            ReqLogin = new ReqLogin
            {
                account = _acct,
                password = _password,
            }
        };
        mNetSvc.SendMsg(newMsg);
        Debug.Log("ReqLogin");
    }

    public void RspLogin(NetMsg msg)
    {
        //如果服务器返回成功，则关闭登陆面板，打开角色创建面板或主城界面
        RspLogin rspData = msg.RspLogin;

        mGameRoot.mLoginWnd.SetWndState(false);
        if (string.IsNullOrEmpty(rspData.data.Name))
        {
            //新账号 
            mGameRoot.mCreateWnd.SetWndState();
        }
        else {
            //旧帐号
            MainCitySys.Instance.EnterMainCity();
        }

        //保存玩家数据 
        mGameRoot.SetPlayerData(rspData.data);
    }

    public void ReqRename(string name) {
        NetMsg newMsg = new NetMsg
        {
            cmd=(int)MsgType.ReqRename,
        };

        if (!string.IsNullOrEmpty(name)) {
            newMsg.ReqRename = new ReqRename {
                name=name,
            };
            mNetSvc.SendMsg(newMsg);
        }
        Debug.Log("ReqRename");
    }

    public void RspRename(NetMsg msg) {
        RspRename rspData = msg.RspRename;

        mGameRoot.mCreateWnd.SetWndState(false);
        MainCitySys.Instance.EnterMainCity();

        Debug.Log("RspRename : "+msg.RspRename.name);
    }
}