using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLineRender : MonoBehaviour
{
    [SerializeField]
    private float startWidth = 0.01f; // 시작 부분의 선의 넓이
    [SerializeField]
    private float endWidth = 0.001f; // 끝 부분의 선의 넓이

    private LineRenderer lineRenderer; // 사용할 라인 렌더러
    private Color fadeColor; // lerp 된 컬러 값을 저장할 변수
    private WaitForSeconds longWait = new WaitForSeconds(0.5f);
    private WaitForSeconds shortWait = new WaitForSeconds(0.05f);

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    public void StartRender(Vector3 startPoint, Vector3 endPoint)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
        lineRenderer.startColor = Color.clear;
        lineRenderer.endColor = Color.clear;

        StopAllCoroutines();
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.startWidth = startWidth;
        yield return longWait;

        float percent = 0;
        float speed = 4;
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadeColor = Color.Lerp(Color.white, Color.clear, percent);
            lineRenderer.startColor = fadeColor;
            lineRenderer.startWidth = Mathf.Lerp(startWidth, endWidth, percent);
            yield return shortWait;
        }

        lineRenderer.startColor = Color.clear;
        lineRenderer.endColor = Color.clear;
        lineRenderer.startWidth = endWidth;
        yield break;
    }
}
