using UnityEngine;
using UnityEngine.InputSystem;


// When player presses WASD or pushes stick direction 
// Player moves in the desired direction
// At a speed controlled by the game 
public class playerMovement : MonoBehaviour
{

    public float movementSpeed;

    private CharacterController cc;

    private PlayerControls playerControls;
    

    //Now what happens after this ??? 
    //Well lets get aquainted with the new input system 

    //Old input system was just input.get axis raw and boom done 
   
    void Awake()
    {
       playerControls= new PlayerControls();
        cc = GetComponent<CharacterController>();
      
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

    }

    void OnMove()
    {
      Vector3 movementInput = playerControls.Gameplay.Move.ReadValue<Vector2>();

        Vector3 move = new Vector3(movementInput.x,0,movementInput.y);//

        transform.Translate (move*movementSpeed*Time.deltaTime);
    }
}
