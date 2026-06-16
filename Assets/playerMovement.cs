using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


// When player presses WASD or pushes stick direction 
// Player moves in the desired direction
// At a speed controlled by the game 

// When camera moves, player rotates in the direction of the camera
//Store Camera rotation and player rotation in a variable and then use that to rotate the player in the direction of the camera
public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed;

    private CharacterController cc;

    private PlayerControls playerControls;

    public float rotationSpeed = 10f;

    private Transform cameraTransform;
    public Vector3 moveDirection;
   
    //Now what happens after this ??? 
    //Well lets get aquainted with the new input system 

    //Old input system was just input.get axis raw and boom done 

    void Awake()
    {
        playerControls = new PlayerControls();

        if (cc == null)
        {
            cc = GetComponent<CharacterController>();

        }





        CinemachineCamera vcam = Object.FindAnyObjectByType<CinemachineCamera>();
        if (vcam != null)
        {
            cameraTransform = vcam.transform;
            Debug.Log("Found Cinemachine virtual camera: " + cameraTransform.name);
            return;
        }


    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {

        OnMove();
        RotatePlayerWithCamera();
    }

    void OnMove()
    {
        Vector2 movementInput = playerControls.Gameplay.Move.ReadValue<Vector2>();

        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);

        if (move.magnitude > 0.1f)
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();


            moveDirection = (cameraForward * move.z + cameraRight * move.x).normalized;


            cc.Move(moveDirection * movementSpeed * Time.deltaTime);
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    void RotatePlayerWithCamera()
    {


      
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        
    }
}
