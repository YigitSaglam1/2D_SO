using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public GameObject topcuk;

    private Vector3 startVector;
    private Vector3 controlVector;
    private Vector3 endVector;
    private float count;
    private void Start()
    {
        startVector = startPoint.transform.position;
        endVector = endPoint.transform.position;

        controlVector = startVector + (endVector - startVector) / 2 + new Vector3(1,0,0) * 5f;
        topcuk.transform.position = startVector;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StopCoroutine(CurveMove());
            StartCoroutine(CurveMove());
        }
    }

    private IEnumerator CurveMove()
    {

        topcuk.transform.position = startVector;
        topcuk.SetActive(true);
        yield return null;
        while (count < 1f)
        {
            count += 1f * Time.deltaTime;

            Vector3 m1 = Vector3.Lerp(startVector, controlVector, count);
            Vector3 m2 = Vector3.Lerp(controlVector, endVector, count);
            topcuk.transform.position = Vector3.Lerp(m1, m2, count);
            yield return null;
        }
        
        yield return null;

        topcuk.SetActive(false);



    }
}
