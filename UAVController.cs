using UnityEngine;

public class UAVController : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    public float maxPitchAngle = 30.0f;
    public float maxAscentSpeed = 6.0f;
    public float maxDescentSpeedVertical = 5.0f;
    public float maxDescentSpeedTilt = 7.0f;
    public float maxSpeed = 23.0f;
    public float altitudeHoldStrength = 10.0f;
    public float obstacleAvoidanceDistance = 5.0f; // Distance to check for obstacles
    public LayerMask obstacleMask; // Layers to consider as obstacles
    public Rigidbody rb;

    private bool isAltitudeHoldActive = false;
    private float targetAltitude = 0.0f;

    private void Update()
    {
        // Check for obstacles in front of the UAV
        Vector3 avoidanceDirection = AvoidObstacle();
        if (avoidanceDirection != Vector3.zero)
        {
            // Apply avoidance force
            rb.AddRelativeForce(avoidanceDirection, ForceMode.Acceleration);
        }

        // Check for Altitude Hold input
        if (Input.GetButtonDown("AltitudeHold"))
        {
            ToggleAltitudeHold();
        }

        if (!isAltitudeHoldActive)
        {
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");
            float rotationInput = Input.GetAxis("Rotation");
            float throttleInput = Input.GetAxis("Throttle");

            RotateZ(horizontalInput);
            RotateY(rotationInput);

            ApplyThrottle(throttleInput);
            ApplyPitch(verticalInput);
        }
        else
        {
            AdjustAltitudeHold();

            float horizontalInput = Input.GetAxis("Horizontal");
            float rotationInput = Input.GetAxis("Rotation");

            RotateZ(horizontalInput);
            RotateY(rotationInput);
        }
    }

    private Vector3 AvoidObstacle()
    {
        // Cast a ray forward from the UAV's position
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // If the ray hits an obstacle within the defined distance
        if (Physics.Raycast(ray, out hit, obstacleAvoidanceDistance, obstacleMask))
        {
            Debug.Log("Obstacle detected! Avoiding...");
            // Determine the direction to avoid the obstacle
            Vector3 avoidanceDirection = transform.position - hit.point;
            avoidanceDirection.y = 0; // Consider only horizontal avoidance


            return avoidanceDirection.normalized * maxSpeed;
        }

        return Vector3.zero; // No obstacle detected
    }

    private void RotateZ(float amount)
    {
        transform.Rotate(Vector3.forward, amount * -rotationSpeed);
    }

    private void RotateY(float amount)
    {
        transform.Rotate(Vector3.up, amount * rotationSpeed);
    }

    private void ApplyThrottle(float amount)
    {
        Vector3 force = new Vector3(0, amount * maxSpeed, 0);
        rb.AddRelativeForce(force, ForceMode.Acceleration);
    }

    private void ApplyPitch(float amount)
    {
        float pitchAngle = Mathf.Clamp(amount * maxPitchAngle, -maxPitchAngle, maxPitchAngle);
        transform.Rotate(Vector3.right, pitchAngle * Time.deltaTime * rotationSpeed);
    }

    private void AdjustAltitudeHold()
    {
        float altitudeError = targetAltitude - transform.position.y;
        float requiredThrust = altitudeError * altitudeHoldStrength;

        rb.AddForce(Vector3.up * requiredThrust, ForceMode.Acceleration);
    }

    private void ToggleAltitudeHold()
    {
        isAltitudeHoldActive = !isAltitudeHoldActive;
        if (isAltitudeHoldActive)
        {
            targetAltitude = transform.position.y;
        }
    }
}

