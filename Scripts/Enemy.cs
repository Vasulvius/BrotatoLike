using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	private int health = 100;
	public override void _PhysicsProcess(double delta)
	{
	}

	public void TakeDamage(int dmg)
	{
		health -= dmg;
		if(health < 0){Die();}
	}

	private void Die()
	{
		Player player = Player.Instance;
		player.RemoveEnemy(this);
		QueueFree();
	}
}
