/****************************************************
    文件：SystemRoot.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 19:0:15
	功能：业务系统基类
*****************************************************/

using UnityEngine;

public class SystemRoot : MonoBehaviour 
{
    protected GameRoot mGameRoot;
    protected ResSvc mResSvc;
    protected AudioSvc mAudioSvc;

    public virtual void Init() {
        mGameRoot = GameRoot.Instance;
        mResSvc = ResSvc.Instance;
        mAudioSvc = AudioSvc.Instance;
    }
    
}