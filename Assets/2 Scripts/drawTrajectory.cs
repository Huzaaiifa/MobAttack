using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawTrajectory : MonoBehaviour
{
    public LineRenderer lineRenderer;
    int lineSegmentCount = 20;
    List<Vector3> LinePoints = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTrajectory(Vector3 forceVector, Rigidbody rigidBody, Vector3 startingPoint)
    {
        // Calculate velocity based on force and mass
        Vector3 velocity = forceVector / rigidBody.mass * Time.fixedDeltaTime;

        // Calculate estimated flight duration
        float flightDuration = (2 * velocity.y) / Physics.gravity.y;

        // Calculate time step for each trajectory segment
        float stepTime = flightDuration / lineSegmentCount;

        // Clear existing trajectory points (optional)
        LinePoints.Clear();

        // Loop through trajectory segments (skipping first few for efficiency)
        for (int i = 8; i < lineSegmentCount; i++)
        {
            // Calculate elapsed time for the current segment
            float stepTimePassed = stepTime * i;

            // Calculate movement vector considering velocity and gravity
            Vector3 movementVector = new Vector3(velocity.x * stepTimePassed, velocity.y, velocity.z * stepTimePassed - 8.5f * Physics.gravity.y * stepTimePassed * stepTimePassed);

            // Add current position to trajectory points
            LinePoints.Add(movementVector + startingPoint);
        }

        // Update Line Renderer positions with calculated trajectory points
        lineRenderer.SetPositions(LinePoints.ToArray());
        lineRenderer.positionCount = LinePoints.Count;
    }

    public float FlightDuration(Vector3 initialVelocity)
    {
        // Calculate flight duration based on initial vertical velocity and gravity
        return (2 * initialVelocity.y) / Physics.gravity.y;
    }
}
