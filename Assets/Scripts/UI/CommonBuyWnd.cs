/****************************************************
	文件：CommonBuyWnd.cs
	作者：David
	邮箱: 1785275942@qq.com
	日期：2019/06/06 14:28   	
	功能：公共的购买窗口
*****************************************************/

using Protocol;
using UnityEngine.UI;

public class CommonBuyWnd : WindowRoot
{
    private Button btnClose;
    private Button btnEnsure;
    private Button btnCancel;
    private Text txtTitle;
    private Text txtContent;


    private int buyType;

    private PlayerData playerData;

    private void Awake()
    {
        btnClose = transform.Find("BtnClose").GetComponent<Button>();
        btnEnsure = transform.Find("BtnEnsure").GetComponent<Button>();
        btnCancel = transform.Find("BtnCancel").GetComponent<Button>(); 
        txtTitle = transform.Find("TxtTitle").GetComponent<Text>();
        txtContent = transform.Find("ImgContent/TxtContent").GetComponent<Text>();

        AddClickListener();
    }

    protected override void InitWnd(object[] args = null)
    {
        base.InitWnd(args);
        playerData = mGameRoot.GetPlayerData();
        buyType = (int)args[0];
        UpdateData();
    }

    public void UpdateData() {
        string titleStr = "";
        string contentStr = "";
        switch (buyType)
        {
            case (int)CommonBuyType.Coin:
                titleStr = string.Format(Language.GetString(48));
                //10颗钻石购买100个金币
                contentStr = string.Format(Language.GetString(45), 10, 100);
                break;
            case (int)CommonBuyType.Power:
                titleStr = string.Format(Language.GetString(47));
                //10颗钻石购买50点体力
                contentStr = string.Format(Language.GetString(46), 10, 50);
                break;
        }

        SetText(txtTitle, titleStr);
        SetText(txtContent, contentStr);
    } 

    private void OnBtnEnsure() { 
        if (playerData != null && playerData.Diamond < 10) {
            mGameRoot.AddTips(Language.GetString(59));
            return;
        }

        MainCitySys.Instance.ReqBuy(buyType);
        SetWndState(false);
    }

    private void OnBtnClose()
    {
        SetWndState(false);
    }

    private void AddClickListener()
    {
        btnClose.onClick.AddListener(OnBtnClose);
        btnEnsure.onClick.AddListener(OnBtnEnsure);
        btnCancel.onClick.AddListener(OnBtnClose); 
    }

}
