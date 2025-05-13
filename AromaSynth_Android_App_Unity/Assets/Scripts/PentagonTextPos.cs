using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PentagonTextPos : MonoBehaviour
{
    public float radius;
    private int vertexCount = 5;
    public GameObject[] TasteTexts;
    public Canvas canvas;

    void Start()
    {
        TextPosPentagon();
    }

    void TextPosPentagon()
    {
        for(int i=0; i<vertexCount; i++)
        {
            float angle = (i * Mathf.PI * 2f) / vertexCount;
            float x = Mathf.Cos(angle + Mathf.PI / 2f) * radius;
            float y = Mathf.Sin(angle + Mathf.PI / 2f) * radius;
            Vector2 point = new Vector2(x,y);
            TasteTexts[i].transform.position = convertWorldToCanvas(point, canvas);
        }
    } 
    private Vector3 convertWorldToCanvas(Vector2 pos, Canvas _canvas) 
    {
        RectTransform canvasRect = _canvas.GetComponent<RectTransform> ();
        pos.x += canvasRect.transform.position.x;
        pos.y += canvasRect.transform.position.y;
        return new Vector3(pos.x, pos.y, canvasRect.transform.position.z);
    }
}
