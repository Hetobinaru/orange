using UnityEngine;

public class WallInteractor : MonoBehaviour
{

    [Header("Wall Slide")]
    [SerializeField]
    [Range(0.1f, 5f)] private float wallSlideMaxSpeed = 2f;

    private CollisionDataRetriever collisionDataRetriever;
    private Rigidbody2D body;

    private Vector2 velocity;
    private bool onWall;

    // Start is called before the first frame update
    void Start()
    {  
        collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;
        onWall = collisionDataRetriever.onWall;

        #region Wall Slide
        if(onWall )
        {
            if(velocity.y < -wallSlideMaxSpeed)
            {
                velocity.y = -wallSlideMaxSpeed;
            }
        }
        #endregion

        body.velocity = velocity;
    }
}
