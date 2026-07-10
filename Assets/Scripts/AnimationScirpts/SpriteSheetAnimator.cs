using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class SpriteSheetAnimator : MonoBehaviour
{
    [Header("Data Source")]
    public TextAsset dataFile;

    [Header("Textures")]
    public Texture2D atlas1;
    public Texture2D atlas2;
    public Texture2D atlas3;

    [Header("Settings")]
    public float defaultFrameRate = 0.1f;
    [Tooltip("Số Pixel mỗi đơn vị trong Unity (thường là 100)")]
    public float pixelsPerUnit = 100f;

    private Dictionary<string, SpriteData> spriteDatabase = new Dictionary<string, SpriteData>();
    private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

    // ✅ Track coroutine riêng cho từng GameObject, dùng InstanceID làm key
    private Dictionary<int, Coroutine> activeCoroutines = new Dictionary<int, Coroutine>();

    public static SpriteSheetAnimator Instance { get; private set; }

    private class SpriteData
    {
        public string name;
        public Rect f_quad;
        public Vector2 fullSize;
        public Vector4 trim;
        public string atlasName;
    }

    void Awake()
    {
        if (atlas1) textures["go_towers-1.png"] = atlas1;
        if (atlas2) textures["go_towers-2.png"] = atlas2;
        if (atlas3) textures["go_towers-3.png"] = atlas3;

        ParseData();
        Instance = this;
    }

    private void ParseData()
    {
        if (dataFile == null) return;

        string content = dataFile.text;
        // Regex cải tiến để bắt được trọn vẹn cả đoạn alias={...} ở phía sau
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

                // 1. Thêm frame gốc vào Database
                if (!spriteDatabase.ContainsKey(data.name))
                    spriteDatabase.Add(data.name, data);

                // 2. Xử lý Alias (Các frame dùng chung hình ảnh)
                string aliasRaw = m.Groups["alias"].Value;
                if (!string.IsNullOrEmpty(aliasRaw))
                {
                    // Tìm tất cả các chuỗi nằm trong dấu ngoặc kép "" bên trong alias={...}
                    MatchCollection aliasMatches = Regex.Matches(aliasRaw, @"""([^""]+)""");
                    foreach (Match am in aliasMatches)
                    {
                        string aliasName = am.Groups[1].Value;

                        // Tạo một bản sao dữ liệu cho frame alias này
                        SpriteData aliasData = new SpriteData
                        {
                            name = aliasName,
                            atlasName = data.atlasName,
                            fullSize = data.fullSize,
                            trim = data.trim,
                            f_quad = data.f_quad
                        };

                        if (!spriteDatabase.ContainsKey(aliasName))
                        {
                            spriteDatabase.Add(aliasName, aliasData);
                        }
                    }
                }
            }
            catch { continue; }
        }
    }

    // ✅ Stop đúng coroutine của target, không ảnh hưởng GameObject khác
    private void StopAnimationFor(GameObject target)
    {
        int id = target.GetInstanceID();
        if (activeCoroutines.TryGetValue(id, out Coroutine old))
        {
            if (old != null) StopCoroutine(old);
            activeCoroutines.Remove(id);
        }
    }

    // ✅ Start coroutine và lưu vào dictionary theo InstanceID
    private void StartAnimationFor(GameObject target, SpriteRenderer renderer, List<string> frames, float frameRate, Action onComplete)
    {
        int id = target.GetInstanceID();
        Coroutine c = StartCoroutine(AnimateRoutine(renderer, frames, frameRate, onComplete));
        activeCoroutines[id] = c;
    }

    // Gốc 1: Chạy theo tiền tố (Prefix)
    public void PlayAnimation(GameObject target, string animPrefix, float frameRate = -1, Action onComplete = null)
    {
        SpriteRenderer renderer = target.GetComponent<SpriteRenderer>();
        if (renderer == null) return;

        List<string> frames = new List<string>();
        foreach (var key in spriteDatabase.Keys)
        {
            if (key.StartsWith(animPrefix)) frames.Add(key);
        }
        frames.Sort();

        if (frames.Count == 1)
        {
            StopAnimationFor(target); // ✅ Chỉ stop target này
            // Gán sprite duy nhất từ Database của bạn
            SpriteData d = spriteDatabase[frames[0]];

            if (textures.TryGetValue(d.atlasName, out Texture2D tex))
            {
                Rect pixelRect = new Rect(
                    d.f_quad.x,
                    tex.height - d.f_quad.y - d.f_quad.height,
                    d.f_quad.width,
                    d.f_quad.height
                );

                float pivotX = (d.fullSize.x * 0.5f - d.trim.x) / d.f_quad.width;
                float pivotY = (d.trim.w) / d.f_quad.height;

                renderer.sprite = Sprite.Create(tex, pixelRect, new Vector2(pivotX, pivotY), pixelsPerUnit);
            }

            // Kích hoạt hành động hoàn thành luôn (nếu có)
            onComplete?.Invoke();
            return; // Thoát hàm hoàn toàn
        }

        if (frames.Count > 0)
        {
            StopAnimationFor(target); // ✅ Chỉ stop target này
            StartAnimationFor(target, renderer, frames, frameRate > 0 ? frameRate : defaultFrameRate, onComplete);
        }
        else
        {
            Debug.LogWarning($"[SpriteSheetAnimator] Không tìm thấy frames với prefix: '{animPrefix}'");
        }
    }

    // Gốc 2: Chạy theo khoảng chỉ số cụ thể (StartFrame -> EndFrame)
    public void PlayAnimation(GameObject target, string animPrefix, int startFrame, int endFrame, float frameRate = -1, Action onComplete = null)
    {
        SpriteRenderer renderer = target.GetComponent<SpriteRenderer>();
        if (renderer == null) return;

        List<string> frames = new List<string>();
        for (int i = startFrame; i <= endFrame; i++)
        {
            string frameName = animPrefix + i.ToString("D4");
            if (spriteDatabase.ContainsKey(frameName))
                frames.Add(frameName);
        }
        if (frames.Count == 1)
        {
            StopAnimationFor(target); // ✅ Chỉ stop target này
            // Gán sprite duy nhất từ Database của bạn
            SpriteData d = spriteDatabase[frames[0]];

            if (textures.TryGetValue(d.atlasName, out Texture2D tex))
            {
                Rect pixelRect = new Rect(
                    d.f_quad.x,
                    tex.height - d.f_quad.y - d.f_quad.height,
                    d.f_quad.width,
                    d.f_quad.height
                );

                float pivotX = (d.fullSize.x * 0.5f - d.trim.x) / d.f_quad.width;
                float pivotY = (d.trim.w) / d.f_quad.height;

                renderer.sprite = Sprite.Create(tex, pixelRect, new Vector2(pivotX, pivotY), pixelsPerUnit);
            }

            // Kích hoạt hành động hoàn thành luôn (nếu có)
            onComplete?.Invoke();
            return; // Thoát hàm hoàn toàn
        }
        if (frames.Count > 0)
        {
            StopAnimationFor(target); // ✅ Chỉ stop target này
            StartAnimationFor(target, renderer, frames, frameRate > 0 ? frameRate : defaultFrameRate, onComplete);
        }
        else
        {
            Debug.LogWarning($"[SpriteSheetAnimator] Không tìm thấy frames từ {startFrame} đến {endFrame} với prefix '{animPrefix}'");
        }
    }

    // Gốc 3: Chạy theo khoảng chỉ số cụ thể VÀ kích hoạt Action tại một Frame bất kỳ ở giữa
    public void PlayAnimation(GameObject target, string animPrefix, int startFrame, int endFrame, int eventFrame, Action onEventTrigger, float frameRate = -1, Action onComplete = null)
    {
        SpriteRenderer renderer = target.GetComponent<SpriteRenderer>();
        if (renderer == null) return;

        List<string> frames = new List<string>();
        int eventFrameIndex = -1; // Vị trí của frame sự kiện trong danh sách List

        for (int i = startFrame; i <= endFrame; i++)
        {
            string frameName = animPrefix + i.ToString("D4");
            if (spriteDatabase.ContainsKey(frameName))
            {
                frames.Add(frameName);

                // Nếu chạy đến đúng frame số mong muốn, lưu lại vị trí (Index) của nó trong mảng
                if (i == eventFrame)
                {
                    eventFrameIndex = frames.Count - 1;
                }
            }
        }
        if (frames.Count == 1)
        {
            StopAnimationFor(target); // ✅ Chỉ stop target này
            // Gán sprite duy nhất từ Database của bạn
            SpriteData d = spriteDatabase[frames[0]];

            if (textures.TryGetValue(d.atlasName, out Texture2D tex))
            {
                Rect pixelRect = new Rect(
                    d.f_quad.x,
                    tex.height - d.f_quad.y - d.f_quad.height,
                    d.f_quad.width,
                    d.f_quad.height
                );

                float pivotX = (d.fullSize.x * 0.5f - d.trim.x) / d.f_quad.width;
                float pivotY = (d.trim.w) / d.f_quad.height;

                renderer.sprite = Sprite.Create(tex, pixelRect, new Vector2(pivotX, pivotY), pixelsPerUnit);
            }

            // Kích hoạt hành động hoàn thành luôn (nếu có)
            onComplete?.Invoke();
            return; // Thoát hàm hoàn toàn
        }
        if (frames.Count > 0)
        {
            StopAnimationFor(target); // ✅ Chỉ stop target này

            // Khởi chạy Coroutine đặc biệt có hỗ trợ bắt sự kiện Event
            int id = target.GetInstanceID();
            Coroutine c = StartCoroutine(AnimateWithEventRoutine(renderer, frames, eventFrameIndex, onEventTrigger, frameRate > 0 ? frameRate : defaultFrameRate, onComplete));
            activeCoroutines[id] = c;
        }
        else
        {
            Debug.LogWarning($"[SpriteSheetAnimator] Không tìm thấy frames từ {startFrame} đến {endFrame} với prefix '{animPrefix}'");
        }
    }
    IEnumerator AnimateWithEventRoutine(SpriteRenderer renderer, List<string> frames, int eventFrameIndex, Action onEventTrigger, float frameRate, Action onComplete)
    {
        int currentIndex = 0;
        bool shouldLoop = (onComplete == null);
        bool eventTriggered = false; // Đảm bảo event chỉ chạy 1 lần duy nhất nếu hoạt ảnh bị lặp (Loop)

        while (true)
        {
            SpriteData d = spriteDatabase[frames[currentIndex]];

            if (textures.TryGetValue(d.atlasName, out Texture2D tex))
            {
                Rect pixelRect = new Rect(
                    d.f_quad.x,
                    tex.height - d.f_quad.y - d.f_quad.height,
                    d.f_quad.width,
                    d.f_quad.height
                );

                float pivotX = (d.fullSize.x * 0.5f - d.trim.x) / d.f_quad.width;
                float pivotY = (d.trim.w) / d.f_quad.height;

                renderer.sprite = Sprite.Create(tex, pixelRect, new Vector2(pivotX, pivotY), pixelsPerUnit);
            }

            // 🔥 CHÍNH XÁC LÀ Ở ĐÂY: Nếu trùng khớp với index của frame đăng ký sự kiện
            if (currentIndex == eventFrameIndex && !eventTriggered)
            {
                // Thực hiện ngay hành động được truyền vào (ví dụ: Gọi hàm SpawnAttack)
                onEventTrigger?.Invoke();
                eventTriggered = true; // Đánh dấu đã kích hoạt xong
            }

            if (currentIndex == frames.Count - 1)
            {
                if (!shouldLoop)
                {
                    yield return new WaitForSeconds(frameRate);
                    onComplete?.Invoke();
                    yield break;
                }
                else
                {
                    // Nếu là trạng thái Loop, reset lại trạng thái kích hoạt event cho vòng lặp sau (nếu cần)
                    eventTriggered = false;
                }
            }

            currentIndex = (currentIndex + 1) % frames.Count;
            yield return new WaitForSeconds(frameRate);
        }
    }

    IEnumerator AnimateRoutine(SpriteRenderer renderer, List<string> frames, float frameRate, Action onComplete)
    {
        int currentIndex = 0;
        bool shouldLoop = (onComplete == null);

        while (true)
        {
            SpriteData d = spriteDatabase[frames[currentIndex]];

            if (textures.TryGetValue(d.atlasName, out Texture2D tex))
            {
                Rect pixelRect = new Rect(
                    d.f_quad.x,
                    tex.height - d.f_quad.y - d.f_quad.height,
                    d.f_quad.width,
                    d.f_quad.height
                );

                float pivotX = (d.fullSize.x * 0.5f - d.trim.x) / d.f_quad.width;
                float pivotY = (d.trim.w) / d.f_quad.height;

                renderer.sprite = Sprite.Create(tex, pixelRect, new Vector2(pivotX, pivotY), pixelsPerUnit);
            }

            if (currentIndex == frames.Count - 1)
            {
                if (!shouldLoop)
                {
                    yield return new WaitForSeconds(frameRate);
                    onComplete?.Invoke();
                    yield break;
                }
            }

            currentIndex = (currentIndex + 1) % frames.Count;
            yield return new WaitForSeconds(frameRate);
        }
    }
    // Thêm hàm này vào trong class SpriteSheetAnimator
    public void DisplaySingleFrame(GameObject target, string animPrefix, int frameNumber, float pivotYOffset = 0f)
    {
        SpriteRenderer renderer = target.GetComponent<SpriteRenderer>();
        if (renderer == null) return;

        // Định dạng số frame thành 4 chữ số (ví dụ: 1 -> "0001") tương tự như logic PlayAnimation cũ
        string frameName = animPrefix + frameNumber.ToString("D4");

        if (spriteDatabase.TryGetValue(frameName, out SpriteData d))
        {
            // Dừng các Coroutine hoạt họa đang chạy trên Object này để tránh bị đè đè hình
            StopAnimationFor(target);

            if (textures.TryGetValue(d.atlasName, out Texture2D tex))
            {
                Rect pixelRect = new Rect(
                    d.f_quad.x,
                    tex.height - d.f_quad.y - d.f_quad.height,
                    d.f_quad.width,
                    d.f_quad.height
                );

                // Tính toán Pivot tự động từ dữ liệu Trim
                float pivotX = (d.fullSize.x * 0.5f - d.trim.x) / d.f_quad.width;

                // Tính Pivot Y gốc và cộng thêm offset điều chỉnh thủ công từ công cụ Viewer
                float basePivotY = d.trim.w / d.f_quad.height;
                float finalPivotY = basePivotY + pivotYOffset; // Cộng thẳng tỉ lệ offset vào đây

                // Tạo và gán Sprite mới
                renderer.sprite = Sprite.Create(tex, pixelRect, new Vector2(pivotX, finalPivotY), pixelsPerUnit);
            }
        }
        else
        {
            Debug.LogWarning($"[SpriteSheetAnimator] Không tìm thấy frame: '{frameName}' trong Database.");
        }
    }
    private void ApplySpriteWithOffset(SpriteRenderer renderer, SpriteData d, float offsetY)
    {
        if (textures.TryGetValue(d.atlasName, out Texture2D tex))
        {
            Rect pixelRect = new Rect(
                d.f_quad.x,
                tex.height - d.f_quad.y - d.f_quad.height,
                d.f_quad.width,
                d.f_quad.height
            );

            float pivotX = (d.fullSize.x * 0.5f - d.trim.x) / d.f_quad.width;

            // Tính toán chính xác theo công thức bạn yêu cầu
            float basePivotY = d.trim.w / d.f_quad.height;
            float finalPivotY = basePivotY + offsetY;

            renderer.sprite = Sprite.Create(tex, pixelRect, new Vector2(pivotX, finalPivotY), pixelsPerUnit);
        }
    }
    public void PlayAnimation(
    GameObject target,
    string animPrefix,
    int startFrame,
    int endFrame,
    int eventFrame,
    Action onEventTrigger,
    List<EnemyAnimConfig> offsetConfigs, // Thêm tham số này
    float frameRate = -1,
    Action onComplete = null)
    {
        SpriteRenderer renderer = target.GetComponent<SpriteRenderer>();
        if (renderer == null) return;

        List<string> frames = new List<string>();
        // List này lưu trữ offsetY tương ứng với từng frame trong danh sách 'frames' phía trên
        List<float> frameOffsets = new List<float>();
        int eventFrameIndex = -1;

        for (int i = startFrame; i <= endFrame; i++)
        {
            string frameName = animPrefix + i.ToString("D4");
            if (spriteDatabase.ContainsKey(frameName))
            {
                frames.Add(frameName);

                // Tìm xem frame hiện tại (i) có cấu hình offset đặc biệt nào không
                float currentOffsetY = 0f;
                if (offsetConfigs != null)
                {
                    // Tìm config có frameOffset trùng với chỉ số frame hiện tại (i)
                    var matchedConfig = offsetConfigs.Find(c => c.frameOffset == i);
                    if (matchedConfig != null)
                    {
                        currentOffsetY = matchedConfig.offsetY;
                    }
                }
                frameOffsets.Add(currentOffsetY);

                if (i == eventFrame)
                {
                    eventFrameIndex = frames.Count - 1;
                }
            }
        }

        if (frames.Count == 1)
        {
            StopAnimationFor(target);
            SpriteData d = spriteDatabase[frames[0]];
            ApplySpriteWithOffset(renderer, d, frameOffsets[0]);
            onComplete?.Invoke();
            return;
        }

        if (frames.Count > 0)
        {
            StopAnimationFor(target);
            int id = target.GetInstanceID();
            // Chạy coroutine mới hỗ trợ cả Event và Offset list
            Coroutine c = StartCoroutine(AnimateWithEventAndOffsetRoutine(
                renderer, frames, frameOffsets, eventFrameIndex, onEventTrigger, frameRate > 0 ? frameRate : defaultFrameRate, onComplete
            ));
            activeCoroutines[id] = c;
        }
        else
        {
            Debug.LogWarning($"[SpriteSheetAnimator] Không tìm thấy frames từ {startFrame} đến {endFrame} với prefix '{animPrefix}'");
        }
    }
    IEnumerator AnimateWithEventAndOffsetRoutine(
    SpriteRenderer renderer,
    List<string> frames,
    List<float> frameOffsets,
    int eventFrameIndex,
    Action onEventTrigger,
    float frameRate,
    Action onComplete)
    {
        int currentIndex = 0;
        bool shouldLoop = (onComplete == null);
        bool eventTriggered = false;

        while (true)
        {
            SpriteData d = spriteDatabase[frames[currentIndex]];
            // Lấy offset Y tương ứng với vị trí frame hiện tại
            float currentOffsetY = frameOffsets[currentIndex];

            // Áp dụng gán sprite kèm offset
            ApplySpriteWithOffset(renderer, d, currentOffsetY);

            // Xử lý Event Trigger
            if (currentIndex == eventFrameIndex && !eventTriggered)
            {
                onEventTrigger?.Invoke();
                eventTriggered = true;
            }

            if (currentIndex == frames.Count - 1)
            {
                if (!shouldLoop)
                {
                    yield return new WaitForSeconds(frameRate);
                    onComplete?.Invoke();
                    yield break;
                }
                else
                {
                    eventTriggered = false;
                }
            }

            currentIndex = (currentIndex + 1) % frames.Count;
            yield return new WaitForSeconds(frameRate);
        }
    }
}