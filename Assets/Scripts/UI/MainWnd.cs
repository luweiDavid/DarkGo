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
using UnityEngine.EventSystems;

public class MainWnd : WindowRoot
{
    #region  公共模属性
    private Text txtName;
    private Text txtFight;
    private Text txtPower;
    private Text txtLevel;
    private Text txtVip;
    private Text txtExpPercent;
    //聊天窗口的txt todo 
    private Button btnHead;
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
    private RectTransform imgFillRectTr;
    private GridLayoutGroup dotGridLayoutGroup;

    private bool hadGetComponent = false;
    #endregion

    #region 动画相关属性
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

    private bool useDoTween = false;
    private Animation rightBottomAnim;
    #endregion

    #region 摇杆相关属性
    private RectTransform rockingBarAreaTr;
    private RectTransform rockingBarBgTr;
    private RectTransform rockingBarPointTr;

    private Vector2 originPos_rbBg; 
    private Vector2 pointerStartPos;
    #endregion

    private CfgGuide curGuideData; 


    private void Awake()
    {
        txtName = transform.Find("LeftTopPanel/BtnHead/TxtName").GetComponent<Text>();
        txtFight = transform.Find("LeftTopPanel/BgFight/TxtFight").GetComponent<Text>(); 
        txtPower = transform.Find("LeftTopPanel/BgPower/TxtPower").GetComponent<Text>();
        txtLevel = transform.Find("LeftTopPanel/BgLv/TxtLv").GetComponent<Text>(); 
        txtVip = transform.Find("RightTopPanel/BtnVip/TxtVip").GetComponent<Text>();
        txtExpPercent = transform.Find("MiddleBottomPanel/TxtExpPercent").GetComponent<Text>();

        btnHead = transform.Find("LeftTopPanel/BtnHead").GetComponent<Button>();
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
        btnChat = transform.Find("MiddleBottomPanel/BtnChat").GetComponent<Button>(); 
        imgExpFill = transform.Find("MiddleBottomPanel/ImgFill").GetComponent<Image>();
        dotGridLayoutGroup = transform.Find("MiddleBottomPanel/ImgFill/DotContainer").GetComponent<GridLayoutGroup>();

        rockingBarAreaTr = transform.Find("LeftBottomPanel/RockingBarArea").GetComponent<RectTransform>();
        rockingBarBgTr = transform.Find("LeftBottomPanel/ImgRockingBar").GetComponent<RectTransform>();
        rockingBarPointTr = transform.Find("LeftBottomPanel/ImgRockingBar/ImgPoint").GetComponent<RectTransform>();
        imgFillRectTr = imgExpFill.GetComponent<RectTransform>();
        originPos_rbBg = rockingBarBgTr.anchoredPosition;

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
        rightBottomAnim = transform.Find("RightBottomPanel").GetComponent<Animation>();

        AddClickListener();
        RegisterRockingBarListener();
        SetExpDotXSpacing();

        hadGetComponent = true;
    }

    /// <summary>
    /// todo
    /// </summary>
    private void SetExpDotXSpacing() {
        float hRatio = (float)Constant.RefScreenHeight / Screen.height;
        float realFillWidth = (Mathf.Abs(imgFillRectTr.offsetMin.x) + Mathf.Abs(imgFillRectTr.offsetMax.x));
        float realFllWidthRatio = realFillWidth / (float)Constant.RefScreenWidth;
        float realScreenWidth = Screen.width * hRatio * realFllWidthRatio;
        float xSpacing = realScreenWidth / 11; 
        dotGridLayoutGroup.spacing = new Vector2(xSpacing, 0);
    }

    protected override void InitWnd(object[] args = null)
    {
        base.InitWnd(args);

        PlayerData data = mGameRoot.GetPlayerData();
        UpdateData(data);

        useDoTween = true;
        OnBtnCtrlState(); 
        SetActive(rockingBarPointTr, false);  
    }

    public void UpdateData(PlayerData data) {
        if (data != null && hadGetComponent)
        {
            //更新角色属性的显示 
            SetText(txtName, data.Name);
            SetText(txtFight, PECommonTool.GetFight(data));
            SetText(txtPower, string.Format(Language.GetString(5), data.Power, PECommonTool.GetPowerLimit(data.Level)));
            SetText(txtLevel, data.Level);   
            float percent = (float)data.Experience / PECommonTool.GetExpUpvalue(data.Level); 
            SetText(txtExpPercent, Mathf.CeilToInt(percent * 100) + "%");
            imgExpFill.fillAmount = percent;

            //更新引导数据
            curGuideData = mCfgSvc.GetCfgGuide(data.GuideID); 
            UpateGuideData();
        }
    }

