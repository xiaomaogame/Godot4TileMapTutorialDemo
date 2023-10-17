using Godot;
using System;


public partial class Player : CharacterBody2D
{
    [Export]
    public int Speed = 20;

    [Export]
    public TileMap MyTileMap;

    private Vector2 moveDirection;

    private AnimatedSprite2D animatedSprite2D;
    private NavigationAgent2D navigationAgent2D;

    public override void _Ready()
    {

        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        navigationAgent2D = GetNode<NavigationAgent2D>("NavigationAgent2D");
        navigationAgent2D.SetNavigationMap(MyTileMap.GetNavigationMap(0));
    }


    public override void _PhysicsProcess(double delta)
    {
        HandleMouseKey();
        //HandleInputKey();
        HandleMove();
        HandleAnimation();
    }

    public void HandleMouseKey()
    {
        if (Input.IsActionJustPressed("left_mouse"))
        {
      
            navigationAgent2D.TargetPosition = GetGlobalMousePosition();
            GD.Print(navigationAgent2D.TargetPosition);
        }

        if (!navigationAgent2D.IsNavigationFinished())
        {
            moveDirection = navigationAgent2D.GetNextPathPosition() - Position;
            GD.Print(moveDirection);
        }
        else
        {
            Velocity = Vector2.Zero;
            moveDirection = Vector2.Zero;
        }

    }

    public void HandleInputKey()
    {
        Vector2 inputDirection = Input.GetVector("my_left", "my_right", "my_up", "my_down");
        moveDirection = inputDirection;
    }

    public void HandleMove()
    {
        base.Velocity = moveDirection.Normalized() * Speed;
     
        MoveAndSlide();
    }

    public void HandleAnimation()
    {
        if (moveDirection.Length() == 0)
        {
            animatedSprite2D.Stop();
            return;
        }

        if (Mathf.Abs(moveDirection.X) > Mathf.Abs(moveDirection.Y))
        {
            moveDirection.Y = 0;
        }
        else
        {
            moveDirection.X = 0;
        }

        if (moveDirection.Y != 0)
        {
            if (moveDirection.Y < 0)
                animatedSprite2D.Play("walkUp");
            if (moveDirection.Y > 0)
                animatedSprite2D.Play("walkDown");
        }
        else
        {
            if (moveDirection.X < 0)
                animatedSprite2D.Play("walkLeft");
            if (moveDirection.X > 0)
                animatedSprite2D.Play("walkRight");
        }


    }
}
