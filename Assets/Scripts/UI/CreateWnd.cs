/****************************************************
    文件：CreateWnd.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/22 9:48:16
	功能：角色创建面板
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class CreateWnd : WindowRoot 
{
    private Button btnRandName;
    private InputField IptName;
    private Button btnEnter;

    private void Awake()
    {
        btnRandName = transform.Find("RightPanel/Bg2/BtnRandName").GetComponent<Button>();
        IptName = transform.Find("RightPanel/Bg2/IptName").GetComponent<InputField>();
        btnEnter = transform.Find("RightPanel/BtnEnter").GetComponent<Button>();

        btnRandName.onClick.AddListener(OnRandNameBtnClick);
        btnEnter.onClick.AddListener(OnEnterBtnClick);
    }

    protected override void InitWnd()
    {
        base.InitWnd();

    }

    private void OnRandNameBtnClick() {
        mAudioSvc.PlayUIAudio(Constant.AudioName_BtnClick);

        string name = mResSvc.GetRandName();
        IptName.text = name; 
    }
    private void OnEnterBtnClick() {
        mAudioSvc.PlayUIAudio(Constant.AudioName_BtnClick);
         
        if (!string.IsNullOrEmpty(IptName.text) ) {
            LoginSys.Instance.ReqRename(IptName.text);
        } 
    }
}