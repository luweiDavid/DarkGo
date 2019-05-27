/****************************************************
    文件：MainWnd.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/27 10:58:40
	功能：Nothing
*****************************************************/

using Protocol;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainWnd : WindowRoot
{
    private Text txtName;
    private Text txtFight;
    private Text txtPower;
    private Text txtLevel;
    private Text txtVip;
    private Text txtExpPercent;
    //聊天窗口的txt todo

    private Button btnUpfight;
    private Button btnBuy;
    private Button btnVip;
    private Button btnCharge;
    private Button btnAudoTask;
    private Button btnFuBen;
    private Button btnTask;
    private Button btnCast;
    private Button btnStrengthen;
    private Button btnCtrlState;
    private Button btnChat; 

    private Image imgExpFill;
     
    private RectTransform upMaskRectTr;
    private RectTransform leftMaskRectTr;
    private RectTransform btnCtrlStateTr;
    private int upMaskCount = 0;
    private int leftMaskCount = 0;
    private float constantWidth = 0;
    private float constantHeight = 0;
    private float targetHeight = 0;
    private float targetWidth = 0;
    private bool isOpenRightBottomPanel = false;
    private Tweener openMaskTweener1;
    private Tweener openMaskTweener2;
    private Tweener closeMaskTweener1; 
    private Tweener closeMaskTweener2;
    private CanvasGroup upmaskCG = null;
    private CanvasGroup leftmaskCG = null;
    private float tweenTime;

    private void Awake()
    {
        txtName = transform.Find("LeftTopPanel/ImgHead/TxtName").GetComponent<Text>();
        txtFight = transform.Find("LeftTopPanel/BgFight/TxtFight").GetComponent<Text>(); 
        txtPower = transform.Find("LeftTopPanel/BgPower/TxtPower").GetComponent<Text>();
        txtLevel = transform.Find("LeftTopPanel/BgLv/TxtLv").GetComponent<Text>(); 
        txtVip = transform.Find("RightTopPanel/BtnVip/TxtVip").GetComponent<Text>();
        txtExpPercent = transform.Find("MiddleBottomPanel/BgExp/Bg/TxtExpPercent").GetComponent<Text>();

        btnUpfight = transform.Find("LeftTopPanel/BgFight/BtnUpfight").GetComponent<Button>();
        btnBuy = transform.Find("LeftTopPanel/BgPower/BtnBuy").GetComponent<Button>();
        btnVip = transform.Find("RightTopPanel/BtnVip").GetComponent<Button>();
        btnCharge = transform.Find("RightTopPanel/BtnCharge").GetComponent<Button>();
        btnAudoTask = transform.Find("RightTopPanel/BtnAutoTask").GetComponent<Button>();
        btnFuBen = transform.Find("RightBottomPanel/ImgUpMask/BtnFuBen").GetComponent<Button>();
        btnTask = transform.Find("RightBottomPanel/ImgUpMask/BtnTask").GetComponent<Button>();
        btnCast = transform.Find("RightBottomPanel/ImgLeftMask/BtnCast").GetComponent<Button>();
        btnStrengthen = transform.Find("RightBottomPanel/ImgLeftMask/BtnStrengthen").GetComponent<Button>();
        btnCtrlState = transform.Find("RightBottomPanel/BtnCtrlState").GetComponent<Button>();
        btnChat = transform.Find("MiddleBottomPanel/BgChat/BtnChat").GetComponent<Button>();

        imgExpFill = transform.Find("MiddleBottomPanel/BgExp/Bg/ImgFill").GetComponent<Image>();

        upMaskRectTr = transform.Find("RightBottomPanel/ImgUpMask").GetComponent<RectTransform>();
        leftMaskRectTr = transform.Find("RightBottomPanel/ImgLeftMask").GetComponent<RectTransform>();
        btnCtrlStateTr = transform.Find("RightBottomPanel/BtnCtrlState").GetComponent<RectTransform>();
        upMaskCount = upMaskRectTr.childCount;
        leftMaskCount = leftMaskRectTr.childCount;
        constantWidth = 107;
        constantHeight = 140;
        targetHeight = constantHeight * upMaskCount;
        targetWidth = constantWidth * leftMaskCount + (leftMaskCount - 1) * 30;
        upmaskCG = upMaskRectTr.GetComponent<CanvasGroup>();
        leftmaskCG = leftMaskRectTr.GetComponent<CanvasGroup>();
        tweenTime = 0.2f;

        AddClickListener(); 
    }

    protected override void InitWnd()
    {
        base.InitWnd();

        PlayerData data = mGameRoot.GetPlayerData();
        if (data != null)
        {
            SetText(txtName, data.Name);
            SetText(txtFight, PECommonTool.GetFight(data));
            SetText(txtPower, string.Format(Language.GetString(5), data.Power, PECommonTool.GetPowerLimit(data.Level)));
            SetText(txtLevel, data.Level);
            SetExp(data);
        }
       

        isOpenRightBottomPanel = true; 
        OpenRightBottomPanel();
    }



    /// <summary>
    /// 设置经验条显示
    /// </summary>
    /// <param name="data"></param>
    private void SetExp(PlayerData data) {
        float percent = (float)data.Experience / PECommonTool.GetExpUpvalue(data.Level);

        SetText(txtExpPercent, Mathf.CeilToInt(percent * 100) + "%");
        imgExpFill.fillAmount = percent;
    }

    private void OnBtnUpfight() { }
    private void OnBtnBuy() { }
    private void OnBtnVip() { }
    private void OnBtnCharge() { }
    private void OnBtnAutoTask() { }
    private void OnBtnFuBen() { }
    private void OnBtnTask() { }
    private void OnBtnCast() { }
    private void OnBtnStrengthen() { }
    private void OnBtnCtrlState() { 
        isOpenRightBottomPanel = !isOpenRightBottomPanel;
        if (isOpenRightBottomPanel)
        {
            OpenRightBottomPanel();
        }
        else {
            CloseRightBottomPanel();
        } 
    }
    private void OnBtnChat() { }

    private void AddClickListener()
    {
        btnUpfight.onClick.AddListener(OnBtnUpfight);
        btnBuy.onClick.AddListener(OnBtnBuy);
        btnVip.onClick.AddListener(OnBtnVip);
        btnCharge.onClick.AddListener(OnBtnCharge);
        btnAudoTask.onClick.AddListener(OnBtnAutoTask);
        btnFuBen.onClick.AddListener(OnBtnFuBen);
        btnTask.onClick.AddListener(OnBtnTask);
        btnCast.onClick.AddListener(OnBtnCast);
        btnStrengthen.onClick.AddListener(OnBtnStrengthen);
        btnCtrlState.onClick.AddListener(OnBtnCtrlState);
        btnChat.onClick.AddListener(OnBtnChat);
    }

    #region 右下角panel动画
    private void SetOpenRightBottomPanelTweener()
    {
        openMaskTweener1 = null;
        openMaskTweener2 = null;
        float hInterval = targetHeight / (tweenTime * 60);
        Vector2 vec1 = new Vector2(0, hInterval);
        float wInterval = targetWidth / (tweenTime * 60);
        Vector2 vec2 = new Vector2(wInterval, 0);

        openMaskTweener1 = DOTween.To(() => upmaskCG.alpha, x => upmaskCG.alpha = x, 1, tweenTime);
        openMaskTweener1.OnUpdate(() =>
        {
            upMaskRectTr.sizeDelta += vec1;
        }).OnComplete(() =>
        {
            btnCtrlState.interactable = true;
            upmaskCG.interactable = true;
            upmaskCG.alpha = 1;
            upMaskRectTr.sizeDelta = new Vector2(constantWidth, targetHeight);
        });
        openMaskTweener2 = DOTween.To(() => leftmaskCG.alpha, x => leftmaskCG.alpha = x, 1, tweenTime);
        openMaskTweener2.OnUpdate(() =>
        {
            leftMaskRectTr.sizeDelta += vec2;
        }).OnComplete(() =>
        {
            btnCtrlState.interactable = true;
            leftmaskCG.interactable = true;
            leftmaskCG.alpha = 1;
            leftMaskRectTr.sizeDelta = new Vector2(targetWidth, constantHeight);
        });
        openMaskTweener1.Pause();
        openMaskTweener2.Pause();
    }
    private void SetCloseRightBottomPanelTweener()
    {
        closeMaskTweener1 = null;
        closeMaskTweener2 = null;
        float hInterval = targetHeight / (tweenTime * 60);
        Vector2 vec1 = new Vector2(0, hInterval);
        float wInterval = targetWidth / (tweenTime * 60);
        Vector2 vec2 = new Vector2(wInterval, 0);
        closeMaskTweener1 = DOTween.To(() => upmaskCG.alpha, x => upmaskCG.alpha = x, 0, tweenTime);
        closeMaskTweener1.OnUpdate(() =>
        {
            upMaskRectTr.sizeDelta -= vec1;
        }).OnComplete(() =>
        {
            btnCtrlState.interactable = true;
            upmaskCG.interactable = false;
            upmaskCG.alpha = 0;
            upMaskRectTr.sizeDelta = new Vector2(constantWidth, 0);
        });

        closeMaskTweener2 = DOTween.To(() => leftmaskCG.alpha, x => leftmaskCG.alpha = x, 0, tweenTime);
        closeMaskTweener2.OnUpdate(() =>
        {
            leftMaskRectTr.sizeDelta -= vec2;
        }).OnComplete(() =>
        {
            btnCtrlState.interactable = true;
            leftmaskCG.interactable = false;
            leftmaskCG.alpha = 0;
            leftMaskRectTr.sizeDelta = new Vector2(targetWidth, constantHeight);
        });
        closeMaskTweener1.Pause();
        closeMaskTweener2.Pause();
    }

    private void OpenRightBottomPanel()
    {
        SetOpenRightBottomPanelTweener();
        btnCtrlState.interactable = false;
        upmaskCG.interactable = false;
        upmaskCG.alpha = 0;
        leftmaskCG.interactable = false;
        leftmaskCG.alpha = 0;
        upMaskRectTr.sizeDelta = new Vector2(constantWidth, 0);
        leftMaskRectTr.sizeDelta = new Vector2(0, constantHeight);
        btnCtrlStateTr.localRotation = new Quaternion(0, 0, 45, 0);

        Vector3 rotVecEnd = new Vector3(0, 0, 0);
        btnCtrlStateTr.DORotate(rotVecEnd, tweenTime);
        openMaskTweener1.Play();
        openMaskTweener2.Play();
    }
    //ImgUpMask： width：107（不变），heigth：140 * childCount
    //ImgLeftMask： width：107*childCount+（childCount-1）*30， heigth：140 （不变）
    private void CloseRightBottomPanel()
    {
        SetCloseRightBottomPanelTweener();
        btnCtrlState.interactable = false;
        upmaskCG.interactable = false;
        upmaskCG.alpha = 1;
        leftmaskCG.interactable = false;
        leftmaskCG.alpha = 1;
        upMaskRectTr.sizeDelta = new Vector2(constantWidth, targetHeight);
        leftMaskRectTr.sizeDelta = new Vector2(targetWidth, constantHeight);
        btnCtrlStateTr.localRotation = new Quaternion(0, 0, 0, 0);

        Vector3 rotVecEnd = new Vector3(0, 0, 45);
        btnCtrlStateTr.DORotate(rotVecEnd, tweenTime);
        closeMaskTweener1.Play();
        closeMaskTweener2.Play();
    }
    #endregion

    protected override void CloseWnd()
    {
        base.CloseWnd();

        openMaskTweener1 = null;
        openMaskTweener2 = null;
        closeMaskTweener1 = null;
        closeMaskTweener2 = null;
    }
}