using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformController : RayCastController
{
    public LayerMask passengerMask;
    public Vector3 move;

    public Vector3[] localWayPoints;
    Vector3[] globalWayPoints;


    public float speed; // speed of platform
    public bool cyclic;
    int fromWayPointIndex;
    float percentBetweenWaypoints;

    List<PassengerMovement> passengerMovement;
    Dictionary<Transform, Controller> passengerDictionary = new Dictionary<Transform, Controller>();


    // Use this for initialization
    public override void Start()
    {
        base.Start();
        globalWayPoints = new Vector3[localWayPoints.Length];

        for (int i = 0; i < localWayPoints.Length; i++)
        {
            globalWayPoints[i] = localWayPoints[i] + transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRaycastOrigins();

        Vector3 velocity = CalculatePlatformMovement();
        CalculatePassengerMovement(velocity);

        MovePassenger(true);
        transform.Translate(velocity);
        MovePassenger(false);
    }


    Vector3 CalculatePlatformMovement()
    {
        fromWayPointIndex %= globalWayPoints.Length;
        int toWaypointIndex = (fromWayPointIndex + 1) % globalWayPoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWayPoints[fromWayPointIndex], globalWayPoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;

        Vector3 newPos = Vector3.Lerp(globalWayPoints[fromWayPointIndex], globalWayPoints[toWaypointIndex], percentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWayPointIndex++;

            if (!cyclic)
            {
                if (fromWayPointIndex >= globalWayPoints.Length - 1)
                {
                    fromWayPointIndex = 0;
                    System.Array.Reverse(globalWayPoints);
                }
            }
        }
        return newPos - transform.position;
    }



    void MovePassenger(bool beforeMovePlatform)
    {
        foreach (PassengerMovement passenger in passengerMovement)
        {
            if (!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller>());
            }

            if (passenger.moveBeforePlatform == beforeMovePlatform)
            {
                passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingPlatform);
                passenger.transform.GetComponent<Controller>().Move(passenger.velocity, passenger.standingPlatform);
            }
        }
    }


    void CalculatePassengerMovement(Vector3 velocity)
    {
        passengerMovement = new List<PassengerMovement>();

        HashSet<Transform> movePassengers = new HashSet<Transform>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // vertically moving platform
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector3 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector3.right * (verticalRaySpacing * i + velocity.x);
                RaycastHit hit;
                bool hitcheck = Physics.Raycast(rayOrigin, Vector3.up * directionY, out hit, rayLength, passengerMask);

                if (hitcheck)
                {
                    if (!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);

                        float pushx = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushx, pushY), directionY == 1, true));
                        //hit.transform.Translate(new Vector3(pushx, pushY));
                    }
                }
            }
        }


        // horizontally moving platform
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector3.up * (horizontalRaySpacing * i);
                RaycastHit hit;
                bool hitcheck = Physics.Raycast(rayOrigin, Vector3.right * directionX, out hit, rayLength, passengerMask);

                if (hitcheck)
                {
                    if (!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);

                        float pushx = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;


                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushx, pushY), false, true));
                        // hit.transform.Translate(new Vector3(pushx, pushY));
                    }
                }
            }



        }

        // passenger on top of a horizontally or downward moving platform
        if (directionY == -1 || velocity.y == 0 || velocity.x != 0)
        {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector3 rayOrigin = raycastOrigins.topLeft + Vector3.right * (verticalRaySpacing * i);
                RaycastHit hit;
                bool hitcheck = Physics.Raycast(rayOrigin, Vector3.right * directionX, out hit, rayLength, passengerMask);

                if (hitcheck)
                {
                    if (!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);

                        float pushx = velocity.x;
                        float pushY = velocity.y;


                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushx, pushY), true, true));
                        // hit.transform.Translate(new Vector3(pushx, pushY));
                    }
                }
            }
        }
        // passenger on top of a horizontally or downward moving platform

        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector3 rayOrigin = raycastOrigins.topLeft + Vector3.right * (verticalRaySpacing * i + velocity.x);
                RaycastHit hit;
                bool hitcheck = Physics.Raycast(rayOrigin, Vector3.up, out hit, rayLength, passengerMask);

                if (hitcheck)
                {
                    if (!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);

                        float pushx = velocity.x;
                        float pushY = velocity.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushx, pushY), true, false));
                        // hit.transform.Translate(new Vector3(pushx, pushY));
                    }
                }
            }

        }

    }

    struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standingPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }

    void OnDrawGizmos()
    {
        if (localWayPoints != null)
        {
            Gizmos.color = Color.red;
            float size = .3f;

            for (int i = 0; i < localWayPoints.Length; i++)
            {
                Vector3 globalWayPointPos = (Application.isPlaying) ? globalWayPoints[i] : localWayPoints[i] + transform.position;
                Gizmos.DrawLine(globalWayPointPos - Vector3.up * size, globalWayPointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWayPointPos - Vector3.left * size, globalWayPointPos + Vector3.left * size);
            }
        }
    }

}
