using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Vector2 moveInput;
    private Player player;
    private bool isMove;

    private float minX;
    private float maxX;

    private void Awake()
    {
        player = GetComponent<Player>();

        isMove = false;
    }

    private void Start()
    {
        InitMoveLimit();
    }

    private void FixedUpdate()
    {
        if (isMove)
        {
            Vector2 pos = player.transform.position;
            pos.x += moveInput.x * player.MoveSpeed * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            player.transform.position = pos;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector2>();
            isMove = true;
            player.MoveAnimation(isMove, moveInput);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
            isMove = false;
            player.MoveAnimation(isMove, moveInput);
        }
    }

    private void InitMoveLimit()
    {
        Camera maincamera = Camera.main;
        Vector2 leftBottom = maincamera.ViewportToWorldPoint(new Vector3(0, 0));
        Vector2 rightTop = maincamera.ViewportToWorldPoint(new Vector3(1, 1));

        minX = leftBottom.x;
        maxX = rightTop.x;
    }
}
