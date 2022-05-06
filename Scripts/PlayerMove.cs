using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.UI; //needed for the Text

public class PlayerMove : MonoBehaviour
{
    //Variables for the character
    public Transform playerTransform;
    public CharacterController playerCharacterController;

    //Variables for the character's speed and sensitivity
    private float speed = 0.07f;
    private float sensitivity = 0.7f;

    //Variables to make the player look around
    private float movementX;
    private float movementY;
    private float mouseX;
    private float mouseY;
    private float pitch;
    private float yaw;

    //Text variable that states 'Items Carrying' on the UI
    public Text itemsCarried;

    //Reflects if the button has been pressed
    public bool checkToMove = true;

    //Variables for the moving cabinets/doors
    public GameObject door;
    public GameObject kitchenCabinetDoor;
    public GameObject pantryDoor;

    //Variables that reflect if salt or flour is grabbed
    private bool saltGrabbed;
    private bool flourGrabbed;

    //Variables to trigger the Animator for the moving cabinets/doors
    private Animator doorAnimator;
    private Animator kitchenCabinetAnimator;
    private Animator pantryAnimator;

    //Variables for the colliders of the kitchen and pantry door
    private Collider kitchenDoorCollide;
    private Collider pantryDoorCollide;

    //Variables that reflect if the door/pantry/cabinet is opened or not
    bool turnOnDoor = true;
    bool turnOnPantry = true;
    bool turnOnCabinet = true;

    //Variable to play audio when ingredient is grabbed
    public AudioSource audioClipInventory;

    void Start()
    {
        //Makes the cursor unlocked
        Cursor.lockState = CursorLockMode.None;

        //Makes the animator variables reference the gameObjects that has the Animator attached
        doorAnimator = door.GetComponent<Animator>();
        pantryAnimator = pantryDoor.GetComponent<Animator>();
        kitchenCabinetAnimator = kitchenCabinetDoor.GetComponent<Animator>();

        //Turns the animator on
        doorAnimator.enabled = true;
        pantryAnimator.enabled = true;
        kitchenCabinetAnimator.enabled = true;

        //References the box colliders on the kitchen cabinet and pantry door
        kitchenDoorCollide = kitchenCabinetDoor.GetComponent<Collider>();
        pantryDoorCollide = pantryDoor.GetComponent<Collider>();

        //refers to the audio clip not playing when the game starts
        audioClipInventory.playOnAwake = false;

    }

    // Update is called once per frame
    void Update()
    {
        /*Once the button is pressed, then the player can move the cursor around as if they were
        looking around*/
        if (checkToMove == false)
        {
            Cursor.lockState = CursorLockMode.Locked;    

            // *** Movement (Player Object) *** //
            // Adding horizontal, vertical axes and "gravity"
            Vector3 vel = (playerTransform.forward * movementY + playerTransform.right * movementX) * speed + Vector3.down;
            playerCharacterController.Move(vel);

            // *** Rotation (Camera Object) *** //
            // Add or subtract rotation based on where the
            // mouse is moving
            pitch -= mouseY * sensitivity;
            // Prevent camera from rotating fully around
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            yaw += mouseX * sensitivity;

            // The player only rotates around the y axis, otherwise it would "tilt"
            Vector3 targetPlayerRotation = new Vector3(0, yaw);
            playerTransform.eulerAngles = targetPlayerRotation;

            // The camera rotates around the y and x axes so it can "look around"
            Vector3 targetCameraRotation = new Vector3(pitch, yaw);
            transform.eulerAngles = targetCameraRotation;
        }
    }

