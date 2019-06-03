/****************************************************
    文件：LoadingWnd.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/21 15:31:16
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : WindowRoot 
{
    private Text txtTips;
    private Image imgFill;
    private Image imgPoint;
    private Text txtValue;

    private float fillWidth; 

    private void Awake() {
        txtTips = transform.Find("BottomPanel/TxtDes").GetComponent<Text>();
        imgFill = transform.Find("BottomPanel/ProgressBarBg/ProgressBarFill").GetComponent<Image>();
        imgPoint = transform.Find("BottomPanel/ProgressBarBg/ImgLightPoint").GetComponent<Image>();
        txtValue = transform.Find("BottomPanel/ProgressBarBg/TxtValue").GetComponent<Text>();
        fillWidth = imgFill.GetComponent<RectTransform>().sizeDelta.x;
    }

    protected override void InitWnd(object[] args = null)
    {
        base.InitWnd(args);
        SetText(txtTips, "这是一条游戏tips");
        imgFill.fillAmount = 0;
        imgPoint.transform.localPosition = new Vector2(-fillWidth / 2, 0);  
        SetText(txtValue, "0%");

    } 

    public void SetProgress(float progress) {
        imgFill.fillAmount = progress;
        imgPoint.transform.localPosition = new Vector2(-fillWidth / 2 + progress * fillWidth, 0); 
        SetText(txtValue, (int)(progress * 100) + "%");
    }
}