using game;
using Godot;
using System;
using System.Linq;
using static game.Nodes;
using static game.Settings;
public partial class Buttons : Node2D
{
	public Node2D[] buttons = [];
	public int currentIndex = 0;
	[Export] AudioStreamPlayer switchsound;
	[Export] AudioStreamPlayer selectsound;
	public override async void _Ready()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		if (Name == "MainMenuButtons")
		{
			if (SettingsManager.AutoStartFight)
			{
				SettingsManager.AutoStartFight = false;
				switch (SettingsManager.RestartingDifficulty)
				{
					case Difficulty.Casual:
						PlayerNode.damage = 15;
						PlayerNode.health = 150;
						gameUI.GetNode<ProgressBar>("ProgressBar").MaxValue = 150;
						break;
					case Difficulty.Normal:
						PlayerNode.damage = 20;
						PlayerNode.health = 100;
						gameUI.GetNode<ProgressBar>("ProgressBar").MaxValue = 100;
						break;
					case Difficulty.Extreme:
						PlayerNode.damage = 31;
						PlayerNode.health = 1;
						gameUI.GetNode<ProgressBar>("ProgressBar").Hide();
						break;
				}
				DisableNode(MenuButtons);
				EnableNode(PlayerNode);
				EnableNode(gameUI);
				EnableNode(map1);
				GetNode<AudioStreamPlayer>("../Menu Music Player").QueueFree();
				FadeRect.Color = new Color(0, 0, 0);
				await FadeRect.FadeOut(1f);
				MenuButtons.QueueFree();
				return;
			}
		}
		buttons = [.. GetChildren().OfType<Node2D>()];
		for (int i = 0; i < buttons.Length; i++)
		{
			if (Name == "MainMenuButtons" || Name == "deathscreen")
			{

				if (i == currentIndex)
				{
					SetHighlighted(buttons[i], true);

				}
				else
				{
					SetHighlighted(buttons[i], false);
				}
			}
			buttons[i].SetProcess(false);
		}
		if (Name == "deathscreen")
		{
			GetNode<Label>("Control/ScoreLabel").Text = string.Format("Score: {0:D2}:{1:D2}", TimeSpan.FromSeconds(BossFightTime).Minutes, TimeSpan.FromSeconds(BossFightTime).Seconds);
			switch (GameDifficulty)
			{
				case Difficulty.Casual:
					TimeSpan ctime = TimeSpan.FromSeconds(SaveSystem.SaveFile["highscore1"].AsDouble());
					GetNode<Label>("Control/HighScoreLabel").Text = string.Format("Highscore: {0:D2}:{1:D2}", ctime.Minutes, ctime.Seconds);
					break;
				case Difficulty.Normal:
					TimeSpan ntime = TimeSpan.FromSeconds(SaveSystem.SaveFile["highscore2"].AsDouble());
					GetNode<Label>("Control/HighScoreLabel").Text = string.Format("Highscore: {0:D2}:{1:D2}", ntime.Minutes, ntime.Seconds);
					break;
				case Difficulty.Extreme:
					TimeSpan etime = TimeSpan.FromSeconds(SaveSystem.SaveFile["highscore3"].AsDouble());
					GetNode<Label>("Control/HighScoreLabel").Text = string.Format("Highscore: {0:D2}:{1:D2}", etime.Minutes, etime.Seconds);
					break;
			}
		}
	}

	private async void OnButtonPressed()
	{
		selectsound.Play();
		switch (buttons[currentIndex].Name)
		{
			case "BackToMenu":
				BossFightTime = 0.0;
				GetTree().ReloadCurrentScene();
				break;
			case "QuitGameButton":
				SettingsManager.SaveSettings();
				GetTree().Quit();
				break;
			case "Retry":
				SettingsManager.AutoStartFight = true;
				SettingsManager.RestartingDifficulty = GameDifficulty;
				BossFightTime = 0.0;
				GetTree().ReloadCurrentScene();
				break;
			case "Casual":
				GameDifficulty = Difficulty.Casual;
				PlayerNode.damage = 15;
				PlayerNode.health = 150;
				gameUI.GetNode<ProgressBar>("ProgressBar").MaxValue = 150;
				await FadeRect.FadeIn(1f);
				await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
				DisableNode(MenuButtons);
				EnableNode(PlayerNode);
				EnableNode(gameUI);
				EnableNode(map1);
				DisableNode(this);
				GetParent().GetNode<AudioStreamPlayer>("../Menu Music Player").QueueFree();
				await FadeRect.FadeOut();
				MenuButtons.QueueFree();
				break;
			case "Normal":
				GameDifficulty = Difficulty.Normal;
				PlayerNode.damage = 20;
				PlayerNode.health = 100;
				gameUI.GetNode<ProgressBar>("ProgressBar").MaxValue = 100;
				await FadeRect.FadeIn(1f);
				await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
				DisableNode(MenuButtons);
				EnableNode(PlayerNode);
				EnableNode(gameUI);
				EnableNode(map1);
				GetParent().GetNode<AudioStreamPlayer>("../Menu Music Player").QueueFree();
				DisableNode(this);
				await FadeRect.FadeOut();
				MenuButtons.QueueFree();
				break;
			case "Extreme":
				GameDifficulty = Difficulty.Extreme;
				PlayerNode.damage = 31;
				PlayerNode.health = 1;
				gameUI.GetNode<ProgressBar>("ProgressBar").Hide();
				await FadeRect.FadeIn(1f);
				await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
				DisableNode(MenuButtons);
				EnableNode(PlayerNode);
				EnableNode(gameUI);
				DisableNode(this);
				EnableNode(map1);
				GetParent().GetNode<AudioStreamPlayer>("../Menu Music Player").QueueFree();
				await FadeRect.FadeOut();
				MenuButtons.QueueFree();
				break;
			default:
				SetProcess(false);
				if (menuAnimationEnabled) await FadeRect.FadeIn();
				buttons[currentIndex].SetProcess(true);
				buttons[currentIndex].Modulate = new Color(1f, 1f, 1f);
				buttons[currentIndex].Scale = new Vector2(1f, 1f);
				foreach (Node2D node in buttons[currentIndex].GetChildren().OfType<Node2D>())
				{
					node.Show();
				}
				foreach (var node in buttons[currentIndex].GetChildren())
				{
					if (node is Control && node.Name == "Control")
					{
						(node as Control).Hide();
					}
				}
				foreach (var node in buttons)
				{
					if (node != buttons[currentIndex])
					{
						node.Hide();
					}
				}
				if (buttons[currentIndex].HasMethod("SetHighlighted"))
					(buttons[currentIndex] as Buttons).SetHighlighted((buttons[currentIndex] as Buttons).buttons[(buttons[currentIndex] as Buttons).currentIndex], true);
				if (buttons[currentIndex].GetNodeOrNull<Control>("Control2") != null)
				{
					buttons[currentIndex].GetNode<Control>("Control2").Show();
				}
				if (menuAnimationEnabled)
				{
					await ToSignal(GetTree().CreateTimer(0.2f), SceneTreeTimer.SignalName.Timeout);
					await FadeRect.FadeOut();
				}
				buttons[currentIndex].Modulate = new Color(1f, 1f, 1f);
				buttons[currentIndex].Scale = new Vector2(1f, 1f);
				break;
		}
	}
	public void SetHighlighted(Node2D node, bool highlighted)
	{
		Tween _tween;
		_tween = node.CreateTween();
		_tween.SetParallel(true);
		if (highlighted)
		{
			if (menuAnimationEnabled)
				_tween.TweenProperty(node, "scale", new Vector2(1.5f, 1.5f), 0.2f);
			_tween.TweenProperty(node, "modulate", new Color(1f, 1f, 0f), 0.2f);
		}
		else
		{
			_tween.TweenProperty(node, "scale", new Vector2(1f, 1f), 0.2f);
			_tween.TweenProperty(node, "modulate", new Color(1f, 1f, 1f), 0.2f);
		}
	}
	public override void _Process(double delta)
	{
		Modulate = new Color(1f, 1f, 1f);
		if (GetNodeOrNull<Control>("Control2") != null && GetNodeOrNull<Control>("Control2").Visible)
		{
			Label label = GetNode<Label>("Control2/Label");
			switch (buttons[currentIndex].Name)
			{
				case "Casual":
					label.Text = "Highscore: " + string.Format("{0:D2}:{1:D2}", TimeSpan.FromSeconds(SaveSystem.SaveFile["highscore1"].AsDouble()).Minutes, TimeSpan.FromSeconds(SaveSystem.SaveFile["highscore1"].AsDouble()).Seconds);
					break;
				case "Normal":
					label.Text = "Highscore: " + string.Format("{0:D2}:{1:D2}", TimeSpan.FromSeconds(SaveSystem.SaveFile["highscore2"].AsDouble()).Minutes, TimeSpan.FromSeconds(SaveSystem.SaveFile["highscore2"].AsDouble()).Seconds);
					break;
				case "Extreme":
					label.Text = "Highscore: " + string.Format("{0:D2}:{1:D2}", TimeSpan.FromSeconds(SaveSystem.SaveFile["highscore3"].AsDouble()).Minutes, TimeSpan.FromSeconds(SaveSystem.SaveFile["highscore3"].AsDouble()).Seconds);
					break;
				default:
					label.Text = "";
					break;
			}
		}
		if (Input.IsActionJustPressed("move_down"))
		{
			currentIndex = (currentIndex + 1) % buttons.Length;
			switchsound.Play();
			for (int i = 0; i < buttons.Length; i++)
			{
				if (i == currentIndex)
				{
					SetHighlighted(buttons[i], true);

				}
				else
				{
					SetHighlighted(buttons[i], false);
				}
			}
		}
		if (Input.IsActionJustPressed("move_up"))
		{
			currentIndex = (currentIndex - 1 + buttons.Length) % buttons.Length;
			switchsound.Play();
			for (int i = 0; i < buttons.Length; i++)
			{
				if (i == currentIndex)
				{
					SetHighlighted(buttons[i], true);

				}
				else
				{
					SetHighlighted(buttons[i], false);
				}
			}
		}
		if (Input.IsActionJustPressed("yellow_heart_shot"))
		{
			OnButtonPressed();
		}
		if (Input.IsActionJustPressed("X") && Name != "MainMenuButtons")
		{
			var parentButtons = GetParent<Buttons>();
			foreach (var node in buttons)
			{
				node.Hide();
			}
			GetNode<Control>("Control").Show();
			if (GetNodeOrNull<Control>("Control2") != null)
				GetNode<Control>("Control2").Hide();
			foreach (Node node in parentButtons.GetChildren())
			{
				(node as Node2D).Show();
			}
			parentButtons.SetProcess(true);
			SetProcess(false);
			SetHighlighted(this, true);
		}
	}
}
