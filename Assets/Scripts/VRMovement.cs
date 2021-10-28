using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRMovement : MonoBehaviour
{
    public float m_Sensetivity = 0.1f;
    public float m_maxSpeed = 1.0f;

    public SteamVR_Action_Boolean m_MovePress = null;
    public SteamVR_Action_Vector2 m_MoveValue = null;
    public Transform m_Head = null;

    private float m_SpeedForvard = 0;
    private float m_SpeedRight = 0;

    private CharacterController m_CharacterController = null;


    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        HandleHeight();
    }

    private void CalculateMovement()
    {
        Vector3 orientationEuler = new Vector3(0, m_Head.transform.eulerAngles.y, 0);
        Quaternion orientation = Quaternion.Euler(orientationEuler);
        Vector3 movement = Vector3.zero;

        if (m_MovePress.GetStateUp(SteamVR_Input_Sources.Any))
            m_SpeedForvard = 0;

        if (m_MovePress.state)
        {
            m_SpeedForvard += m_MoveValue.axis.y * m_Sensetivity;
            m_SpeedRight += m_MoveValue.axis.x * m_Sensetivity;
            m_SpeedForvard = Mathf.Clamp(m_SpeedForvard, -m_maxSpeed, m_maxSpeed);
            m_SpeedRight = Mathf.Clamp(m_SpeedRight, -m_maxSpeed, m_maxSpeed);
            movement += orientation * ((m_SpeedForvard * Vector3.forward) + (m_SpeedRight * Vector3.right)) * Time.deltaTime;
        }

        m_CharacterController.Move(movement);
    }

    private void HandleHeight()
    {
        float headHeight = Mathf.Clamp(m_Head.localPosition.y, 1, 2);
        m_CharacterController.height = headHeight;

        Vector3 newCenter = Vector3.zero;
        newCenter.y = m_CharacterController.height / 2;
        newCenter.y += m_CharacterController.skinWidth;

        newCenter.x = m_Head.localPosition.x;
        newCenter.z = m_Head.localPosition.z;

        newCenter = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * newCenter;

        m_CharacterController.center = newCenter;
        
    }

}
