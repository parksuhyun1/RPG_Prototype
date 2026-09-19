using UnityEngine;

public class Hero_Ctrl : MonoBehaviour
{
    //--- 키보드 이동 관련 변수 선언
    float h = 0, v = 0;
    Vector3 m_KeyMvDir = Vector3.zero;
    float rotVelocity = 0.0f;    // SmoothDampAngle에서 속도 누적 저장
    float rotSmoothTime = 0.1f;  // Slerp(0.13f)와 비슷한 반응 속도
                                 //--- 키보드 이동 관련 변수 선언

    //--- 캐릭터 이동 속도 변수
    float m_MoveVelocity = 5.0f;  //평면 초당 이동 속도...
    //--- 캐릭터 이동 속도 변수

    void Awake()
    {
        Camera_Ctrl a_CamCtrl = Camera.main.GetComponent<Camera_Ctrl>();
        if (a_CamCtrl != null)
            a_CamCtrl.InitCamera(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        KeyBDMove();
    }

    void KeyBDMove()
    {
        h = Input.GetAxisRaw("Horizontal"); //화살표키 좌우키를 누르면 -1.0f ~ 1.0f
        v = Input.GetAxisRaw("Vertical");

        if (0.0f != h || 0.0f != v)
        {
            //ClearMsPickMove(); //마우스 클릭 이동 취소

            m_KeyMvDir = new Vector3(h, 0.0f, v);
            //--- 카메라가 바라보고 있는 전면을 기준으로 회전 시켜주는 코드
            Vector3 a_CamFwVec = Camera.main.transform.forward;
            a_CamFwVec.y = 0.0f;
            a_CamFwVec.Normalize();
            m_KeyMvDir = a_CamFwVec * v;
            //위 아래 조작(카메라가 바라보고 있는 기분으로 앞, 뒤(Z성분)로 얼만큼 이동시킬 건지?
            Vector3 a_CamRtVec = Camera.main.transform.right;
            m_KeyMvDir += a_CamRtVec * h;
            //좌우 조작(카메라가 바라보고 있는 기준으로 좌, 우(X성분)로 얼만큼 이동시킬 건지?
            m_KeyMvDir.y = 0.0f;
            //--- 카메라가 바라보고 있는 전면을 기준으로 회전 시켜주는 코드

            ////--- 카메라 좌표계를 기준으로 방향벡터를 계산해 주는 함수
            //m_KeyMvDir = Camera.main.transform.TransformDirection(m_KeyMvDir);
            //m_KeyMvDir.y = 0;
            ////--- 카메라 좌표계를 기준으로 방향벡터를 계산해 주는 함수

            m_KeyMvDir.Normalize();

            //---SmoothDampAngle() 함수 방식
            //float rotSmoothTime = 0.13f;  //0.13초만에 보간 되도록
            float targetAngle = Mathf.Atan2(m_KeyMvDir.x, m_KeyMvDir.z) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                                        targetAngle, ref rotVelocity, rotSmoothTime);
            transform.rotation = Quaternion.Euler(0, smoothedAngle, 0);
            //---SmoothDampAngle() 함수 방식

            transform.position += m_KeyMvDir * m_MoveVelocity * Time.deltaTime;

            //ChangeAnimState(AnimState.move);

        }//if (0.0f != h || 0.0f != v)
    }//void KeyBDMove()
}
