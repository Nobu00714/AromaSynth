using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AromaSlider : MonoBehaviour
{
    public float slider_gain = 0.5f;
    private bool isDrag = false;
    private Vector3 previousMousePos = new Vector3(0, 0, 0);
    private Transform graph;
    private Transform background;
    private Vector2 backgroundSize;
    private Vector2 graphSize;
    void Start()
    {
        graph = this.gameObject.transform.Find("Graph");
        graphSize = graph.gameObject.GetComponent<RectTransform>().sizeDelta;
        background = this.gameObject.transform.Find("Background");
        backgroundSize = background.gameObject.GetComponent<RectTransform>().sizeDelta;
    }
    void Update()
    {
        if (isDrag)
        {
            // マウスの移動量を計算
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 deltaMousePos = currentMousePos - previousMousePos;

            // ドラッグによる移動幅が同じになるように調整してグラフサイズを変更
            if (graph != null)
            {
                graph.gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, deltaMousePos.x * slider_gain);
                //サイズをバックグラウンドの範囲内に調整
                if (graph.gameObject.GetComponent<RectTransform>().sizeDelta.y < 0)
                {
                    graph.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(graphSize.x, 0);
                }
                if (graph.gameObject.GetComponent<RectTransform>().sizeDelta.y > backgroundSize.y)
                {
                    graph.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(graphSize.x, backgroundSize.y);
                }
            }
            else
            {
                Debug.Log("Graph not found");
            }



            // 1フレーム前のマウス位置を保存
            previousMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
        }
    }

    public void OnClick()
    {
        isDrag = true;
        previousMousePos = Input.mousePosition;
    }

    public void SetGraphSize(float size)
    {
        if (graph != null)
        {
            graph.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(graphSize.x, size);
        }
        else
        {
            Debug.Log("Graph not found");
        }
    }
}
