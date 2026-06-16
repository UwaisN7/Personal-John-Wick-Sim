using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


//Camera first follows the player from an original position
//Do we make the cam a child of the player or not. So no 
// Now we need the camera to detect the player how ?? Simple game object??


//When player inputs right clikc or LT 
//Cam zooms in on left or right shoulder based on what they chose when they press Rs or mouse wheel while aiming
//Player movement slows down and cam speed also slows down 

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;

   
    [SerializeField] private GameObject playerTarget;

    public CinemachineInputAxisController sensitivity;

    [Header("Sensitivity Settings")]
    public float multiplierX = 1f;
    public float multiplierY = 1f;
    public bool invertY = false;
    void Start()
    {
        if (playerTarget == null)
        {
            playerTarget = GameObject.FindGameObjectWithTag("Player");
        }

        
        if (virtualCamera != null && playerTarget != null)
        { 
            virtualCamera.Follow = playerTarget.transform;
             virtualCamera.LookAt = playerTarget.transform;
        }
    }
    void Update()
    {
        UpdateGainMultiplier();
    }

    public void UpdateGainMultiplier()
    {
        if (sensitivity == null) return;

        foreach (var driver in sensitivity.Controllers)
        {

       
        if (driver.Name == "Look X" || driver.Name.Contains("X"))
                {
                    driver.Input.Gain = multiplierX;
                }
                else if (driver.Name == "Look Y" || driver.Name.Contains("Y"))
                {

                float yGain = invertY ? -multiplierY : multiplierY;
                driver.Input.Gain = yGain;
            }
    }
        }

}
