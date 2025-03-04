using Godot;
using System;
using System.Collections.Generic;
using ScoundrelGame;

public partial class Deck : Control
{
	private List<Card> _cards = new List<Card>();
	private PackedScene cardScene = GD.Load<PackedScene>("res://scenes/card.tscn");

	public override void _Ready()
	{
		InitializeDeck(_cards);
		Shuffle(_cards);
	}

	private void InitializeDeck(List<Card> cards)
	{
		foreach (Enums.Suit suit in Enum.GetValues(typeof(Enums.Suit)))
		{
			foreach (Enums.Rank rank in Enum.GetValues(typeof(Enums.Rank)))
			{
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

	public Card DrawCard(List<Card> cards)
	{
		if (cards.Count == 0)
		{
			GD.Print("Empty deck!");
			return null;
		}

		Card card = cards[0];
		cards.RemoveAt(0);
		return card;
	}

	private void _OnDrawPressed()
	{
		Card drawnCard = DrawCard(_cards);
		if (drawnCard == null) return;

		Card card = cardScene.Instantiate<Card>();
		card.Suit = drawnCard.Suit;
		card.Rank = drawnCard.Rank;
		card.Position = GlobalPosition;
		
		GetParent().AddChild(card);
	}
}
