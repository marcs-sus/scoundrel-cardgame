using Godot;
using System;
using System.Collections.Generic;
using ScoundrelGame;

/// <summary>
/// Discard pile class to handle the discarded cards
/// </summary>
public partial class DiscardPile : Control
{
	private List<Card> discardPile = new List<Card>();
	public int DiscardedCards { get => discardPile.Count; }

	public void DiscardCard(Card card)
	{
		card.GetParent()?.RemoveChild(card);

		card.slotPosition = Position;
		card.interactable = false;
		discardPile.Add(card);

		// Optional visualization effect
		// AddChild(card);
	}

	public void ClearDiscardPile() // Optional
	{
		// Remove all cards from the discard pile
		foreach (Card card in discardPile)
			card.QueueFree();

		discardPile.Clear();
	}
}