    // Part of the InputSystem—called when the move action event is triggered
    void OnMove(InputValue inputValue)
    {
        // Get the Vector2 data from the movementValue
        Vector2 movementVector = inputValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Part of the InputSystem—called when the "Look" action is triggered (probably by the mouse)
    void OnLook(InputValue inputValue)
    {
        Vector2 mouseVector = inputValue.Get<Vector2>();

        mouseX = mouseVector.x;
        mouseY = mouseVector.y;
    }

    void OnFire()
    {
        // Raycast and see what we hit
        RaycastHit hit;

        /* In the game, the box collider of the cabinet collides with the ingredient, so it makes it difficult to grab. Thus, the BoxCollider for the door is turned off 
        to make it easier for the Player to grab the ingredient. Once the ingredient is grabbed, then the BoxCollider is turned back on, so the Player can close the
        cabinet again. */
        if (turnOnCabinet == false && saltGrabbed == true)
        {
            kitchenDoorCollide.enabled = true;
        }

        if (turnOnPantry == false && flourGrabbed == true)
        {
            pantryDoorCollide.enabled = true;
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {   
            // If I hit an ingredient, destroy it. Otherwise, do nothing! Also, it reference the gameManager's boolean variables to true.
            //Side note: game object needs a BoxCollider in order to work
            if (hit.transform.gameObject.CompareTag("FLOUR"))
            {
                audioClipInventory.GetComponent<AudioSource>().Play();
                gameManager.flourReceive = true;
                Destroy(hit.transform.gameObject);
                flourGrabbed = true;  //Is needed to see if flour is grabbed to turn the BoxCollider back on
            }
            
            if (hit.transform.gameObject.CompareTag("SALT"))
            {
                audioClipInventory.GetComponent<AudioSource>().Play();
                gameManager.saltReceive = true;
                saltGrabbed = true;   //Is needed to see if salt is grabbed to turn the BoxCollider back on
                Destroy(hit.transform.gameObject);
            }

            if (hit.transform.gameObject.CompareTag("YEAST"))
            {
                audioClipInventory.GetComponent<AudioSource>().Play();
                gameManager.yeastReceive = true;
                Destroy(hit.transform.gameObject);
            }

            if (hit.transform.gameObject.CompareTag("WATER"))
            {
                audioClipInventory.GetComponent<AudioSource>().Play();
                gameManager.waterReceive = true;
                Destroy(hit.transform.gameObject);
            }

            if (hit.transform.gameObject.CompareTag("BOWL"))
            {
                //If each of the ingredients is received, then change the gameManager's boolean variable for touching the bowl to true
                if (gameManager.waterReceive == true && gameManager.flourReceive == true && gameManager.saltReceive == true && gameManager.yeastReceive == true)
                {
                    gameManager.touchTheBowl = true;
                }
            }
            
            //If the door is hit, then open the door. If it is hit again, then close the door.
            if (hit.collider.tag == "Door")
            {                
                if (turnOnDoor == true)
                {
                    doorAnimator.SetBool("isOpen_Obj_1", true);
                    turnOnDoor = false;
                }
                else
                {
                    doorAnimator.SetBool("isOpen_Obj_1", false);
                    turnOnDoor = true;
                }
            }

            /* If the pantry door is hit, then open the pantry door. If it is hit again, then close the door. However, in the game, the box collider of the pantry 
            collides with the ingredient, so it makes it difficult to grab. Thus, the BoxCollider for the door is turned off to make it easier for the Player to grab 
            the ingredient. 
            */
            if (hit.collider.tag == "Pantry")
            {
                if (turnOnPantry == true)
                {
                    pantryAnimator.SetBool("isOpen_Obj_1", true);
                    turnOnPantry = false;
                    pantryDoorCollide.enabled = false;
                }
                else
                {
                    pantryAnimator.SetBool("isOpen_Obj_1", false);
                    turnOnPantry = true;
                }
            }

            /* If the cabinet door is hit, then open the cabinet door. If it is hit again, then close the door. However, in the game, the box collider of the cabinet 
            collides with the ingredient, so it makes it difficult to grab. Thus, the BoxCollider for the door is turned off to make it easier for the Player to grab 
            the ingredient. 
            */
            if (hit.collider.tag == "Cabinet")
            {
                if (turnOnCabinet == true)
                {
                    kitchenCabinetAnimator.SetBool("isOpen_Obj_1", true);
                    turnOnCabinet = false;
                    kitchenDoorCollide.enabled = false;
                }
                else
                {
                    kitchenCabinetAnimator.SetBool("isOpen_Obj_1", false);
                    turnOnCabinet = true;
                }
            }
        }
    }

    //Is attached to the Button's OnClick()
    /* If the button is clicked on, the title text for the inventory appears and it 
    changes checkToMove to false. If the variable weren't here, then the player wouldn't be
    able to click on the button without the camera moving*/
    public void CustomOnPressed()
    {
        checkToMove = false;
        itemsCarried.text = "Items Carrying:";
    }
}
