using UnityEngine;

public class CarWaypointTracker : MonoBehaviour
{
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private CarController car;
    private int currentWaypointIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waypoint"))
        {
            int waypointIndex = int.Parse(other.gameObject.name);
            if (waypointIndex == currentWaypointIndex + 1)
            {
                currentWaypointIndex = waypointIndex;
                canvasManager.CarPassedWaypoint(car, waypointIndex);
            }
        }
    }
}
