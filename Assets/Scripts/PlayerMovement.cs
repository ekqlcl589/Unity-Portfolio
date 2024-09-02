using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour {

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    [SerializeField]
    private Camera mainCamera;

    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        playerRigidbody.interpolation = RigidbodyInterpolation.Interpolate; // Interpolate 설정
        playerRigidbody.drag = 0f;
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate() {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행, 기본값은 0.02 
        Move();

        RotateToMouse();

        HandleAnimation();

        Use();

        Roll();

        PickUp();

        //playerAnimator.SetFloat("Speed", playerInput.move);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move() {
        Vector3 moveForward = playerInput.move * transform.forward; 
        Vector3 moveStrafe = playerInput.strafe * transform.right;

        Vector3 moveDirection = moveForward + moveStrafe;

        if (playerInput.move != 0 && playerInput.strafe != 0)
        {
            moveDirection = moveDirection.normalized;
        }

        Vector3 moveVelocity = moveDirection * Constants.MOVE_SPEED * Time.deltaTime;

        playerRigidbody.MovePosition(playerRigidbody.position + moveVelocity);

        float combinedSpeed = Mathf.Sqrt(Mathf.Pow(playerInput.move, 2) + Mathf.Pow(playerInput.strafe, 2));

        playerAnimator.SetFloat("Speed", combinedSpeed);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate() {
        float turn = playerInput.strafe * Constants.ROATATE_SPEED * Time.deltaTime;
        
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(Constants.ZERO_FORCE, turn, Constants.ZERO_FORCE);
    }

    private void RotateToMouse()
    {
        if (mainCamera is null || playerRigidbody is null)
            return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 pointToLook = ray.GetPoint(rayDistance);
            Vector3 direction = (pointToLook - transform.position).normalized;
            direction.y = 0; // y축은 회전하지 않도록

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerRigidbody.rotation = Quaternion.RotateTowards(playerRigidbody.rotation, targetRotation, 360f * Time.deltaTime);

        }
    }
    private void HandleAnimation()
    {
        bool isRotating = Mathf.Abs(playerInput.rotate) > Mathf.Epsilon;
        bool isMoving = Mathf.Abs(playerInput.move) > Mathf.Epsilon;
        
        if (isRotating is true && isMoving is false)
        {
            playerAnimator.SetBool("isRotating", true);
        }
        else
        {
            playerAnimator.SetBool("isRotating", false);
        }
    }
    //입력값에 따라 캐릭터 점프
    private void Jump()
    {
        playerRigidbody.velocity = Vector3.zero;

        playerRigidbody.AddForce(new Vector3(Constants.ZERO_FORCE, Constants.JUMP_FORCE, Constants.ZERO_FORCE));

    }

    private void Use()
    {
        if (Input.GetKeyUp(KeyCode.E))
            playerAnimator.SetTrigger("Use");      
    }
    private void Roll()
    {
        if (Input.GetKeyUp(KeyCode.T))
            playerAnimator.SetTrigger("Roll");
    }

    private void PickUp()
    {
        if (Input.GetKeyUp(KeyCode.Y))
            playerAnimator.SetTrigger("Pickup");
    }

}