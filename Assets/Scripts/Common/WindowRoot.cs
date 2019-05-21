/****************************************************
    文件：WindowRoot.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 18:9:28
	功能：UI窗口基类
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour
{
    protected ResSvc mResSvc;
    
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
        mResSvc = ResSvc.Instance;

    }

    protected virtual void CloseWnd() {
        mResSvc = null;
    }

    #region TOOL FUNC
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