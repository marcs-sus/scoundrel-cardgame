using Godot;
using System;
using ScoundrelGame;

/// <summary>
/// Player classe to handle player stats
/// </summary>
public partial class Player : Node
{
	private int HP = 20;
	public int Health
	{
		// Update HP and health label when Health is set
		get => HP;
		set
		{
			if (HP != value)
			{
				HP = value;
				UpdateHealthLabel();
			}
		}
	}
	public int MaxHealth { get; set; } = 20;
	public Card EquippedWeapon { get; set; } = null;
	public Card LastSlainMonster { get; set; } = null;
	public bool healthPotionUsed { get; set; } = false;

	private Label healthLabel => GetNode<Label>("HealthLabel");

	public override void _Ready()
	{
		UpdateHealthLabel();
	}

	public void UpdateHealthLabel()
	{
		// Update health label text
		healthLabel.Text = $"{Health}/{MaxHealth}";
	}
}
