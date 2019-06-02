/****************************************************
    文件：AudioSvc.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 19:1:0
	功能：服务层 -- 音乐系统
*****************************************************/

using UnityEngine;

public class AudioSvc : ServiceRoot<AudioSvc>
{ 
    public AudioSource bgAudioSrc;
    public AudioSource uiAudioSrc;

    public void PlayBgAudio(string name, bool isLoop) {
        if (bgAudioSrc != null) {
            if (bgAudioSrc.clip == null || bgAudioSrc.clip.name != name) {
                bgAudioSrc.clip = ResSvc.Instance.GetAudio("Audio/" + name, true);
                bgAudioSrc.loop = isLoop;
                bgAudioSrc.Play();
            }
        }
    }

    public void PlayUIAudio(string name)
    {
        if (uiAudioSrc != null)
        {
            if (uiAudioSrc.clip == null || uiAudioSrc.clip.name != name)
            {
                uiAudioSrc.clip = ResSvc.Instance.GetAudio("Audio/" + name, true);
                uiAudioSrc.Play();
            }
        }
    }

}