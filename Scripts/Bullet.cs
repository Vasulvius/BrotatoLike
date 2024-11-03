using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export] private float Speed = 300f;
	[Export] private int dmg = 10;
	[Export] private Timer timer;
	public Vector2 Direction = Vector2.Right;

	public override void _Ready()
	{
		timer.Start();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += Direction * Speed * (float)delta;
	}

	private void OnBodyEntered(Node body)
	{
		if(body.GetGroups().Contains("Enemy"))
		{
			Enemy enemy = (Enemy)body;
			enemy.TakeDamage(dmg);
			QueueFree();
		}
	}

	private void Destroy()
	{
		QueueFree();
	}
}
