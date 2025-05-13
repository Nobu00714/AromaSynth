using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasteSlider : MonoBehaviour
{
    public enum TasteType
    {
        salt,
        sweet,
        sour,
        umami,
        bitter
    }
    public TasteType taste;
    public TasteEqualizer tasteEqualizer;
    public RectTransform[] graphRectTransforms = new RectTransform[5];
    private bool isDrag = false;
    private Vector3 clickedPos;
    private Vector3 previousMousePos = new Vector3(0,0,0);
    private int count;
    public float slider_gain = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDrag)
        {
            if(count == 0)
            {
                previousMousePos = Input.mousePosition;
                count++;
            }
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 deltaMousePos = currentMousePos - previousMousePos;
            // graphRectTransforms[(int)taste].sizeDelta += new Vector2(0, deltaMousePos.y);

            tasteEqualizer.updateEqualizer_Slider((int)taste, deltaMousePos.y * slider_gain);
            previousMousePos = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            isDrag = false;
        }
    }

    public void OnClick()
    {
        count = 0;
        Debug.Log("saltSliderClicked");
        isDrag = true;
        previousMousePos = Input.mousePosition;
    }
}
