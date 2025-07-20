using UnityEngine;

public class Item_Hold : MonoBehaviour
{

    public Torch_Use Torch_Use;
    public Looking_and_Close Looking_And_Close;
    public float itemRadius;
    public float detectionAngle;
    public Transform itemTransform;
    public static Transform activeTransform;
    public Transform camTransform;
    public static bool itemPickedUPBool;
    public static GameObject inventorySlot1Object;
    public static GameObject inventorySlot2Object;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemRadius = 5f;
        detectionAngle = 15f;
        Torch_Use = gameObject.GetComponent<Torch_Use>();
        Looking_And_Close = GetComponent<Looking_and_Close>();
        itemTransform = gameObject.transform;

        inventorySlot2Object = null;
    }

    // Update is called once per frame
    void Update()
    {

        if (itemPickedUPBool == false) 
        {
            Interact();
        }
        else
        {
            HoldItem(activeTransform);
        }
        if (itemPickedUPBool == true && Input.GetKey("q")) 
        {
            DropItem();
        }
        if (itemPickedUPBool == true && Input.GetKeyDown("f")) 
        {
            SwapItem();
        }
        
        // pickup and enabling item hold or inventory

        // check if item in inventory/not in hand

        // make item unusable & hidden 

        // check if item is in hand

        // move item to player hand

        // have item follow camera of player

        // enable the script component of item

        //Inputs
        // if swap key is pressed, swap inventory to hand and put hand to inventory
    }

     void Interact() 
    {
        if (Looking_And_Close.IsInRadius(itemRadius) && Looking_And_Close.IsLooking(itemTransform, detectionAngle))
        {
            if (Input.GetKey("e"))
            {
                activeTransform = itemTransform;
                print("Im Close Looking and Interacting with " + itemTransform + " transform");
                Pickup(activeTransform);
                

            }
        }
    }

    void Pickup(Transform transform) 
    {
        activeTransform.position = camTransform.position + camTransform.right.normalized;
        itemPickedUPBool = true;
    }
    void HoldItem(Transform transform) 
    {
        transform.position = camTransform.position + camTransform.right.normalized;
    }
    void SwapItem() 
    {
        activeTransform.gameObject.SetActive(false);
        if (inventorySlot2Object == null)
        {
            print("null");
            inventorySlot2Object = activeTransform.gameObject;
            activeTransform = null;
        }
        else if (inventorySlot2Object != null) 
        {

            print("not null");
            inventorySlot1Object = activeTransform.gameObject;
            activeTransform = inventorySlot2Object.transform;
            inventorySlot2Object = inventorySlot1Object;
        }
        activeTransform.gameObject.SetActive(true);
    }
    void DropItem() 
    {
        activeTransform = null;
        itemPickedUPBool = false;
    }
}
