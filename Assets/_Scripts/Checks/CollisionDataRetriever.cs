using UnityEngine;

public class CollisionDataRetriever : MonoBehaviour
{

    public bool onGround { get; private set; }
    public bool onWall { get; private set; }
    public float friction { get; private set; }

    public Vector2 contactNormal {get; private set;}    
    private PhysicsMaterial2D material;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround =false;
        friction= 0.0f;
        onWall= false;
    }

    public void EvaluateCollision(Collision2D collision)
    {
        for(int i=0; i < collision.contactCount; i++)
        {
            contactNormal = collision.GetContact(i).normal;
            onGround |= contactNormal.y >= 0.9f;
            onWall = Mathf.Abs(contactNormal.x) >= 0.9f;
        }
    }

    private void RetrieveFriction(Collision2D collision)
    {
       material = collision.rigidbody.sharedMaterial;

        friction = 0;
        if(material != null)
        {
            friction = material.friction;
        }
    }

    public bool GetOnGround()
    {
        return onGround;
    }

    public float GetFriction()
    {
        return friction;
    }
}
