/****************************************************
    文件：LoginSys.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 14:37:6
	功能：业务系统 - 登陆系统
*****************************************************/

using UnityEngine;

public class LoginSys : SystemRoot 
{
    public static LoginSys Instance;


    public override void Init() {
        base.Init();

        Instance = this; 
    }
    public void EnterLogin() {

        mResSvc.AsyncLoadScene(Constant.SceneName_Login, () =>
        {
            //打开登陆面板 
            mGameRoot.mLoginWnd.SetWndState(true);
            mAudioSvc.PlayBgAudio(Constant.AudioName_BgLogin, true); 
        }); 

    }

    public void ReqLogin(string acct, string password)
    {


    }

    public void RspLogin()
    { 
        //如果服务器返回成功，则关闭登陆面板，打开角色创建面板或主城界面
    }
}