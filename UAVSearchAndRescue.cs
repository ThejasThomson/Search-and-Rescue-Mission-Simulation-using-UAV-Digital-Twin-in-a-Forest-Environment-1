using UnityEngine;

public class UAVSearchAndRescue : MonoBehaviour
{
    public float hoverHeight = 10f; // Desired altitude for the UAV
    public float speed = 5f; // Horizontal speed
    public float verticalSpeed = 2f; // Vertical speed for takeoff
    public float searchGridSize = 50f; // Defines the size of each grid cell
    public int searchResolution = 10; // Defines the number of grid cells
    public LayerMask terrainMask; // Layer for the terrain
    public LayerMask humanMask; // Layer for the human objects

    private Vector3 currentTarget;
    private int currentX;
    private int currentY;

    void Start()
    {
        // Initialize the starting target for the UAV
        currentTarget = GetNextSearchPoint();
    }

    void Update()
    {
        // Check and adjust altitude
        AdjustAltitude();

        // Move the UAV towards the current target
        MoveTowardsTarget();

        // Simulate LIDAR to detect terrain and human objects
        SimulateLidar();

        // Check if the UAV has reached the current target
        if (Vector3.Distance(transform.position, currentTarget) < 1f)
        {
            // Update to the next target in the search grid
            currentTarget = GetNextSearchPoint();
        }
    }

    void AdjustAltitude()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, hoverHeight + 1f, terrainMask))
        {
            float heightError = hoverHeight - (transform.position.y - hit.point.y);
            transform.position += Vector3.up * heightError * verticalSpeed * Time.deltaTime;
        }
    }

    void MoveTowardsTarget()
    {
        // Calculate horizontal direction
        Vector3 horizontalTarget = new Vector3(currentTarget.x, transform.position.y, currentTarget.z);
        Vector3 direction = (horizontalTarget - transform.position).normalized;

        // Move towards the target
        transform.position += direction * speed * Time.deltaTime;
    }

    Vector3 GetNextSearchPoint()
    {
        // Simple grid search pattern
        currentX = (currentX + 1) % searchResolution;
        if (currentX == 0) currentY = (currentY + 1) % searchResolution;

        return new Vector3(currentX * searchGridSize, transform.position.y, currentY * searchGridSize);
    }

    void SimulateLidar()
    {
        // Simulate LIDAR scanning using raycasting
        for (int i = 0; i < 360; i += 10)
        {
            Vector3 direction = Quaternion.Euler(0, i, 0) * transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, terrainMask))
            {
                // Handle terrain detection (optional)
            }

            if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, humanMask))
            {
                // Human detected - you can implement specific logic here
                Debug.Log("Human detected at: " + hit.point);
            }
        }
    }
}

