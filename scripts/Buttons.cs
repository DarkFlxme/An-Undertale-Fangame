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
	public override void _Ready()
	{
		buttons = [.. GetChildren().OfType<Node2D>()];
		for (int i = 0; i < buttons.Length; i++)
		{
			if (Name == "MainMenuButtons")
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
	}

	private async void OnButtonPressed()
	{
		selectsound.Play();
		switch (buttons[currentIndex].Name)
		{
			case "QuitYes":
				GetTree().Quit();
				break;
			case "QuitNo":
				var parentButtons = GetParent<Buttons>();
				foreach (var node in buttons)
				{
					node.Hide();
				}
				GetNode<Control>("Control").Show();
				foreach (Node node in parentButtons.GetChildren())
				{
					(node as Node2D).Show();
				}
				parentButtons.SetProcess(true);
				SetProcess(false);
				SetHighlighted(this, true);
				break;
			case "Casual":
				GameDifficulty = Difficulty.Casual;
				PlayerNode.maxHealth = PlayerNode.health = 150;
				PlayerNode.damage = 15;
				gameUI.GetNode<ProgressBar>("ProgressBar").MaxValue = 150;
				await FadeRect.FadeIn(1f);
				await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
				DisableNode(MenuButtons);
				EnableNode(PlayerNode);
				EnableNode(gameUI);
				EnableNode(map1);
				DisableNode(this);
				GetParent().GetNode<AudioStreamPlayer>("../Menu Music Player").QueueFree();
				await FadeRect.FadeOut(1f);
				MenuButtons.QueueFree();
				break;
			case "Normal":
				GameDifficulty = Difficulty.Normal;
				PlayerNode.health= PlayerNode.maxHealth = 100;
				PlayerNode.damage = 20;
				gameUI.GetNode<ProgressBar>("ProgressBar").MaxValue = 100;
				await FadeRect.FadeIn(1f);
				await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
				DisableNode(MenuButtons);
				EnableNode(PlayerNode);
				EnableNode(gameUI);
				EnableNode(map1);
				GetParent().GetNode<AudioStreamPlayer>("../Menu Music Player").QueueFree();
				DisableNode(this);
				await FadeRect.FadeOut(1f);
				MenuButtons.QueueFree();
				break;
			case "Extreme":
				PlayerNode.health= PlayerNode.maxHealth = 1;
				PlayerNode.damage = 31;
				gameUI.GetNode<ProgressBar>("ProgressBar").Hide();
				await FadeRect.FadeIn(1f);
				await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
				DisableNode(MenuButtons);
				EnableNode(PlayerNode);
				EnableNode(gameUI);
				DisableNode(this);
				EnableNode(map1);
				GetParent().GetNode<AudioStreamPlayer>("../Menu Music Player").QueueFree();
				await FadeRect.FadeOut(1f);
				MenuButtons.QueueFree();
				GameDifficulty = Difficulty.Extreme;
				break;
			default:
				if (menuAnimationEnabled) await FadeRect.FadeIn();
				buttons[currentIndex].SetProcess(true);
				SetProcess(false);
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
				buttons[currentIndex].Modulate = new Color(1f, 1f, 1f);
				buttons[currentIndex].Scale = new Vector2(1f, 1f);
				if (buttons[currentIndex].HasMethod("SetHighlighted"))
					(buttons[currentIndex] as Buttons).SetHighlighted((buttons[currentIndex] as Buttons).buttons[(buttons[currentIndex] as Buttons).currentIndex], true);
				if (menuAnimationEnabled)
				{
					await ToSignal(GetTree().CreateTimer(0.2f), SceneTreeTimer.SignalName.Timeout);
					await FadeRect.FadeOut();
				}
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
