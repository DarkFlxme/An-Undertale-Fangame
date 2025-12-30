using game;
using Godot;
using System;
using static game.Nodes;

public partial class Buttons : Node2D
{
	[Export] public Node2D[] buttons;
	[Export] public int elemanSayisi;
	[Export] public float speed;
	public int i = 0;
	Vector2 targetScale;
	Vector2 firstScale;
	Vector2[] firstPosition;
	[Export] float buyuklukKati;
	public bool selected = false;
	[Export] Node2D Title;
	[Export] int indexMenu;
	[Export] Node2D[] buttons2;
	[Export]AudioStreamPlayer switchsound;
	[Export]AudioStreamPlayer selectsound;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		i = 0;
		firstPosition = new Vector2[buttons.Length];
		firstScale = buttons[0].GlobalScale;
		targetScale = new Vector2(firstScale.X * buyuklukKati, firstScale.Y * buyuklukKati);
		for (int a = 0; a < buttons.Length; a++)
		{
			firstPosition[a] = buttons[a].Position;
		}
		for (int a = 0; a < buttons2.Length; a++)
		{
			buttons2[a].SetProcess(false);
		}
	}
	private async void FadeIn(float duration = 0.5f)
    {
        await FadeRect.FadeIn(duration);
    }
	private async void FadeOut(float duration = 0.5f)
	{
		await FadeRect.FadeOut(duration);
	}
	private async void OnPlayButtonPressed()
	{
		await FadeRect.FadeIn(1.0f);
		EnableNode(PlayerNode);
		EnableNode(map1);
		DisableNode(this);
		EnableNode(GetNode<Control>("../GameUI"));
		await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
		await FadeRect.FadeOut(1.0f);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("yellow_heart_shot") && !selected)
		{
			if (buttons[i].IsInGroup("Buttons"))
			{
				Nodes.i++;
				selected = true;
			}
			if (buttons[i].Name == "QuitYes")
			{
				GetTree().Quit();
			}
			if (buttons[i].Name == "QuitNo")
			{
				Nodes.i--;
				if (GetParent().GetParent() is Buttons quitNo)
				{
					quitNo.selected = false;
				}
			}
			if (buttons[i].Name == "SettingsButton")
			{
				foreach (var child in buttons[i].GetChildren())
				{
					if (child is SettingsNode settingsMenu)
					{
						EnableNode(settingsMenu);
					}
				}
			}
			if (buttons[i].Name == "PlayButton")
			{
				OnPlayButtonPressed();
			}
			selectsound.Play();
		}
		if (Input.IsActionJustPressed("move_right") && buttons[i] is Listecik liste && liste.i != 0)
		{
			liste.i++;
		}
		if (Input.IsActionJustPressed("move_left") && buttons[i] is Listecik liste2 && liste2.i != liste2.secenekler.Length)
		{
			liste2.i++;
		}
		if (Input.IsActionJustPressed("move_up") && !selected)
		{
			if (i == 0)
			{
				i = elemanSayisi - 1;
			}
			else
			{
				i--;
			}
			switchsound.Play();
		}
		else if (Input.IsActionJustPressed("move_down") && !selected)
		{
			if (i == elemanSayisi - 1)
			{
				i = 0;
			}
			else
			{
				i++;
			}
			switchsound.Play();
		}
		foreach (var childButton in buttons2)
		{
			if (childButton is Buttons but)
			{
				if (Nodes.i < indexMenu)
				{
					but.i = 0;
				}
			}			
		}
		foreach (Node2D button in buttons)
		{
			int index = Array.IndexOf(buttons, button);
			int currentIndex = Array.IndexOf(buttons, buttons[i]);
			if (button == buttons[i] && !selected)
			{
				Label label = button.GetNode<Label>("Label");
				button.GlobalScale = button.GlobalScale.Lerp(targetScale, speed * (float)delta);
				label.SelfModulate = label.SelfModulate.Lerp(Colors.Yellow, speed * (float)delta);
			}
			else if (button != buttons[i] && !selected)
			{
				Label label = button.GetNode<Label>("Label");
				button.GlobalScale = button.GlobalScale.Lerp(firstScale, speed * (float)delta);
				label.SelfModulate = label.SelfModulate.Lerp(Colors.White, speed * (float)delta);
			}

			if (selected)
			{
				Color a = buttons2[currentIndex].Modulate;
				a.A = 1;
				buttons2[currentIndex].Modulate = buttons2[currentIndex].Modulate.Lerp(a, speed * (float)delta);
				if (button == buttons[i] && Nodes.i <= indexMenu)
				{
					buttons2[index].SetProcess(true);
					Label label = button.GetNode<Label>("Label");
					button.GlobalPosition = button.GlobalPosition.Lerp(Title.GlobalPosition, speed * (float)delta);
					if (button.Name != "QuitGameButton")
					{
						label.SelfModulate = label.SelfModulate.Lerp(Colors.White, speed * (float)delta);
					}
					else
					{
						label.SelfModulate = label.SelfModulate.Lerp(new Color(1, 1, 1, -1), speed * (float)delta);
					}
				}
				else if (button != buttons[i] && Nodes.i <= indexMenu)
				{
					Label label = button.GetNode<Label>("Label");
					Color b = label.SelfModulate;
					b.A = -1;
					label.SelfModulate = label.SelfModulate.Lerp(b, speed * (float)delta);
				}

				if (Input.IsActionJustPressed("X") && Nodes.i == indexMenu)
				{
					Nodes.i--;
					selected = false;
					if (buttons[i].Name == "SettingsButton")
					{
						foreach (var child in buttons[i].GetChildren())
						{
							if (child is SettingsNode settingsMenu)
							{
								settingsMenu.Hide();
							}
						}
					}
				}

				if (Nodes.i > indexMenu && button == buttons[i])
				{
					Label label = button.GetNode<Label>("Label");
					Color b = button.Modulate;
					b.A = -1;
					label.SelfModulate = label.SelfModulate.Lerp(b, speed * (float)delta);
				}
			}
			else
			{
				button.Position = button.Position.Lerp(firstPosition[index], speed * (float)delta);
				Color a = button.Modulate;
				a.A = -1;
				buttons2[index].Modulate = buttons2[index].Modulate.Lerp(a, speed * (float)delta);
				buttons2[index].SetProcess(false);
			}
		}
	}
}
