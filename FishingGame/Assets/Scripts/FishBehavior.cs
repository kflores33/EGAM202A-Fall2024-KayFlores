using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FishBehavior : MonoBehaviour
{
    //this script handles the instantiated fishs' behavior

    // self references
    public Transform thisTransform;
    public Slider slider;

    // scene references
    public PlayerStates player;
    public GeneralGameManager manager;

    public FishData data;

    public float maxHealth;
    public float currentHealth;

    bool isDead;

    public Vector3 targetPos;
    public Vector3 boxSize;

    public Vector3 waterArea;
    public Transform waterOrigin;

    Coroutine reInitiateSelfDestruct;
    Coroutine selfDestruct;
    Coroutine moveBehavior;

    bool moveBehaviorComplete;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = data.healthValue;
        maxHealth = data.healthValue;
        isDead = false;

        slider.maxValue = maxHealth;
        slider.minValue = 0;

        slider.value = currentHealth;

        slider.enabled = false;
        slider.gameObject.SetActive(false);

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

            // move behavior controller
            if (moveBehavior == null)
            {
                slider.enabled = true;
                slider.gameObject.SetActive(true);
                moveBehavior = StartCoroutine(MoveBehavior());
            }
            // resets move behavior once completed
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

        yield return new WaitForSeconds(Random.Range(data.minTime, data.maxTime));

        // start position is player position
        Vector3 startPos = transform.position;  

        // defines newPosition variable
        Vector3 newPosition = Vector3.zero;

        int attempts = 100;

        bool isNotWater = true;

        while (isNotWater)
        {
            Vector3 randomOffset = Vector3.zero;
            randomOffset.x = Random.Range(-waterArea.x,waterArea.x);
            randomOffset.y = 0;
            randomOffset.z = Random.Range(-waterArea.z,waterArea.z);

            newPosition = waterOrigin.position + randomOffset;

            // note to self: never try using boxcast for this again LMAO
            bool hitDetect = Physics.Raycast(newPosition, Vector3.down, out RaycastHit hit);

            if (hitDetect)
            {
                Debug.Log("hit detected");
                if (hit.collider.GetComponent<Water>() != null)
                {
                    isNotWater = false;
                }
            }
            else
            {
                Debug.Log("no collisions detected dipshit");
            }

            attempts--;
            if (attempts <= 0)
            {
                Debug.Log("while loop doesnt fucking work lol");
                break;
            }
        }

        // lerp between positions (do not remove!!!)
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / data.speed;

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
