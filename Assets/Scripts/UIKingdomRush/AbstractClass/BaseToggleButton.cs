using UnityEngine;
using UnityEngine.UI;

public abstract class BaseToggleButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    // Trạng thái hiện tại của nút
    protected bool isCurrentStateOn = true;

    protected virtual void Start()
    {
        // Khởi tạo trạng thái ban đầu (ví dụ: lấy từ PlayerPrefs đã lưu trước đó)
        isCurrentStateOn = GetInitialState();
        UpdateUI(isCurrentStateOn);
    }

    protected virtual void OnDestroy()
    {

    }

    // Hàm gọi khi người chơi Click vào nút (Gán vào Event của Button Component)
    public void OnPointerClick()
    {
        isCurrentStateOn = !isCurrentStateOn;
        UpdateUI(isCurrentStateOn);

        // Gọi hàm xử lý riêng của từng loại nút (Sound hoặc Music)
        OnToggle(isCurrentStateOn);
    }

    protected void UpdateUI(bool isOn)
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isOn ? onSprite : offSprite;
        }
    }

    // Lớp con BẮT BUỘC phải cài đặt hàm này để bắn đúng Event mong muốn
    protected abstract void OnToggle(bool isOn);

    // Lớp con có thể ghi đè để quyết định trạng thái mặc định khi mở game
    protected virtual bool GetInitialState()
    {
        return true;
    }
}