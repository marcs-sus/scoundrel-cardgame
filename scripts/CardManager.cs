using Godot;
using System;
using System.Collections.Generic;
using ScoundrelGame;

/// <summary>
/// Auto-loaded card manager for texture loading and management
/// </summary>
public partial class CardManager : Node
{
	public Dictionary<string, Texture2D> CardTextures { get; private set; } = new();

	public override void _Ready()
	{
		foreach (Enums.Suit Suit in Enum.GetValues(typeof(Enums.Suit)))
		{
			foreach (Enums.Rank Rank in Enum.GetValues(typeof(Enums.Rank)))
			{
				string cardPath = $"res://assets/visuals/cards/card_{Suit.ToString().ToLower()}_{Rank.ToString().ToLower()}.png";
				CardTextures.Add($"{Suit}_{Rank}", GD.Load<Texture2D>(cardPath));
			}
		}
	}
}
