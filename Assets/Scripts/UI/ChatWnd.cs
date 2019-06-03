using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWnd : WindowRoot
{
    private Button btnClose;
    private Button btnSend;
    private Text txtChat;
    private InputField iptChat;
    private Toggle[] toggleArr;

    private List<string> chatList = new List<string>();
    private int curToggleIndex = 0;


    private void Awake()
    {
        btnClose = transform.Find("BtnClose").GetComponent<Button>();
        btnSend = transform.Find("BtnSend").GetComponent<Button>();
        txtChat = transform.Find("TxtChat").GetComponent<Text>();
        iptChat = transform.Find("IptChat").GetComponent<InputField>();

        toggleArr = new Toggle[3];
        for (int i = 0; i < toggleArr.Length; i++)
        {
            toggleArr[i] = transform.Find("UpCon/Toggle" + i.ToString()).GetComponent<Toggle>();
        }
        for (int i = 0; i < toggleArr.Length; i++)
        {
            toggleArr[i].OnValueChgedExt(OnToggleValueChged);
            toggleArr[i].isOn = false;
        }
        AddClickListener();
    }
    private void InitTogglesState()
    {
        for (int i = 0; i < toggleArr.Length; i++)
        {
            toggleArr[i].isOn = false;
        }
    }

    private void OnToggleValueChged(string str)
    {
        int pos = int.Parse(str.Substring(6, 1));
        curToggleIndex = pos;
        Debug.Log(pos);
        UpdateData();
    }
    protected override void InitWnd(object[] args = null)
    {
        base.InitWnd(args);
        InitTogglesState();
        toggleArr[0].isOn = true; 
    }

    public void AddChatMsg(string str) {
        if (chatList.Count > 10) {
            chatList.RemoveAt(0);
        }
        chatList.Add(str);

        //这里可以在界面打开时才刷新，没打开就不刷新
        UpdateData();
    }

    private void UpdateData() {
        string str = "";
        switch (curToggleIndex)
        {
            case 0:
                foreach (string tmp in chatList)
                {
                    str += tmp;
                    str += "\n";
                }
                break;
            case 1:
                str = "未加入公会";
                break;
            case 2:
                str = "暂无好友消息";
                break;
        }

        txtChat.text = str;
    }

    private void OnBtnSend()
    {
        if (string.IsNullOrEmpty(iptChat.text)) {
            mGameRoot.AddTips("请输入聊天内容");
            return;
        }
        MainCitySys.Instance.SendChatMsg(iptChat.text.ToString());
    } 

    private void OnBtnClose()
    {
        SetWndState(false);
    }

    private void AddClickListener()
    {
        btnClose.onClick.AddListener(OnBtnClose);
        btnSend.onClick.AddListener(OnBtnSend); 
    }
}
