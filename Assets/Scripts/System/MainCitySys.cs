/****************************************************
    文件：MainCitySys.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/27 10:59:41
	功能：Nothing
*****************************************************/

using UnityEngine;

public class MainCitySys : SystemRoot
{
    public static MainCitySys Instance;


    public override void Init()
    {
        base.Init();

        Instance = this;
    }

    public void EnterMainCity() {
        mResSvc.AsyncLoadScene(Constant.Scene_MainCity, () =>
        {
            mGameRoot.mMainWnd.SetWndState(true);
            mAudioSvc.PlayBgAudio(Constant.Audio_BgMainCity, true);
        });
    }
}