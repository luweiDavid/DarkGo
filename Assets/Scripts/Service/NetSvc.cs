/****************************************************
    文件：NetSvc.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/22 14:34:53
	功能：网络连接服务系统
*****************************************************/

using UnityEngine;
using PENet;
using Protocol;
using System.Collections.Generic;

public class NetSvc : ServiceRoot<NetSvc>
{
    private const string obj = "lock"; 

    PESocket<ClientSession, NetMsg> socket = null;
    private Queue<NetMsg> msgQueue = new Queue<NetMsg>();
     

    private void Start()
    {
        socket = new PESocket<ClientSession, NetMsg>();
        socket.StartAsClient(ServerCfg.IP, ServerCfg.Port);

        SetLog();
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
        if (msg.err != (int)ErrorCode.None) {
            Debug.Log("网络返回错误码: "+((ErrorCode)(msg.err)).ToString());
            HandleErrorCode((ErrorCode)msg.err);
            return;
        } 
        switch ((MsgType)msg.cmd)
        {  
            case MsgType.RspLogin: 
                LoginSys.Instance.RspLogin(msg);
                break;
            case MsgType.RspRename:
                LoginSys.Instance.RspRename(msg);
                break;
            case MsgType.RspGuide:
                MainCitySys.Instance.RspGuide(msg); 
                break;
            case MsgType.RspStrong:
                MainCitySys.Instance.RspStrong(msg);
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
            case ErrorCode.NameExisted:
                GameRoot.Instance.AddTips(Language.GetString(6)); 
                break;
            case ErrorCode.UpdateDBFailed:
                PECommonTool.Log("数据库更新失败");
                GameRoot.Instance.AddTips(Language.GetString(7));
                break;
            case ErrorCode.ServerDataError:
                PECommonTool.Log("数据库更新失败");
                GameRoot.Instance.AddTips(Language.GetString(8));
                break;
            case ErrorCode.LackCoin:
                GameRoot.Instance.AddTips(Language.GetString(62));
                break;
            case ErrorCode.LackCrystal:
                GameRoot.Instance.AddTips(Language.GetString(61));
                break;
            case ErrorCode.LackLevel:
                GameRoot.Instance.AddTips(Language.GetString(60));
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