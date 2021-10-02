using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpHeight = 5;
    [SerializeField] private float jumpTime;
    private float jumpTimeCounter;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckLength;
    [SerializeField] private LayerMask groundMask;

    [Header("Character")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool hasKey = false;
    [SerializeField] private Control currentControl;
    [SerializeField] private Vector2 squashAndStetchMax;
    [SerializeField] private Vector2 squashAndStetchMin;
    [SerializeField] private float squashAndStretchTime = 3f;
    [SerializeField] private float timeBeforeNormal = 0.5f;

    private bool isJumping, isGrounded, goToNormalScale;

    private float direction, squashAndStretchTimer;

    private Vector2 newColliderSize;

    [Header("Camera")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float cameraSpeed = 50;
    [SerializeField] private Vector2 cameraClamp;
    [SerializeField] [Range(0, 1)] private float timeSlow;
    private Vector2 borders;
    private Vector3 cameraDirection, resetedCameraPos;

    private enum Control
    {
        Player,
        Camera,
    }

    public bool HasKey
    {
        get
        {
            return hasKey;
        }
        set
        {
            hasKey = value;
        }
    }


    private void Awake()
    {
        if(body == null)
            this.gameObject.GetComponent<Rigidbody2D>();
        if(spriteRenderer == null)
            this.gameObject.GetComponent<SpriteRenderer>();
        if(playerCamera == null)
            playerCamera = Camera.main;

        goToNormalScale = false;
        squashAndStretchTimer = timeBeforeNormal;
    }

    void Update()
    {
        currentControl = Input.GetKey(KeyCode.LeftShift) ? Control.Camera : Control.Player;
        if(currentControl == Control.Camera)
            Time.timeScale = timeSlow;
        else if(currentControl == Control.Player)
        {
            Time.timeScale = 1;
            resetedCameraPos = this.transform.position;
            resetedCameraPos.z = playerCamera.transform.position.z;
            playerCamera.transform.position = resetedCameraPos;
        }

        if(GameManager.Instance.StateOfGame.Equals(GameManager.GameState.InGame))
        {
            CheckSurroundings();
            Jump();

            if(currentControl == Control.Camera)
                CameraMovements();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameManager.Instance.StateOfGame.Equals(GameManager.GameState.InGame))
                GameManager.Instance.StateOfGame = GameManager.GameState.Pause;
            else
                GameManager.Instance.StateOfGame = GameManager.GameState.InGame;
        }

        if(this.transform.position.y <= -5)
            Death();
    }
    private void FixedUpdate()
    {
        if(GameManager.Instance.StateOfGame.Equals(GameManager.GameState.InGame))
        {
            if(currentControl == Control.Player)
                PlayerMovements();
        }
    }

    private void PlayerMovements()
    {
        direction = Input.GetAxis("Horizontal");
        if(direction < 0)
            spriteRenderer.flipX = true;
        else if (direction > 0)
            spriteRenderer.flipX = false;
        body.velocity = new Vector2(speed * direction, body.velocity.y);
    }

    private void CameraMovements()
    {
        cameraDirection.x = Input.GetAxis("Horizontal");
        cameraDirection.y = Input.GetAxis("Vertical");

        cameraDirection.x *= cameraSpeed * Time.fixedDeltaTime;
        cameraDirection.y *= cameraSpeed * Time.fixedDeltaTime;

        playerCamera.transform.localPosition = new Vector3
        (
            Mathf.Clamp(playerCamera.transform.localPosition.x + cameraDirection.x, -cameraClamp.x, cameraClamp.x),
            Mathf.Clamp(playerCamera.transform.localPosition.y + cameraDirection.y, -cameraClamp.y, cameraClamp.y),
            playerCamera.transform.position.z
        );
    }

    private void Jump()
    {
        SquashAndStretch();

        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            body.velocity = Vector2.up * jumpHeight;
        }
        if (Input.GetButton("Jump") && isJumping)
        {
            if(jumpTimeCounter > 0)
            {
                body.velocity = Vector2.up * jumpHeight;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
                isJumping = false;
        }

        if(Input.GetButtonUp("Jump"))
            isJumping = false;
    }

    private void SquashAndStretch()
    {
        newColliderSize.x = spriteRenderer.transform.localScale.x;
        newColliderSize.y = boxCollider.size.y;
        boxCollider.size = newColliderSize;
        if(!isGrounded)
        {
            spriteRenderer.transform.localScale = Vector3.Lerp(spriteRenderer.transform.localScale, squashAndStetchMax, Time.deltaTime * squashAndStretchTime);
            goToNormalScale = false;
            squashAndStretchTimer = timeBeforeNormal;
        }
        else if(isGrounded && !goToNormalScale)
        {
            squashAndStretchTimer -= Time.deltaTime;
            spriteRenderer.transform.localScale = Vector3.Lerp(spriteRenderer.transform.localScale, squashAndStetchMin, Time.deltaTime * squashAndStretchTime);
            if(squashAndStretchTimer <= 0)
                goToNormalScale = true;
        }
        else if (isGrounded && goToNormalScale)
        {
            spriteRenderer.transform.localScale = Vector3.Lerp(spriteRenderer.transform.localScale, Vector3.one, Time.deltaTime * squashAndStretchTime);
            if(spriteRenderer.transform.localScale.Equals(Vector3.one))
                goToNormalScale = false;
        }
    }

    private void CheckSurroundings()
    {
        
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, groundCheckLength, groundMask);
        Color rayColor;
        if(raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
            rayColor = Color.red;

        Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + groundCheckLength), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + groundCheckLength), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y), Vector2.right * (boxCollider.bounds.extents.x), rayColor);
        isGrounded = raycastHit.collider != null;
    }

    private void Death()
    {
        GameManager.Instance.StateOfGame = GameManager.GameState.GameOver;
    }
}
