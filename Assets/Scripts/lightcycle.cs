using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lightcycle : MonoBehaviour
{

    public Light2D globalLight; // Link til din Global Light 2D

    [SerializeField]
    //public float dayDurationInMinutes = 24f;
    public float dayDurationInMinutes = 0.1667f; // 10 sekunder for et helt døgn


    [SerializeField]
    private float timeOfDay; // Værdi fra 0 til 1

    void Update()
    {
        // Gør tiden uafhængig af framerate
        timeOfDay += Time.deltaTime / (dayDurationInMinutes * 60f);
        if (timeOfDay > 1f)
            timeOfDay -= 1f;


        UpdateLighting(timeOfDay);
    }

    void UpdateLighting(float t)
    {
        // Sinuskurve giver smooth overgang
        float rawIntensity = Mathf.Sin(t * Mathf.PI * 2f - Mathf.PI / 2f) * 0.5f + 0.5f;

        // Justér minimumslysstyrke (f.eks. 0.2 → aldrig helt mørkt)
        float intensity = Mathf.Lerp(0.2f, 1f, rawIntensity);

        globalLight.intensity = intensity;

        // Blend farve fra mørk blå → hvid
        globalLight.color = Color.Lerp(new Color(0.1f, 0.2f, 0.4f), Color.white, intensity);
    }
}


