using UnityEngine;
[System.Serializable]
public class MonsterAnimConfig
{
    public string monsterName;   // Tên hiển thị gợi nhớ
    public string animPrefix;    // Tiền tố như "forest_troll_"
    public int startFrame;
    public int endFrame;
}
public class EnemyAnimation : MonoBehaviour
{
    private int currentSelectionIndex = -1;
    private SpriteRenderer spriteRenderer;

    public MonsterAnimConfig[] testAnimations = new MonsterAnimConfig[]
    {
        new MonsterAnimConfig { monsterName = "Troll - Walk Down", animPrefix = "forest_troll_", startFrame = 51, endFrame = 72 },
        new MonsterAnimConfig { monsterName = "Troll - Walk Up", animPrefix = "forest_troll_", startFrame = 26, endFrame = 50 },
        new MonsterAnimConfig { monsterName = "Troll - Walk Right", animPrefix = "forest_troll_", startFrame = 1, endFrame = 25 },
        new MonsterAnimConfig { monsterName = "Troll - Attack Down", animPrefix = "forest_troll_", startFrame = 73, endFrame = 86 },
        new MonsterAnimConfig { monsterName = "Troll - Idle", animPrefix = "forest_troll_", startFrame = 102, endFrame = 121 },
        new MonsterAnimConfig { monsterName = "Troll - Attack Up", animPrefix = "forest_troll_", startFrame = 122, endFrame = 133 },
        new MonsterAnimConfig { monsterName = "Troll - Death Normal", animPrefix = "forest_troll_", startFrame = 87, endFrame = 101 },
        new MonsterAnimConfig { monsterName = "Troll - Death Explosion", animPrefix = "forest_troll_", startFrame = 134, endFrame = 160 }
    };

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayAnimationByDirection(string direction, bool flipX)
    {
        int targetIndex = -1;
        switch (direction)
        {
            case "Down": targetIndex = 0; break;
            case "Up": targetIndex = 1; break;
            case "Right": targetIndex = 2; break;
            case "Idle": targetIndex = 4; break;
        }

        if (spriteRenderer != null) spriteRenderer.flipX = flipX;
        if (targetIndex == currentSelectionIndex || targetIndex == -1) return;

        PlayAnimationByIndex(targetIndex);
    }

    private void PlayAnimationByIndex(int index)
    {
        if (index < 0 || index >= testAnimations.Length) return;

        currentSelectionIndex = index;
        MonsterAnimConfig config = testAnimations[index];

        if (MonsterSpriteSheetAnimator.Instance != null)
        {
            MonsterSpriteSheetAnimator.Instance.PlayMonsterAnimation(
                this.gameObject,
                config.animPrefix,
                config.startFrame,
                config.endFrame
            );
        }
    }
}