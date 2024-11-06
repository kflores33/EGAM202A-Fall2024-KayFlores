using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FishBehavior : MonoBehaviour
{
    //this script handles the instantiated fishs' behavior

    public Transform thisTransform;
    public PlayerStates player;
    public Slider slider;
    public GeneralGameManager manager;

    public float minTime;
    public float maxTime;

    public float speed;

    public float minDistance;
    public float maxDistance;

    public float healthValue = 20;
    public float maxHealth;
    public float currentHealth;

    bool isDead;

    public Vector3 targetPos;
    public Vector3 boxSize;

    Coroutine reInitiateSelfDestruct;
    Coroutine selfDestruct;
    Coroutine moveBehavior;

    bool moveBehaviorComplete;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;

        slider.maxValue = maxHealth;
        slider.minValue = 0;

        slider.value = currentHealth;

        slider.enabled = false;

        // start the disappear coroutine
        selfDestruct = StartCoroutine(SelfDestructTimer());
    }

    // Update is called once per frame
    void Update()
    {
        // stop disappear coroutine if player clicked reel in and start movement coroutine
        if (player.currentState == PlayerStates.PlayerStateMachine.FishingActive)
        {
            if (selfDestruct != null) StopCoroutine(selfDestruct);

            // probably have some coroutine that repeats itself
            if (moveBehavior == null)
            {
                slider.enabled = true;
                moveBehavior = StartCoroutine(MoveBehavior());
            }

            if (moveBehaviorComplete)
            {
                StopCoroutine(moveBehavior);
                moveBehavior = null;

                moveBehavior = StartCoroutine(MoveBehavior());
            }
        }

        // draw ray to show angle
        Vector3 pos = transform.position;
        Vector3 dir = (player.transform.position - transform.position).normalized;
        Debug.DrawRay(pos, dir * 25, Color.blue);
    }        
    IEnumerator SelfDestructTimer()
    {
        yield return new WaitForSeconds(3);

        manager.TriggerLoseScreen();

        Destroy(this.gameObject);
    }
    IEnumerator MoveBehavior()
    {
        moveBehaviorComplete = false;

        yield return new WaitForSeconds(Random.Range(minTime, maxTime));

        // attempt to find new location to move to
        Vector3 startPos = transform.position;  

        Vector3 newPosition = Vector3.zero;

        int attempts = 100;

        bool isNotWater = true;
        while (isNotWater) 
        {
            Vector3 offset = Vector3.zero;
            offset.x = Random.Range(-targetPos.x, targetPos.x);
            offset.y = 0;
            offset.z = Random.Range(-1, 1);

            newPosition = startPos + offset;

            //Ray ray = new Ray(newPosition, Vector3.zero);

            bool hitDetect = Physics.BoxCast(newPosition, boxSize, Vector3.zero, out RaycastHit hit, Quaternion.identity, 0);

            if (hitDetect)
            {
                if (hit.collider.GetComponent<Water>() != null)
                {
                    isNotWater = false;
                }
                else
                {
                    isNotWater = true;
                }
            }

            attempts--;
            if (attempts <= 0) 
            {
                break;
            }
        }

        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / speed;

            if (t > 1) { t = 1; }

            transform.position = Vector3.Slerp(startPos, newPosition, t);

            if(Vector3.Distance(transform.position, newPosition) <= 0.5f)
            {
                break;
            }

            yield return null;
        }

        moveBehaviorComplete = true;
    }

    public void GetHit(float amount)
    {
        if (isDead) return;

        // stop self destruct measures
        if (reInitiateSelfDestruct != null)
        {
            StopCoroutine(reInitiateSelfDestruct);
            reInitiateSelfDestruct = null;
        }
        if (selfDestruct != null) 
        { 
            StopCoroutine(selfDestruct);
            selfDestruct = null;
        }

        currentHealth -= amount;

        slider.value = currentHealth;
        
        // start self destruct back up
        if (reInitiateSelfDestruct == null) 
        {
            reInitiateSelfDestruct = StartCoroutine(ReInitiateSelfDestruct());
        }

        if (currentHealth > 0)
        {
            Debug.Log("hit but not dead");
        }
        else
        {
            Debug.Log("fish dead!!!!!!!!");
            isDead = true;
            manager.UpdateFishCaught();
            Destroy(this.gameObject);
        }
    }

    IEnumerator ReInitiateSelfDestruct()
    {
        yield return new WaitForSeconds(3);

        selfDestruct = StartCoroutine(SelfDestructTimer());
    }
}
