using UnityEngine;
using UnityEngine.UI;

public class InspectUI : MonoBehaviour
{
    public static InspectUI Instance { get; private set; }

    [Header("UI References")]
    public GameObject panel; // Root panel to enable/disable
    public Text titleText;
    public Text descriptionText;
    public Image iconImage;
    public Button closeButton;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (closeButton != null) closeButton.onClick.AddListener(Hide);
        if (panel != null) panel.SetActive(false);
    }

    public void Show(string title, string description, Sprite icon = null)
    {
        if (panel != null) panel.SetActive(true);
        if (titleText != null) titleText.text = title ?? "";
        if (descriptionText != null) descriptionText.text = description ?? "";
        if (iconImage != null) iconImage.sprite = icon;
    }

    public void Hide()
    {
        if (panel != null) panel.SetActive(false);
    }
}