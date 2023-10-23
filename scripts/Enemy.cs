using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    public NavigationAgent2D Agent;
    public AnimationPlayer AniPlayer;

    [Export]
    public TileMap Map;
    public override void _Ready()
    {
        AniPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        Agent.SetNavigationMap(Map.GetNavigationMap(0));
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("right_mouse"))
        {
            Agent.TargetPosition = GetGlobalMousePosition();
        }

        if (!Agent.IsNavigationFinished())
        {
            var dir = Agent.GetNextPathPosition() - Position;
            Velocity = dir.Normalized() * 20;
            if (Velocity.X < 0)
            {
                AniPlayer.Play("walk_left");
            }
            else if (Velocity.X > 0)
            {
                AniPlayer.Play("walk_right");
            }
         
        }
        else
        {
            Velocity = Vector2.Zero;
            AniPlayer.Play("idle_right");
            //moveDirection = Vector2.Zero;
        }
        MoveAndSlide();
    }
}
