using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MaterialChanger : MonoBehaviour
{
    public Renderer car1Renderer; // Drag Car 1 Renderer here
    public Renderer car2Renderer; // Drag Car 2 Renderer here

    [System.Serializable]
    public class ButtonVFXPair
    {
        public Button button;   // Button assigned in Inspector
        public GameObject leftVFX;  // Left wheel VFX
        public GameObject rightVFX; // Right wheel VFX
    }

    public List<ButtonVFXPair> car1VFXPairs; // Assign buttons and VFX for Car 1
    public List<ButtonVFXPair> car2VFXPairs; // Assign buttons and VFX for Car 2

    private GameObject activeLeftVFXCar1 = null; // Track active left VFX for Car 1
    private GameObject activeRightVFXCar1 = null; // Track active right VFX for Car 1
    private GameObject activeLeftVFXCar2 = null; // Track active left VFX for Car 2
    private GameObject activeRightVFXCar2 = null; // Track active right VFX for Car 2

    private void Start()
    {
        // Assign event listeners for each button in Car 1
        foreach (var pair in car1VFXPairs)
        {
            pair.button.onClick.AddListener(() => ChangeCarColorAndVFX("Car1", pair));
        }

        // Assign event listeners for each button in Car 2
        foreach (var pair in car2VFXPairs)
        {
            pair.button.onClick.AddListener(() => ChangeCarColorAndVFX("Car2", pair));
        }
    }

    private void ChangeCarColorAndVFX(string carName, ButtonVFXPair pair)
    {
        Color newColor = pair.button.GetComponent<Image>().color; // Get button color

        if (carName == "Car1")
        {
            car1Renderer.material.color = newColor; // Change car color
            ActivateVFX(ref activeLeftVFXCar1, ref activeRightVFXCar1, pair.leftVFX, pair.rightVFX);
        }
        else if (carName == "Car2")
        {
            car2Renderer.material.color = newColor; // Change car color
            ActivateVFX(ref activeLeftVFXCar2, ref activeRightVFXCar2, pair.leftVFX, pair.rightVFX);
        }
    }

    private void ActivateVFX(ref GameObject activeLeftVFX, ref GameObject activeRightVFX, GameObject newLeftVFX, GameObject newRightVFX)
    {
        // Disable previous left VFX
        if (activeLeftVFX != null) 
            activeLeftVFX.SetActive(false);

        // Disable previous right VFX
        if (activeRightVFX != null) 
            activeRightVFX.SetActive(false);

        // Activate new VFX
        activeLeftVFX = newLeftVFX;
        activeRightVFX = newRightVFX;

        if (activeLeftVFX != null) 
            activeLeftVFX.SetActive(true);

        if (activeRightVFX != null) 
            activeRightVFX.SetActive(true);
    }
}
