using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : MonoBehaviour {
    public bool isFacingRight = true;
    public float leftX, rightX;
    public float speed = 0.2f;
	
	void Update () {
       


		if (isFacingRight)
        {
            if (transform.position.x <= rightX )
            transform.position += Vector3.right * Time.deltaTime * speed;
            else
                Flip();
        }
        else
        {
            if (leftX <= transform.position.x)
                transform.position += Vector3.left * Time.deltaTime * speed;
            else
                Flip();
        }
	}

    private void Flip()
    {
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }
}
