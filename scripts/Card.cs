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

	private const int ATLAS_CARD_WIDTH = 42, ATLAS_CARD_HEIGHT = 60, ATLAS_DISTANCE = 65, BASE_X = 11, BASE_Y = 2;
	private const string CARD_ALTAS_PATH = "res://assets/visuals/cards_tilesheet.png";
	private Dictionary<Enums.Suit, int> suitMap = new Dictionary<Enums.Suit, int>()
	{
		// Map the suit to the index in the atlas
		{ Enums.Suit.Hearts, 0 },
		{ Enums.Suit.Diamonds, 1 },
		{ Enums.Suit.Clubs, 2 },
		{ Enums.Suit.Spades, 3 }
	};
	private Dictionary<Enums.Rank, int> rankMap = new Dictionary<Enums.Rank, int>()
	{
		// Map the rank to the index in the atlas
		{ Enums.Rank.Two, 1 },
		{ Enums.Rank.Three, 2 },
		{ Enums.Rank.Four, 3 },
		{ Enums.Rank.Five, 4 },
		{ Enums.Rank.Six, 5 },
		{ Enums.Rank.Seven, 6 },
		{ Enums.Rank.Eight, 7 },
		{ Enums.Rank.Nine, 8 },
		{ Enums.Rank.Ten, 9 },
		{ Enums.Rank.Jack, 10 },
		{ Enums.Rank.Queen, 11 },
		{ Enums.Rank.King, 12 },
		{ Enums.Rank.Ace, 0 }
	};

	public Vector2 slotPosition { get; set; }
	private bool followingMouse = false;
	private Vector2 dragOffset = Vector2.Zero;
	public bool interactable { get; set; } = true;

	private readonly static PackedScene cardScene = GD.Load<PackedScene>("res://scenes/card.tscn");
	private TextureRect cardTexture => GetNode<TextureRect>("CardTexture");
	private Game game => GetNode<Game>("/root/Game");

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
		Texture2D atlas = (Texture2D)GD.Load(CARD_ALTAS_PATH);

		int suitIndex = suitMap[Suit], rankIndex = rankMap[Rank];
		int regionX = BASE_X + rankIndex * ATLAS_DISTANCE, regionY = BASE_Y + suitIndex * ATLAS_DISTANCE;

		AtlasTexture atlasTexture = new AtlasTexture()
		{
			// Define the region in the atlas based on the suit and rank
			Atlas = atlas,
			Region = new Rect2(regionX, regionY, ATLAS_CARD_WIDTH, ATLAS_CARD_HEIGHT)
		};
		cardTexture.Texture = atlasTexture;
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
