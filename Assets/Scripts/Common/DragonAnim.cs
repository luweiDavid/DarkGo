/****************************************************
    文件：DragonAnim.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 23:43:28
	功能：Nothing
*****************************************************/

using UnityEngine;

public class DragonAnim : MonoBehaviour 
{
    private Animation mAnim;

    private void Awake()
    {
        mAnim = GetComponent<Animation>();

        InvokeRepeating("PlayAnim", 0, 20);
    }

    private void PlayAnim() {
        if (mAnim != null) {
            mAnim.Play();
        }
    }


}