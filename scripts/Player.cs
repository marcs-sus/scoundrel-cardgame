using Godot;
using System;
using ScoundrelGame;

public partial class Player : Node
{
	public int Health { get; set; } = 20;
	public int MaxHealth { get; set; } = 20;
	public Card EquippedWeapon { get; set; } = null;
	public Card LastSlainMonster { get; set; } = null;
	public bool healthPotionUsed { get; set; } = false;
	public bool previousRoomAvoided { get; set; } = false;

	private Label healthLabel => GetNode<Label>("HealthLabel");

	public override void _Process(double delta)
	{
		healthLabel.Text = $"{Health}/{MaxHealth}";
	}
}
