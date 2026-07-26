using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class CharacterSpriteAnimator : MonoBehaviour
{
    [System.Serializable]
    public class AtlasMapping
    {
        public string atlasNameInTxt;
        public Texture2D texture;
    }

    [System.Serializable]
    public class CharacterAnimationConfig
    {
        public string characterId;
        public TextAsset dataFile;
        public List<AtlasMapping> atlases;
    }

    [Header("Character Databases")]
    public List<CharacterAnimationConfig> characterConfigs;

    [Header("Global Settings")]
    public float defaultFrameRate = 0.1f;
    public float pixelsPerUnit = 100f;

    private class SpriteData
    {
        public string name;
        public Rect f_quad;
        public Vector2 fullSize;
        public Vector4 trim;
        public string atlasName;
    }

    // Đổi Key thành chữ thường (lowercase) để triệt tiêu lỗi Hoa/Thường
    private Dictionary<string, Dictionary<string, SpriteData>> enemyDatabases = new Dictionary<string, Dictionary<string, SpriteData>>();
    private Dictionary<string, Dictionary<string, Texture2D>> enemyTextures = new Dictionary<string, Dictionary<string, Texture2D>>();
    private Dictionary<int, Coroutine> activeCoroutines = new Dictionary<int, Coroutine>();

    public static CharacterSpriteAnimator Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializeDatabases();
    }

    private void InitializeDatabases()
    {
        if (characterConfigs == null) return;

        foreach (var config in characterConfigs)
        {
            if (string.IsNullOrEmpty(config.characterId)) continue;

            // Chuẩn hóa ID quái: Xóa khoảng trắng và đưa về chữ thường
            string cleanEnemyId = config.characterId.Trim().ToLower();

            // 1. Xử lý Texture
            var texDict = new Dictionary<string, Texture2D>();
            if (config.atlases != null)
            {
                foreach (var mapping in config.atlases)
                {
                    if (!string.IsNullOrEmpty(mapping.atlasNameInTxt) && mapping.texture != null)
                    {
                        string cleanAtlasName = mapping.atlasNameInTxt.Trim().ToLower();
                        texDict[cleanAtlasName] = mapping.texture;
                    }
                }
            }
            enemyTextures[cleanEnemyId] = texDict;

            // 2. Xử lý Parse Data
            var spriteDict = new Dictionary<string, SpriteData>();
            ParseData(config.dataFile, spriteDict);
            enemyDatabases[cleanEnemyId] = spriteDict;

            Debug.Log($"[EnemySpriteAnimator] Khởi tạo thành công Enemy ID: '{cleanEnemyId}' với {spriteDict.Count} frames.");
        }
    }

    private void ParseData(TextAsset dataFile, Dictionary<string, SpriteData> targetDatabase)
    {
        if (dataFile == null) return;

        string content = dataFile.text;

        // Regex quét toàn bộ dòng bao gồm cả alias bằng cụm `alias=\{(?<alias>.*?)\}`
        string pattern = @"\[""(?<name>.+?)""\].+?size=\{(?<size>.+?)\}.+?trim=\{(?<trim>.+?)\}.+?a_name=""(?<atlas>.+?)"".+?f_quad=\{(?<quad>.+?)\}.+?alias=\{(?<alias>.*?)\}";
        MatchCollection matches = Regex.Matches(content, pattern);

        foreach (Match m in matches)
        {
            try
            {
                string mainName = m.Groups["name"].Value.Trim().ToLower();
                string atlasName = m.Groups["atlas"].Value.Trim().ToLower();

                string[] s = m.Groups["size"].Value.Split(',');
                string[] t = m.Groups["trim"].Value.Split(',');
                string[] q = m.Groups["quad"].Value.Split(',');

                SpriteData data = new SpriteData
                {
                    name = mainName,
                    atlasName = atlasName,
                    fullSize = new Vector2(float.Parse(s[0]), float.Parse(s[1])),
                    trim = new Vector4(float.Parse(t[0]), float.Parse(t[1]), float.Parse(t[2]), float.Parse(t[3])),
                    f_quad = new Rect(float.Parse(q[0]), float.Parse(q[1]), float.Parse(q[2]), float.Parse(q[3]))
                };

                // Add frame gốc vào DB
                if (!targetDatabase.ContainsKey(mainName))
                    targetDatabase.Add(mainName, data);

                // 🔥 XỬ LÝ ALIAS: Nếu frame này đại diện cho các frame alias khác, clone dữ liệu sang luôn
                string aliasRaw = m.Groups["alias"].Value;
                if (!string.IsNullOrEmpty(aliasRaw))
                {
                    MatchCollection aliasMatches = Regex.Matches(aliasRaw, @"""(?<aliasName>.+?)""");
                    foreach (Match am in aliasMatches)
                    {
                        string aliasName = am.Groups["aliasName"].Value.Trim().ToLower();
                        if (!targetDatabase.ContainsKey(aliasName))
                        {
                            targetDatabase.Add(aliasName, data); // Trỏ chung data vùng cắt ảnh
                        }
                    }
                }
            }
            catch { continue; }
        }
    }

    private void StopAnimationFor(GameObject target)
    {
        int id = target.GetInstanceID();
        if (activeCoroutines.TryGetValue(id, out Coroutine old))
        {
            if (old != null) StopCoroutine(old);
            activeCoroutines.Remove(id);
        }
    }

    private bool ApplySpriteFrame(SpriteRenderer renderer, string enemyId, SpriteData d)
    {
        if (!enemyTextures.TryGetValue(enemyId, out var texDict)) return false;
        if (!texDict.TryGetValue(d.atlasName, out Texture2D tex))
        {
            Debug.LogError($"⚠️ Không tìm thấy atlas '{d.atlasName}' cho enemy '{enemyId}'!");
            return false;
        }

        // ── 1. Rect miếng ảnh trong atlas (flip Y vì Unity tọa độ từ dưới lên) ──
        Rect pixelRect = new Rect(
            d.f_quad.x,
            tex.height - d.f_quad.y - d.f_quad.height,
            d.f_quad.width,
            d.f_quad.height
        );

        // ── 2. Kích thước thực của miếng ảnh đã cắt ──
        //    trim = {left, top, right, bottom} → lượng pixel bị cắt mỗi cạnh
        float trimLeft = d.trim.x;
        float trimTop = d.trim.y;
        float trimRight = d.trim.z;
        float trimBottom = d.trim.w;

        float croppedW = d.fullSize.x - trimLeft - trimRight;   // == f_quad.width
        float croppedH = d.fullSize.y - trimTop - trimBottom;  // == f_quad.height

        // ── 3. Chọn anchor cố định trên khung GỐC 72×72 ──
        //    Ví dụ: bottom-center của khung gốc = (36, 0)
        //    Bạn đổi anchorFullX/Y tuỳ ý (pixel, hệ top-left của khung gốc)
        float anchorFullX = d.fullSize.x * 0.5f;   // 36px - chính giữa theo X
        float anchorFullY = d.fullSize.y;           // 72px - đáy khung gốc (top-left Y đi xuống)

        // ── 4. Map anchor từ khung gốc → tọa độ bên trong miếng ảnh đã cắt ──
        //    Trục X: anchorFullX - trimLeft
        //    Trục Y: anchorFullY - trimTop  (top-left system, Y đi xuống)
        float localX = anchorFullX - trimLeft;
        float localY = anchorFullY - trimTop;

        // ── 5. Quy đổi ra [0,1] theo kích thước miếng cắt ──
        //    Unity Sprite pivot: X trái→phải, Y DƯỚI→TRÊN (flip Y!)
        float pivotX = Mathf.Clamp01(localX / croppedW);
        float pivotY = Mathf.Clamp01(1f - (localY / croppedH));  // ← flip Y quan trọng!

        renderer.sprite = Sprite.Create(
            tex, pixelRect,
            new Vector2(pivotX, pivotY),
            pixelsPerUnit
        );
        return true;
    }

    // --- INTERFACE CHẠY THEO PREFIX (Đã bọc LOG bắt bệnh cực kỹ) ---
    public void PlayAnimation(GameObject target, string enemyId, string animPrefix, float frameRate = -1, Action onComplete = null)
    {
        if (target == null) return;
        SpriteRenderer renderer = target.GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            Debug.LogError($"⚠️ [LỖI TARGET] GameObject {target.name} không có thành phần SpriteRenderer!");
            return;
        }

        // Làm sạch string đầu vào
        string cleanEnemyId = enemyId.Trim().ToLower();
        string cleanPrefix = animPrefix.Trim().ToLower();

        if (!enemyDatabases.TryGetValue(cleanEnemyId, out var database))
        {
            Debug.LogError($"⚠️ [LỖI ID] Không tìm thấy Enemy ID '{cleanEnemyId}' trong Database! Hãy kiểm tra lại ô điền ở Inspector.");
            return;
        }

        List<string> frames = new List<string>();
        foreach (var key in database.Keys)
        {
            if (key.StartsWith(cleanPrefix)) frames.Add(key);
        }
        frames.Sort();

        if (frames.Count == 0)
        {
            Debug.LogError($"⚠️ [LỖI ANIMATION] Enemy '{cleanEnemyId}' KHÔNG có frame nào bắt đầu bằng tiền tố: '{cleanPrefix}'! Hãy xem lại file .txt");
            return;
        }

        // In log check số lượng frame quét được thực tế
        Debug.Log($"🎬 [PLAY] Phát hoạt ảnh '{cleanPrefix}' cho {target.name} ({cleanEnemyId}). Tìm thấy {frames.Count} frames hợp lệ.");

        if (frames.Count == 1)
        {
            StopAnimationFor(target);
            ApplySpriteFrame(renderer, cleanEnemyId, database[frames[0]]);
            onComplete?.Invoke();
            return;
        }

        StopAnimationFor(target);
        int id = target.GetInstanceID();
        float finalFrameRate = frameRate > 0 ? frameRate : defaultFrameRate;
        Coroutine c = StartCoroutine(AnimateRoutine(renderer, cleanEnemyId, database, frames, finalFrameRate, onComplete));
        activeCoroutines[id] = c;
    }
    // Thêm hàm interface này vào file EnemySpriteAnimator.cs để truyền số trực tiếp
    // --- INTERFACE CHẠY THEO RANGE DATA (HỖ TRỢ OFFSET TỪNG FRAME) ---
    public void PlayAnimationByRange(GameObject target, string enemyId, string animPrefix, 
        AnimationFrameRange rangeConfig, float frameRate = -1, Action onComplete = null)
    {
        if (target == null || rangeConfig == null) return;
        SpriteRenderer renderer = target.GetComponent<SpriteRenderer>();
        if (renderer == null) return;

        string cleanEnemyId = enemyId.Trim().ToLower();
        string cleanPrefix = animPrefix.Trim().ToLower();

        if (!enemyDatabases.TryGetValue(cleanEnemyId, out var database)) return;

        List<string> validFrames = new List<string>();
        List<int> frameNumbers = new List<int>(); // Lưu lại số frame gốc để map với config offset

        for (int i = rangeConfig.startFrame; i <= rangeConfig.endFrame; i++)
        {
            string frameKey = $"{cleanPrefix}{i:D4}";
            if (database.ContainsKey(frameKey))
            {
                validFrames.Add(frameKey);
                frameNumbers.Add(i);
            }
        }

        if (validFrames.Count == 0) return;

        StopAnimationFor(target);
        int id = target.GetInstanceID();
        float finalFrameRate = frameRate > 0 ? frameRate : defaultFrameRate;

        // Truyền cả list cấu hình offset vào Routine để xử lý realtime
        Coroutine c = StartCoroutine(AnimateRoutineWithOffset(renderer, cleanEnemyId, database, validFrames, frameNumbers, rangeConfig.animationConfigOffset, finalFrameRate, onComplete));
        activeCoroutines[id] = c;
    }

    //IEnumerator AnimateRoutineWithOffset(SpriteRenderer renderer, string enemyId, Dictionary<string, SpriteData> database, List<string> frames, List<int> frameNumbers, List<EnemyAnimConfig> offsetConfigs, float frameRate, Action onComplete)
    //{
    //    int currentIndex = 0;
    //    bool shouldLoop = (onComplete == null);

    //    while (true)
    //    {
    //        string currentFrameKey = frames[currentIndex];
    //        int currentActualFrameNum = frameNumbers[currentIndex];
    //        SpriteData d = database[currentFrameKey];

    //        // Tìm kiếm xem frame hiện tại có cấu hình offset riêng không
    //        float calculatedOffsetY = 0f;
    //        if (offsetConfigs != null && offsetConfigs.Count > 0)
    //        {
    //            // Tìm kiếm cấu hình có frameOffset trùng với số frame hiện tại
    //            EnemyAnimConfig configForFrame = offsetConfigs.Find(c => c.frameOffset == currentActualFrameNum);
    //            if (configForFrame != null)
    //            {
    //                calculatedOffsetY = configForFrame.offsetY;
    //            }
    //        }

    //        // Gọi hàm ApplySpriteFrame cải tiến có tham số pivotYOffset
    //        ApplySpriteFrame(renderer, enemyId, d, calculatedOffsetY);

    //        if (currentIndex == frames.Count - 1)
    //        {
    //            if (!shouldLoop)
    //            {
    //                yield return new WaitForSeconds(frameRate);
    //                onComplete?.Invoke();
    //                yield break;
    //            }
    //        }

    //        currentIndex = (currentIndex + 1) % frames.Count;
    //        yield return new WaitForSeconds(frameRate);
    //    }
    //}
    IEnumerator AnimateRoutineWithOffset(SpriteRenderer renderer, string enemyId, Dictionary<string, SpriteData> database, List<string> frames, List<int> frameNumbers, List<EnemyAnimConfig> offsetConfigs, float frameRate, Action onComplete)
    {
        int currentIndex = 0;
        bool shouldLoop = (onComplete == null);

        // Lấy reference đến EnemyController để kiểm tra trạng thái đóng băng
        EnemyController enemyCtrl = renderer.GetComponent<EnemyController>();

        while (true)
        {
            // 🔥 NẾU ENEMY BỊ ĐÓNG BĂNG: Tạm dừng xử lý tại đây, không đổi frame mới
            if (enemyCtrl != null && enemyCtrl.isFrozen)
            {
                yield return null; // Chờ sang frame tiếp theo rồi kiểm tra lại
                continue;
            }

            string currentFrameKey = frames[currentIndex];
            int currentActualFrameNum = frameNumbers[currentIndex];
            SpriteData d = database[currentFrameKey];

            // Ghi nhớ frame hiện tại vào EnemyController trước khi đóng băng (để gán nếu cần)
            if (enemyCtrl != null)
            {
                enemyCtrl.lastAnimPrefixBeforeFreeze = currentFrameKey;
                enemyCtrl.lastFrameNumberBeforeFreeze = currentActualFrameNum;
            }

            // Tìm kiếm xem frame hiện tại có cấu hình offset riêng không
            float calculatedOffsetY = 0f;
            if (offsetConfigs != null && offsetConfigs.Count > 0)
            {
                EnemyAnimConfig configForFrame = offsetConfigs.Find(c => c.frameOffset == currentActualFrameNum);
                if (configForFrame != null)
                {
                    calculatedOffsetY = configForFrame.offsetY;
                }
            }

            // Gọi hàm ApplySpriteFrame cải tiến có tham số pivotYOffset
            ApplySpriteFrame(renderer, enemyId, d, calculatedOffsetY);

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
    //IEnumerator AnimateRoutine(SpriteRenderer renderer, string enemyId, Dictionary<string, SpriteData> database, List<string> frames, float frameRate, Action onComplete)
    //{
    //    int currentIndex = 0;
    //    bool shouldLoop = (onComplete == null);

    //    while (true)
    //    {
    //        SpriteData d = database[frames[currentIndex]];
    //        ApplySpriteFrame(renderer, enemyId, d);

    //        if (currentIndex == frames.Count - 1)
    //        {
    //            if (!shouldLoop)
    //            {
    //                yield return new WaitForSeconds(frameRate);
    //                onComplete?.Invoke();
    //                yield break;
    //            }
    //        }

    //        currentIndex = (currentIndex + 1) % frames.Count;
    //        yield return new WaitForSeconds(frameRate);
    //    }
    //}
    IEnumerator AnimateRoutine(SpriteRenderer renderer, string enemyId, Dictionary<string, SpriteData> database, List<string> frames, float frameRate, Action onComplete)
    {
        int currentIndex = 0;
        bool shouldLoop = (onComplete == null);
        EnemyController enemyCtrl = renderer.GetComponent<EnemyController>();

        while (true)
        {
            // 🔥 NẾU ENEMY BỊ ĐÓNG BĂNG: Tạm dừng xử lý, giữ nguyên frame hiện tại
            if (enemyCtrl != null && enemyCtrl.isFrozen)
            {
                yield return null;
                continue;
            }

            SpriteData d = database[frames[currentIndex]];

            if (enemyCtrl != null)
            {
                enemyCtrl.lastAnimPrefixBeforeFreeze = frames[currentIndex];
            }

            ApplySpriteFrame(renderer, enemyId, d);

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
    // Hàm hiển thị duy nhất 1 frame tĩnh, xử lý được cả các frame bị ẩn trong alias
    // 1. Hàm hiển thị đơn nâng cấp - có nhận thêm biến pivotYOffset (Mặc định bằng 0f)
    public bool DisplaySingleFrame(GameObject target, string enemyId, string frameKey, float pivotYOffset = 0f)
    {
        if (target == null) return false;
        SpriteRenderer renderer = target.GetComponent<SpriteRenderer>();
        if (renderer == null) return false;

        string cleanEnemyId = enemyId.Trim().ToLower();
        string cleanFrameKey = frameKey.Trim().ToLower();

        // Tìm database của con quái
        if (enemyDatabases.TryGetValue(cleanEnemyId, out var database))
        {
            // Nếu trong database chứa frame này
            if (database.TryGetValue(cleanFrameKey, out SpriteData d))
            {
                // Stop mọi animation tự động đang chạy nếu có để tránh xung đột
                int id = target.GetInstanceID();
                if (activeCoroutines.TryGetValue(id, out Coroutine old))
                {
                    if (old != null) StopCoroutine(old);
                    activeCoroutines.Remove(id);
                }

                // Ép ảnh vào SpriteRenderer và truyền thêm lượng dịch tâm Pivot Y vào
                return ApplySpriteFrame(renderer, cleanEnemyId, d, pivotYOffset);
            }
        }
        return false;
    }

    // 2. Hàm xử lý cắt ảnh nâng cấp - trực tiếp cộng trừ lượng pivotOffset vào trục Y
    private bool ApplySpriteFrame(SpriteRenderer renderer, string enemyId, SpriteData d, float pivotYOffset = 0f)
    {
        if (!enemyTextures.TryGetValue(enemyId, out var texDict)) return false;
        if (!texDict.TryGetValue(d.atlasName, out Texture2D tex))
        {
            Debug.LogError($"⚠️ Không tìm thấy atlas '{d.atlasName}' cho enemy '{enemyId}'!");
            return false;
        }

        // ── 1. Rect miếng ảnh trong atlas (flip Y vì Unity tọa độ từ dưới lên) ──
        Rect pixelRect = new Rect(
            d.f_quad.x,
            tex.height - d.f_quad.y - d.f_quad.height,
            d.f_quad.width,
            d.f_quad.height
        );

        // ── 2. Kích thước thực của miếng ảnh đã cắt ──
        float trimLeft = d.trim.x;
        float trimTop = d.trim.y;
        float trimRight = d.trim.z;
        float trimBottom = d.trim.w;

        float croppedW = d.fullSize.x - trimLeft - trimRight;   // == f_quad.width
        float croppedH = d.fullSize.y - trimTop - trimBottom;  // == f_quad.height

        // ── 3. Chọn anchor cố định trên khung GỐC 72×72 ──
        float anchorFullX = d.fullSize.x * 0.5f;   // 36px - chính giữa theo X
        float anchorFullY = d.fullSize.y;           // 72px - đáy khung gốc

        // ── 4. Map anchor từ khung gốc → tọa độ bên trong miếng ảnh đã cắt ──
        float localX = anchorFullX - trimLeft;
        float localY = anchorFullY - trimTop;

        // ── 5. Quy đổi ra [0,1] theo kích thước miếng cắt và cộng thêm PIVOT OFFSET ──
        float pivotX = Mathf.Clamp01(localX / croppedW);

        // Tính toán pivotY chuẩn theo hệ tọa độ gốc, sau đó cộng thêm lượng offset của bạn
        float basePivotY = Mathf.Clamp01(1f - (localY / croppedH));
        float pivotY = Mathf.Clamp01(basePivotY + pivotYOffset);

        // Tạo Sprite với tâm Pivot mới đã được bù trừ hoàn hảo
        renderer.sprite = Sprite.Create(
            tex,
            pixelRect,
            new Vector2(pivotX, pivotY),
            pixelsPerUnit
        );

        return true;
    }
}