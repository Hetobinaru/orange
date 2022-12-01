using SuperTiled2Unity;
using UnityEngine;

public class Move : MonoBehaviour
{

    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private Animator anim;
    private CollisionDataRetriever ground;

    private bool isFacingRight = true;

    private float maxSpeedChange;
    private float acceleration;
    private bool onGround1;
    private bool isWalking;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<CollisionDataRetriever>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateAnimations();
        CheckMovementDirection();
        direction.x = input.RetrieveMoveInput();
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);
    }

    private void FixedUpdate()
    {
        onGround1 = ground.GetOnGround();
        velocity = body.velocity;

        acceleration = onGround1 ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        body.velocity = velocity;
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && direction.x < 0f)
        {
            Flip();
        }
        else if (!isFacingRight && direction.x > 0f)
        {
            Flip();
        }

        if(body.velocity.x != 0)
        {
            isWalking = true;
           
        }
        else
        {
            isWalking= false;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }
}
