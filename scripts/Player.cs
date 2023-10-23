using Godot;
using Godot.Collections;
using System;


public partial class Player : CharacterBody2D
{
    [Export]
    public int Speed = 20;

    [Export]
    public TileMap MyTileMap;

    [Export]
    private Timer timer;


    private Vector2 moveDirection;

    private AnimatedSprite2D animatedSprite2D;
    private NavigationAgent2D navigationAgent2D;


    int layer = 4;
    int landLayer = 3;
    Vector2I atlatsCoors = new Vector2I(6, 2);
    private Dictionary<Vector2I, int> placeInfo = new Dictionary<Vector2I, int>();

    public override void _Ready()
    {
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        navigationAgent2D = GetNode<NavigationAgent2D>("NavigationAgent2D");
        navigationAgent2D.SetNavigationMap(MyTileMap.GetNavigationMap(0));
        navigationAgent2D.VelocityComputed += NavigationAgent2D_VelocityComputed;

        timer.Timeout += HandleGrow;
        timer.WaitTime = 3;
        timer.Start();
    }


    private void NavigationAgent2D_VelocityComputed(Vector2 safeVelocity)
    {
        //GD.Print(safeVelocity);
    }

    public override void _PhysicsProcess(double delta)
    {
        //HandleMouseKey();
        HandleInputKey();
        HandlePlace();
        HandleMove();
        HandleAnimation();

    }

    public void HandleGrow()
    {
        foreach (var item in placeInfo)
        {
            if (item.Value < 3)
            {
                MyTileMap.SetCell(layer, item.Key, 1, new Vector2I(atlatsCoors.X+ item.Value, atlatsCoors.Y));
                placeInfo[item.Key]++;
            }
        }
    }

    public void HandlePlace()
    {

        if (Input.IsActionJustPressed("left_mouse"))
        {
            Vector2 mousePos = GetGlobalMousePosition();
            Vector2I mapPos = MyTileMap.LocalToMap(mousePos);

            var tileData = MyTileMap.GetCellTileData(landLayer, mapPos);
            if (tileData != null)
            {
                if (!placeInfo.ContainsKey(mapPos))
                {
                    placeInfo.Add(mapPos, 1);

                    Variant canPlaceSeeds = tileData.GetCustomData("can_place");
                    if (canPlaceSeeds.AsBool())
                    {
                        MyTileMap.SetCell(layer, mapPos, 1, atlatsCoors);
                    }
                    else
                    {
                        GD.Print("can not place");
                    }
                }
                else {
                    GD.Print("have placed");
                }
            }
        }
    }

    public void HandleMouseKey()
    {
        if (Input.IsActionJustPressed("left_mouse"))
        {

            navigationAgent2D.TargetPosition = GetGlobalMousePosition();
            //GD.Print(navigationAgent2D.TargetPosition);
        }

        if (!navigationAgent2D.IsNavigationFinished())
        {
            moveDirection = navigationAgent2D.GetNextPathPosition() - Position;
            //GD.Print(moveDirection);
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
        var dir = moveDirection.Normalized() * Speed;
        //GD.Print(dir);
        //if (!navigationAgent2D.IsTargetReached())
        //{
        //    navigationAgent2D.SetVelocityForced(dir);
        //}

        base.Velocity = dir;
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
