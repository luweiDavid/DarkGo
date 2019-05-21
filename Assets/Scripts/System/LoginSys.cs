/****************************************************
    文件：LoginSys.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 14:37:6
	功能：业务系统 - 登陆系统
*****************************************************/

using UnityEngine;

public class LoginSys : MonoBehaviour 
{
    public static LoginSys Instance;


    public void Init() {
        Instance = this; 
    }
    public void EnterLogin() {

        ResSvc.Instance.AsyncLoadScene(Constant.SceneName_Login, () =>
        {
            //打开登陆面板 
            GameRoot.Instance.mLoginWnd.SetWndState(true);
        }); 

    }


}