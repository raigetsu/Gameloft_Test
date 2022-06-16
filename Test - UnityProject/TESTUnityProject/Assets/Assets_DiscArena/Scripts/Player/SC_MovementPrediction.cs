using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MovementPrediction : MonoBehaviour
{
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float distanceBetweenEachPoint = 0.5f;
    [SerializeField] private GameObject pointPrefab = null;

    [Header("DEBUG")]
    [SerializeField] private GameObject debugObjectForGizmo;
    [SerializeField] private Vector3 debugGizmoDefaultDir = new Vector3(4, 0, -4);

    private List<GameObject> pointList = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < maxDistance / distanceBetweenEachPoint; i++)
        {
            pointList.Add(Instantiate(pointPrefab));
            pointList[pointList.Count - 1].name = "Sphere_" + i;
        }
    }

    public void CalculateTrajectory(Vector3 pStartPosition, Vector3 pDirection, float pDistanceDo, int pPointIndex)
    {
        if (pDistanceDo >= maxDistance)
        {
            //print("MaxDistance : " + DistanceDo);
            return;
        }

        Ray ray = new Ray(pStartPosition, pDirection);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 0.55f, out hit))
        {
            #region Debug gizmo
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                Vector3 GizmoDir = hit.point;
                GizmoDir.y = pStartPosition.y;
                Gizmos.DrawLine(pStartPosition, hit.point);
            }
#endif
            #endregion

            #region Set Prediction Point Position
            int _StartIndex = pPointIndex;

            for (int i = _StartIndex; i < pointList.Count; i++)
            {
                if (hit.distance > distanceBetweenEachPoint * (i - _StartIndex))
                {
                    pointList[i].SetActive(true);
                    Vector3 _Dir = hit.point - pStartPosition;
                    _Dir = _Dir.normalized;

                    pointList[i].transform.position = pStartPosition + _Dir * distanceBetweenEachPoint * (i - _StartIndex);
                    pPointIndex++;
                }
                else
                {
                    pointList[i].SetActive(false);
                }
            }
            #endregion

            // The Disc will stop nearly
            if (pDirection.magnitude < 1f)
            {
                return;
            }

            #region Calculate Next Direction

            // Calculate Friction
            // it's not physic accurate to make the calcul more light
            float Time = hit.distance / pDirection.magnitude;
            float FrictionForce = Time * 0.01f * 40f; // The *40 is to make the friction more accurate (value based on some test)
            Vector3 AbsDir = AbsVector(pDirection);

            AbsDir = new Vector3(AbsDir.x - FrictionForce, 0f, AbsDir.z - FrictionForce);
            AbsDir.x = AbsDir.x * Mathf.Sign(pDirection.x);
            AbsDir.z = AbsDir.z * Mathf.Sign(pDirection.z);
            pDirection = AbsDir;

            // Calculate normal with bounce factor
            Vector3 normal = new Vector3(hit.normal.x * 0.5f, 0f, hit.normal.z * 0.5f);
            normal = AbsVector(normal);

            // Calculate the velocity lose with rebound
            normal = new Vector3(normal.x * pDirection.x, 0f, normal.z * pDirection.z);

            // Setup the new velocity
            pDirection = pDirection - normal;

            Vector3 newDirection = Vector3.Reflect(pDirection, hit.normal);
            newDirection.y = 0f;

            #endregion
            CalculateTrajectory(hit.point, newDirection, pDistanceDo + hit.distance, pPointIndex);
        }
        #region Debug Gizmo
#if UNITY_EDITOR

        else
        {
            if (Application.isPlaying == false)
            Gizmos.DrawLine(pStartPosition, pDirection * 10f);
        }
#endif
        #endregion
    }

    Vector3 AbsVector(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    private void OnDrawGizmos()
    {
        CalculateTrajectory(debugObjectForGizmo.transform.position, debugGizmoDefaultDir, 0f, 0);
    }
}
