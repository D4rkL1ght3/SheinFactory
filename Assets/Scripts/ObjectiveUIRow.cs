using TMPro;
using UnityEngine;

public class ObjectiveUIRow : MonoBehaviour
{
    [SerializeField] private TMP_Text materialName;

    [SerializeField] private TMP_Text progressText;

    public void Setup(ObjectiveData objective)
    {
        materialName.text = objective.targetMaterial.ToString();

        progressText.text =
            $"{objective.deliveredAmount} / {objective.requiredAmount}";
    }
}