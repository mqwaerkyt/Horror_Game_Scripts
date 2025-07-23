using System;
using UnityEngine;
using UnityEngine.AI;

public class AI_Navigation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public NavMeshAgent NMA_monster;
    public GameObject playerGameObject;
    public Transform playerTransform;
    public NavMeshPath path;
    public LayerMask layerMaskPlayer;
    public float hiddenRadius;
    public float stalkingRadius;
    public float chaseRadius;
    public float fastChaseRadius;
    public float caughtRadius;
    public float chaseMode;
    public Movement_Player playerMovementScript;
    Looking_and_Close looking_And_Close;
    public float detectionAngle;
    public float agroMeter;
    Torch_Use torch_Use;
    public float modeTimer;
    public float startTimer;
    public float flashedCounter;
    public float Tasktimer;
    public bool isDoingTask;
    public float hiddenTime;
    public float stalkingTime;
    public float chaseTime;
    public float fastChaseTime;
    public float caughtTime;
    public bool hasreset;
    public float flashTime;
    public float flashTimer;
    public bool hasResetFlash;



    void Start()
    {
        path = new NavMeshPath();
        hiddenRadius = 50;
        stalkingRadius = 30;
        chaseRadius = 15;
        fastChaseRadius = 5;
        caughtRadius = 3;
        flashTime = 10;

        looking_And_Close = GetComponent<Looking_and_Close>();
        playerMovementScript = playerGameObject.GetComponent<Movement_Player>();
    }


    // Update is called once per frame
    void Update()
    {
        // making path
        NMA_monster.CalculatePath(playerTransform.position,path);

        // path line
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
        Debug.DrawLine(transform.position, playerTransform.position, Color.blue);

        // applying path
        NMA_monster.SetPath(path);

        // path logic
        NMA_monster.isStopped = true;

        //sphere detection
        float[] radii = {hiddenRadius,stalkingRadius,chaseRadius,fastChaseRadius,caughtRadius};
        Array.Sort(radii);
        for (int i = 0; i < radii.Length ; i++) 
        {
            if (CheckSphereMonster(radii[i]) == true)
            {
                // finds i with closest radius with player then breaks
                chaseMode = i;
                break;
            }
            else 
            {
                chaseMode = i+1;
            }
        }

        //print(chaseMode);

        CalcAgroMeter();

        switch (chaseMode) 
        {
            case 5:
                TooFar();
                break;
            case 4:
                Hidden();
                break;
            case 3:
                Stalk();
                break;
            case 2:
                Chase();
                break;
            case 1:
                FastChase();
                break;
            case 0:
                Caught();
                break;

        }

    }

    void CalcAgroMeter() 
    {
        // hidden agro progression
        if (chaseMode == 4 && (playerMovementScript.stamina <= 1 || isDoingTask == true || ModeTimer(4, 15, true)) )
        {
            agroMeter = 1;
            print(agroMeter +"im agro");
        }

        // stalk agro mode progression
        //if player is looking at monster or monster not agro enough stop
        if (chaseMode == 3 && (ModeTimer(6,flashTime,true) || Tasktimer >= 10 || ModeTimer(3,15,true)))
        {
            agroMeter = 2;
            print(agroMeter + "im agrooo");
        }
        
        // if player uses flashlight while looking at monster, reduce agro
        if (looking_And_Close.IsLooking(transform, detectionAngle) == true && torch_Use.torchOn == true)
        {
            agroMeter -= 1;
        }


        if (chaseMode == 5) 
        {
            agroMeter = 0;
        }
       
    }

    bool ModeTimer(float chasMode, float targetTime,bool startFromZero) 
    {
        if (chaseMode <= 3)
        {
            hiddenTime = 0;
        }
        if (chaseMode != 3)
        {
            stalkingTime = 0;
        }
        if (chaseMode != 3)
        {
            flashTimer = 0;
        }

        switch (chasMode) 
        {
            case 7:
                // tasktimer
                break;
            case 6:
                //flashtimer
                flashTimer += Time.deltaTime;
                if (startFromZero == true && hasResetFlash == false)
                {
                    flashTimer = 0;
                    hasResetFlash = true;

                }
                if (flashTimer >= targetTime)
                {
                    hasResetFlash = false;
                    return (true);
                }
                break;
            case 5:
                break;
            case 4:
                hiddenTime += Time.deltaTime;
                if (startFromZero == true && hasreset == false) 
                {
                    hiddenTime = 0;
                    hasreset = true;
                    
                }
                if (hiddenTime >= targetTime)
                {
                    hasreset = false;
                    return (true); 
                }
                break;
            case 3:  
                stalkingTime += Time.deltaTime;
                if (startFromZero == true && hasreset == false)
                {
                    stalkingTime = 0;
                    hasreset = true;

                }
                if (stalkingTime >= targetTime)
                {
                    hasreset = false;
                    return (true);
                }
                break;
            case 2:
                break;
            case 1:
                break;
            case 0:
                break;
            
        }

        return (false);
    }
     bool CheckSphereMonster(float radius)
    {
        if (Physics.CheckSphere(transform.position, radius, layerMaskPlayer) == true)
        {
            //Debug.Log("player in " + radius + "m");
            return (true);
        }
        return (false);
    }

    void DistanceFromPlayerViaPath() 
    {
        
    }
    void TooFar()
    {

        //chasemode = 5
        // go to next stage
        print("im far");
        NMA_monster.isStopped = false;

    }
    void Hidden()
    {
        //chasemode = 4
        if (agroMeter == 1)
        {
            NMA_monster.isStopped = false;
            print("im coming");
        }
        else
        {
            NMA_monster.isStopped = true;

            print("im hidden");
        }
        // if player is out of stamina or after some time go to next stage


    }
    void Stalk()
    {
        //chasemode = 3
        if (agroMeter == 2)
        {
            NMA_monster.isStopped = false;
            print("im coming");
        }
        else
        {
            NMA_monster.isStopped = true;

            print("im stalking");
        }

    }
    void Chase() 
    {
        //chasemode = 2
        NMA_monster.isStopped = false;
        NMA_monster.speed = 3;
        print("im chasing");
    }

    void FastChase()
    {
        //chasemode = 1
        NMA_monster.isStopped = false;
        NMA_monster.speed = 6;
        print("run");

    }

    void Caught() 
    {
        //chasemode = 0
        print("caught");

    }
}
