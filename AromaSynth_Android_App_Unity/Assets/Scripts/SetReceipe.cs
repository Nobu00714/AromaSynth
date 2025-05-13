using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetReceipe : MonoBehaviour
{
    public float Salt_substances;
    public float Sweet_substances;
    public float Sour_substances;
    public float Umami_substances;
    public float Bitter_substances;
    public TasteEqualizer tasteEqualizer;

    public void OnClick()
    {
        tasteEqualizer.updateEqualizer_Receipe(Salt_substances, Sweet_substances, Sour_substances, Umami_substances,Bitter_substances);
    }
}
