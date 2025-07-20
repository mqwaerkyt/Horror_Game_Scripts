using System;
using UnityEngine;
using UnityEngine.AI;

public class AI_Navigation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public NavMeshAgent NMA_monster;
    public Transform playerTransform;
    public NavMeshPath path;
    public LayerMask layerMaskPlayer;
    public float hiddenRadius;
    public float stalkingRadius;
    public float chaseRadius;
    public float fastChaseRadius;
    public float caughtRadius;
    public float chaseMode;
    Movement_Player playerMovmentScript;
    Looking_and_Close looking_And_Close;
    public float detectionAngle;
    public float agroMeter;
    Torch_Use torch_Use;

    void Start()
    {
        path = new NavMeshPath();
        hiddenRadius = 50;
        stalkingRadius = 30;
        chaseRadius = 15;
        fastChaseRadius = 5;
        caughtRadius = 3;

        playerMovmentScript = GetComponent<Movement_Player>();
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

    }
     bool CheckSphereMonster(float radius)
    {
        if (Physics.CheckSphere(transform.position, radius, layerMaskPlayer) == true)
        {
            Debug.Log("player in " + radius + "m");
            return (true);
        }
        return (false);
    }

    void DistanceFromPlayerViaPath() 
    {
        
    }
    void ToorFar()
    {
        //chasemode = 5
        // go to next stage
        NMA_monster.isStopped = false;

    }
    void Hidden()
    {
        //chasemode = 4
        NMA_monster.isStopped = true;

        // if player is out of stamina or after some time go to next stage
        if (playerMovmentScript.stamina <= 1 || agroMeter == 1) 
        {
            NMA_monster.isStopped = false;
        }

    }
    void Stalk()
    {
        //chasemode = 3
        //if player is looking at monster or monster not agro enough stop
        if (looking_And_Close.IsLooking(transform, detectionAngle) == true || agroMeter < 2)
        {
            NMA_monster.isStopped = true;
        }
        else 
        {
            NMA_monster.isStopped = false;
        }
        // if player uses flashlight while looking at monster, reduce agro
        if (looking_And_Close.IsLooking(transform, detectionAngle) == true && torch_Use.torchOn == true)
        {
            agroMeter -= 1;
        }
        
    }
    void Chase() 
    {
        //chasemode = 2
        NMA_monster.isStopped = false;
        NMA_monster.speed = 3;
    }

    void FastChase()
    {
        //chasemode = 1
        NMA_monster.isStopped = false;
        NMA_monster.speed = 6;

    }

    void Caught() 
    {
        //chasemode = 0

    }
}
