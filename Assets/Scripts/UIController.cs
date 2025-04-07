using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance; //singleton type variable
    private void Awake()
    {
        instance = this; //sætter den her UI controller instans til at være THE ONE, som alle de andre ui controllers (der måske senere kommer til) arbejder efter 
    }

    public GameObject[] toolbarActivatorIcons;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SwitchTool(0); //bare til test om den faktisk skifter imellem de forskellige tools ud fra mit array i UI canvas


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchTool(int selected)
    {
        foreach (GameObject icon in toolbarActivatorIcons)
        {
            icon.SetActive(false); //deaktivere alle mine gameobjekter i mit UI canvas array
        }

        toolbarActivatorIcons[selected].SetActive(true);
    }
}
