/****************************************************
    文件：NetSvc.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/22 14:34:53
	功能：Nothing
*****************************************************/

using UnityEngine;
using PENet;
using Protocal;

public class NetSvc : MonoBehaviour 
{
    PESocket<ClientSession, NetMsg> socket = null;


    private void Start()
    {
        socket = new PESocket<ClientSession, NetMsg>();
        socket.StartAsClient(IPCfg.srvIP, IPCfg.srvPort);

        //str ： 错误信息， lev ： 错误等级
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            socket.session.SendMsg(new NetMsg
            {
                content = "Hello Unity"
            });
        }
    }

}