using UnityEngine;
using static MaterialItem;

public class MaterialItem : MonoBehaviour
{
    [System.Serializable]
    public struct MaterialSprite
    {
        public MaterialType materialType;
        public Sprite sprite;
    }

    [SerializeField]
    private MaterialSprite[] materialSprites;

    [Header("Material")]
    [SerializeField] private MaterialType materialType;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    public MaterialType MaterialType => materialType;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        RefreshVisual();
    }

    public void SetMaterialType(MaterialType type)
    {
        materialType = type;
        RefreshVisual();
    }

    public void RefreshVisual()
    {
        if (spriteRenderer == null)
            return;

        foreach (MaterialSprite material in materialSprites)
        {
            if (material.materialType == materialType)
            {
                spriteRenderer.sprite = material.sprite;
                return;
            }
        }

        Debug.LogWarning($"No sprite assigned for material type: {materialType}");
    }
}