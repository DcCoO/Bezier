using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPoint : MonoBehaviour {
    
    public int set;
    private Transform t;
    public BezierSet dad;
    public float x;
    public float y;
    public int number;

    private void Start() {
        t = transform;
    }

    private void Update() {
        x = t.position.x;
        y = t.position.y;
    }

    private void OnMouseDrag() {
        if (BezierController.deleting) {
            dad.DelPoint(number);
        }
        else {
            t.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            t.Translate(0, 0, -t.position.z);
        }
    }

    private void OnMouseUp() {
        StartCoroutine(dad.Bezier());
    }

    public Vector2 vec() {
        return new Vector2(x, y);
    }




}
