using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class MapDecoAnimationManager : MonoBehaviour
{
    public static MapDecoAnimationManager Instance { get; private set; }

    [Header("Resources")]
    [Tooltip("File .txt chứa thông số dữ liệu Lua")]
    [SerializeField] private TextAsset dataTxtFile;
    [Tooltip("File ảnh chứa toàn bộ các hiệu ứng")]
    [SerializeField] private Texture2D atlasTexture;

    // Dictionary lưu tên Animation gốc -> Danh sách các Sprite theo thứ tự
    private Dictionary<string, List<Sprite>> animationDict = new Dictionary<string, List<Sprite>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ParseDataAndSliceSprites();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Đọc file TXT dạng Lua và tiến hành cắt Texture thành các Sprite
    /// </summary>
    private void ParseDataAndSliceSprites()
    {
        if (dataTxtFile == null || atlasTexture == null)
        {
            Debug.LogError("[MapDecoManager] Vui lòng gán đầy đủ file Txt và Ảnh trong Inspector!");
            return;
        }

        string textData = dataTxtFile.text;
        int texWidth = atlasTexture.width;
        int texHeight = atlasTexture.height;

        // Regex bóc tách dữ liệu tên frame và cụm f_quad={x,y,w,h}
        string pattern = @"\[""([^""]+)""\].*?f_quad=\{([\d]+),([\d]+),([\d]+),([\d]+)\}";
        MatchCollection matches = Regex.Matches(textData, pattern);

        foreach (Match match in matches)
        {
            string fullFrameName = match.Groups[1].Value; // Ví dụ: mapDeco_smoke_0001
            int x = int.Parse(match.Groups[2].Value);
            int y = int.Parse(match.Groups[3].Value);
            int w = int.Parse(match.Groups[4].Value);
            int h = int.Parse(match.Groups[5].Value);

            // Đổi hệ toạ độ từ Top-Left (Lua) sang Bottom-Left (Unity)
            int unityY = texHeight - y - h;

            // Tạo Sprite từ vùng ảnh cắt ra
            Rect spriteRect = new Rect(x, unityY, w, h);
            Sprite slicedSprite = Sprite.Create(atlasTexture, spriteRect, new Vector2(0.5f, 0.5f), 100f);
            slicedSprite.name = fullFrameName;

            // CẢI TIẾN: Dùng Regex cắt bỏ phần số cuối (Ví dụ: "mapDeco_smoke_0001" -> "mapDeco_smoke")
            // Giúp bạn truyền tên có dấu "_" ở cuối hay không đều chạy chuẩn.
            string animName = Regex.Replace(fullFrameName, @"_?\d+$", "");

            if (!animationDict.ContainsKey(animName))
            {
                animationDict[animName] = new List<Sprite>();
            }
            animationDict[animName].Add(slicedSprite);
        }

        // CẢI TIẾN: Sắp xếp danh sách Sprite theo đúng số thứ tự tự nhiên (0002 đứng trước 0010)
        foreach (var key in animationDict.Keys)
        {
            animationDict[key].Sort((a, b) => {
                int numA = GetFrameNumber(a.name);
                int numB = GetFrameNumber(b.name);
                return numA.CompareTo(numB);
            });
        }

        Debug.Log($"[MapDecoManager] Đã tải thành công {animationDict.Count} nhóm hiệu ứng animation!");
    }

    // Hàm phụ trợ lấy số thứ tự ở cuối tên frame phục vụ việc Sort chuẩn xác
    private int GetFrameNumber(string frameName)
    {
        Match match = Regex.Match(frameName, @"(\d+)$");
        if (match.Success && int.TryParse(match.Value, out int result))
        {
            return result;
        }
        return 0;
    }
    public void PlayAnimation(string animName, GameObject targetObject, float frameRate = 12f, bool loop = true, int startFrame = -1, int endFrame = -1, Action onComplete = null)
    {
        string cleanAnimName = Regex.Replace(animName, @"_+$", "");

        if (!animationDict.ContainsKey(cleanAnimName))
        {
            Debug.LogWarning($"[MapDecoManager] Không tìm thấy animation nào có tên: {cleanAnimName}");
            return;
        }

        if (targetObject == null) return;

        List<Sprite> originalSprites = animationDict[cleanAnimName];
        List<Sprite> filteredSprites = new List<Sprite>();

        if (startFrame != -1 && endFrame != -1)
        {
            foreach (Sprite sp in originalSprites)
            {
                int fNum = GetFrameNumber(sp.name);
                if (fNum >= startFrame && fNum <= endFrame)
                {
                    filteredSprites.Add(sp);
                }
            }
        }
        else
        {
            filteredSprites = originalSprites;
        }

        if (filteredSprites.Count == 0)
        {
            Debug.LogWarning($"[MapDecoManager] Không tìm thấy frame nào trong khoảng từ {startFrame} đến {endFrame} của {cleanAnimName}");
            return;
        }

        Image targetImage = targetObject.GetComponent<Image>();
        if (targetImage == null) return;

        UIAnimationPlayer player = targetObject.GetComponent<UIAnimationPlayer>();
        if (player == null)
        {
            player = targetObject.AddComponent<UIAnimationPlayer>();
        }

        // CẢI TIẾN: Truyền thêm thông số onComplete vào player
        player.StartAnimation(filteredSprites, frameRate, loop, onComplete);
    }
    /// <summary>
    /// BỔ SUNG: Dừng animation đang chạy trên một đối tượng cụ thể mà không ảnh hưởng tới các đối tượng khác
    /// </summary>
    /// <param name="targetObject">GameObject cần dừng hiệu ứng</param>
    public void StopAnimation(GameObject targetObject)
    {
        if (targetObject == null) return;

        UIAnimationPlayer player = targetObject.GetComponent<UIAnimationPlayer>();
        if (player != null)
        {
            player.StopAnimation();
        }
    }
}