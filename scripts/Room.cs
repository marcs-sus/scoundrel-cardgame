using Godot;
using System;
using System.Collections.Generic;
using ScoundrelGame;

/// <summary>
/// Room class to handle the room cards, slots and logic
/// </summary>
public partial class Room : Control
{
	private List<Marker2D> slots = new List<Marker2D>();
	public List<Card> cards { get; private set; } = new List<Card>();
	[Export] public int MaxRoomSize = 4;
	[Export] public int RefreshAt = 1;

	private Deck deck => GetNode<Deck>("../Deck");

	public override void _Ready()
	{
		// Initialize slots
		for (int i = 0; i < MaxRoomSize; i++)
		{
			Marker2D slot = GetNode<Marker2D>($"Slots/Slot{i + 1}");
			slots.Add(slot);
		}
	}

	public void DrawRoom(Deck deck)
	{
		GD.Print("Drawing new room!");

		// Assign the cards positions to the slots 
		for (int i = 0; i < cards.Count; i++)
			cards[i].slotPosition = slots[i].Position;

		for (int i = cards.Count; i < MaxRoomSize && deck.CardsRemaining > 0; i++)
		{
			// Draw a new card
			Card drawnCard = deck.DrawCard(slots[i].Position);
			// Create a new card instance from the drawn card
			Card cardNode = Card.NewCard(drawnCard.Suit, drawnCard.Rank, drawnCard.slotPosition);

			// Add the card to the room
			cards.Add(cardNode);
			GetNode($"Cards").AddChild(cardNode);
		}
	}
}
