using Godot;
using System;
using System.Collections.Generic;
using ScoundrelGame;

public partial class Game : Control
{
	private Deck deck => GetNode<Deck>("Deck");
	private Room room => GetNode<Room>("Room");
	private DiscardPile discardPile => GetNode<DiscardPile>("DiscardPile");
	private Player player => GetNode<Player>("Player");
	private Control equippedWeapon => GetNode<Control>("EquippedWeapon");

	public override void _Ready()
	{
		PlayerTurn();
	}

	private void PlayerTurn()
	{
		GD.Print("Starting new turn!");

		player.healthPotionUsed = false;
		room.DrawRoom(deck);
	}

	public void OnCardChosen(Card card)
	{
		GD.Print($"Card chosen: {card.CardName} : {card.Type}");

		switch (card.Type)
		{
			case Enums.CardType.Health:
				CardHealth(card);
				break;

			case Enums.CardType.Weapon:
				CardWeapon(card);
				break;

			case Enums.CardType.Monster:
				CardMonster(card);
				break;
		}

		room.cards.Remove(card);

		GD.Print($"Cards left in room: {room.cards.Count}");

		if (room.cards.Count == 1)
		{
			PlayerTurn();
		}
	}

	private void CardHealth(Card card)
	{
		if (!player.healthPotionUsed)
		{
			int heal = card.Value;
			int previousHealth = player.Health;

			player.Health = Math.Min(player.MaxHealth, player.Health + heal);
			player.healthPotionUsed = true;

			GD.Print($"Healed for {heal} points (Health: {previousHealth} -> {player.Health}).");
		}
		else
		{
			GD.Print("Health potion already used this turn! Discarding card.");
		}
		discardPile.DiscardCard(card);
	}

	private void CardWeapon(Card card)
	{
		if (player.EquippedWeapon != null)
		{
			GD.Print($"Discarding previous weapon: {player.EquippedWeapon.Name}.");

			discardPile.DiscardCard(player.EquippedWeapon);
		}

		if (card.GetParent() != null)
			card.GetParent().RemoveChild(card);

		card.slotPosition = equippedWeapon.GetNode<Marker2D>("WeaponSlot").GlobalPosition;
		equippedWeapon.AddChild(card);

		player.EquippedWeapon = card;
		player.LastSlainMonsterValue = null;

		GD.Print($"Equipped new weapon: {card.Name}.");
	}

	private void CardMonster(Card card)
	{
		//TODO

		discardPile.DiscardCard(card);
	}
}
