using Godot;
using System;
using System.Linq;
using ScoundrelGame;

/// <summary>
/// Main game logic script
/// </summary>
public partial class Game : Control
{
	private Deck deck => GetNode<Deck>("Deck");
	private Room room => GetNode<Room>("Room");
	private DiscardPile discardPile => GetNode<DiscardPile>("DiscardPile");
	private Player player => GetNode<Player>("Player");
	private Control equippedWeapon => GetNode<Control>("EquippedWeapon");
	private Button withWeaponBtn => GetNode<Button>("FightOptions/MarginContainer/Options/WithWeapon");
	private Button barehandedBtn => GetNode<Button>("FightOptions/MarginContainer/Options/Barehanded");
	private Button avoidBtn => GetNode<Button>("AvoidOption/MarginContainer/Avoid");
	private Label gameOverLabel => GetNode<Label>("GameOver");

	public override void _Ready()
	{
		// Start the game with a new turn
		PlayerTurn(previousRoomAvoided: false);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("exit") || GetNode<Button>("Exit").ButtonPressed)
			GetTree().ChangeSceneToFile("res://scenes/menu.tscn");
	}

	private void PlayerTurn(bool previousRoomAvoided)
	{
		GD.Print("Starting new turn!");

		// Reset player potion used flag, only 1 may be used per turn
		player.healthPotionUsed = false;

		room.DrawRoom(deck);

		// Update avoid button disabled state
		avoidBtn.Disabled = !(room.cards.Count == room.MaxRoomSize && !previousRoomAvoided);
	}

	private void _OnAvoidPressed()
	{
		GD.Print("Avoiding combat!");

		// Remove all room cards and add them to the deck
		foreach (Card card in room.cards.ToList())
		{
			card.GetParent()?.RemoveChild(card);
			deck.AddCard(card);
		}

		room.cards.Clear();

		// Drawn new cards by starting a new turn
		PlayerTurn(previousRoomAvoided: true);
	}

	public void OnCardChosen(Card card)
	{
		GD.Print($"Card chosen: {card.CardName} : {card.Type}");

		// Handle card based on its type
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
		if (room.cards.Count == 0)
			GameOver(lost: false);

		// If a room reaches its minimum size, start a new turn
		if (room.cards.Count == room.RefreshAt)
		{
			PlayerTurn(previousRoomAvoided: false);
		}
		else
		{
			// Update avoid button disabled state
			avoidBtn.Disabled = !(room.cards.Count == room.MaxRoomSize);
		}
	}

	private void CardHealth(Card card)
	{
		// Only 1 health potion may be used per turn
		if (!player.healthPotionUsed)
		{
			int heal = card.Value;
			int previousHealth = player.Health;

			// Ensure health does not exceed max health
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
		// Discard the previously equipped weapon and last slain monster if a weapon is already equipped
		if (player.EquippedWeapon != null)
		{
			GD.Print($"Discarding previous weapon: {player.EquippedWeapon.Name}.");
			discardPile.DiscardCard(player.EquippedWeapon);

			if (player.LastSlainMonster != null)
				discardPile.DiscardCard(player.LastSlainMonster);
		}

		card.GetParent()?.RemoveChild(card);

		// Assign the new weapon to the corresponding slot
		card.slotPosition = equippedWeapon.GetNode<Marker2D>("Slots/WeaponSlot").Position;
		card.interactable = false;
		equippedWeapon.GetNode<Control>("Weapon").AddChild(card);

		// Update the player's equipped weapon and reset the last slain monster
		player.EquippedWeapon = card;
		player.LastSlainMonster = null;

		GD.Print($"Equipped new weapon: {card.Name}.");
	}

	private void CardMonster(Card card)
	{
		bool combatWithWeapon = withWeaponBtn.ButtonPressed && player.EquippedWeapon != null;

		// Handle combat based on choice and/or equipped weapon status
		if (!combatWithWeapon || (player.LastSlainMonster != null && player.LastSlainMonster.Value < card.Value))
			BarehandedCombat(card);
		else
			WeaponCombat(card);

		// End the game if player health reaches 0
		if (player.Health <= 0)
			GameOver(lost: true);
	}

	private void BarehandedCombat(Card card)
	{
		// Simply subtract the damage from the player's health
		int damage = card.Value;
		player.Health = Math.Max(0, player.Health - damage);

		GD.Print("Fought barehanded");
		GD.Print($"Monster: {card.CardName} (Value: {card.Value})");
		GD.Print($"Damage Taken: {damage}");

		discardPile.DiscardCard(card);
	}

	private void WeaponCombat(Card card)
	{
		// Subtract the damage from the player's health, considering the equipped weapon's value
		int damage = Math.Max(0, card.Value - player.EquippedWeapon.Value);
		player.Health = Math.Max(0, player.Health - damage);

		GD.Print($"Fought with weapon: {player.EquippedWeapon.Name}");
		GD.Print($"Monster: {card.CardName} (Value: {card.Value})");
		GD.Print($"Damage Taken: {damage}");

		if (player.LastSlainMonster != null)
			discardPile.DiscardCard(player.LastSlainMonster);

		// Add the last monster slain to the player weapon
		player.LastSlainMonster = card;

		card.GetParent()?.RemoveChild(card);

		// Assign the last monster slain to the corresponding slot
		card.slotPosition = equippedWeapon.GetNode<Marker2D>("Slots/LastMonster").Position;
		card.interactable = false;
		equippedWeapon.GetNode<Control>("Monster").AddChild(card);
	}

	public void GameOver(bool lost)
	{
		// Display game over message
		GD.Print("Game Over! - " + (lost ? "You Died!" : "You Win!"));
		gameOverLabel.Text = "Game Over!\n" + (lost ? "You Died!" : "You Win!");

		// Disable all interactions
		avoidBtn.Disabled = true;
		barehandedBtn.Disabled = true;
		withWeaponBtn.Disabled = true;
		room.cards.ForEach(card => card.interactable = false);

		// Set timer to exit the game
		Timer timer = GetNode<Timer>("Timer");
		timer.Start();
		timer.Timeout += () => GetTree().ChangeSceneToFile("res://scenes/menu.tscn");
	}
}
