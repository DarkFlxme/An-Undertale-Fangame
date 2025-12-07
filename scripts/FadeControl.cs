using Godot;
using System.Threading.Tasks;

public partial class FadeControl : ColorRect
{
    private Tween _tween;

    public async Task FadeIn(float duration = 0.5f)
    {
        _tween?.Kill();

        _tween = CreateTween();
        _tween.TweenProperty(this, "color:a", 1.0f, duration);

        await ToSignal(_tween, Tween.SignalName.Finished);
    }

    public async Task FadeOut(float duration = 0.5f)
    {
        _tween?.Kill();

        _tween = CreateTween();
        _tween.TweenProperty(this, "color:a", 0.0f, duration);

        await ToSignal(_tween, Tween.SignalName.Finished);
    }
}
