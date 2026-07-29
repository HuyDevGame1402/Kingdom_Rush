using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ArrowColorEffect : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material blueMaterial;

    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock propBlock;
    private static readonly int IceBlendID = Shader.PropertyToID("_IceBlend");

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        propBlock = new MaterialPropertyBlock();

        if (defaultMaterial == null)
            defaultMaterial = spriteRenderer.sharedMaterial;
    }

    public void SetBlue()
    {
        if (blueMaterial != null)
            spriteRenderer.sharedMaterial = blueMaterial;

        // TẠO MỚI HOẶC CLEAR BLOCK TRƯỚC KHI SET
        propBlock.Clear();
        propBlock.SetFloat(IceBlendID, 0.9f);
        spriteRenderer.SetPropertyBlock(propBlock);
    }

    public void ResetColor()
    {
        spriteRenderer.sharedMaterial = defaultMaterial;

        // CLEAR DỌN SẠCH OVERRIDE ĐỂ LẤY MẶC ĐỊNH CỦA DEFAULT MATERIAL
        propBlock.Clear();
        spriteRenderer.SetPropertyBlock(propBlock);
    }
}