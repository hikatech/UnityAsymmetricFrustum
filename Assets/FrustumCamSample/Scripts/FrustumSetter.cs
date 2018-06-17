using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class FrustumSetter : MonoBehaviour
{

    Camera m_cam = null;

    [SerializeField]
    float m_nearFarDistance = 100;

    [SerializeField]
    GameObject m_monitorQuad = null;

    // Use this for initialization
    void Start()
    {
        m_cam = GetComponent<Camera>();

        if (m_monitorQuad != null)
        {
            Renderer r = m_monitorQuad.GetComponent<Renderer>();
            if (r != null)
            {
                r.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_monitorQuad != null && m_cam != null)
        {
            SetFrustumFromQuad(m_cam, m_monitorQuad);
        }
    }

    void SetFrustumFromQuad(Camera cam, GameObject quad)
    {
        float m_left = -1;
        float m_right = 1;
        float m_top = 1;
        float m_bottom = -1;
        float m_near = 1;
        float m_far = 1000;

        // quads Z rotation (roll) is locked
        quad.transform.localEulerAngles = new Vector3(quad.transform.localEulerAngles.x, quad.transform.localEulerAngles.y, 0);

        // set near far plane
        m_near = GetDistanceFromQuad(cam, quad);
        cam.nearClipPlane = m_near;
        m_far = m_near + m_nearFarDistance;
        cam.farClipPlane = m_far;

        // set camera rotation
        cam.transform.forward = m_monitorQuad.transform.forward;

        //set left right bottom top
        {
            Matrix4x4 worldToCamMat = cam.worldToCameraMatrix;
            Mesh quadMesh = quad.GetComponent<MeshFilter>().sharedMesh;

            //transform Quad corner-vertices position from local space to world space
            Vector3 vertexLeftBottom = quad.transform.localToWorldMatrix.MultiplyPoint(quadMesh.vertices[0]);
            Vector3 vertexRightTop = quad.transform.localToWorldMatrix.MultiplyPoint(quadMesh.vertices[1]);

            //transform from world space to camera space
            Vector3 leftBottom = worldToCamMat.MultiplyPoint(vertexLeftBottom);
            Vector3 rightTop = worldToCamMat.MultiplyPoint(vertexRightTop);

            m_left = leftBottom.x;
            m_bottom = leftBottom.y;
            m_right = rightTop.x;
            m_top = rightTop.y;
        }
        //set projection matrix
        cam.projectionMatrix = Matrix4x4.Frustum(m_left, m_right, m_bottom, m_top, m_near, m_far);
    }

    float GetDistanceFromQuad(Camera cam, GameObject quad)
    {
        Vector3 qNormal = quad.transform.forward.normalized;
        Vector3 qPos = cam.transform.position - quad.transform.position;

        float distance = Mathf.Abs(Vector3.Dot(qNormal, qPos));

        return distance;
    }


}