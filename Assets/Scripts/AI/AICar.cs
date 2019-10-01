using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum Direction
{
    Up = 1,
    Down = 2,
    Left = 4,
    Right = 8,
}

[SelectionBase]
public class AICar : MonoBehaviour
{
    IntersectionWaypoint lastWaypoint;
    public float distanceThreshold;
    public float speed;

    public Direction currentDirection;

    // Start is called before the first frame update
    void Start()
    {
        SetDirection(currentDirection);
    }

    // Update is called once per frame
    void Update()
    {
        //We're in 2D so we use transform.up instead of transfor.forward
        transform.position = transform.position + (transform.up * speed * Time.deltaTime);

        IntersectionWaypoint closestWaypoint = GetClosestWaypoint();
        if(closestWaypoint != null && closestWaypoint != lastWaypoint)
        {
            ChooseNewDirection(closestWaypoint);
        }
    }

    IntersectionWaypoint GetClosestWaypoint()
    {
        IntersectionWaypoint[] allWaypoints = FindObjectsOfType<IntersectionWaypoint>();
        for(int i = 0; i < allWaypoints.Length; i++)
        {
            if(Vector3.Distance(transform.position, allWaypoints[i].transform.position) < distanceThreshold)
            {
                return allWaypoints[i];
            }
        }

        return null;
    }

    void ChooseNewDirection(IntersectionWaypoint waypoint)
    {
        List<Direction> possibleDirections = new List<Direction>();
        if ((waypoint.connectedRoads & Direction.Up     ) != 0) possibleDirections.Add(Direction.Up     );
        if ((waypoint.connectedRoads & Direction.Down   ) != 0) possibleDirections.Add(Direction.Down   );
        if ((waypoint.connectedRoads & Direction.Left   ) != 0) possibleDirections.Add(Direction.Left   );
        if ((waypoint.connectedRoads & Direction.Right  ) != 0) possibleDirections.Add(Direction.Right  );

        //Only turn around (180 deg) if there's nowhere else to go
        if(possibleDirections.Count > 1)
        {
            if (currentDirection == Direction.Up) possibleDirections.Remove(Direction.Down);
            if (currentDirection == Direction.Down) possibleDirections.Remove(Direction.Up);
            if (currentDirection == Direction.Left) possibleDirections.Remove(Direction.Right);
            if (currentDirection == Direction.Right) possibleDirections.Remove(Direction.Left);
        }

        SetDirection(possibleDirections[Random.Range(0, possibleDirections.Count)]);

        SnapToWaypointAxis(waypoint);

        lastWaypoint = waypoint;
    }

    void SetDirection(Direction direction)
    {
        if (direction == Direction.Up) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (direction == Direction.Down) transform.rotation = Quaternion.Euler(0, 0, 180);
        if (direction == Direction.Left) transform.rotation = Quaternion.Euler(0, 0, 90);
        if (direction == Direction.Right) transform.rotation = Quaternion.Euler(0, 0, -90);

        currentDirection = direction;
    }

    void SnapToWaypointAxis(IntersectionWaypoint waypoint)
    {
        Vector3 pos = transform.position;
        if (currentDirection == Direction.Up || currentDirection == Direction.Down) pos.x = waypoint.transform.position.x;
        if (currentDirection == Direction.Right || currentDirection == Direction.Left) pos.y = waypoint.transform.position.y;

        transform.position = pos;
    }

    private void OnValidate()
    {
        //Update the orientation of the car when editing it's direction in the inspector
        SetDirection(currentDirection);
    }
}
