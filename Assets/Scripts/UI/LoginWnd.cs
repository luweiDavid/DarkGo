/****************************************************
    文件：LoginWnd.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 16:40:45
	功能：登陆界面
*****************************************************/

using Protocol;
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

        btnNotice.onClick.AddListener(OnNoticeBtnClick);
        btnEnter.onClick.AddListener(OnEnterBtnClick);
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

    private void OnEnterBtnClick()
    {
        mAudioSvc.PlayUIAudio(Constant.AudioName_BtnLogin);
        string _acct = iptAccount.text;
        string _password = iptPassword.text; 

        if (!string.IsNullOrEmpty(_acct) && !string.IsNullOrEmpty(_password))
        { 
            LoginSys.Instance.ReqLogin(_acct, _password);
        }
        else {
            mGameRoot.AddTips(Language.GetString(1));
        }
    }

    private void OnNoticeBtnClick() {
        mAudioSvc.PlayUIAudio(Constant.AudioName_BtnClick);

        mGameRoot.AddTips(Language.GetString(1000));
    }

}