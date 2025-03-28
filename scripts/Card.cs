using Godot;
using System;
using System.Collections.Generic;
using ScoundrelGame;

/// <summary>
/// Card classe to handle card properties, events and visuals
/// </summary>
public partial class Card : Button
{
	[Export] public Enums.Suit Suit { get; set; }
	[Export] public Enums.Rank Rank { get; set; }

	public string CardName => $"{Rank} of {Suit}";
	public int Value => (int)Rank;
	public Enums.CardType Type
	{
		// Return the card type based on the suit
		get
		{
			if (Suit == Enums.Suit.Hearts)
				return Enums.CardType.Health;
			if (Suit == Enums.Suit.Diamonds)
				return Enums.CardType.Weapon;
			return Enums.CardType.Monster;
		}
	}

	public Vector2 slotPosition { get; set; }
	private bool followingMouse = false;
	private Vector2 dragOffset = Vector2.Zero;
	public bool interactable { get; set; } = true;

	private readonly static PackedScene cardScene = GD.Load<PackedScene>("res://scenes/card.tscn");
	private TextureRect cardTexture => GetNode<TextureRect>("CardTexture");
	private Game game => GetNode<Game>("/root/Game");
	private CardManager cardManager => GetNode<CardManager>("/root/CardManager");

	public static Card NewCard(Enums.Suit suit, Enums.Rank rank, Vector2 slotPosition)
	{
		// Create a new card instance
		Card card = cardScene.Instantiate<Card>();
		card.Suit = suit;
		card.Rank = rank;
		card.slotPosition = slotPosition;
		return card;
	}

	public override void _Ready()
	{
		// Load the card atlas texture
		string textureStr = $"{Suit}_{Rank}";

		try
		{
			cardTexture.Texture = cardManager.CardTextures[textureStr];
		}
		catch (KeyNotFoundException ex)
		{
			GD.PrintErr(ex.Message);
		}
	}

	public override void _Process(double delta)
	{
		if (followingMouse)
		{
			FollowMouse();
		}
		else
		{
			// Linearly interpolate the card position to the slot position
			Position = Position.Lerp(slotPosition, 0.05f);
			if (Position.DistanceTo(slotPosition) < 2.0f)
				Position = slotPosition;
		}
	}

	private void FollowMouse()
	{
		// Move the card to the global mouse position
		Vector2 mousePos = GetGlobalMousePosition();
		Position = mousePos - dragOffset;
	}

	private void _OnGuiInput(InputEvent @event)
	{
		if (!(@event is InputEventMouseButton mouseEvent)) return;
		if (mouseEvent.ButtonIndex != MouseButton.Left) return;

		// Handle card events when double-clicked
		if (mouseEvent.DoubleClick && interactable)
		{
			game.OnCardChosen(this);
			return;
		}

		// Handle card events when clicked
		if (mouseEvent.Pressed)
		{
			// Prevent snap when dragging the card with an offset
			dragOffset = GetGlobalMousePosition() - Position;
			followingMouse = true;

			// Move the card to the top of the parent node
			Node parent = GetParent();
			if (parent != null)
				parent.MoveChild(this, parent.GetChildCount() - 1);
		}
		else
		{
			followingMouse = false;
		}
	}
}
