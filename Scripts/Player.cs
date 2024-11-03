using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody2D
{
	// Singleton vars
	public static Player Instance { get; private set; }
	// Other
	[Export] private float Speed = 300.0f;
	[Export] private float FireRate = 0.1f;
	[Export] private PackedScene BulletScene;
	private Timer CD_Shoot;
	private  Godot.Collections.Array<Enemy> enemies = new Godot.Collections.Array<Enemy>();


    public override void _Ready()
    {
		// Create this as a singleton
		Instance = this;
		// Récupère le Timer de la scène
        CD_Shoot = GetNode<Timer>("CD_Shoot");

		CD_Shoot.WaitTime = FireRate;
        CD_Shoot.Start();
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity = direction * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void Shoot()
	{
		Bullet bullet = (Bullet)BulletScene.Instantiate();

		// Positionne le projectile au niveau du personnage
        bullet.Position = GlobalPosition;

        // Définit la direction du projectile
        bullet.Direction = SetTargetDirection(); // Exemple : vers la droite
        
        // Ajoute le projectile à la scène
		GetTree().Root.AddChild(bullet);
	}

	private void OnBodyEntered(Node body)
	{
		if(body.GetGroups().Contains("Enemy") && !enemies.Contains(body))
		{
			enemies.Add((Enemy)body);
		}
	}

	public void RemoveEnemy(Enemy enemy)
	{
		if(enemies.Contains(enemy))
		{
			enemies.Remove(enemy);
		}
	}

	private Vector2 SetTargetDirection()
	{
		if(enemies.Count > 0)
		{
			Enemy target = enemies[0];
			Vector2 direction = target.Position - GlobalPosition;
			return direction.Normalized();
		}

		return new Vector2(1, 0);
	}
}
