using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Rigidbody2D rb2d;

    public float speed;

    public Transform[] points;

    public int targetIndex;

    public void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (points.Length == 0)
            return;

        if ((transform.position - points[targetIndex].position).magnitude < 0.002f)
        {
            targetIndex = (targetIndex + 1) % points.Length;
        }

        Vector2 positionToTarget = points[targetIndex].position - transform.position;
        float magnitude = positionToTarget.magnitude;

        rb2d.MovePosition(rb2d.position + positionToTarget.normalized * Mathf.Min(speed * Time.deltaTime, magnitude));

        for (int i = 0; i < points.Length; ++i)
        {
            Debug.DrawLine(points[i].position, points[(i + 1) % points.Length].position, Color.cyan);
        }
    }
}
