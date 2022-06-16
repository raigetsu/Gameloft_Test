using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MovementPrediction : MonoBehaviour
{
    [SerializeField] private SC_DiscMaster disc = null;
    [SerializeField] private float MaxDistance = 1000f;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount >= 1)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    print("Touch Began");
                    break;
                case TouchPhase.Moved:
                    print("Touch Moved");
                    break;
                case TouchPhase.Ended:
                    print("Touch ended");
                    break;
                case TouchPhase.Canceled:
                    break;
            }
        }
    }

    void CalculateTrajectory(Vector3 StartPosition, Vector3 Direction, float DistanceDo, Color gizmoColor)
    {
        if (DistanceDo >= MaxDistance)
        {
            //print("MaxDistance : " + DistanceDo);
            return;
        }

        Gizmos.color = gizmoColor;

        Ray ray = new Ray(StartPosition, Direction);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 0.55f, out hit))
        {
            // Draw Gizmo Debug
            Vector3 GizmoDir = hit.point;
            GizmoDir.y = StartPosition.y;
            Gizmos.DrawLine(StartPosition, hit.point);

            // The Disc will stop nearly
            if (Direction.magnitude < 1f)
            {
                return;
            }

            // Calculate Friction
            // it's not physic accurate to make the calcul more light
            float Time = hit.distance / Direction.magnitude;
            float FrictionForce = Time * 0.01f * 40f; // The *40 is to make the friction more accurate (value based on some test)
            Vector3 AbsDir = AbsVector(Direction);
            AbsDir = new Vector3(AbsDir.x - FrictionForce, 0f, AbsDir.z - FrictionForce);
            AbsDir.x = AbsDir.x * Mathf.Sign(Direction.x);
            AbsDir.z = AbsDir.z * Mathf.Sign(Direction.z);
            Direction = AbsDir;

            // Calculate normal with bounce factor
            Vector3 normal = new Vector3(hit.normal.x * 0.5f, 0f, hit.normal.z * 0.5f);
            normal = AbsVector(normal);
            // Calculate the velocity lose with rebound
            normal = new Vector3(normal.x * Direction.x, 0f, normal.z * Direction.z);

            // Setup the new velocity
            Direction = Direction - normal;            

            Vector3 newDirection = Vector3.Reflect(Direction, hit.normal);
            newDirection.y = 0f;

            CalculateTrajectory(hit.point, newDirection, DistanceDo + hit.distance, Color.white);
        }
        else
        {
            Gizmos.DrawLine(StartPosition, Direction * 10f);
        }
    }

    Vector3 AbsVector(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }
    private void OnDrawGizmos()
    {
        Vector3 StartPos = disc.gameObject.transform.position;
        // StartPos.y += 0.2f;
        CalculateTrajectory(StartPos, new Vector3(4, 0, -4), 0f, Color.yellow);
        // CalculateTrajectory(disc.gameObject.transform.position, new Vector3(-4, 0, -4), 0f, Color.blue);
    }
}
