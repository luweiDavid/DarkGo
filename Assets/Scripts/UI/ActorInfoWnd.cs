/****************************************************
    文件：ActorInfoWnd.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/29 16:6:45
	功能：属性详情界面
*****************************************************/

using Protocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActorInfoWnd : WindowRoot 
{

    private Button btnClose;
    private Button btnDetail; 

    private Transform charShowTr;
    private Image expFillImg;
    private Image powerFillImg;

    private Text txtName;
    private Text txtExpValue;
    private Text txtPowerValue;
    private Text txtJob;
    private Text txtFight;
    private Text txtHp;
    private Text txtA;
    private Text txtDef;

    #region 详细属性
    private Transform detailPanelTr;
    private Button btnCloseDetail;
    private Text txtHpValue;
    private Text txtAdValue;
    private Text txtApValue;
    private Text txtAdDefValue;
    private Text txtApDefValue;
    private Text txtDodgeValue;
    private Text txtPierceValue;
    private Text txtCriticalValue; 
    #endregion

    private float startPos_X;

    private void Awake()
    {
        btnClose = transform.Find("BgBottom/BtnClose").GetComponent<Button>(); 
        btnDetail = transform.Find("RightPanel/BtnDetail").GetComponent<Button>();
        charShowTr = transform.Find("LeftPanel/CharShow").GetComponent<Transform>(); 
        expFillImg = transform.Find("RightPanel/ExpBg/ProgressBg/ExpFillImg").GetComponent<Image>();
        powerFillImg = transform.Find("RightPanel/PowerBg/ProgressBg/PowerFillImg").GetComponent<Image>(); 
        txtName = transform.Find("LeftPanel/BgName/TxtName").GetComponent<Text>();
        txtExpValue=transform.Find("RightPanel/ExpBg/TxtValue").GetComponent<Text>(); 
        txtPowerValue = transform.Find("RightPanel/PowerBg/TxtValue").GetComponent<Text>(); 
        txtJob = transform.Find("RightPanel/VerticalContainer/JobBg/TxtDes").GetComponent<Text>();
        txtFight = transform.Find("RightPanel/VerticalContainer/FigthBg/TxtDes").GetComponent<Text>();
        txtHp = transform.Find("RightPanel/VerticalContainer/HpBg/TxtDes").GetComponent<Text>();
        txtA = transform.Find("RightPanel/VerticalContainer/ABg/TxtDes").GetComponent<Text>();
        txtDef = transform.Find("RightPanel/VerticalContainer/DefBg/TxtDes").GetComponent<Text>();

        #region 详细属性相关
        detailPanelTr = transform.Find("DetailPanel").transform;
        btnCloseDetail = detailPanelTr.Find("BG/BtnCloseDetail").GetComponent<Button>();
        txtHpValue = detailPanelTr.Find("VerticalContainer/HpPro/Value").GetComponent<Text>();
        txtAdValue = detailPanelTr.Find("VerticalContainer/AdPro/Value").GetComponent<Text>();
        txtApValue = detailPanelTr.Find("VerticalContainer/ApPro/Value").GetComponent<Text>();
        txtAdDefValue = detailPanelTr.Find("VerticalContainer/AdDefPro/Value").GetComponent<Text>();
        txtApDefValue = detailPanelTr.Find("VerticalContainer/ApDefPro/Value").GetComponent<Text>();
        txtDodgeValue = detailPanelTr.Find("VerticalContainer/DodgePro/Value").GetComponent<Text>();
        txtPierceValue = detailPanelTr.Find("VerticalContainer/PiercePro/Value").GetComponent<Text>();
        txtCriticalValue = detailPanelTr.Find("VerticalContainer/CriticalPro/Value").GetComponent<Text>();

        #endregion

        AddClickListener();
    } 

    protected override void InitWnd()
    {
        base.InitWnd();

        PlayerData pd = mGameRoot.GetPlayerData();
        UpdateData(pd);

        SetDetailPanelActive(false);
        RegisterCharShowListener();
    }

    private void UpdateData(PlayerData data) {
        SetText(txtName, string.Format(Language.GetString(115), data.Name, data.Level));
        SetText(txtExpValue, string.Format(Language.GetString(114), "E03683", data.Experience, PECommonTool.GetExpUpvalue(data.Level)));
        expFillImg.fillAmount = data.Experience * 1.0f / PECommonTool.GetExpUpvalue(data.Level);
        SetText(txtPowerValue, string.Format(Language.GetString(114), "E03683", data.Power, PECommonTool.GetPowerLimit(data.Level)));
        powerFillImg.fillAmount = data.Power * 1.0f / PECommonTool.GetPowerLimit(data.Level);
        SetText(txtJob, string.Format(Language.GetString(109), Language.GetString(20)));
        SetText(txtFight, string.Format(Language.GetString(110), PECommonTool.GetFight(data)));
        SetText(txtHp, string.Format(Language.GetString(111), data.Hp));
        SetText(txtA, string.Format(Language.GetString(112), data.Ad + data.Ap));
        SetText(txtDef, string.Format(Language.GetString(113), data.Addef + data.Apdef));

        
    } 

    private void RegisterCharShowListener() {
        RegisterPointerDownCB(charShowTr.gameObject, (PointerEventData evtData) => {
            startPos_X = evtData.position.x;
            MainCitySys.Instance.SetPlayerStartRotate();
        });

        RegisterDragCB(charShowTr.gameObject, (PointerEventData evtData) =>
        {
            float rot = startPos_X - evtData.position.x;
            MainCitySys.Instance.SetPlayerRotate(rot);
        });
    }


    private void OnBtnClose() {
        SetWndState(false);
        MainCitySys.Instance.SetPlayerCamDisable();
    }
    private void OnBtnDetail() {
        SetDetailPanelActive();
    }
    private void OnBtnCloseDetail()
    {
        SetDetailPanelActive(false);
    }

    private void SetDetailPanelActive(bool isActive = true) {
        if (isActive) {
            UpdateDetailPanelData();
        }
        detailPanelTr.gameObject.SetActive(isActive);
    }

    private void UpdateDetailPanelData() {
        PlayerData data = mGameRoot.GetPlayerData();
        SetText(txtHpValue, data.Hp); 
        SetText(txtAdValue, data.Ad); 
        SetText(txtApValue, data.Ap); 
        SetText(txtAdDefValue, data.Addef); 
        SetText(txtApDefValue, data.Apdef); 
        SetText(txtDodgeValue, string.Format(Language.GetString(38), data.Dodge)); 
        SetText(txtPierceValue, string.Format(Language.GetString(38), data.Pierce)); 
        SetText(txtCriticalValue, string.Format(Language.GetString(38), data.Critical));   
}

    private void AddClickListener() {
        btnClose.onClick.AddListener(OnBtnClose);
        btnDetail.onClick.AddListener(OnBtnDetail);
        btnCloseDetail.onClick.AddListener(OnBtnCloseDetail);
    }



}