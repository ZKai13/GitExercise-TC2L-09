using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // If using TextMeshPro

public class ButtonHoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject descriptionTextBox;  // The text box GameObject to show
    public string descriptionText;         // The description to display
    public Canvas parentCanvas;            // Reference to the parent canvas

    private TextMeshProUGUI descriptionTextMeshPro;  // For TMP Text
    private RectTransform descriptionRectTransform;  // RectTransform of the description box
    
    Vector2 offset = new Vector2(150f, 60f); // Offset to move description slightly from mouse position

    private bool isHovering = false; // Track if we're currently hovering over the button

    void Start()
    {
        // Set the description text based on whether you're using TMP
        if (descriptionTextBox.TryGetComponent(out TextMeshProUGUI tmpComponent))
        {
            descriptionTextMeshPro = tmpComponent;
        }

        // Get the RectTransform of the description box to move it later
        descriptionRectTransform = descriptionTextBox.GetComponent<RectTransform>();

        // If parentCanvas is not assigned, try to find it
        if (parentCanvas == null)
        {
            parentCanvas = GetComponentInParent<Canvas>();
        }

        // Hide the description text box initially
        descriptionTextBox.SetActive(false);
    }

    // Show the description when the cursor enters the button area
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true; // We're hovering over the button
        ShowDescription();
    }

    // Hide the description when the cursor exits the button area
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false; // No longer hovering
        HideDescription();
    }

    void Update()
    {
        // If hovering, update the position of the text box to follow the mouse
        if (isHovering)
        {
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform, 
                Input.mousePosition, 
                parentCanvas.worldCamera, 
                out mousePos);

            // Update the position of the description text box, adding the offset
            descriptionRectTransform.anchoredPosition = mousePos + offset;
        }
    }

    // Show the description text
    private void ShowDescription()
    {
        if (descriptionTextMeshPro != null)
        {
            descriptionTextMeshPro.text = descriptionText;
        }
    
        ChangeLayerOrder();
        descriptionTextBox.SetActive(true);
    }

    // Hide the description text
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
