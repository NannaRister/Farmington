using UnityEngine;
using UnityEngine.InputSystem; 

public class GrowBlock : MonoBehaviour
{
    public enum GrowthStage
    {
        barren,
        ploughed,
        planted,
        growing1,
        growing2,
        ripe
    }

    public GrowthStage currentStage;

    public SpriteRenderer sR;
    public Sprite soilTilled;

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        Debug.Log("Tvinger sprite til soilTilled: " + soilTilled.name);
    //        sR.sprite = soilTilled;
    //    }
    //}

    public void AdvanceStage()
    {
        currentStage = currentStage + 1;

        if ((int)currentStage >= 6)
        {
            currentStage = GrowthStage.barren;
        }
    }

    public void SetSoilSprite()
    {
        //Debug.Log("Skifter sprite til: " + (currentStage == GrowthStage.barren ? "null" : soilTilled.name));

        if (currentStage == GrowthStage.barren)
        {
            sR.sprite = null;
        }
        else
        {
            sR.sprite = soilTilled;
        }
    }

    public void PloughSoil()
    {
        //Debug.Log("GrowBlock kaldes på: " + gameObject.name + " | Position: " + transform.position);


        if (currentStage == GrowthStage.barren)
        {
            currentStage = GrowthStage.ploughed;

            SetSoilSprite();
        }
    }


}
