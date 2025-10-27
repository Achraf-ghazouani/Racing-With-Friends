using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance; // Singleton instance

    [SerializeField] private CarController car1; // Reference to the first car
    [SerializeField] private CarController car2; // Reference to the second car

    [SerializeField] private Image speedometerCar1; // UI image for Car 1 speed
    [SerializeField] private Image speedometerCar2; // UI image for Car 2 speed

    [SerializeField] private TextMeshProUGUI speedTextCar1; // Speed text for Car 1
    [SerializeField] private TextMeshProUGUI speedTextCar2; // Speed text for Car 2

    [SerializeField] private TextMeshProUGUI positionTextCar1; // Position text for Car 1
    [SerializeField] private TextMeshProUGUI positionTextCar2; // Position text for Car 2

    [SerializeField] private TextMeshProUGUI lapTextCar1; // Lap text for Car 1
    [SerializeField] private TextMeshProUGUI lapTextCar2; // Lap text for Car 2

    [SerializeField] private TextMeshProUGUI playerNameCar1; // Name display for Car 1
    [SerializeField] private TextMeshProUGUI playerNameCar2; // Name display for Car 2

    [SerializeField] private TextMeshProUGUI timerTextCar1; // Timer for Car 1
    [SerializeField] private TextMeshProUGUI timerTextCar2; // Timer for Car 2

    [SerializeField] private GameObject nameInputPanel; // Panel for name input
    [SerializeField] private TMP_InputField inputNameCar1; // Input for Car 1's name
    [SerializeField] private TMP_InputField inputNameCar2; // Input for Car 2's name
    [SerializeField] private Button saveButton; // Button to save the names

    [SerializeField] private Transform[] waypoints; // Track waypoints

    [SerializeField] private GameObject victoryImageCar1; // Victory image for Car 1
    [SerializeField] private GameObject victoryImageCar2; // Victory image for Car 2

    private bool gamePaused = true; // Game starts paused
    private float maxSpeed = 100f; // Maximum speed for scaling speedometer
    private float timerCar1 = 0f;
    private float timerCar2 = 0f;
    private bool car1Won = false; // Track if Car 1 has won
    private bool car2Won = false; // Track if Car 2 has won

    private void Awake()
    {
        Instance = this; // Set the singleton instance
    }

    private void Start()
    {
        // Assign the save button click event
        saveButton.onClick.AddListener(SavePlayerNames);

        // Pause the game initially
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (!gamePaused)
        {
            // Update timers for both cars
            if (!car1Won) timerCar1 += Time.deltaTime;
            if (!car2Won) timerCar2 += Time.deltaTime;
            UpdateTimerUI();

            // Update speedometer UI and speed text for both cars
            UpdateSpeedometer(car1, speedometerCar1, speedTextCar1);
            UpdateSpeedometer(car2, speedometerCar2, speedTextCar2);

            // Update car positions and lap count
            UpdateCarPositions();
            UpdateLapCountUI();
        }
    }

    private void UpdateTimerUI()
    {
        timerTextCar1.text = "" + timerCar1.ToString("F2") + "";
        timerTextCar2.text = "" + timerCar2.ToString("F2") + "";
    }

    private void UpdateSpeedometer(CarController car, Image speedometerImage, TextMeshProUGUI speedText)
    {
        if (car == null || speedometerImage == null || speedText == null) return;

        // Get car speed from Rigidbody velocity (in meters per second)
        float speed = car.GetComponent<Rigidbody>().velocity.magnitude * 3.6f; // Convert to km/h

        // Normalize speed (0 to 1) based on max speed
        float speedNormalized = Mathf.Clamp(speed / maxSpeed, 0f, 1f);

        // Update the speedometer image (fill amount for graphical representation)
        speedometerImage.fillAmount = speedNormalized;

        // Update the speed text
        speedText.text = Mathf.RoundToInt(speed) + " km/h";
    }

    private void UpdateCarPositions()
    {
        if (car1 == null || car2 == null) return;

        // Compare by lap first, then waypoint progress
        if (car1.currentLap > car2.currentLap || 
           (car1.currentLap == car2.currentLap && car1.lastPassedWaypoint > car2.lastPassedWaypoint))
        {
            SetCarPositionText(positionTextCar1, "POS : 1st");
            SetCarPositionText(positionTextCar2, "POS : 2nd");
        }
        else
        {
            SetCarPositionText(positionTextCar1, "POS : 2nd");
            SetCarPositionText(positionTextCar2, "POS : 1st");
        }
    }

    private void SetCarPositionText(TextMeshProUGUI textElement, string position)
    {
        if (textElement != null)
            textElement.text = position;
    }

    private void UpdateLapCountUI()
    {
        lapTextCar1.text = $"Lap: {car1.currentLap}/1";
        lapTextCar2.text = $"Lap: {car2.currentLap}/1";
    }

    public void CarPassedWaypoint(CarController car, int waypointIndex)
    {
        if (car.lastPassedWaypoint < waypointIndex)
        {
            car.lastPassedWaypoint = waypointIndex;

            // Check if car completed a lap
            if (waypointIndex == waypoints.Length - 1)
            {
                car.currentLap++;
                car.lastPassedWaypoint = 0;

                // Show victory image if car completes 1 lap
                if (car.currentLap == 1)
                {
                    ShowVictoryImage(car);
                }
            }
        }
    }

    // Method to save player names
    private void SavePlayerNames()
    {
        // Get the names from input fields
        string nameCar1 = inputNameCar1.text;
        string nameCar2 = inputNameCar2.text;

        // Update the name displays in the main UI
        if (!string.IsNullOrEmpty(nameCar1))
            playerNameCar1.text = nameCar1;
        if (!string.IsNullOrEmpty(nameCar2))
            playerNameCar2.text = nameCar2;

        // Disable the name input panel
        nameInputPanel.SetActive(false);

        // Unpause the game and start the race
        gamePaused = false;
        Time.timeScale = 1f; // Resume the game
    }

    // Method to show the victory image
    public void ShowVictoryImage(CarController car)
    {
        if (car == car1)
        {
            victoryImageCar1.SetActive(true);
            car1Won = true; // Stop the timer for Car 1
        }
        else if (car == car2)
        {
            victoryImageCar2.SetActive(true);
            car2Won = true; // Stop the timer for Car 2
        }
    }
}
