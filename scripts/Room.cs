using Godot;
using System;
using System.Collections.Generic;
using ScoundrelGame;

public partial class Room : Control
{
	private List<Marker2D> slots = new List<Marker2D>();
	public List<Card> cards { get; private set; } = new List<Card>();
	public int MaxRoomSize => 4;

	private Deck deck => GetNode<Deck>("../Deck");

	public override void _Ready()
	{
		for (int i = 0; i < MaxRoomSize; i++)
		{
			Marker2D slot = GetNode<Marker2D>($"Slots/Slot{i + 1}");
			slots.Add(slot);
		}
	}

	public void DrawRoom(Deck deck)
	{
		GD.Print("Drawing new room!");

		for (int i = 0; i < cards.Count; i++)
		{
			cards[i].slotPosition = slots[i].GlobalPosition;
		}

		for (int i = cards.Count; i < MaxRoomSize && deck.CardsRemaining > 0; i++)
		{
			Card drawnCard = deck.DrawCard(slots[i].GlobalPosition);
			Card cardNode = Card.NewCard(drawnCard.Suit, drawnCard.Rank, drawnCard.slotPosition);

			cards.Add(cardNode);
			GetNode($"Cards").AddChild(cardNode);
		}

		/* int i = 0;
		while (cards.Count < MaxRoomSize && deck.CardsRemaining > 0)
		{
			Card drawnCard = deck.DrawCard(slots[i].GlobalPosition);
			Card cardNode = Card.NewCard(drawnCard.Suit, drawnCard.Rank, drawnCard.slotPosition);

			cards.Add(cardNode);
			slots[i].AddChild(cardNode);

			i++;
		} */
	}
}
