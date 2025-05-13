using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartDrawer : MonoBehaviour
{
    public float[] radius;
    public float width;
    public Canvas canvas;
    private int vertexCount;
    public LineRenderer[] lineRenderer = new LineRenderer[5];

    void Start()
    {
        vertexCount = radius.Length;
        for(int i=0; i<5; i++)
        {
            lineRenderer[i].material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer[i].startWidth = width;
            lineRenderer[i].endWidth = width;
            lineRenderer[i].positionCount = 2;
        }
        DrawPentagon();
    }

    public void DrawPentagon()
    {
        Vector3[] positions = new Vector3[vertexCount];

        for(int i=0; i<vertexCount; i++)
        {
            float angle = (i * Mathf.PI * 2f) / vertexCount;
            float x = Mathf.Cos(angle + Mathf.PI / 2f) * radius[i];
            float y = Mathf.Sin(angle + Mathf.PI / 2f) * radius[i];
            Vector2 point = new Vector2(x,y);
            positions[i] = convertWorldToCanvas(point, canvas);
        }

        for(int i=0; i<5; i++)
        {
            Vector3[] linePositions = {convertWorldToCanvas(new Vector2(0,0), canvas), positions[i]};
            lineRenderer[i].SetPositions(linePositions);
        }
        
    } 

    private Vector3 convertWorldToCanvas(Vector2 pos, Canvas _canvas) 
    {
        RectTransform canvasRect = _canvas.GetComponent<RectTransform> ();
        pos.x += canvasRect.transform.position.x;
        pos.y += canvasRect.transform.position.y;
        return new Vector3(pos.x, pos.y, canvasRect.transform.position.z + 10.0f);
    }
}
