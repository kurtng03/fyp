using UnityEngine;
using System.Collections;

public class Controller : RayCastController
{
    //e04
    float maxClimbAngle = 80;
    //e04 end

    //e05
    float maxDescendAngle = 75;
    //e05 end


    // E03
    public CollisionInfo collisions;
    // e03 end

    public override void Start()
    {
        base.Start();
    }



    public void Move(Vector3 velocity, bool standingInPlatform = false)
    {
        UpdateRaycastOrigins();

        //e03 start
        collisions.Reset();
        //e03 end

        //e05
        collisions.velocityOld = velocity;

        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
        //e05 end 


        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);

        if (standingInPlatform)
        {
            collisions.below = true;
        }

    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector3.up * (horizontalRaySpacing * i);
            RaycastHit hit;
            bool hitcheck = Physics.Raycast(rayOrigin, Vector3.right * directionX, out hit, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector3.right * directionX * rayLength, Color.red);

            if (hitcheck)
            {
                if (hit.distance == 0)
                {
                    continue;
                }

                //e04 攞撞到既野既角度
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    //e05
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }
                    //e05 end
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {

                    //e04 end

                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;


                    //e03

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;

                    //e03 end

                }
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector3 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector3.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit hit;
            bool hitcheck = Physics.Raycast(rayOrigin, Vector3.up * directionY, out hit, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector3.up * directionY * rayLength, Color.red);

            if (hitcheck)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                //e04 
                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                //e03 start
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
                //e03 end
            }
        }

        //e05
        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector3 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector3.up * velocity.y;
            RaycastHit hit;
            bool hitcheck = Physics.Raycast(rayOrigin, Vector3.right * directionX, out hit, rayLength, collisionMask);

            if (hitcheck)
            {
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
        //e05 end

    }



    // E03 start

    public struct CollisionInfo
    {

        public bool above, below;
        public bool left, right;
        //e05
        public bool descendingSlope;
        public Vector3 velocityOld;
        //e05 end
        public bool climbingSlope;
        public float slopeAngle, slopeAngleOld;

        public void Reset()
        {

            above = below = false;
            left = right = false;
            //e04
            climbingSlope = false;

            //e05
            descendingSlope = false;
            //e05 end


            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
            //e04 end


        }

    }


    // E03 end

    //e04
    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    //e04 end

    //e05
    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit hit;
        bool hitcheck = Physics.Raycast(rayOrigin, -Vector3.up, out hit, Mathf.Infinity, collisionMask);

        if (hitcheck)
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }
    //e05 end

}