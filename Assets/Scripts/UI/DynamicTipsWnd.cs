/****************************************************
    文件：DynamicTipsWnd.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/22 0:0:13
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class DynamicTipsWnd : WindowRoot
{
    private Animator mAnim;
    private Text txtTips;

    private bool isShowing = false;
    private Queue<string> tipsQueue = new Queue<string>();

    private void Awake()
    {
        mAnim = transform.Find("Panel/TxtTips").GetComponent<Animator>();
        txtTips = transform.Find("Panel/TxtTips").GetComponent<Text>();
    }

    protected override void InitWnd(object[] args = null)
    {
        base.InitWnd(args);
        isShowing = false;
        SetActive(txtTips, false);
    }

    private void Update()
    {
        if (tipsQueue.Count > 0 && isShowing == false) {
            isShowing = true;
            string tips = tipsQueue.Dequeue();
            ShowTips(tips);
        }
    }

    public void AddTips(string tips) {
        if (tipsQueue != null) {
            tipsQueue.Enqueue(tips);
        }
    } 

    private void ShowTips(string str) {
        SetActive(txtTips, false);
        SetText(txtTips, str);

        SetActive(txtTips);
        AnimationClip clip = mAnim.runtimeAnimatorController.animationClips[0];
        StartCoroutine(DelayHideTips(clip.length, () =>
        {
            SetActive(txtTips, false);
            isShowing = false;
        }));
    }

    IEnumerator DelayHideTips(float sec, Action cb) {
        yield return new WaitForSeconds(sec);
        if (cb != null) {
            cb();
        }
    }

}