using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCtrlWnd : WindowRoot
{
    #region 摇杆相关属性
    private RectTransform rockingBarAreaTr;
    private RectTransform rockingBarBgTr;
    private RectTransform rockingBarPointTr;

    private Vector2 originPos_rbBg;
    private Vector2 pointerStartPos;
    #endregion


    private Text txtHp;
    private Text txtLevel; 
    private Image imgHpFill;

    #region  技能相关属性
    private Button btnSkill_0;
    private Button btnSkill_1;
    private Button btnSkill_2;
    private Button btnSkill_3;
    private Button btnSkill_4;

    private Text txtCd_1;
    private Text txtCd_2;
    private Text txtCd_3;
    private Text txtCd_4;

    private Image imgFillCd_1;
    private Image imgFillCd_2;
    private Image imgFillCd_3;
    private Image imgFillCd_4;
    #endregion

    private void Awake()
    {
        rockingBarAreaTr = transform.Find("LeftBottomPanel/RockingBarArea").GetComponent<RectTransform>();
        rockingBarBgTr = transform.Find("LeftBottomPanel/ImgRockingBar").GetComponent<RectTransform>();
        rockingBarPointTr = transform.Find("LeftBottomPanel/ImgRockingBar/ImgPoint").GetComponent<RectTransform>();
        originPos_rbBg = rockingBarBgTr.anchoredPosition; 

        txtHp = transform.Find("LeftTopPanel/BgHp/TxtHp").GetComponent<Text>(); 
        txtLevel = transform.Find("LeftTopPanel/BgLv/TxtLv").GetComponent<Text>();  
        imgHpFill = transform.Find("LeftTopPanel/BgHp/ImgHpFill").GetComponent<Image>(); 

        btnSkill_0 = transform.Find("RightBottomPanel/BgSkill0/BtnSkill0").GetComponent<Button>();
        btnSkill_1 = transform.Find("RightBottomPanel/BgSkill1/BtnSkill1").GetComponent<Button>();
        btnSkill_2 = transform.Find("RightBottomPanel/BgSkill2/BtnSkill2").GetComponent<Button>();
        btnSkill_3 = transform.Find("RightBottomPanel/BgSkill3/BtnSkill3").GetComponent<Button>();
        btnSkill_4 = transform.Find("RightBottomPanel/BgSkill4/BtnSkill4").GetComponent<Button>(); 
        txtCd_1 = transform.Find("RightBottomPanel/BgSkill1/ImgCD/TxtCD").GetComponent<Text>();
        txtCd_2 = transform.Find("RightBottomPanel/BgSkill2/ImgCD/TxtCD").GetComponent<Text>();
        txtCd_3 = transform.Find("RightBottomPanel/BgSkill3/ImgCD/TxtCD").GetComponent<Text>();
        txtCd_4 = transform.Find("RightBottomPanel/BgSkill4/ImgCD/TxtCD").GetComponent<Text>(); 
        imgFillCd_1 = transform.Find("RightBottomPanel/BgSkill1/ImgCD").GetComponent<Image>();
        imgFillCd_2 = transform.Find("RightBottomPanel/BgSkill2/ImgCD").GetComponent<Image>();
        imgFillCd_3 = transform.Find("RightBottomPanel/BgSkill3/ImgCD").GetComponent<Image>();
        imgFillCd_4 = transform.Find("RightBottomPanel/BgSkill4/ImgCD").GetComponent<Image>();

        AddClickListener();
        RegisterRockingBarListener();
    }

    protected override void InitWnd(object[] args = null)
    {
        base.InitWnd(args);
         
         
        SetActive(rockingBarPointTr, false);
    }

    /// <summary>
    /// 摇杆事件注册
    /// </summary>
    private void RegisterRockingBarListener()
    {
        //这里需要做适配， 根据CanvasScaler的Match属性，根据宽高所占比例，本项目是根据高度
        float hRatio = Constant.RefScreenHeight / (float)Screen.height;
        float wRatio = Constant.RefScreenWidth / (float)Screen.width;
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
            else
            {
                rockingBarPointTr.anchoredPosition = dir;
            }

            //MainCitySys.Instance.SetPlayerMove(dir);
        });

        RegisterPointerUpCB(rockingBarAreaTr.gameObject, (PointerEventData evtData) =>
        {
            rockingBarBgTr.anchoredPosition = originPos_rbBg;
            SetActive(rockingBarPointTr, false);
            rockingBarPointTr.anchoredPosition = Vector2.zero;

           // MainCitySys.Instance.SetPlayerMove(Vector3.zero);
        });
    }


    private void OnBtnSkill_0() {

    }
    private void OnBtnSkill_1()
    {

    }
    private void OnBtnSkill_2()
    {

    }
    private void OnBtnSkill_3()
    {

    }
    private void OnBtnSkill_4()
    {

    }

    private void AddClickListener() {
        btnSkill_0.onClick.AddListener(OnBtnSkill_0);
        btnSkill_1.onClick.AddListener(OnBtnSkill_1);
        btnSkill_2.onClick.AddListener(OnBtnSkill_2);
        btnSkill_3.onClick.AddListener(OnBtnSkill_3);
        btnSkill_4.onClick.AddListener(OnBtnSkill_4);
    }





}
