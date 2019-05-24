/****************************************************
    文件：NetSvc.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/22 14:34:53
	功能：Nothing
*****************************************************/

using UnityEngine;
using PENet;
using Protocol;
using System.Collections.Generic;

public class NetSvc : MonoBehaviour 
{
    private const string obj = "lock";

    public static NetSvc Instance;

    PESocket<ClientSession, NetMsg> socket = null;
    private Queue<NetMsg> msgQueue = new Queue<NetMsg>();




    private void Start()
    {
        socket = new PESocket<ClientSession, NetMsg>();
        socket.StartAsClient(ServerCfg.IP, ServerCfg.Port);

        SetLog();
    }

    public void Init()
    {
        Instance = this;
        Debug.Log("NetSvc Init Done");
    }

    public void SendMsg(NetMsg msg) {
        if (socket.session != null)
        {
            socket.session.SendMsg(msg);
        }
        else {
            GameRoot.Instance.AddTips(Language.GetString(2));
            Init();   
        }
    }

    public void AddMsg(NetMsg msg) {
        lock (obj) {
            msgQueue.Enqueue(msg);
        }
    }

    public void Update()
    {
        if (msgQueue.Count > 0) {
            NetMsg msg = msgQueue.Dequeue();
            lock (obj) {
                HandleMsg(msg);
            }
        }
    }

    private void HandleMsg(NetMsg msg) {
        Debug.Log("HandleMsg");
        if (msg.err != (int)ErrorCode.None) {
            HandleErrorCode((ErrorCode)msg.err);
            return;
        }
        switch ((MsgType)msg.cmd)
        {  
            case MsgType.RspLogin:
                LoginSys.Instance.RspLogin(msg);
                break; 
        }
    }



    public void HandleErrorCode(ErrorCode err) {
        switch (err)
        { 
            case ErrorCode.AlreadyOnline:
                GameRoot.Instance.AddTips(Language.GetString(3));
                break;
            case ErrorCode.InvalidPassword:
                GameRoot.Instance.AddTips(Language.GetString(4));
                break; 
        }
    }



    private void SetLog() {
        socket.SetLog(true, (str, lev) =>
        {
            switch (lev)
            {
                case 1:
                    Debug.Log(str);
                    break;
                case 2:
                    Debug.LogWarning(str);
                    break;
                case 3:
                    Debug.LogError(str);
                    break;
                case 4:
                    Debug.LogError(str);
                    break;
                default:
                    break;
            }
        });
    }

}