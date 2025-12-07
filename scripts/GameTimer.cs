using Godot;
using System;

public partial class GameTimer : Label
{
    private float elapsedTime = 0.0f;

    public override void _Process(double delta)
    {
        if (IsVisibleInTree())
        {
            elapsedTime += (float)delta;
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            Text = string.Format("ZİNA    {0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
