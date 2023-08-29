using UnityEngine;

public class GridSearchUAV : MonoBehaviour
{
    public Terrain terrain;
    public float searchSpeed = 5.0f;
    public float searchHeight = 2.0f;
    public float gridSpacing = 5.0f;
    public float obstacleAvoidanceDistance = 5.0f; // Distance to check for obstacles
    public LayerMask obstacleMask; // Layers to consider as obstacles
    public LayerMask humanMask; // Layers to consider as humans
    public float humanDetectionRadius = 10.0f; // Radius to detect humans

    private Rigidbody rb;
    private bool isSearching;
    private Vector3 nextPoint;
    private bool movingRight = true;
    private int humansDetected; // Counter for humans detected

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.drag = 0.5f;
        rb.angularDrag = 0.5f;

        IsSearching = true;

        nextPoint = transform.position;
        nextPoint.y = terrain.SampleHeight(transform.position) + searchHeight;
    }

    void Update()
    {
        if (isSearching)
        {
            Debug.Log("Performing Search");
            PerformSearch();
        }
        else
        {
            Debug.Log("Search is off");
        }
    }

    public void ToggleSearch()
    {
        IsSearching = !IsSearching;
    }

    public bool IsSearching
    {
        get { return isSearching; }
        set
        {
            isSearching = value;
            rb.useGravity = !isSearching;
            transform.forward = Vector3.forward;
            Debug.Log(isSearching ? "Search is on" : "Search is off");
        }
    }

    private void PerformSearch()
    {
        if (AvoidObstacle())
        {
            return;
        }

        Vector3 newPosition = Vector3.MoveTowards(transform.position, nextPoint, searchSpeed * Time.deltaTime);
        Vector3 movementDirection = newPosition - transform.position;

        if (movementDirection.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(movementDirection.normalized);
        }

        transform.position = newPosition;

        CalculateNextPoint();

        // Detect humans
        DetectHumans();
    }

    private bool AvoidObstacle()
    {
        // Raycast forward to detect obstacles
        if (Physics.Raycast(transform.position, transform.forward, obstacleAvoidanceDistance, obstacleMask))
        {
            // Move up to avoid the obstacle
            transform.position += Vector3.up * searchHeight;
            return true;
        }

        return false;
    }

    private void CalculateNextPoint()
    {
        if (Vector3.Distance(transform.position, nextPoint) < 0.1f)
        {
            if (movingRight)
            {
                nextPoint.x += gridSpacing;
                if (nextPoint.x > terrain.terrainData.size.x - 2.0f)
                {
                    nextPoint.x = terrain.terrainData.size.x - 2.0f;
                    nextPoint.z += gridSpacing;
                    movingRight = false;
                }
            }
            else
            {
                nextPoint.x -= gridSpacing;
                if (nextPoint.x < 2.0f)
                {
                    nextPoint.x = 2.0f;
                    nextPoint.z += gridSpacing;
                    movingRight = true;
                }
            }

            nextPoint.z = Mathf.Clamp(nextPoint.z, 2.0f, terrain.terrainData.size.z - 2.0f);
            nextPoint.y = terrain.SampleHeight(nextPoint) + searchHeight;
        }
    }

    private void DetectHumans()
    {
        Collider[] humanColliders = Physics.OverlapSphere(transform.position, humanDetectionRadius, humanMask);
        humansDetected = humanColliders.Length;
        Debug.Log("Humans Detected: " + humansDetected);
    }
}
