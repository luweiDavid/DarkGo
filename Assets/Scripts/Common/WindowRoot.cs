/****************************************************
    文件：WindowRoot.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 18:9:28
	功能：UI窗口基类
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour
{
    protected GameRoot mGameRoot;
    protected ResSvc mResSvc;
    protected AudioSvc mAudioSvc;
    protected NetSvc mNetSvc;
    
    public void SetWndState(bool isActive = true) {
        if (gameObject.activeSelf != isActive) {
            SetActive(gameObject, isActive);
        }
        if (isActive)
        {
            InitWnd();
        }
        else {
            CloseWnd();
        }
    }

    protected virtual void InitWnd() {
        mGameRoot = GameRoot.Instance;
        mResSvc = ResSvc.Instance;
        mAudioSvc = AudioSvc.Instance;
        mNetSvc = NetSvc.Instance;
    }

    protected virtual void CloseWnd() {
        mGameRoot = null;
        mResSvc = null;
        mAudioSvc = null;
        mNetSvc = null;
    }

    #region 点击拖拽事件
    private T GetComponentExt<T>(GameObject go) where T:Component{
        T t = go.GetComponent<T>();
        if (t == null) {
            t = go.AddComponent<T>();
        }
        return t;
    }

    protected virtual void RegisterPointerDownCB(GameObject go,Action<PointerEventData> cb) {
        if (cb != null && go != null) {
            ListenerExt listener = GetComponentExt<ListenerExt>(go);
            listener.pointerDownCB = cb;
        }
    }

    protected virtual void RegisterPointerUpCB(GameObject go, Action<PointerEventData> cb)
    {
        if (cb != null && go != null)
        {
            ListenerExt listener = GetComponentExt<ListenerExt>(go);
            listener.pointerUpCB = cb;
        }
    }

    protected virtual void RegisterDragCB(GameObject go, Action<PointerEventData> cb)
    {
        if (cb != null && go != null)
        {
            ListenerExt listener = GetComponentExt<ListenerExt>(go);
            listener.onDragCB = cb;
        }
    }

    #endregion


    #region TOOL FUNC
    protected void SetSprite(Image img, string path) {
        Sprite spt = mResSvc.GetSprite(path, true);
        img.sprite = spt;
    }



    protected void SetActive(GameObject go, bool isActive = true) {
        go.SetActive(isActive);
    }

    protected void SetActive(Transform trans, bool isActive = true) {
        trans.gameObject.SetActive(isActive);
    }

    protected void SetActive(RectTransform rectTrs, bool isActive = true) {
        rectTrs.gameObject.SetActive(isActive);
    }

    protected void SetActive(Image img, bool isActive = true) {
        img.transform.gameObject.SetActive(isActive);
    }

    protected void SetActive(Text txt, bool isActive = true)
    {
        txt.transform.gameObject.SetActive(isActive);
    }


    protected void SetText(Text txt, string content = "") {
        txt.text = content;
    }

    protected void SetText(Transform trans, int num = 0) {
        SetText(trans.GetComponent<Text>(), num);
    }
    protected void SetText(Transform trans, string content = "") {
        SetText(trans.GetComponent<Text>(), content);
    }

    protected void SetText(Text txt, int num = 0) {
        SetText(txt, num.ToString());
    }
    #endregion
}