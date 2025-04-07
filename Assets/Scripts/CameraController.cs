using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;


    public Transform clampMin, clampMax;

    private Camera cam;
    private float halfWidth, halfHeight;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = FindAnyObjectByType<PlayerController>().transform;

        clampMin.SetParent(null);
        clampMax.SetParent(null);

        cam = GetComponent<Camera>();
        halfHeight = cam.orthographicSize; //projection i inspector vinduet p� main camera - her 8, svarende til 8 bloks
        halfWidth = cam.orthographicSize * cam.aspect;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        Vector3 clampedPosition = transform.position;
        //vi laver en lokalvariable clampedPosition

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, clampMin.position.x + halfWidth, clampMax.position.x - halfWidth);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, clampMin.position.y + halfHeight, clampMax.position.y - halfHeight);
        //vi tilf�jet halfWidth og halfHeight p� metoderne

        transform.position = clampedPosition;

    }
}
