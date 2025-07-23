using UnityEngine;

public class Item_Hold : MonoBehaviour
{

    public Torch_Use Torch_Use;
    public Looking_and_Close Looking_And_Close;
    public float itemRadius;
    public float detectionAngle;
    public Transform itemTransform;
    public static Transform activeTransform;
    public static Transform passiveTransform;
    public static Transform fillerTransform;
    public GameObject activeObject;
    public Transform camTransform;
    public static bool itemPickedUPBool;
    public static bool itemPickedUPCapBool;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemRadius = 5f;
        detectionAngle = 15f;
        Torch_Use = gameObject.GetComponent<Torch_Use>();
        Looking_And_Close = GetComponent<Looking_and_Close>();
        itemTransform = gameObject.transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (itemPickedUPCapBool == false)
        {
            Interact();
        }
        if (activeTransform != null) 
        {
            HoldItem(activeTransform);

            activeTransform.gameObject.SetActive(true);
        }
        if (passiveTransform != null)
        {
            HoldItem(passiveTransform);

            passiveTransform.gameObject.SetActive(false);
        }

        if (itemPickedUPBool == true && itemPickedUPCapBool == false && Input.GetKeyDown("q"))
        {
            DroplastItem();
        }
        if (itemPickedUPBool == true && Input.GetKeyDown("q")) 
        {
            Drop1of2Items();
        }
        
        if (Input.GetKeyDown("f")) 
        {
            SwapItem();
        }
        //HoldItem(inventorySlot1Object.transform);
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
            if (Input.GetKey("e") && itemPickedUPBool == false)
            {
                passiveTransform = activeTransform;
                activeTransform = itemTransform;
                print("Im Close Looking and Interacting with " + itemTransform + " transform");
                itemPickedUPBool = true;
                Pickup(activeTransform);
                

            }
            else if (Input.GetKey("e") && itemPickedUPBool == true) 
            {
                passiveTransform = activeTransform;
                activeTransform = itemTransform;
                print("Im Close Looking and Interacting with " + itemTransform + " transform");
                Pickup(activeTransform);
                itemPickedUPCapBool = true;

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
        fillerTransform = activeTransform;
        activeTransform = passiveTransform;
        passiveTransform = fillerTransform;
        // check if have item in main hand
        // check if have item in 2nd slot
        // swap main hand item with 2nd slot


        //if (itemIn1stSlotBool == true)
        //{
        //    print("swap main with 2nd");
        //    inventorySlot1Object = activeTransform.gameObject;
        //    //Slot 1 becomes item
        //    activeObject = inventorySlot2Object;
        //    inventorySlot2Object = inventorySlot1Object;
        //    // Slot2 item = Slot1 item
        //    inventorySlot1Object = activeObject;
        //    //Swap done
        //    inventorySlot2Object.SetActive(false);
        //    inventorySlot1Object.SetActive(true);
        //    itemPickedUPBool = false;
        //}
        //else 
        //{
        //    //no swap
        //}

    }
    void Drop1of2Items() 
    {
        
        if (itemPickedUPCapBool == true && itemPickedUPBool == true) 
        {
            //2 items
            print(passiveTransform);
            activeTransform = null;
            activeTransform = passiveTransform;

            itemPickedUPCapBool = false;

        }
        

    }
    void DroplastItem()
    {
        // 1 item
        print("1 items");
        activeTransform = null;
        itemPickedUPBool = false;
    }
}
