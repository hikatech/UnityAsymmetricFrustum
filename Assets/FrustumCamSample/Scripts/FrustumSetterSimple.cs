using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class FrustumSetterSimple : MonoBehaviour
{

    [SerializeField]
    float m_left = -1;
    [SerializeField]
    float m_right = 1;
    [SerializeField]
    float m_top = 1;
    [SerializeField]
    float m_bottom = -1;
    [SerializeField]
    float m_near = 1;
    [SerializeField]
    float m_far = 1000;

    Camera m_cam = null;

    // Use this for initialization
    void Start () {
        m_cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_cam != null)
        {
            // set projection matrix
            m_cam.projectionMatrix = Matrix4x4.Frustum(m_left, m_right, m_bottom, m_top, m_near, m_far);

            // for displaying right bounds of viewing frustum (Not absolutely necessary)
            m_cam.nearClipPlane = m_near;
            m_cam.farClipPlane = m_far;
        }
    }
}
