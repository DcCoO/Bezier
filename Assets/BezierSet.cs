using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierSet : MonoBehaviour {

    public int number = -1;
    public List<BezierPoint> points;
    public Color color;
    public LineRenderer lineRenderer;

    public Vector2 T;


	public void init(int number) {
        this.number = number;
        color = new Color(Random.value, Random.value, Random.value, 1.0f);
        points = new List<BezierPoint>();
    }

    public void AddPoint(BezierPoint point) {
        point.dad = this;
        point.number = points.Count;
        point.gameObject.name = "Set " + number.ToString() + " - Point " + points.Count.ToString();
        points.Add(point);
        point.gameObject.GetComponent<SpriteRenderer>().material.color = color;
        point.transform.Translate(0, 0, -point.transform.position.z);
        StartCoroutine(Bezier());
    }

    public void DelPoint(int numb) {
        GameObject g = points[numb].gameObject;
        points.RemoveAt(numb);
        Destroy(g);
        StartCoroutine(Bezier());
    }



    int getPt(int n1, int n2, float perc) {
        int diff = n2 - n1;

        return n1 + (int)((float)diff * perc);
    }

    List<Vector2> positions;
    float numPoints = 100f;

    public IEnumerator Bezier() {
        //100 pontos
        if(points.Count == 1) {
            T = points[0].vec();
        }
        if(points.Count == 2) {
            lineRenderer.positionCount = 2;
            lineRenderer.material.color = color;

            for (int i = 0; i < 2; i++) {
                lineRenderer.SetPosition(i, points[i].vec());
                yield return null;
            }

            T = points[0].vec() + 0.2f * (points[1].vec() - points[0].vec()); 
        }

        else if (points.Count > 2) {
            positions = new List<Vector2>();

            bool calc = false;

            float rate = 1f / (numPoints) ;

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
                if (!calc && i > 0.2f) {
                    T = temp[0]; calc = true;
                }

                yield return null;
            }

            //draw lines
            lineRenderer.positionCount = positions.Count;
            lineRenderer.material.color = color;

            for (int i = 0; i < positions.Count; i++) {
                lineRenderer.SetPosition(i, positions[i]);
                yield return null;
            }
        }


    }

    float interpolate(float n1, float n2, float prec) {
        return n1 + ((n2 - n1) * prec);
    }

}
