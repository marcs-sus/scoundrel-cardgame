using Godot;
using System;
using System.Collections.Generic;
using ScoundrelGame;

public partial class Game : Control
{
	private Deck deck => GetNode<Deck>("Deck");
	private Room room => GetNode<Room>("Room");
	private Player player => GetNode<Player>("Player");
	private Marker2D weaponSlot => GetNode<Marker2D>("EquippedWeapon/WeaponSlot");

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
				//CardHealth(card);
				break;

			case Enums.CardType.Weapon:
				//CardWeapon(card);
				break;

			case Enums.CardType.Monster:
				//CardMonster(card);
				break;
		}
		
		room.cards.Remove(card);
		card.QueueFree();

		GD.Print($"Cards left in room: {room.cards.Count}");

		if (room.cards.Count == 1)
		{
			PlayerTurn();
		}
	}
}
