using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierController : MonoBehaviour {
    public static bool deleting = false;
    public GameObject point;
    public GameObject set;
    public int setNumber = 0;
    BezierSet[] list;
    public LineRenderer lineRenderer;

    private void Awake() {
        list = new BezierSet[10];
        for(int i = 0; i < 10; i++) {
            GameObject g = Instantiate(set, Vector3.zero, Quaternion.identity);
            g.name = "Set " + i.ToString();
            list[i] = g.GetComponent<BezierSet>();
            list[i].init(i);
        }
    }
    void Update() {
        deleting = Input.GetKey(KeyCode.D);
        if (Input.GetMouseButtonDown(1)) {
            GameObject g = Instantiate(point, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            list[setNumber].AddPoint(
                g.GetComponent<BezierPoint>()
            );
            g.transform.parent = list[setNumber].transform;
        }

        for (int i = 0; i < 10; i++) {
            if (!Input.GetKeyDown(keyCodes[i])) continue;
            setNumber = i;
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            StartCoroutine(SuperBezier());
        }

    }

    List<Vector2> positions;
    float numPoints = 100f;

    public IEnumerator SuperBezier() {
        //100 pontos
        List<Vector2> points = new List<Vector2>();
        for(int i = 0; i < 10; i++) {
            if (list[i].points.Count != 0) points.Add(list[i].T);
            yield return null;
        }

        
        if (points.Count == 2) {
            lineRenderer.positionCount = 2;
            lineRenderer.material.color = Color.white;

            for (int i = 0; i < 2; i++) {
                lineRenderer.SetPosition(i, points[i]);
                yield return null;
            }
            
        }

        else if (points.Count > 2) {
            positions = new List<Vector2>();

            bool calc = false;

            float rate = 1f / (numPoints);

            positions.Add(new Vector2(points[0].x, points[0].y));

            for (float i = 0f; i < 1f; i += rate) {

                List<Vector2> temp = new List<Vector2>();
                for (int j = 1; j < points.Count; ++j)
                    temp.Add(new Vector2(interpolate(points[j - 1].x, points[j].x, i),
                                            interpolate(points[j - 1].y, points[j].y, i)));

                while (temp.Count > 1) {
                    List<Vector2> temp2 = new List<Vector2>();

                    for (int j = 1; j < temp.Count; ++j) {
                        temp2.Add(
                            new Vector2(
                                interpolate(temp[j - 1].x, temp[j].x, i), interpolate(temp[j - 1].y, temp[j].y, i)
                            )
                        );
                    }
                    temp = temp2;
                }
                positions.Add(temp[0]);

                yield return null;
            }

            //draw lines
            lineRenderer.positionCount = positions.Count;
            lineRenderer.material.color = Color.white;

            for (int i = 0; i < positions.Count; i++) {
                lineRenderer.SetPosition(i, positions[i]);
                yield return null;
            }
        }


    }

    float interpolate(float n1, float n2, float prec) {
        return n1 + ((n2 - n1) * prec);
    }








    private KeyCode[] keyCodes = {
        KeyCode.Alpha0,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
     };



}