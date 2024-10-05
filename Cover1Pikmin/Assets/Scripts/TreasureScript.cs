using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureScript : MonoBehaviour
{
    // Create a list to house position transforms
    public List<Transform> PossiblePositions = new List<Transform>();

    public int numberOfPikminRequired;
    public int numberOfPikminCurrent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if numberOfPikminCurrent == numberOfPikminRequired
        // allow for the treasure to move

        // if m1 is clicked, DismissPikmin()
    }

    private void OnCollisionEnter(Collision col)
    {
        MoveCharacter pikmin = col.gameObject.GetComponent<MoveCharacter>();
        if (pikmin != null) 
        {
            //if ()
            //{
                // check each position to see if it has a child (pikmin)
                // if yes, check next position in the list
                // if no, make the pikmin a child of that position
                // ++numberOfPikminCurrent
            //}
        }
    }

    private void CountChildrenOfTreasure()
    {
        // count the number of positions (could probably just do this in the inspector though this number will not change)
    }

    private void DismissPikmin() {
        // unparent the pikmin from their positions
        // reset current pikmin count to 0
    }
}
