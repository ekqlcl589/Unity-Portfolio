using UnityEngine;
using UnityEngine.SceneManagement;

// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트들이 사용할 수 있도록 제공
public class PlayerInput : MonoBehaviour {
    private string moveAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    private string strafeAxisName  = "Horizontal"; // 좌우 회전을 위한 입력축 이름
    private string rotateAxisName = "Mouse X";
    private string fireButtonName = "Fire1"; // 발사를 위한 입력 버튼 이름
    private string reloadButtonName = "Reload"; // 재장전을 위한 입력 버튼 이름
    private string jumpButtonName = "Jump";
    private string useButtonName = "Use";

    // 값 할당은 내부에서만 가능
    public float move { get; private set; } // 감지된 움직임 입력값
    public float strafe { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; } // 감지된 발사 입력값
    public bool reload { get; private set; } // 감지된 재장전 입력값

    public bool jump { get; private set; } // 감지된 점프 입력값

    public bool Use { get; private set; }
    // 매프레임 사용자 입력을 감지

    private void Start()
    {

    }
    private void Update() {
        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        if (GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            move = Constants.ZERO_FORCE;
            strafe = Constants.ZERO_FORCE;
            rotate = Constants.ZERO_FORCE;
            fire = false;
            reload = false;
            jump = false;
            Use = false;
            return;
        }
        else
        {
            // move에 관한 입력 감지
            move = Input.GetAxis(moveAxisName);
            // rotate에 관한 입력 감지
            rotate = Input.GetAxis(rotateAxisName);

            strafe = Input.GetAxis(strafeAxisName);
            // fire에 관한 입력 감지
            fire = Input.GetButton(fireButtonName);
            // reload에 관한 입력 감지
            reload = Input.GetButton(reloadButtonName);

            jump = Input.GetButtonDown(jumpButtonName);

            Use = Input.GetButtonDown(useButtonName);

        }

        if (SceneManager.GetActiveScene().buildIndex == Constants.BUILD_INDEX2 && !GameManager.Instance.SafeHouse)
        {
            gameObject.transform.position = new Vector3(Constants.NEW_POSITION_X, Constants.NEW_POSITION_Y, Constants.NEW_POSITION_Z);
        }

    }


}