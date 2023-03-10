
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placerAi : MonoBehaviour
{
    public List<GameObject> buildingsToPlace;
    public int maxBuildings;
    public int currentbuildings;
    public objectSoul soul;
    public bool isDoing, isIdle, isWandering;

    public void OnEnable()
    {
        soul = GetComponent<objectSoul>();
        detectState();

    }

    public void detectState()
    {
        if (soul.currentAiState == aiState.idle)
        {
            if (currentbuildings < maxBuildings) soul.currentAiState = aiState.doingAction;
            else if (currentbuildings >= maxBuildings) soul.currentAiState = aiState.wandering;
        }

        switch (soul.currentAiState)
        {
            case aiState.doingAction:
                if(!isDoing) StartCoroutine(doAction());
                break;
            case aiState.wandering:
                if(!isWandering)
                StartCoroutine(wander());
                break;
            case aiState.idle:
                
                break;
        }
    }

    public IEnumerator doAction()
    {
        soul.currentAiState = aiState.doingAction;
        isDoing = true;

        if (soul.agent.isOnNavMesh)
        {
            soul.agent.SetDestination(getRandomPosition(soul.sightRadius));

            while (!soul.agent.isStopped)
            {
                yield return new WaitForEndOfFrame();
                if (soul.agent.remainingDistance < 0.5f) break;
            }
        }


        print("play build animation");
        yield return new WaitForSeconds(4f);
        Instantiate(buildingsToPlace[Random.Range(0, buildingsToPlace.Count+1)], transform.position + (transform.forward * 1f), Quaternion.identity, GameObject.Find("PlacedObjects").transform);
        currentbuildings++;

        float randomTimeToWait = Random.Range(soul.minTimeBetweenChoice, soul.maxTimeBetweenChoice);
        print(randomTimeToWait);
        yield return new WaitForSeconds(randomTimeToWait);

        soul.currentAiState = aiState.idle;
        isDoing = false;

        detectState();
    }

    public IEnumerator wander()
    {
        soul.currentAiState = aiState.wandering;
        isWandering = true;

        float randomTimeToWait = Random.Range(soul.minTimeBetweenChoice, soul.maxTimeBetweenChoice);
        int randomNumOfWanders = Random.Range(1, 5);
        for (int i = 0; i < randomNumOfWanders; i++)
        {
            if (soul.agent.isOnNavMesh)
            {
                soul.agent.SetDestination(getRandomPosition(soul.sightRadius));

                while (!soul.agent.isStopped)
                {
                    yield return new WaitForEndOfFrame();
                }
            }


            yield return new WaitForSeconds(randomTimeToWait / randomNumOfWanders);

        }

        soul.currentAiState = aiState.idle;
        isWandering = false;
        detectState();

    }

    private Vector3 getRandomPosition(float range)
    {
        Vector3 temp = Vector3.zero;

        temp.x = (Random.insideUnitCircle.x * range) + temp.x;
        temp.z = (Random.insideUnitCircle.y * range )+ temp.z;

        RaycastHit hit;

        Physics.Raycast(temp + Vector3.up * 1.5f, Vector3.down, out hit);

        temp.y = hit.point.y;

        return temp;
    }
}
