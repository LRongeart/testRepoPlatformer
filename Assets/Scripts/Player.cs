using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float Smoothing;
    [SerializeField] private float JumpForce = 6f;
    [SerializeField] private float FallGravityScaleMultiplier = 2f;
    [SerializeField] private float JumpGravityScaleMultiplier = 0.6f;
    [SerializeField] private float GroundCheckWidth;
    [SerializeField] private float GroundCheckHeight;
    [SerializeField] private float WallCheckWidth;
    [SerializeField] private float WallCheckHeight;
    [SerializeField] private float CoyoteTime;
    private Rigidbody2D RbPlayer;
    private int HorizontalInput;
    private Vector2 velocity = Vector2.zero;
    private Vector2 GroundCheckPosition;
    private Vector2 WallCheckPositionLeft;
    private Vector2 WallCheckPositionRight;
    private float OriginalGravityScale;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask JumpLayerMask;

    [SerializeField] private bool isTouchingWallLeft;
    [SerializeField] private bool isTouchingWallRight;


    // Start is called before the first frame update
    void Start()
    {
        RbPlayer = GetComponent<Rigidbody2D>();
        OriginalGravityScale = RbPlayer.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCheckPosition();
        HorizontalInput = (int)Input.GetAxisRaw("Horizontal");

        Collider2D col = Physics2D.OverlapBox(GroundCheckPosition, new Vector2(GroundCheckWidth, GroundCheckHeight), 0, JumpLayerMask);
       
        if (col != null)
        {
            isGrounded = true;
        }
        else if(isGrounded)
        {
           StartCoroutine(ChangeIsGroundedState(false));
        }
        // Ou plus court :
        // isGrounded = (col != null);


        if (Input.GetButton("Jump") && isGrounded)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
    }
    private void FixedUpdate()
    {
        Collider2D colLeft = Physics2D.OverlapBox(WallCheckPositionLeft, new Vector2(WallCheckWidth, WallCheckHeight), 0, JumpLayerMask);
        isTouchingWallLeft = (colLeft != null);

        Collider2D colRight = Physics2D.OverlapBox(WallCheckPositionRight, new Vector2(WallCheckWidth, WallCheckHeight), 0, JumpLayerMask);
        isTouchingWallRight = (colRight != null);

        if (!((HorizontalInput > 0 && isTouchingWallRight) || (HorizontalInput < 0 && isTouchingWallLeft)))
        {
            Move();
        }

        /*
        if (!(HorizontalInput > 0 && isTouchingWallRight) && !(HorizontalInput < 0 && isTouchingWallLeft))
        {
            Move();
        }
        */

        if (isJumping)
        {
            Jump();
        }

        if (RbPlayer.velocity.y < 0)
        {
            // On est en chute
            RbPlayer.gravityScale = OriginalGravityScale * FallGravityScaleMultiplier;
        }
        else
        {
            if (Input.GetButton("Jump"))
            {
                // On monte plus vite
                RbPlayer.gravityScale = OriginalGravityScale * JumpGravityScaleMultiplier;
            }
            else
            {
                // On monte ou on est posé
                RbPlayer.gravityScale = OriginalGravityScale;
            }
        }
    }

    private IEnumerator ChangeIsGroundedState(bool isGroundedState)
    {
        yield return new WaitForSeconds(CoyoteTime);
        isGrounded = isGroundedState;
    }

    private void Move()
    {
        Vector2 targetVelocity = new Vector2(HorizontalInput * Speed, RbPlayer.velocity.y);
        RbPlayer.velocity = Vector2.SmoothDamp(RbPlayer.velocity, targetVelocity, ref velocity, Smoothing);
    }

    private void Jump()
    {
        RbPlayer.velocity = new Vector2(RbPlayer.velocity.x, JumpForce);
        isGrounded = false;
    }

    private void UpdateCheckPosition()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float largeur = spriteRenderer.bounds.size.x;
        float hauteur = spriteRenderer.bounds.size.y;
        GroundCheckPosition = new Vector2(transform.position.x,transform.position.y - hauteur/2f);
        WallCheckPositionLeft = new Vector2(transform.position.x - largeur/2f, transform.position.y);
        WallCheckPositionRight = new Vector2(transform.position.x + largeur / 2f, transform.position.y);
    }

    private void OnDrawGizmos()
    {
        UpdateCheckPosition();
        Gizmos.color = new Color(0, 1f, 0, 0.2f);
        Gizmos.DrawCube(GroundCheckPosition, new Vector2(GroundCheckWidth, GroundCheckHeight));
        Gizmos.DrawCube(WallCheckPositionLeft, new Vector2(WallCheckWidth, WallCheckHeight));
        Gizmos.DrawCube(WallCheckPositionRight, new Vector2(WallCheckWidth, WallCheckHeight));
    }
}