using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    int bullets;

    float XtouchStartPos;
    float YtouchStartPos;
    Vector3 touchStartPos;
    Vector3 touchEndPos;

    private bool isDragging = false;

    float xDrag = 0f;
    float yDrag = 0f;
    Vector3 dragDistance;

    int lineSegmentCount = 20;
    private List<Vector3> linePoints = new List<Vector3>();
    public GameObject arrow;
    public LineRenderer lineRendererr;
    LineRenderer lineRenderer;
    public int trajectoryResolution = 30;
    public float dragMultiplier = 0.15f; // Adjust this value to reduce sensitivity
    public float forceMultiplier = 1f; // Adjust this to control the force applied
    public float trajectoryPercentage = 1f; // Show 75% of the trajectory

    [Header("Display Controls")]
    [SerializeField]
    [Range(10, 100)]
    private int LinePoints = 25;
    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float TimeBetweenPoints = 0.1f;

    public GameObject markerPrefab; // Add this line
    private GameObject currentMarker; // Add this line

    public float trajectoryOffset = 100f; // Adjust this value as needed
    public float initialDragOffset = 1f; // Adjust this value to simulate initial drag

    private Color defaultStartColor;
    private Color defaultEndColor;
    public Color hitColor = Color.green;
    private Gradient defaultGradient;
    private Gradient hitGradient;
    public Color hitColorStart = Color.green;
    public Color hitColorEnd = Color.yellow;

    GameObject gameManager;

    AudioSource wooshAudio;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        wooshAudio = gameObject.GetComponent<AudioSource>();
        lineRenderer = Instantiate(lineRendererr).GetComponent<LineRenderer>();
        defaultGradient = lineRenderer.colorGradient;
        hitGradient = new Gradient();
        hitGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(hitColorStart, 0.0f), new GradientColorKey(hitColorEnd, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });
        lineRenderer.positionCount = trajectoryResolution;

        // Initialize the marker but keep it inactive
        if (markerPrefab != null)
        {
            currentMarker = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity);
            currentMarker.SetActive(false);
        }

        defaultStartColor = lineRenderer.startColor;
        defaultEndColor = lineRenderer.endColor;
    }

    void Update()
    {
        if (bullets > 0)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began) // Handle touch began
                {
                    XtouchStartPos = touch.position.x;
                    YtouchStartPos = touch.position.y + 130f;
                    touchStartPos = touch.position;


                    isDragging = true;
                    lineRenderer.enabled = true;

                    // Enable the marker
                    if (currentMarker != null)
                    {
                        currentMarker.SetActive(true);
                    }
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    touchEndPos = touch.position;

                    // Apply initial drag offset
                    //                Vector3 initialOffset = new Vector3(initialDragOffset, 0, 0);
                    dragDistance = (touchStartPos - touchEndPos) * dragMultiplier;
                    xDrag = (touchEndPos.x - XtouchStartPos) * dragMultiplier;
                    yDrag = (YtouchStartPos - touchEndPos.y) * dragMultiplier;

                    // Calculate and draw trajectory
                    Vector3 force = dragDistance * forceMultiplier;
                    force.x = xDrag;
                    force.y = yDrag * 0.75f;
                    float zDrag = Mathf.Sqrt(Mathf.Pow(xDrag, 2) + Mathf.Pow(yDrag, 2));
                    force.z = zDrag;
                    Vector3 direction = new Vector3(-force.x, force.y, force.z).normalized;

                    // Create a quaternion looking at the desired direction
                    Quaternion lookRotation = Quaternion.LookRotation(direction);

                    // Apply the rotation to the object
                    transform.rotation = lookRotation;

                    DrawProjection(force);
                }
                else if (touch.phase == TouchPhase.Ended) // Handle touch ended
                {
                    isDragging = false;
                    dragDistance = (touchStartPos - touchEndPos) * dragMultiplier;

                    // Instantiate and launch the arrow
                    bullets--;
                    GameObject bullet = Instantiate(arrow, gameObject.transform.position, transform.rotation);
                    if (wooshAudio != null)
                    {
                        wooshAudio.PlayOneShot(wooshAudio.clip, 0.5f);
                    }
                    if (bullets == 0)
                    {
                        bullet.GetComponent<move>().last = true;
                    }


                    Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward);
                    transform.rotation = Quaternion.Euler(0, 0, 0);


                    bullet.GetComponent<move>().force = dragDistance * forceMultiplier;
                    bullet.GetComponent<move>().xDrag = xDrag * forceMultiplier;
                    bullet.GetComponent<move>().yDrag = yDrag * forceMultiplier;

                    // Clear the trajectory and disable the marker
                    lineRenderer.positionCount = 0;
                    lineRenderer.enabled = false;

                    if (currentMarker != null)
                    {
                        currentMarker.SetActive(false);
                    }
                }
            }
            else
            {
                // If no touch, ensure the marker and line renderer are disabled
                if (currentMarker != null)
                {
                    currentMarker.SetActive(false);
                }
                lineRenderer.enabled = false;
            }

        }
    }


    private Vector3 lastEndPosition; // To keep track of the last position to avoid unnecessary updates


    private void DrawProjection(Vector3 force)
    {
        float mass = 1f;
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;
        Vector3 startPosition = gameObject.transform.position;
        Vector3 startVelocity = force / mass;
        startVelocity.x *= -1;

        int i = 0;
        lineRenderer.SetPosition(i, startPosition);

        Vector3 lastPosition = startPosition;
        bool hitTarget = false;

        for (float time = 0; time < LinePoints * trajectoryPercentage; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            lineRenderer.SetPosition(i, point);

            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude, LayerMask.GetMask("Default", "Obstacle"))) // Adjust layer mask as needed
            {
                lineRenderer.SetPosition(i, hit.point);
                lineRenderer.positionCount = i + 1;

                // Check if the hit object has the target tag
                if (hit.collider.CompareTag("obstacle"))
                {
                    hitTarget = true;
                }

                // Instantiate or move the marker at the hit point
                if (markerPrefab != null)
                {
                    if (currentMarker == null)
                    {
                        currentMarker = Instantiate(markerPrefab, hit.point, Quaternion.identity);
                    }
                    else
                    {
                        currentMarker.transform.position = hit.point;
                    }
                }

                lastEndPosition = hit.point; // Update the last end position
                break; // Exit the loop since we hit an obstacle
            }

            lastPosition = point;
        }

        // If no hit, place marker at the last point of the trajectory
        Vector3 endPosition = lineRenderer.GetPosition(i - 1);

        if (markerPrefab != null)
        {
            if (currentMarker == null)
            {
                currentMarker = Instantiate(markerPrefab, endPosition, Quaternion.identity);
            }
            else if (endPosition != lastEndPosition)
            {
                currentMarker.transform.position = endPosition;
                lastEndPosition = endPosition; // Update the last end position
            }
        }

        // Apply the appropriate gradient based on hit or not
        lineRenderer.colorGradient = hitTarget ? hitGradient : defaultGradient;
    }

    public void setBullets(int x)
    {
        bullets = x;
    }
    public int getBullets()
    {
        return bullets;
    }
}
