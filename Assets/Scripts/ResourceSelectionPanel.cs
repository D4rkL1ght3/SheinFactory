using UnityEngine;
using UnityEngine.UI;

public class ResourceSelectionPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Buttons")]
    [SerializeField] private Button stoneButton;
    [SerializeField] private Button woodButton;
    [SerializeField] private Button sandButton;

    private Resourcer currentResourcer;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void Open(Resourcer resourcer)
    {
        currentResourcer = resourcer;

        panel.SetActive(true);

        AssignButton(stoneButton, MaterialType.Stone);
        AssignButton(sandButton, MaterialType.Sand);
        AssignButton(woodButton, MaterialType.Wood);
    }

    public void Close()
    {
        currentResourcer = null;

        panel.SetActive(false);
    }

    private void AssignButton(Button button, MaterialType material)
    {
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            currentResourcer.SetMaterial(material);
            Close();
        });
    }
}