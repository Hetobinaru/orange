using UnityEngine;
using UnityEngine.UIElements;

public class Jump : MonoBehaviour
{

    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
    [SerializeField, Range(0f, 5f)] private int maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.5f;
    [SerializeField, Range(0f, 0.3f)] private float coyoteTime = 0.13f;
    [SerializeField, Range(0f, 0.3f)] private float jumpBufferTime = 0.05f;

    private Rigidbody2D body;
    private Animator anim;
    private CollisionDataRetriever ground;
    private Vector2 velocity;

    private int jumpPhase;
    private float defaultGravityScale, jumpSpeed, coyoteCounter, jumpBufferCounter;

    private bool desiredJump, onGround, isJumping;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ground = GetComponent<CollisionDataRetriever>();

        defaultGravityScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimations();
        desiredJump |= input.RetrieveJumpInput();
    }

    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();
        velocity = body.velocity;

        

        if (onGround && body.velocity.y == 0)
        {
            jumpPhase = 0;
            coyoteCounter = coyoteTime;
            isJumping = false;

        }
            else
        {
            coyoteCounter -= Time.deltaTime;
        }




        if (desiredJump)
        {
            desiredJump = false;
            jumpBufferCounter = jumpBufferTime;
        }
        else if (!desiredJump && jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

      

        if (jumpBufferCounter >0)
        {
            JumpAction();
        }


        if (body.velocity.y > 0f)
        {
            body.gravityScale = upwardMovementMultiplier;
        }
        else if (body.velocity.y < 0f)
        {
            body.gravityScale = downwardMovementMultiplier;
        }
        else if (body.velocity.y == 0f)
        {
            body.gravityScale = defaultGravityScale;
        }

        body.velocity = velocity;
    }

    private void JumpAction()
    {
 

        if (coyoteCounter >0f || jumpPhase < maxAirJumps && isJumping)
        {
            if(isJumping)
            {
                jumpPhase += 1;
            }

            jumpBufferCounter = 0;
            coyoteCounter = 0;
            

            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            isJumping = true;
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f)
            {
                velocity.y = 0f;
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);

            }

            velocity.y += jumpSpeed;
        }



    }

    private void UpdateAnimations()
    {
        anim.SetBool("isGrounded", onGround);
        anim.SetFloat("yVelocity", body.velocity.y);
    }
}
