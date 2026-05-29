using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class MonsterSpriteSheetAnimator : MonoBehaviour
{
    [Header("Data Source")]
    public TextAsset monsterDataFile;

    [Header("Textures Atlas")]
    [Tooltip("Kéo file ảnh 'go_enemies_acaroth-1.png' vào đây")]
    public Texture2D enemyAtlas;

    [Header("Settings")]
    public float defaultFrameRate = 0.08f;
    public float pixelsPerUnit = 100f;

    private Dictionary<string, SpriteData> spriteDatabase = new Dictionary<string, SpriteData>();
    private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

    // SỬA LỖI: Quản lý Coroutine riêng biệt theo từng GameObject để tránh xung đột đè lệnh
    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();

    public static MonsterSpriteSheetAnimator Instance { get; private set; }

    private class SpriteData
    {
        public string name;
        public Rect f_quad;      // Tọa độ trên Atlas
        public Vector2 fullSize; // Kích thước size={w, h}
        public Vector4 trim;     // trim={left, top, right, bottom}
        public string atlasName;
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (enemyAtlas) textures["go_enemies_acaroth-1.png"] = enemyAtlas;

        ParseMonsterData();
    }

    private void ParseMonsterData()
    {
        if (monsterDataFile == null) return;

        string content = monsterDataFile.text;
        string pattern = @"\[""(?<name>.+?)""\].+?size=\{(?<size>.+?)\}.+?trim=\{(?<trim>.+?)\}.+?a_name=""(?<atlas>.+?)"".+?f_quad=\{(?<quad>.+?)\}.+?alias=\{(?<alias>.*?)\}";
        MatchCollection matches = Regex.Matches(content, pattern);

        foreach (Match m in matches)
        {
            try
            {
                string[] s = m.Groups["size"].Value.Split(',');
                string[] t = m.Groups["trim"].Value.Split(',');
                string[] q = m.Groups["quad"].Value.Split(',');

                SpriteData data = new SpriteData
                {
                    name = m.Groups["name"].Value,
                    atlasName = m.Groups["atlas"].Value,
                    fullSize = new Vector2(float.Parse(s[0]), float.Parse(s[1])),
                    trim = new Vector4(float.Parse(t[0]), float.Parse(t[1]), float.Parse(t[2]), float.Parse(t[3])),
                    f_quad = new Rect(float.Parse(q[0]), float.Parse(q[1]), float.Parse(q[2]), float.Parse(q[3]))
                };

                if (!spriteDatabase.ContainsKey(data.name))
                    spriteDatabase.Add(data.name, data);

                string aliasRaw = m.Groups["alias"].Value;
                if (!string.IsNullOrEmpty(aliasRaw))
                {
                    MatchCollection aliasMatches = Regex.Matches(aliasRaw, @"""([^""]+)""");
                    foreach (Match am in aliasMatches)
                    {
                        string aliasName = am.Groups[1].Value;
                        if (!spriteDatabase.ContainsKey(aliasName))
                        {
                            spriteDatabase.Add(aliasName, data);
                        }
                    }
                }
            }
            catch { continue; }
        }
        Debug.Log($"[Atlas] Đã nạp thành công {spriteDatabase.Count} khung hình (bao gồm cả các Alias số lẻ)!");
    }

    /// <summary>
    /// Chạy animation của quái dựa theo tiền tố tên và khoảng frame chỉ định
    /// </summary>
    public void PlayMonsterAnimation(GameObject target, string animPrefix, int startFrame, int endFrame, float frameRate = -1)
    {
        if (target == null) return;
        SpriteRenderer renderer = target.GetComponent<SpriteRenderer>();
        if (renderer == null) return;

        List<string> frames = new List<string>();

        // CẢI TIẾN BẢO VỆ: Chỉ nạp các từ khóa nằm trong giới hạn nghiêm ngặt từ start đến end
        for (int i = startFrame; i <= endFrame; i++)
        {
            string frameName = animPrefix + i.ToString("D4");
            if (spriteDatabase.ContainsKey(frameName))
            {
                frames.Add(frameName);
            }
        }

        if (frames.Count > 0)
        {
            // SỬA LỖI CHÍ MẠNG: Dừng chính xác Coroutine cũ của riêng biệt đối tượng này thay vì dừng toàn bộ hệ thống
            if (activeCoroutines.TryGetValue(target, out Coroutine oldCoroutine))
            {
                if (oldCoroutine != null)
                {
                    StopCoroutine(oldCoroutine);
                }
                activeCoroutines.Remove(target);
            }

            // Kích hoạt vòng lặp hoạt ảnh mới và lưu tham chiếu vào Dictionary
            Coroutine newCoroutine = StartCoroutine(AnimateRoutine(renderer, frames, frameRate > 0 ? frameRate : defaultFrameRate));
            activeCoroutines.Add(target, newCoroutine);
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy frames quái vật hợp lệ cho: {animPrefix} từ {startFrame} đến {endFrame}");
        }
    }

    // Hàm dọn dẹp bộ nhớ khi quái vật bị tiêu diệt
    public void ClearTargetRegister(GameObject target)
    {
        if (activeCoroutines.TryGetValue(target, out Coroutine c))
        {
            if (c != null) StopCoroutine(c);
            activeCoroutines.Remove(target);
        }
    }

    public string CurrentPlayingFrameName { get; private set; } = "None";

    private IEnumerator AnimateRoutine(SpriteRenderer renderer, List<string> frames, float frameRate)
    {
        int currentIndex = 0;
        while (true)
        {
            if (renderer == null) yield break;

            string currentFrameKey = frames[currentIndex];

            // Cập nhật tên khung hình thực tế để hiển thị lên giao diện kiểm tra
            CurrentPlayingFrameName = currentFrameKey;

            SpriteData d = spriteDatabase[currentFrameKey];

            if (textures.TryGetValue(d.atlasName, out Texture2D tex))
            {
                Rect pixelRect = new Rect(d.f_quad.x, tex.height - d.f_quad.y - d.f_quad.height, d.f_quad.width, d.f_quad.height);
                float relativeAnchorX = (d.fullSize.x * 0.5f) - d.trim.x;
                float pivotX = relativeAnchorX / d.f_quad.width;
                float relativeAnchorY = d.fullSize.y - d.trim.y - d.f_quad.height;
                float pivotY = -relativeAnchorY / d.f_quad.height;
                Vector2 customPivot = new Vector2(Mathf.Clamp01(pivotX), Mathf.Clamp01(pivotY));

                renderer.sprite = Sprite.Create(tex, pixelRect, customPivot, pixelsPerUnit);
            }

            // Chia lấy dư bảo đảm vòng lặp luôn nằm trong phạm vi mảng được chỉ định
            currentIndex = (currentIndex + 1) % frames.Count;
            yield return new WaitForSeconds(frameRate);
        }
    }
}