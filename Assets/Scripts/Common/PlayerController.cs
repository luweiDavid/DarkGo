/****************************************************
    文件：PlayerController.cs
	作者：David
    邮箱: 1785275942@qq.com
    日期：2019/5/29 10:48:2
	功能：Nothing
*****************************************************/

using UnityEngine;

public class PlayerController : MonoBehaviour 
{

    private Animator anim;
    private CharacterController charCtrl;
    private Transform mainCamTr;
    private Vector3 camOffset;

    private bool isMoving = false;
    private Vector3 moveDir;
    public Vector3 MoveDir {
        get {
            return moveDir;
        }
        set {
            moveDir = value;
            if (moveDir != Vector3.zero)
            {
                SetBlend(Constant.MoveBlend);
            }
            else {
                SetBlend(Constant.IdleBlend);
            }
            
        }
    }

    private float curBlend;
    private float targetBlend;

    //private void Awake()
    //{
    //    Init();
    //}

    public void Init()
    {
        anim = GetComponent<Animator>();
        charCtrl = GetComponent<CharacterController>();
        mainCamTr = Camera.main.transform;
        camOffset = transform.position - mainCamTr.position;

        moveDir = Vector3.zero;
    }

    public void Update()
    {
        //float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");
        //MoveDir = new Vector3(x, y, 0);

        if (moveDir != Vector3.zero)
        {
            //开始移动
            isMoving = true; 
        }
        else
        {
            //停止移动
            isMoving = false; 
        }

        if (isMoving)
        {
            OnMove();
        } 
        if (curBlend != targetBlend)
        { 
            UpdateMoveAction();
        }
    }

    public void SetBlend(float blend) {
        targetBlend = blend; 
    }

    private void OnMove() {
        //调整方向
        float angle = Vector2.SignedAngle(moveDir, new Vector2(0, 1));//+ mainCamTr.eulerAngles.y;
        transform.localEulerAngles = new Vector3(0, angle, 0);

        charCtrl.Move(Time.deltaTime * Constant.PlayerMoveSpeed * transform.forward);

        if (mainCamTr != null)
        {
            mainCamTr.position = transform.position - camOffset;
        }
    }

    public void SetMainCamFollowPlayer() {
        if (mainCamTr != null)
        {
            mainCamTr.position = transform.position - camOffset;
        }
    }

    private void UpdateMoveAction()
    {
        if (Mathf.Abs(curBlend - targetBlend) < Constant.AccelerSpeed * Time.deltaTime)
        {
            curBlend = targetBlend;
        }
        else if (curBlend > targetBlend)
        {
            curBlend -= Constant.AccelerSpeed * Time.deltaTime;
        }
        else {
            curBlend += Constant.AccelerSpeed * Time.deltaTime;
        }

        anim.SetFloat("Blend", curBlend);
    }
}