using UnityEngine;

public class ParallaxGround : MonoBehaviour
{
    private Transform camTF;
    private Vector3 lastFrameCameraPos;

    [SerializeField] private Vector2 parallaxFactor;    

    private void Start()
    {
        camTF = Camera.main.transform;
        lastFrameCameraPos = camTF.position;
    }

    private void Update()
    {
        Vector2 deltaMovement = camTF.position - lastFrameCameraPos;
        transform.position += new Vector3(deltaMovement.x * parallaxFactor.x, deltaMovement.y * parallaxFactor.y);
        lastFrameCameraPos = camTF.position;

    }
}
