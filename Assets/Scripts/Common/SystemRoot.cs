/****************************************************
    文件：SystemRoot.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 19:0:15
	功能：业务系统基类
*****************************************************/

using UnityEngine;

public class SystemRoot<T> : MonoBehaviour where T:MonoBehaviour
{
    public static T Instance;

    protected GameRoot mGameRoot;
    protected CfgSvc mCfgSvc;
    protected ResSvc mResSvc;
    protected AudioSvc mAudioSvc;
    protected NetSvc mNetSvc;


    public virtual void Init() {
        Instance = this as T;

        mGameRoot = GameRoot.Instance;
        mCfgSvc = CfgSvc.Instance;
        mResSvc = ResSvc.Instance;
        mAudioSvc = AudioSvc.Instance;
        mNetSvc = NetSvc.Instance;
    }
    
}