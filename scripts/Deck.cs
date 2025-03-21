using Godot;
using System;
using System.Collections.Generic;
using ScoundrelGame;

/// <summary>
/// Deck class to handle the deck of cards, initialization and shuffling
/// </summary>
public partial class Deck : Control
{
	private List<Card> _cards = new List<Card>();
	public int CardsRemaining { get { return _cards.Count; } }
	private PackedScene cardScene = GD.Load<PackedScene>("res://scenes/card.tscn");

	public override void _Ready()
	{
		InitializeDeck(_cards);
		Shuffle(_cards);
	}

	private void InitializeDeck(List<Card> cards)
	{
		// Create a full deck of playing cards
		foreach (Enums.Suit suit in Enum.GetValues(typeof(Enums.Suit)))
		{
			foreach (Enums.Rank rank in Enum.GetValues(typeof(Enums.Rank)))
			{
				// Skip faces and ace of hearts
				if (suit == Enums.Suit.Hearts && rank > Enums.Rank.Ten)
					continue;

				Card card = (Card)cardScene.Instantiate();
				card.Suit = suit;
				card.Rank = rank;

				cards.Add(card);
			}
		}
	}

	private void Shuffle(List<Card> cards)
	{
		// Shuffle the deck using the Fisher-Yates algorithm
		Random rng = new Random();
		int n = cards.Count;

		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			Card temp = cards[k];
			cards[k] = cards[n];
			cards[n] = temp;
		}
	}

	public Card DrawCard(Vector2 slot)
	{
		// Check for an empty deck
		if (_cards.Count == 0)
		{
			GD.Print("Empty deck!");
			return null;
		}

		// Draw a card from the top of the deck
		Card card = _cards[0];
		card.slotPosition = slot;

		// Remove the card from the deck and return it
		_cards.RemoveAt(0);
		return card;
	}

	public void AddCard(Card card)
	{
		_cards.Add(card);
	}
}
