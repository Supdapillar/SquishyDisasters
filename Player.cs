using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody2D
{
    [Export]
    public Vector2 Delta = new Vector2();
    [Export]
    public float Gravity = 200f;
    public bool FacingLeft = false;

    [Signal]
    public delegate void WasHit();

    public int playerId = 0;
    public float health = 100;

    public float IFrames = 0;
    public bool IsParachuting = false;

    //Powerup stats
    public float Speed = 100f;
    public float MaxHealth = 100;
    public float JumpBoost = 1f;
    public bool IsShielded = false;
    public int Coins = 0;



    public List<Vector2> accessory_offsets = new List<Vector2>()
    {
        new Vector2(0,-4),
        new Vector2(0,-4),
        new Vector2(0,-6),
        new Vector2(0,-3),
        new Vector2(0,1),
        new Vector2(0,0),
        new Vector2(0,0),
    };




    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ProgressBar healthBar = GetNode<ProgressBar>("PlayerStats/HealthBar");
        Label hpLabel = healthBar.GetNode<Label>("HP Label");

        hpLabel.Text = "P" + (playerId + 1) + "-HP";

        int devidedScreen = 1920 / (Global.PlayerInputs.Count + 1);
        healthBar.RectPosition = new Vector2((devidedScreen) + (devidedScreen) * playerId, 1020);


    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float dt)
    {
        Position += (Delta * dt);
        Delta.y += Gravity * dt;


        //Parachuting
        if (GetNode<AnimatedSprite>("Parachute").Frame == 3)
            GetNode<AnimatedSprite>("Parachute").Visible = false;
        if (IsParachuting)
        {
            GetNode<AnimatedSprite>("Parachute").Visible = true;
            Delta.y = 40;
            if (IsOnFloor() || Input.IsActionPressed("move_down_" + playerId))
            {
                IsParachuting = false;
                GetNode<AnimatedSprite>("Parachute").Playing = true;
            }
        }


        //IFrames
        IFrames -= dt;
        if (IFrames > 0)
        {
            //Modulate = new Color(1f, 1f, 1f, Math.Max(0.5f + (1f - IFrames) / 2, 0.5f));
            Modulate = new Color(1f, 1f, 1f, 0.5f);

        }
        else
        {
            Modulate = new Color(1f, 1f, 1f, 1f);

        }

        //Update Health Bar
        ProgressBar healthBar = GetNode<ProgressBar>("PlayerStats/HealthBar");
        healthBar.Value = (int)health;
        healthBar.GetNode<Label>("Health Percent").Text = Math.Ceiling(health).ToString() + "%";


        health += 2 * dt;
        if (health > MaxHealth)
        {
            health = MaxHealth;
        }

        Label coinLabel = healthBar.GetNode<Label>("Coins");
        //Coins
        if (Coins == 0)
        {
            coinLabel.Visible = false;
        }
        else
        {
            coinLabel.Visible = true;
        }

        //Cosmetics
        AnimatedSprite Aspr = GetNode<AnimatedSprite>("AnimatedSprite");
        if (Global.PlayerColors[playerId] != null)
        {
            Aspr.Modulate = Global.PlayerColors[playerId];
        }
        else
        {
            Aspr.Modulate = new Color(1, 1, 1, 1);
        }

        AnimatedSprite Cosmetic = GetNode<AnimatedSprite>("Cosmetics");
        if (Global.Accessories[playerId] != 0)
        {
            Cosmetic.Show();
        }
        else
        {
            Cosmetic.Hide();

        }

        switch (Global.Accessories[playerId])
        {
            case 0:
                Cosmetic.Hide();
                break;
            case 1:
                Cosmetic.Animation = "CatEars";
                break;
            case 2:
                Cosmetic.Animation = "ChrismasHat";
                break;
            case 3:
                Cosmetic.Animation = "Glasses";
                break;
            case 4:
                Cosmetic.Animation = "Mustash";
                break;
            case 5:
                Cosmetic.Animation = "Monical";
                break;
            case 6:
                Cosmetic.Animation = "ClownNose";
                break;
        }
        //Controls
        Delta.x = Input.GetAxis(("move_left_" + playerId), ("move_right_" + playerId)) * Speed;
        if (IsOnFloor())
        {
            Delta.y = 0;
            if (Input.IsActionPressed("jump_" + playerId))
            {
                Delta.y = -170 * JumpBoost;
            }
        }
        if (Input.IsActionPressed("move_down_" + playerId) || Delta.y < 0)
        {
            SetCollisionMaskBit(4, false);
        }
        else
        {
            SetCollisionMaskBit(4, true);
        }





        //Change which way the player is facing
        Sprite spr = GetNode<Sprite>("Sprite");
        if (Delta.x < 0)
        {
            Aspr.Animation = "Moving";
            FacingLeft = true;
            spr.Position = new Vector2(0, 1);
            spr.FlipH = false;
            Cosmetic.Position = new Vector2(accessory_offsets[Global.Accessories[playerId]].x, accessory_offsets[Global.Accessories[playerId]].y + 1);
            Cosmetic.FlipH = true;
        }
        else if (Delta.x > 0)
        {
            Aspr.Animation = "Moving";
            FacingLeft = false;
            spr.Position = new Vector2(0, 1);
            spr.FlipH = true;
            Cosmetic.Position = new Vector2(accessory_offsets[Global.Accessories[playerId]].x, accessory_offsets[Global.Accessories[playerId]].y + 1);
            Cosmetic.FlipH = false;
        }
        else
        {
            if (FacingLeft)
            {
                Aspr.Animation = "Standing";
                spr.Position = new Vector2(0, 0);
                spr.FlipH = false;
                Cosmetic.Position = accessory_offsets[Global.Accessories[playerId]];
                Cosmetic.FlipH = true;
            }
            else
            {
                Aspr.Animation = "Standing";
                spr.Position = new Vector2(0, 0);
                spr.FlipH = true;
                Cosmetic.Position = accessory_offsets[Global.Accessories[playerId]];
                Cosmetic.FlipH = false;
            }
        }
        MoveAndSlide(Delta, new Vector2(0, -1));
    }

    public override void _PhysicsProcess(float dt)
    {
        if (IFrames < 0f)
        {
            if (Position.y > 1104)
            {
                Delta.y = -Math.Abs(Delta.y);
                DamageSquishy(50f, 1f);
            }
        }
    }
    //// Signals ////

    public void _on_Area2D_area_entered(Area2D area2d)
    {
        if (area2d.GetParent() is Powerup powerup)
        {
            PowerupCollision(powerup);
        }
        else if (area2d.GetParent().GetParent() is Rain rain)
        {
            DamageSquishy(50f, 0.5f);
        }
        else if (area2d.GetParent() is Anvil anvil)
        {
            if (anvil.delta.y > 3 && anvil.Position.y < Position.y)
            {
                DamageSquishy(75f, 1f);
                anvil.hasHitPlayer = true;
                anvil.GetNode<Timer>("DeathTimer").Start();

                anvil.GetNode<KinematicBody2D>("PlatformShape").GetNode<CollisionShape2D>("CollisionShape2D").QueueFree();
                anvil.GetNode<Area2D>("Area2D").GetNode<CollisionShape2D>("CollisionShape2D").QueueFree();
                anvil.GetNode<CollisionShape2D>("CollisionShape2D").QueueFree();
            }
        }
        else if (area2d.GetParent() is Coin coin)
        {
            ProgressBar healthBar = GetNode<ProgressBar>("PlayerStats/HealthBar");
            Label coinLabel = healthBar.GetNode<Label>("Coins");

            Coins++;

            coinLabel.Text = "+" + Coins.ToString() + " COINS";
            coin.PlayerCollect();

        }
        else if (area2d.GetParent() is Drone drone)
        {
            drone.delta.x = -drone.delta.x;
            drone.delta.y = -drone.delta.y;
            drone.GetNode<CPUParticles2D>("CPUParticles2D").Emitting = true;
            DamageSquishy(40f, 1f);
        }
        else if (area2d.GetParent() is DonutChunk donutChunk)
        {
            donutChunk.hitPlayer();
            DamageSquishy(40f, 0.5f);
        }
        else if (area2d.GetParent() is Donut donut)
        {
            donut.hitGround(false);
            DamageSquishy(80f, 1.5f);
        }
    }

    //All the collision functions
    private void PowerupCollision(Powerup powerup)
    {
        ProgressBar healthBar = GetNode<ProgressBar>("PlayerStats/HealthBar");

        Label speedLabel = healthBar.GetNode<Label>("Speed");
        Label jumpLabel = healthBar.GetNode<Label>("Jump");
        Label healthLabel = healthBar.GetNode<Label>("Health");


        switch (powerup.PowerupType)
        {
            case "Speed":
                Speed += 20;
                speedLabel.Text = "+" + Math.Floor((Speed - 100) / 20).ToString();
                break;
            case "Jump":
                JumpBoost += 0.2f;
                jumpLabel.Text = "+" + Math.Floor((JumpBoost - 1) * 5).ToString();

                break;
            case "Health":
                MaxHealth += 20;
                healthLabel.Text = "+" + Math.Floor((MaxHealth - 100) / 20).ToString();
                break;
            case "Shield":
                IsShielded = true;
                GetNode<Sprite>("Shield").Visible = true;
                break;
        }
        powerup.QueueFree();
    }

    private void DamageSquishy(float hp, float iframe)
    {
        if (IsShielded)
        {
            IsShielded = false;
            GetNode<Sprite>("Shield").Visible = false;
            GetNode<CPUParticles2D>("ShieldParticles").Emitting = true;
        }
        else
        {
            if (IFrames > 0)
                return;
            health -= hp;
            IFrames = iframe;
        }





    }


}
