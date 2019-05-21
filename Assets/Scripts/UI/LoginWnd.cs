/****************************************************
    文件：LoginWnd.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 16:40:45
	功能：登陆界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : WindowRoot 
{
    private Button btnNotice;
    private InputField iptAccount;
    private InputField iptPassword;
    private Button btnEnter;

    private void Awake()
    {
        btnNotice = transform.Find("BtnNotice").GetComponent<Button>();
        iptAccount = transform.Find("RightPanel/IptAccountBg/IptAccount").GetComponent<InputField>();
        iptPassword = transform.Find("RightPanel/IptPasswordBg/IptPassword").GetComponent<InputField>();
        btnEnter = transform.Find("RightPanel/BtnEnter").GetComponent<Button>();
    }

    protected override void InitWnd()
    {
        base.InitWnd();

        if (PlayerPrefs.HasKey("Account") && PlayerPrefs.HasKey("Password"))
        {
            iptAccount.text = PlayerPrefs.GetString("Account");
            iptPassword.text = PlayerPrefs.GetString("Password");
        }
        else
        {
            iptAccount.text = "";
            iptPassword.text = "";
        }
    }

    

}