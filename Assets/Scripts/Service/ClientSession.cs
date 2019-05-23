/****************************************************
    文件：ClientSession.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/22 17:36:46
	功能：Nothing
*****************************************************/

using UnityEngine;
using PENet;
using Protocal;

public class ClientSession :  PESession<NetMsg>
{
    protected override void OnConnected()
    {
    }

    protected override void OnReciveMsg(NetMsg msg)
    {
        PETool.LogMsg("Server Response:" + msg.content);
    }

    protected override void OnDisConnected()
    {
    }

}