using UnityEngine;

public class ParallaxGround : MonoBehaviour
{
    private Transform camTF;
    private Vector3 lastFrameCameraPos;
    private float textureUnitSizeX;
    private float textureUnitSizeY;

    [SerializeField] private Vector2 parallaxFactor;
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;

    private void Start()
    {
        camTF = Camera.main.transform;
        lastFrameCameraPos = camTF.position;
        Sprite sprite = this.GetComponent<SpriteRenderer>().sprite;
        textureUnitSizeX = sprite.texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = sprite.texture.height / sprite.pixelsPerUnit;

    }

    private void Update()
    {
        Vector2 deltaMovement = camTF.position - lastFrameCameraPos;
        transform.position += new Vector3(deltaMovement.x * parallaxFactor.x, deltaMovement.y * parallaxFactor.y);
        lastFrameCameraPos = camTF.position;

        if (lockX)
        {
            if (Mathf.Abs(camTF.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offsetX = camTF.position.x - transform.position.x;
                transform.position += new Vector3(camTF.position.x + offsetX, transform.position.y);  
            }
        }

        if (lockY)
        {
            if (Mathf.Abs(camTF.position.y - transform.position.y) >= textureUnitSizeY)  
            {
                float offsetY = camTF.position.y - transform.position.y;
                transform.position += new Vector3(transform.position.x, camTF.position.y + offsetY );  
            }
        }
    }
}
