# Search-and-Rescue-Mission-Simulation-using-UAV-Digital-Twin-in-a-Forest-Environment-1
IMPLEMENTATION AND SIMULATION

1	Model Importation
The initial step of the implementation involved the importation of the DJI Matrice 300 RTK with Zenmuse L1 model into the Unity environment. The model, which was available in the FBX format, ensured that the high-fidelity representations of both the external and internal components of the drone were accurately captured, paving the way for a realistic simulation.

2 Component Attachment and Configuration

Once the model was securely positioned within the Unity workspace:
1. Rigidbody Component: A Rigidbody component was added to the drone model. This component provides realistic physics-based interactions and allows the UAV to respond to forces, hence simulating actual drone flight dynamics.
2. Box Collider: To facilitate accurate collision detection and responses, a Box Collider component was incorporated into the model. This serves as the boundary and interaction point for the drone when navigating environments replete with obstacles.
3. Script Attachment: The UAV's behavior and interactions are driven by scripts. Hence, essential scripts like `UAVController.cs`, `GridSearchUAV.cs`, `UAVManager.cs`, and `UAVBattery.cs` were duly attached to the model, making it ready for both manual and autonomous operations.
4. LiDAR Integration: Within the drone model, an empty GameObject was initialized to represent the LiDAR sensor. This GameObject was anchored at a strategic location to mimic the real-world positioning of a LiDAR system on a UAV. The `LidarSensor.cs` script was then linked to this GameObject, ensuring that the sensor's data capture and storage functions were localized to this point.

3 Layer Creation and Configuration
For clarity and efficient object interaction within the simulation, different layers were established:

- Terrain Layer: Represents the ground or any other base upon which the UAV operates.
- Obstacle Layer: Denotes structures or objects that the UAV must identify and navigate around.
- Human Layer: Specifically designed for the search-and-rescue mission, this layer is populated with human entities that the UAV must detect.
4 Simulation Execution

With all components and layers in place, the simulation was initiated. The UAV promptly began to scan the terrain area in its signature grid-search pattern. During its operational phase, every human entity, strategically scattered across the terrain, was successfully detected, showcasing the efficiency of the search algorithm.

5 LiDAR Point Cloud Generation and Visualization
Post-simulation, the data points captured by the `LidarSensor.cs` script were stored in a text file. This data, essentially a series of X, Y, and Z coordinates representing points detected by the LiDAR, forms the foundation for the 3D point cloud.

For the visualization phase:

- A separate MATLAB script was employed. The script starts by reading the aforementioned data from the "points.txt" file. 
- Using MATLAB's extensive visualization functions, a 3D scatter plot was generated, accurately displaying the LiDAR point cloud map. 
- Colors were mapped based on the Y value, creating a visually pleasing and informative representation of the environment. 
- The resultant graph not only showcased the UAV's path and areas it had covered but also provided insights into the terrain's topography and other salient features. 

