using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class DecorSpriteAnimator : MonoBehaviour
{
    [System.Serializable]
    public class AtlasMapping
    {
        public string atlasNameInTxt;
        public Texture2D texture; // Để ảnh SINGLE ở đây, code tự xử lý Runtime Create!
    }

    [System.Serializable]
    public class DecorAnimationConfig
    {
        public string decorId; // Ví dụ: stage_grass
        public TextAsset dataFile;
        public List<AtlasMapping> atlases;
    }

    [Header("Decor Databases")]
    public List<DecorAnimationConfig> decorConfigs;

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

    private Dictionary<string, Dictionary<string, SpriteData>> decorDatabases = new Dictionary<string, Dictionary<string, SpriteData>>();
    private Dictionary<string, Dictionary<string, Texture2D>> decorTextures = new Dictionary<string, Dictionary<string, Texture2D>>();
    private Dictionary<int, Coroutine> activeCoroutines = new Dictionary<int, Coroutine>();

    public static DecorSpriteAnimator Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializeDatabases();
    }

    private void InitializeDatabases()
    {
        if (decorConfigs == null) return;

        foreach (var config in decorConfigs)
        {
            if (string.IsNullOrEmpty(config.decorId)) continue;

            string cleanDecorId = config.decorId.Trim().ToLower();

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
            decorTextures[cleanDecorId] = texDict;

            // 2. Parse Data
            var spriteDict = new Dictionary<string, SpriteData>();
            ParseData(config.dataFile, spriteDict);
            decorDatabases[cleanDecorId] = spriteDict;

            Debug.Log($"[DecorSpriteAnimator] Khởi tạo thành công Decor ID: '{cleanDecorId}' với {spriteDict.Count} frames.");
        }
    }

    private void ParseData(TextAsset dataFile, Dictionary<string, SpriteData> targetDatabase)
    {
        if (dataFile == null) return;

        string content = dataFile.text;
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

                if (!targetDatabase.ContainsKey(mainName))
                    targetDatabase.Add(mainName, data);

                string aliasRaw = m.Groups["alias"].Value;
                if (!string.IsNullOrEmpty(aliasRaw))
                {
                    MatchCollection aliasMatches = Regex.Matches(aliasRaw, @"""(?<aliasName>.+?)""");
                    foreach (Match am in aliasMatches)
                    {
                        string aliasName = am.Groups["aliasName"].Value.Trim().ToLower();
                        if (!targetDatabase.ContainsKey(aliasName))
                        {
                            targetDatabase.Add(aliasName, data);
                        }
                    }
                }
            }
            catch { continue; }
        }
    }

    public void PlayAnimation(GameObject target, string decorId, string animPrefix, float frameRate = -1)
    {
        if (target == null) return;
        SpriteRenderer renderer = target.GetComponent<SpriteRenderer>();
        if (renderer == null) return;

        string cleanDecorId = decorId.Trim().ToLower();
        string cleanPrefix = animPrefix.Trim().ToLower();

        if (!decorDatabases.TryGetValue(cleanDecorId, out var database)) return;

        List<string> frames = new List<string>();
        foreach (var key in database.Keys)
        {
            if (key.StartsWith(cleanPrefix)) frames.Add(key);
        }
        frames.Sort();

        if (frames.Count == 0) return;

        StopAnimationFor(target);
        int id = target.GetInstanceID();
        float finalFrameRate = frameRate > 0 ? frameRate : defaultFrameRate;
        Coroutine c = StartCoroutine(AnimateRoutine(renderer, cleanDecorId, database, frames, finalFrameRate));
        activeCoroutines[id] = c;
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

    IEnumerator AnimateRoutine(SpriteRenderer renderer, string decorId, Dictionary<string, SpriteData> database, List<string> frames, float frameRate)
    {
        int currentIndex = 0;
        while (true)
        {
            if (renderer == null) yield break;

            SpriteData d = database[frames[currentIndex]];
            ApplySpriteFrame(renderer, decorId, d);

            currentIndex = (currentIndex + 1) % frames.Count;
            yield return new WaitForSeconds(frameRate);
        }
    }

    private bool ApplySpriteFrame(SpriteRenderer renderer, string decorId, SpriteData d)
    {
        if (!decorTextures.TryGetValue(decorId, out var texDict)) return false;
        if (!texDict.TryGetValue(d.atlasName, out Texture2D tex)) return false;

        Rect pixelRect = new Rect(
            d.f_quad.x,
            tex.height - d.f_quad.y - d.f_quad.height,
            d.f_quad.width,
            d.f_quad.height
        );

        float trimLeft = d.trim.x;
        float trimTop = d.trim.y;
        float trimRight = d.trim.z;
        float trimBottom = d.trim.w;

        float croppedW = d.fullSize.x - trimLeft - trimRight;
        float croppedH = d.fullSize.y - trimTop - trimBottom;

        float anchorFullX = d.fullSize.x * 0.5f;
        float anchorFullY = d.fullSize.y;

        float localX = anchorFullX - trimLeft;
        float localY = anchorFullY - trimTop;

        float pivotX = Mathf.Clamp01(localX / croppedW);
        float pivotY = Mathf.Clamp01(1f - (localY / croppedH));

        renderer.sprite = Sprite.Create(
            tex, pixelRect,
            new Vector2(pivotX, pivotY),
            pixelsPerUnit
        );
        return true;
    }
}