public class AnimationDirection
{
    public VerticalAnimation vertical;

    public bool faceLeft;

    public AnimationDirection(
        VerticalAnimation vertical,
        bool faceLeft)
    {
        this.vertical = vertical;
        this.faceLeft = faceLeft;
    }
}