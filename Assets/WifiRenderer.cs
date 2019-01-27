using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WifiRenderer : MonoBehaviour
{
    public enum ColorSchema
    {
        GGJ = 0,
        Green = 1,
        Golden = 2
    }
    private ColorSchema _color;
    public ColorSchema Colors;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Set color schema to shader.
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material.SetInt("_ColorSchema", (int)Colors);

        Vector3 center = gameObject.transform.position;
        float radius = gameObject.transform.lossyScale.x;
        GameObject[] casters = GameObject.FindGameObjectsWithTag("ShadowCaster");
        List<Vector4> lines = new List<Vector4>();

        foreach (GameObject caster in casters.Take(16))
        {
            BoxCollider2D box = caster.GetComponent<BoxCollider2D>();
            if (box == null) continue;

            // Ignore platforms that are too far away.
            Vector2 normal = box.bounds.ClosestPoint(center) - center;
            Vector2 baseLine = new Vector2(normal.y, -normal.x);
            //float baseAngle = Mathf.Atan2(baseLine.y, baseLine.x);
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
                float angle = Vector2.Dot(baseLine.normalized, dir.normalized);
                angleDists.Add(new Vector3(angle, dir.x / radius, dir.y / radius));
            }

            angleDists.Sort((p, q) => { return p.x.CompareTo(q.x); });

            lines.Add(new Vector4(angleDists[0].y, angleDists[0].z, angleDists[3].y, angleDists[3].z));
        }

        // Set in shader.

        renderer.material.SetInt("_NumShadowLines", lines.Count);
        for (int l = lines.Count; l < 16; ++l)
            lines.Add(Vector4.zero);
        Debug.Assert(lines.Count == 16);
        renderer.material.SetVectorArray("_ShadowLines", lines.ToArray());
    }
}
