using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MParentWeapon : MonoBehaviour
{
    public GameObject mParentCon;

    private enum Mode
    {
        Idle,
        Back,
        HandL,
        HandR,
        DualHanded
    }

    private Mode m_Mode;

    public void Update()
    {
        if (m_Mode != Mode.Back)
        {
            var constraint = mParentCon.GetComponent<MultiParentConstraint>();
            var sourceObjects = constraint.data.sourceObjects;

            sourceObjects.SetWeight(0, m_Mode == Mode.Back ? 1f : 0f);
            sourceObjects.SetWeight(1, m_Mode == Mode.HandL ? 1f : 0f);
            sourceObjects.SetWeight(2, m_Mode == Mode.HandR ? 1f : 0f);
            sourceObjects.SetWeight(3, m_Mode == Mode.DualHanded ? 1f : 0f);

            constraint.data.sourceObjects = sourceObjects;

            m_Mode = Mode.Idle;
        }
    }

    public void Start()
    {
        Back();
    }
    public void HandL()
    {
        m_Mode = Mode.HandL;
        Debug.Log("left hand");
    }
    public void HandR()
    {
        m_Mode = Mode.HandR;
        Debug.Log("right hand");
    }
    public void Back()
    {
        m_Mode = Mode.Back;
        Debug.Log("back");
    }
    public void DualHanded()
    {
        m_Mode = Mode.DualHanded;
        Debug.Log("dual wielding");
    }
}
