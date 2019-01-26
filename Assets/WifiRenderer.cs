using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifiRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 center = gameObject.transform.position;
        float radius = gameObject.transform.lossyScale.x;
        GameObject[] casters = GameObject.FindGameObjectsWithTag("ShadowCaster");
        List<Vector4> lines = new List<Vector4>();
        Vector3 beep = Vector3.zero;
        Vector3 boop = Vector3.zero;

        foreach (GameObject caster in casters)
        {
            BoxCollider2D box = caster.GetComponent<BoxCollider2D>();
            if (box == null) continue;

            // Ignore platforms that are too far away.
            Vector2 normal = box.bounds.ClosestPoint(center) - center;
            Vector2 baseLine = new Vector2(-normal.y, normal.x);
            float baseAngle = Mathf.Atan2(baseLine.y, baseLine.x);
            float distance = normal.magnitude;
            if (distance >= radius) continue;

            // This box overlaps with the WiFi.
            // Compute min and max angle-distance pairs.
            Vector3 minB = box.bounds.min;
            Vector3 maxB = box.bounds.max;
            Vector2[] corners = { minB, new Vector2(minB.x, maxB.y), maxB, new Vector2(maxB.x, minB.y) };
            // Angle dist contains (angle a, distance a, angle b, distance b).
            List<Vector3> angleDists = new List<Vector3>(4);

            // Sort corners by angle relative to a base direction.
            foreach (Vector2 corner in corners)
            {
                Vector2 dir = corner - (Vector2)center;
                float angle = Mathf.Atan2(dir.y, dir.x);
                float relativeAngle = Mathf.DeltaAngle(angle, baseAngle);
                angleDists.Add(new Vector3(relativeAngle, angle, dir.magnitude / radius));
            }

            angleDists.Sort((p, q) => { return p.x.CompareTo(q.x); });

            lines.Add(new Vector4(angleDists[0].y, angleDists[0].z, angleDists[3].y, angleDists[3].z));
        }


        // Debug rendering.
        foreach (Vector4 line in lines)
        {
            Vector3 a = center + new Vector3(Mathf.Cos(line.x) * line.y, Mathf.Sin(line.x) * line.y, -0.1f);
            Vector3 b = center + new Vector3(Mathf.Cos(line.z) * line.w, Mathf.Sin(line.z) * line.w, -0.1f);
            Debug.DrawLine(a, b);
        }
    }
}
