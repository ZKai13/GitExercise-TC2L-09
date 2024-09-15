using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // If using TextMeshPro

public class ButtonHoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject descriptionTextBox;  
    public string descriptionText;         
    public Canvas parentCanvas;            

    private TextMeshProUGUI descriptionTextMeshPro;  
    private RectTransform descriptionRectTransform;  
    
    Vector2 offset = new Vector2(150f, 60f); // Offset to move description slightly from mouse position

    private bool isHovering = false; 

    void Start()
    {
        
        if (descriptionTextBox.TryGetComponent(out TextMeshProUGUI tmpComponent))
        {
            descriptionTextMeshPro = tmpComponent;
        }

       
        descriptionRectTransform = descriptionTextBox.GetComponent<RectTransform>();

       
        if (parentCanvas == null)
        {
            parentCanvas = GetComponentInParent<Canvas>();
        }

        descriptionTextBox.SetActive(false);
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true; 
        ShowDescription();
    }

    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false; 
        HideDescription();
    }

    void Update()
    {
   
        if (isHovering)
        {
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform, 
                Input.mousePosition, 
                parentCanvas.worldCamera, 
                out mousePos);

 
            descriptionRectTransform.anchoredPosition = mousePos + offset;
        }
    }


    private void ShowDescription()
    {
        if (descriptionTextMeshPro != null)
        {
            descriptionTextMeshPro.text = descriptionText;
        }
    
        ChangeLayerOrder();
        descriptionTextBox.SetActive(true);
    }

    private void HideDescription()
    {
        descriptionTextBox.SetActive(false);
    }


//Layer
    public GameObject canvasGroupToMove;
    public int newLayerIndex;
    void ChangeLayerOrder()
    {
        RectTransform rectTransform = canvasGroupToMove.GetComponent<RectTransform>();
        rectTransform.SetSiblingIndex(newLayerIndex);
    }

    
}
