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
			Marker2D slot = GetNode<Marker2D>($"Slot{i+1}");
			slots.Add(slot);
		}
	}

	public void DrawRoom(Deck deck)
	{
		while (cards.Count < MaxRoomSize && deck.CardsRemaining > 0)
		{
			cards.Add(deck.DrawCard());
		}

		AssignCards(cards);
	}

	public void AssignCards(List<Card> cards)
	{
		if (cards.Count != MaxRoomSize)
		{
			GD.PrintErr("Room require 4 cards!");
			return;
		}

		for (int i = 0; i < MaxRoomSize; i++)
		{
			Card card = cards[i];
			card.slotPosition = slots[i].GlobalPosition;
		}
	}
}
