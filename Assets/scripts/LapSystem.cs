using UnityEngine;

public class LapSystem : MonoBehaviour
{
    [SerializeField] private int totalLaps = 1; // Define the max number of laps as 1
    [SerializeField] private Transform[] checkpoints; // Array of checkpoints

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("car 1") || other.CompareTag("car 2")) // Check if a car crosses
        {
            CarController carController = other.GetComponent<CarController>();

            // Check if the car is crossing a checkpoint
            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (other.transform.position == checkpoints[i].position)
                {
                    // Check if the car is crossing the next checkpoint in sequence
                    if (carController.lastPassedWaypoint == i - 1 || (carController.lastPassedWaypoint == checkpoints.Length - 1 && i == 0))
                    {
                        carController.lastPassedWaypoint = i;

                        // Check if the car completed a lap
                        if (i == checkpoints.Length - 1)
                        {
                            carController.currentLap++;
                            carController.lastPassedWaypoint = -1; // Reset checkpoint counter

                            if (carController.currentLap >= totalLaps)
                            {
                                carController.currentLap = totalLaps; // Ensure lap count does not exceed total laps
                                carController.DisableCar(); // Disable car controls
                                CanvasManager.Instance.ShowVictoryImage(carController); // Trigger victory condition
                            }
                        }
                    }
                    break;
                }
            }
        }
    }
}
