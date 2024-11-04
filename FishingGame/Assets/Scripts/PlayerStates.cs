using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    // this script handles player states & behaviors
    public enum PlayerStateMachine
    {
        NotFishing, // nothing planned for this yet other than just "cast the fishing line"
        FishingIdle, // line is cast but fish has not bitten the line
        FishingActive // fish has bitten the line and player is reeling it in
    }
    public PlayerStateMachine currentState;

    [Header("Inputs")]
    public KeyCode castLine;
    public KeyCode reelIn;

    [Header("X position of cursor")]
    public float mouseX;
    //public float mouseY; // y is not important

    [Header("misc")]
    public bool isFishing;
    public Vector3 lastClickLocation;



    // references
    private FishingBehavior fishingBehaviorScript;
    private Camera gameCamera;
    public GameObject currentFish;
    public CursorScript cursor;

    // Start is called before the first frame update
    void Start()
    {
        fishingBehaviorScript = GetComponentInChildren<FishingBehavior>();
        gameCamera = Camera.main;
        cursor = FindObjectOfType<CursorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case PlayerStateMachine.NotFishing:
                UpdateNotFishing(); break;
            case PlayerStateMachine.FishingIdle:
                UpdateFishingIdle(); break;
            case PlayerStateMachine.FishingActive:
                UpdateFishingActive(); break;
        }

        // get mouse position
    }

    void UpdateNotFishing()
    {
        if (Input.GetKeyDown(castLine)) 
        {
            // grabs the mouse's position on click and stores it, switches state if water is clicked.
            GetMousePositionOnClick();
        }
    }
    void UpdateFishingIdle()
    {
        if (Input.GetKeyDown(reelIn))
        {
            CheckForFish();
        }
    }
    void UpdateFishingActive()
    {
        cursor.CheckCursorAngle();
    }

    void GetMousePositionOnClick()
    {
        Debug.Log("clicked!");
        Vector2 mousePosition = Input.mousePosition;
        Ray mouseRay = gameCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, 100))
        {
            Water water = hitInfo.transform.GetComponent<Water>();

            if (water != null)
            {
                lastClickLocation = hitInfo.point;

                isFishing = true;
                Debug.Log("switch to fishing idle");
                currentState = PlayerStateMachine.FishingIdle;
            }
            else
            {
                Debug.Log("cant find wawas");
            }
        }
    }

    void CheckForFish()
    {
        // defines current fish 
        if (GameObject.FindObjectOfType<FishBehavior>() != null)
        {
            currentFish = FindObjectOfType<FishBehavior>().gameObject;
        }
        else
        {
            currentFish = null;
        }

        // if there is a fish, switch to fishingactive, else switch back to idle
        if (currentFish != null)
        {
            Debug.Log("fish detected!");
            currentState = PlayerStateMachine.FishingActive;
        }
        else 
        {
            Debug.Log("no fish; return to not fishing");
            isFishing = false;
            currentState = PlayerStateMachine.NotFishing; 
        }
    }
}