using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public Text seedCount;


    public void updateSeedCount(int newCount, bool seedsFull)
    {
        seedCount.text = " X " + newCount;
        if (seedsFull)
            seedCount.color = Color.red;
        else
            seedCount.color = new Color(0.5283019f, 0.5283019f, 0.5283019f);

    }
}
