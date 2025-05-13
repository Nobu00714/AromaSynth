using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTasteButton : MonoBehaviour
{
    public TasteEqualizer tasteEqualizer;
    public void ResetTaste()
    {
        tasteEqualizer.updateEqualizer_Receipe(0,0,0,0,0);
    }
}
