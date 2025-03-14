using Godot;
using System;
using System.Collections.Generic;
using ScoundrelGame;

public partial class DiscardPile : Control
{
	private List<Card> discardPile = new List<Card>();
	public int DiscardedCards { get => discardPile.Count; }

	public void DiscardCard(Card card)
	{
		card.GetParent()?.RemoveChild(card);

		card.slotPosition = Position;
		discardPile.Add(card);

		AddChild(card); // Optional visualization effect
	}

	public void ClearDiscardPile()
	{
		foreach (Card card in discardPile)
			card.QueueFree();

		discardPile.Clear();
	}
}
