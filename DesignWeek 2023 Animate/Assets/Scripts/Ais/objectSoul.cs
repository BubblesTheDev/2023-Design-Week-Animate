using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class objectSoul : MonoBehaviour
{
    [Header("Object Information")]
    public runeDataContainer objPersonality;
    public runeDataContainer objMotivation;
    public runeDataContainer objJob;

    [Space]
    /*[HideInInspector]*/ public List<placerAi> placerBasedAi;
    /*[HideInInspector]*/ public List<removerAi> removerBasedAi;

    [Space, Header("Base Ai Settings")]
    public aiState currentAiState;
    [Range(2f,7f)]
    public float objMoveSpeed = 2f;
    public float sightRadius = 10f;
    public float minTimeBetweenChoice, maxTimeBetweenChoice;
    /*[HideInInspector]*/ public NavMeshAgent agent;
    /*[HideInInspector]*/ public placerAi selectedPlacer;
    /*[HideInInspector]*/ public removerAi selectedRemover;

    [Space, Header("Wander Ai Settings")]
    public Vector3 posToWanderTo;
    [Space, Header("Do Action Ai Settings")]
    public Vector3 posOfAction;

    private void Awake()
    {
        placerBasedAi = GetComponents<placerAi>().ToList();
        removerBasedAi = GetComponents<removerAi>().ToList();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4)) print(agent.isOnNavMesh);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, sightRadius);

        if(posToWanderTo != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, posToWanderTo);
        }
    }
}

public enum aiState
{
    idle,
    wandering,
    doingAction
}
