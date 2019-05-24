/****************************************************
    文件：ClientSession.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/22 17:36:46
	功能：客户端网络会话
*****************************************************/

using UnityEngine;
using PENet; 
using Protocol;

public class ClientSession :  PESession<NetMsg>
{
    protected override void OnConnected()
    {

    }

    protected override void OnReciveMsg(NetMsg msg)
    {
        Debug.Log("OnReciveMsg:  " + ((MsgType)(msg.cmd)).ToString());
        NetSvc.Instance.AddMsg(msg);
    }

    protected override void OnDisConnected()
    {
    }

}