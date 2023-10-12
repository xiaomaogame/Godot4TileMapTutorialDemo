using Godot;
using System;


public partial class Player : CharacterBody2D
{
    [Export]
    private int speed = 20;
    private Vector2 moveDirection;

    private AnimatedSprite2D animatedSprite2D;

    public override void _Ready()
    {
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }


    public void HandleDirection()
    {
        Vector2 inputDirection = Input.GetVector("my_left","my_right","my_up","my_down");
        moveDirection = inputDirection.Normalized();
    }

    public void HandleMove()
    {
        base.Velocity = moveDirection * speed;
        MoveAndSlide();
    }

    public void HandleAnimation()
    { 
        
    }

    public override void _PhysicsProcess(double delta)
    {
        HandleDirection();
        HandleMove();
    }
}
