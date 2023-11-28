using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float detection_delay = 0.5f;

    public enemyNavAgent eVA;

    private Collider player_collider;
    private SphereCollider detection_collider;
    private Bounds player_bounds;
    private Coroutine detect_player;

    private void Awake()
    {
        detection_collider = this.GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.gameObject;
            detect_player = StartCoroutine(DetectPlayer());
            player_collider = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            target = null;
            StopCoroutine(detect_player);
            // player is hidden
        }
    }

    IEnumerator DetectPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(detection_delay);

            Vector3[] points = GetBoundingPoints(player_collider.bounds);

            int points_hidden = 0;

            foreach (Vector3 point in points)
            {
                Vector3 target_direction = point - this.transform.position;
                float target_distance = Vector3.Distance(this.transform.position, point);
                float target_angle = Vector3.Angle(target_direction, this.transform.forward);

                if (IsPointCovered(target_direction, target_distance) || target_angle > 70)
                    ++points_hidden;
            }

            if (points_hidden >= points.Length)
            {
                //player is hidden
                //eVA.pursue = true;
                eVA.patrol();

            }
            else
            {
                //player is visible
                //eVA.patrol();
                eVA.pursue = true;

            }
            //eVA.Update();
        }
    }

    private bool IsPointCovered(Vector3 target_direction, float target_distance)
    {
        RaycastHit[] hits = Physics.RaycastAll(this.transform.position, target_direction, detection_collider.radius);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Cover"))
            {
                float cover_distance = Vector3.Distance(this.transform.position, hit.point);

                if (cover_distance < target_distance)
                    return true;
            }
        }

        return false;
    }

    private Vector3[] GetBoundingPoints(Bounds bounds)
    {
        Vector3[] bounding_points =
        {
            bounds.min,
            bounds.max,
            new Vector3( bounds.min.x, bounds.min.y, bounds.max.z ),
            new Vector3( bounds.min.x, bounds.max.y, bounds.min.z ),
            new Vector3( bounds.max.x, bounds.min.y, bounds.min.z ),
            new Vector3( bounds.min.x, bounds.max.y, bounds.max.z ),
            new Vector3( bounds.max.x, bounds.min.y, bounds.max.z ),
            new Vector3( bounds.max.x, bounds.max.y, bounds.min.z )
        };

        return bounding_points;
    }

}
