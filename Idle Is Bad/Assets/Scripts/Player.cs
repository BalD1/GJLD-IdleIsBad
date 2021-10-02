using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpHeight = 5;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundMask;

    [Header("Character")]
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool hasKey = false;
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

    private float direction;

    private bool isGrounded;

    private void Awake()
    {
        if(body == null)
            this.gameObject.GetComponent<Rigidbody2D>();
        if(spriteRenderer == null)
            this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(GameManager.Instance.StateOfGame.Equals(GameManager.GameState.InGame))
        {
            CheckSurroundings();
            JumpFunc();
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
            Movements();
    }

    private void Movements()
    {
        direction = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(speed * direction, body.velocity.y);
    }

    private void JumpFunc()
    {
        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            body.velocity = Vector2.up * jumpHeight;
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
    }

    private void Death()
    {
        GameManager.Instance.StateOfGame = GameManager.GameState.GameOver;
    }
}
