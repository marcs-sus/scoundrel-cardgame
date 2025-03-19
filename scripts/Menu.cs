using Godot;
using System;
using ScoundrelGame;

public partial class Menu : Control
{
	private Card showCard => GetNode<Card>("ShowCard");

	public override void _Ready()
	{
		showCard.slotPosition = showCard.Position;
		showCard.interactable = false;
	}

	private void _OnPlayPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/game.tscn");
	}

	private void _OnOptionsPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/options.tscn");
	}

	private void _OnExitPressed()
	{
		GetTree().Quit();
	}
}