    /// <summary>
    /// 更新引导相关信息
    /// </summary>
    private void UpateGuideData()
    {
        if (curGuideData == null)
        {
            return;
        }

        int npcId = curGuideData.npcID;
        Image img = btnAudoTask.GetComponent<Image>();
        string sptPath = "";
        switch (npcId)
        {
            case (int)GuideNpcIDType.NpcWiseman:
                sptPath = PathDefine.WisemanIcon;
                break;
            case (int)GuideNpcIDType.NpcGeneral:
                sptPath = PathDefine.GeneralIcon;
                break;
            case (int)GuideNpcIDType.NpcArtisan:
                sptPath = PathDefine.ArtisanIcon;
                break;
            case (int)GuideNpcIDType.NpcTrader:
                sptPath = PathDefine.TraderIcon;
                break;
            default:
                sptPath = PathDefine.TaskIcon;
                break;
        }
        SetSprite(img, sptPath);
    }


    private void OnBtnHead() {  
        mGameRoot.mActorInfoWnd.SetWndState();
        MainCitySys.Instance.SetPlayerCam();
    }

    private void OnBtnUpfight() { }


    private void OnBtnBuy() {
        //购买体力
        mGameRoot.mCommonBuyWnd.SetWndState(true, new object[1] { CommonBuyType.Power });
    }


    private void OnBtnVip() { }
    private void OnBtnCharge() { }
    private void OnBtnAutoTask() {
        //引导任务按钮
        MainCitySys.Instance.StartGuideTask(curGuideData);
    }
    private void OnBtnFuBen() {
        //打开副本界面
        FuBenSys.Instance.OpenFuBenWnd();
    }
    private void OnBtnTask() { }



    private void OnBtnCast() {
        //铸造
        mGameRoot.mCommonBuyWnd.SetWndState(true, new object[1] { CommonBuyType.Coin });
    }



    private void OnBtnStrengthen() {
        //强化系统
        mGameRoot.mStrongWnd.SetWndState();

    }
    private void OnBtnCtrlState() {
        //用两种方式播放动画，1：DOTween， 2，Animation
        //用DOTween的话就比较灵活，后续加入新的item也不用修改代码
        //用Animation的话，加了新的item就要重新做动画 
        mAudioSvc.PlayUIAudio(Constant.Audio_BtnExten);
        isOpenRightBottomPanel = !isOpenRightBottomPanel;
        if (!useDoTween)
        {
            //AnimationClip clip = null; 
            //if (isOpenRightBottomPanel)
            //{
            //    clip = rightBottomAnim.GetClip("MainWnd_OpenRightPanel");  
            //}
            //else {
            //    clip = rightBottomAnim.GetClip("MainWnd_CloseRightPanel"); 
            //}
            //rightBottomAnim.Play(clip.name);
        }
        else {
            if (isOpenRightBottomPanel)
            {
                OpenRightBottomPanel();
            }
            else
            {
                CloseRightBottomPanel();
            }
        } 
    }
    private void OnBtnChat() {
        mGameRoot.mChatWnd.SetWndState();

    }
     
       
    /// <summary>
    /// 摇杆事件注册
    /// </summary>
    private void RegisterRockingBarListener() {
        //这里需要做适配， 根据CanvasScaler的Match属性，根据宽高所占比例，本项目是根据高度
        float hRatio = Constant.RefScreenHeight/(float)Screen.height;
        float wRatio = Constant.RefScreenWidth/(float)Screen.width;
        float ratio = hRatio; 

        RegisterPointerDownCB(rockingBarAreaTr.gameObject, (PointerEventData evtData) =>
        {
            pointerStartPos = evtData.position;
            rockingBarBgTr.anchoredPosition = new Vector2(evtData.position.x * ratio, evtData.position.y * ratio);
            SetActive(rockingBarPointTr);
            rockingBarPointTr.anchoredPosition = Vector2.zero;
        });

        RegisterDragCB(rockingBarAreaTr.gameObject, (PointerEventData evtData) =>
        {
            Vector2 endPos = evtData.position;
            Vector2 dir = endPos - pointerStartPos;
            float dis = dir.magnitude;

            if (dis > Constant.RockingBarDis)
            {
                Vector2 adjustDir = Vector2.ClampMagnitude(dir, Constant.RockingBarDis);
                rockingBarPointTr.anchoredPosition = adjustDir;
            }
            else {
                rockingBarPointTr.anchoredPosition = dir;
            }

            MainCitySys.Instance.SetPlayerMove(dir);
        });

        RegisterPointerUpCB(rockingBarAreaTr.gameObject, (PointerEventData evtData) =>
        {
            rockingBarBgTr.anchoredPosition = originPos_rbBg;
            SetActive(rockingBarPointTr, false);
            rockingBarPointTr.anchoredPosition = Vector2.zero;

            MainCitySys.Instance.SetPlayerMove(Vector3.zero);
        });
    }

    private void AddClickListener()
    {
        btnHead.onClick.AddListener(OnBtnHead);
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