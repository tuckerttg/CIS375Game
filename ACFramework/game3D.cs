using System;
using System.Drawing;
using System.Windows.Forms;


//ahhhhhhhhhhhhhhhhhhhhhhh test
// mod: setRoom1 doesn't repeat over and over again
//created
// oh hai there jess
//hey there it's jess
namespace ACFramework
{ 
	
	class cCritterDoor : cCritterWall 
	{
        private bool hitDoor;

        public bool HitDoor
        {
            get { return hitDoor; }
            set { hitDoor = value; }
        }

	    public cCritterDoor(cVector3 enda, cVector3 endb, float thickness, float height, cGame pownergame ) 
		    : base( enda, endb, thickness, height, pownergame ) 
	    {
            HitDoor = false;
	    }
		
		public override bool collide( cCritter pcritter ) 
		{ 
			bool collided = base.collide( pcritter ); 
			if ( collided && pcritter.IsKindOf( "cCritter3DPlayer" ) ) 
			{ 
				(( cGame3D ) Game ).setdoorcollision( );
                hitDoor = true;
				return true; 
			} 
			return false; 
		}
 
        public override bool IsKindOf( string str )
        {
            return str == "cCritterDoor" || base.IsKindOf( str );
        }

        public override string RuntimeClass
        {
            get
            {
                return "cCritterDoor";
            }
        }
	} 
	
	//==============Critters for the cGame3D: Player, Ball, Treasure ================ 
	
	class cCritter3DPlayer : cCritterArmedPlayer 
	{ 
        private bool warningGiven = false;
		
        public cCritter3DPlayer( cGame pownergame ) 
            : base( pownergame ) 
		{
            BulletClass = new cCritterBulletHomework();
            Sprite = new cSpriteQuake(ModelsMD2.Sorcerer);
			Sprite.SpriteAttitude = cMatrix3.scale( 2, 0.8f, 0.4f ); 
			setRadius( cGame3D.PLAYERRADIUS ); //Default cCritter.PLAYERRADIUS is 0.4.  
			setHealth( 10 );
            moveTo(new cVector3(20, 20, 20));
			//moveTo( _movebox.LoCorner.add( new cVector3( 200.0f, 0.0f, 2.0f ))); 
			WrapFlag = cCritter.CLAMP; //Use CLAMP so you stop dead at edges.
			Armed = true; //Let's use bullets.
			MaxSpeed =  cGame3D.MAXPLAYERSPEED; 
			AbsorberFlag = true; //Keeps player from being buffeted about.
			ListenerAcceleration = 160.0f; //So Hopper can overcome gravity.  Only affects hop.


            Listener = new cMyGameListener(0.2f, 12.0f); 
            // the two arguments are walkspeed and hop strength -- JC
            
            addForce( new cForceGravity( 50.0f )); /* Uses  gravity. Default strength is 25.0.
			Gravity	will affect player using cListenerHopper. */ 
			AttitudeToMotionLock = false; //It looks nicer is you don't turn the player with motion.
			Attitude = new cMatrix3( new cVector3(0.0f, 0.0f, -1.0f), new cVector3( -1.0f, 0.0f, 0.0f ), 
                new cVector3( 0.0f, 1.0f, 0.0f ), Position); 
		}

        public override void update(ACView pactiveview, float dt)
        {
            base.update(pactiveview, dt); //Always call this first
            
        } 

        public override bool collide( cCritter pcritter ) 
		{ 
			bool playerhigherthancritter = Position.Y - Radius > pcritter.Position.Y; 
		/* If you are "higher" than the pcritter, as in jumping on it, you get a point
	and the critter dies.  If you are lower than it, you lose health and the
	critter also dies. To be higher, let's say your low point has to higher
	than the critter's center. We compute playerhigherthancritter before the collide,
	as collide can change the positions. */
            _baseAccessControl = 1;
			bool collided = base.collide( pcritter );
            _baseAccessControl = 0;
            if (!collided) 
				return false;
		/* If you're here, you collided.  We'll treat all the guys the same -- the collision
	 with a Treasure is different, but we let the Treasure contol that collision. */ 
			if ( playerhigherthancritter ) 
			{
                //Framework.snd.play(Sound.Goopy); 
				addScore( 10 ); 
			} 
			else 
			{ 
				damage( 1 );
                //Framework.snd.play(Sound.Tahn); 
			} 
			//pcritter.die(); 
			return true; 
		}

        public override cCritterBullet shoot()
        {
            Framework.snd.play(Sound.Ahhh);
            return base.shoot();
        }

        public override bool IsKindOf( string str )
        {
            return str == "cCritter3DPlayer" || base.IsKindOf( str );
        }
		
        public override string RuntimeClass
        {
            get
            {
                return "cCritter3DPlayer";
            }
        }
	} 
	
   
	class cCritter3DPlayerBullet : cCritterBullet 
	{

        public cCritter3DPlayerBullet() { }

        public override cCritterBullet Create()
            // has to be a Create function for every type of bullet -- JC
        {
            return new cCritter3DPlayerBullet();
        }
		
		public override void initialize( cCritterArmed pshooter ) 
		{ 
			base.initialize( pshooter );
            Sprite.FillColor = Color.Crimson;
            // can use setSprite here too
            setRadius(0.1f);
		} 

        public override bool IsKindOf( string str )
        {
            return str == "cCritter3DPlayerBullet" || base.IsKindOf( str );
        }
		
        public override string RuntimeClass
        {
            get
            {
                return "cCritter3DPlayerBullet";
            }
        }
	}

    class cCritterBulletHomework : cCritterBullet
    {

        public cCritterBulletHomework() { }

        public override cCritterBullet Create()
        // has to be a Create function for every type of bullet -- JC
        {
            return new cCritterBulletHomework();
        }

        public override void initialize(cCritterArmed pshooter)
        {
            base.initialize(pshooter);

            Sprite = new cSpriteQuake(ModelsMD2.Sorcerer);
            Sprite.SpriteAttitude = cMatrix3.scale(2, 0.8f, 0.4f); 

            // can use setSprite here too
            setRadius(0.1f);
            
        }

        public override bool IsKindOf(string str)
        {
            return str == "cCritter3DPlayerBullet" || base.IsKindOf(str);
        }

        public override string RuntimeClass
        {
            get
            {
                return "cCritter3DPlayerBullet";
            }
        }
    } 
	

	class cCritter3Dcharacter : cCritter  
	{

        public cCritter3Dcharacter( cGame pownergame ) 
            : base( pownergame ) 
		{
            _health = 2;

            addForce(new cForceObjectSeek(Player, 15.5f));
			addForce( new cForceGravity( 25.0f, new cVector3( 0.0f, -1, 0.00f ))); 
			addForce( new cForceDrag( 2.0f ) );  // default friction strength 0.5 
			Density = 2.0f; 
			MaxSpeed = 30.0f;
            if (pownergame != null) //Just to be safe.
                Sprite = new cSpriteQuake(Framework.models.selectRandomCritter());
            
            // example of setting a specific model
            // setSprite(new cSpriteQuake(ModelsMD2.Knight));
            
            if ( Sprite.IsKindOf( "cSpriteQuake" )) //Don't let the figurines tumble.  
			{ 
				AttitudeToMotionLock = false;   
				Attitude = new cMatrix3( new cVector3( 0.0f, 0.0f, 1.0f ), 
                    new cVector3( 1.0f, 0.0f, 0.0f ), 
                    new cVector3( 0.0f, 1.0f, 0.0f ), Position); 
				/* Orient them so they are facing towards positive Z with heads towards Y. */ 
			} 
			Bounciness = 0.0f; //Not 1.0 means it loses a bit of energy with each bounce.
			setRadius( 1.0f );
            MinTwitchThresholdSpeed = 4.0f; //Means sprite doesn't switch direction unless it's moving fast 
			randomizePosition( new cRealBox3( new cVector3( _movebox.Lox, _movebox.Loy, _movebox.Loz + 4.0f), 
				new cVector3( _movebox.Hix, _movebox.Loy, _movebox.Midz - 1.0f))); 
				/* I put them ahead of the player  */ 
			randomizeVelocity( 0.0f, 30.0f, false ); 

                        
			if ( pownergame != null ) //Then we know we added this to a game so pplayer() is valid 
				addForce( new cForceObjectSeek( Player, 0.5f ));

            int begf = Framework.randomOb.random(0, 171);
            int endf = Framework.randomOb.random(0, 171);

            if (begf > endf)
            {
                int temp = begf;
                begf = endf;
                endf = temp;
            }

			Sprite.setstate( State.Other, begf, endf, StateType.Repeat );


            _wrapflag = cCritter.BOUNCE;

		} 

		
		public override void update( ACView pactiveview, float dt ) 
		{ 
			base.update( pactiveview, dt ); //Always call this first
			if ( (_outcode & cRealBox3.BOX_HIZ) != 0 ) /* use bitwise AND to check if a flag is set. */ 
				delete_me(); //tell the game to remove yourself if you fall up to the hiz.
        } 

		// do a delete_me if you hit the left end 
	
		public override void die() 
		{ 
			Player.addScore( Value ); 
			base.die(); 
		} 

       public override bool IsKindOf( string str )
        {
            return str == "cCritter3Dcharacter" || base.IsKindOf( str );
        }
	
        public override string RuntimeClass
        {
            get
            {
                return "cCritter3Dcharacter";
            }
        }
	} 
	
	class cCritterTreasure : cCritter 
	{   // Try jumping through this hoop
	
		public cCritterTreasure( cGame pownergame ) : 
		base( pownergame ) 
		{ 
			/* The sprites look nice from afar, but bitmap speed is really slow
		when you get close to them, so don't use this. */ 
			cPolygon ppoly = new cPolygon( 24 ); 
			ppoly.Filled = false; 
			ppoly.LineWidthWeight = 0.5f;
			Sprite = ppoly; 
			_collidepriority = cCollider.CP_PLAYER + 1; /* Let this guy call collide on the
			player, as his method is overloaded in a special way. */ 
			rotate( new cSpin( (float) Math.PI / 2.0f, new cVector3(0.0f, 0.0f, 1.0f) )); /* Trial and error shows this
			rotation works to make it face the z diretion. */ 
			setRadius( cGame3D.TREASURERADIUS ); 
			FixedFlag = true; 
			moveTo( new cVector3( _movebox.Midx, _movebox.Midy - 2.0f, 
				_movebox.Loz - 1.5f * cGame3D.TREASURERADIUS )); 
		} 

		
		public override bool collide( cCritter pcritter ) 
		{ 
			if ( contains( pcritter )) //disk of pcritter is wholly inside my disk 
			{
                //Framework.snd.play(Sound.Clap); 
				pcritter.addScore( 100 ); 
				pcritter.addHealth( 1 ); 
				pcritter.moveTo( new cVector3( _movebox.Midx, _movebox.Loy + 1.0f,
                    _movebox.Hiz - 3.0f )); 
				return true; 
			} 
			else 
				return false; 
		} 

		//Checks if pcritter inside.
	
		public override int collidesWith( cCritter pothercritter ) 
		{ 
			if ( pothercritter.IsKindOf( "cCritter3DPlayer" )) 
				return cCollider.COLLIDEASCALLER; 
			else 
				return cCollider.DONTCOLLIDE; 
		} 

		/* Only collide
			with cCritter3DPlayer. */ 

       public override bool IsKindOf( string str )
        {
            return str == "cCritterTreasure" || base.IsKindOf( str );
        }
	
        public override string RuntimeClass
        {
            get
            {
                return "cCritterTreasure";
            }
        }
	} 
	
	//======================cGame3D========================== 
	
	class cGame3D : cGame 
	{ 
		public static readonly float TREASURERADIUS = 1.2f; 
		public static readonly float WALLTHICKNESS = 0.5f; 
		public static readonly float PLAYERRADIUS = 0.2f; 
		public static readonly float MAXPLAYERSPEED = 30.0f; 
		private cCritterTreasure _ptreasure; 
		private bool doorcollision;
        private bool wentThrough = false;
        private float startNewRoom;

        private bool onRoom2;
        private bool onRoom3;

        private cCritterDoor door0;
        private cCritterDoor door2;
        private cCritterDoor door3;

        private cCritterWall movingWall;

        private cCritter3Dcharacter sailorMoon;
        private cCritter3Dcharacter penguin;
		public cGame3D() 
		{
            
			doorcollision = false;
            onRoom3 = false;
            onRoom2 = false;
			_menuflags &= ~ cGame.MENU_BOUNCEWRAP; 
			_menuflags |= cGame.MENU_HOPPER; //Turn on hopper listener option.
			_spritetype = cGame.ST_MESHSKIN; 
			setBorder( 64.0f, 16.0f, 64.0f ); // size of the world
		
			cRealBox3 skeleton = new cRealBox3();
            skeleton.copy(_border);
			setSkyBox( skeleton );
		/* In this world the coordinates are screwed up to match the screwed up
		listener that I use.  I should fix the listener and the coords.
		Meanwhile...
		I am flying into the screen from HIZ towards LOZ, and
		LOX below and HIX above and
		LOY on the right and HIY on the left. */ 
			SkyBox.setSideSolidColor( cRealBox3.HIZ, Color.Black ); //Make the near HIZ transparent 
			SkyBox.setSideSolidColor( cRealBox3.LOZ, Color.Black ); //Far wall 
			SkyBox.setSideSolidColor( cRealBox3.LOX, Color.Black ); //left wall 
            SkyBox.setSideSolidColor( cRealBox3.HIX, Color.Black ); //right wall 
			SkyBox.setSideSolidColor( cRealBox3.LOY, Color.Black ); //floor 
			SkyBox.setSideSolidColor( cRealBox3.HIY, Color.Black ); //ceiling 
		
			WrapFlag = cCritter.BOUNCE; 
			_seedcount = 7; 
			setPlayer( new cCritter3DPlayer( this ));
           
			_ptreasure = new cCritterTreasure( this ); 


			/* In this world the x and y go left and up respectively, while z comes out of the screen.
		A wall views its "thickness" as in the y direction, which is up here, and its
		"height" as in the z direction, which is into the screen. */ 
			//First draw a wall with dy height resting on the bottom of the world.
			float zpos = 0.0f; /* Point on the z axis where we set down the wall.  0 would be center,
			halfway down the hall, but we can offset it if we like. */ 
			float height = 0.1f * _border.YSize; 
			float ycenter = -_border.YRadius + height / 2.0f; 
			float wallthickness = cGame3D.WALLTHICKNESS;
            cCritterWall pwall = new cCritterWall( 
				new cVector3( _border.Midx + 2.0f, ycenter, zpos ), 
				new cVector3( _border.Hix, ycenter, zpos ), 
				height, //thickness param for wall's dy which goes perpendicular to the 
					//baseline established by the frist two args, up the screen 
				wallthickness, //height argument for this wall's dz  goes into the screen 
				this ); 

            // fill the wall with textures 
			cSpriteTextureBox pspritebox = 
				new cSpriteTextureBox( pwall.Skeleton, BitmapRes.lightning, 16 ); //Sets all sides 
				/* We'll tile our sprites three times along the long sides, and on the
			short ends, we'll only tile them once, so we reset these two. */
          pwall.Sprite = pspritebox;

          
		
			//Then draw a ramp to the top of the wall.  Scoot it over against the right wall.
			float planckwidth = 0.75f * height; 
			pwall = new cCritterWall( 
				new cVector3( _border.Hix -planckwidth / 2.0f, _border.Loy, _border.Hiz - 2.0f), 
				new cVector3( _border.Hix - planckwidth / 2.0f, _border.Loy + height, zpos ), 
				planckwidth, //thickness param for wall's dy which is perpenedicualr to the baseline, 
						//which goes into the screen, so thickness goes to the right 
				wallthickness, //_border.zradius(),  //height argument for wall's dz which goes into the screen 
				this );

           Player.moveTo(new cVector3(_border.Midx, _border.Loy + 5, _border.Midz + 10)); 



            cCritterWall pPillar = new cCritterWall(
            new cVector3(_border.Midx +2 , _border.Loy, 0),
            new cVector3(_border.Midx  - 2 , _border.Loy, 0),
            2,
            2,
            this);

           /* pPillar.rotate(new cSpin(((float) Math.PI)/6.0f, // rotate in rads
            new cVector3(0.0f, 1.0f , 0.0f))); // what axis to rotate 
            */
            pPillar.FillColor = Color.Blue;
            
            cSpriteTextureBox pspriteboxPillar =
        new cSpriteTextureBox(pPillar.Skeleton, BitmapRes.Wall3, 16); //Sets all sides 
            pPillar.Sprite = pspriteboxPillar;
            /* We'll tile our sprites three times along the long sides, and on the
        short ends, we'll only tile them once, so we reset these two. */


            
           

            cSpriteTextureBox stb = new cSpriteTextureBox(pwall.Skeleton, 
                BitmapRes.Wood2, 2 );
            pwall.Sprite = stb;
		
			door0 = new cCritterDoor( 
				new cVector3( _border.Lox, _border.Loy, _border.Midz ), 
				new cVector3( _border.Lox, _border.Midy - 3, _border.Midz ), 
				0.1f, 2, this ); 
			cSpriteTextureBox pspritedoor = 
				new cSpriteTextureBox( door0.Skeleton, BitmapRes.Door ); 
			door0.Sprite = pspritedoor; 
		}

        
        public void setRoom1( )
        {
            
<<<<<<< HEAD
=======

            Player.moveTo(new cVector3(0.0f, Border.Loy, Border.Hiz - 3.0f)); 
>>>>>>> 842158de08565d77fd8165d34c279e9b7870d5f1
            onRoom2 = true;
            Biota.purgeCritters("cCritterWall");
            Biota.purgeCritters("cCritter3Dcharacter");
            setBorder(10.0f, 15.0f, 10.0f); 
	        cRealBox3 skeleton = new cRealBox3();
            skeleton.copy( _border );
	        setSkyBox(skeleton);
            SkyBox.setAllSidesTexture(BitmapRes.mySprite, 2);
	        SkyBox.setSideTexture( cRealBox3.LOY, BitmapRes.mySprite );
	        SkyBox.setSideSolidColor( cRealBox3.HIY, Color.Blue );
	        _seedcount = 0;
	        Player.setMoveBox( new cRealBox3( 10.0f, 15.0f, 10.0f ) );/////////////////////////////////////////////////////////////
            float zpos = 0.0f; /* Point on the z axis where we set down the wall.  0 would be center,
			halfway down the hall, but we can offset it if we like. */
            float height = 0.1f * _border.YSize;
            float ycenter = -_border.YRadius + height / 2.0f;
            float wallthickness = cGame3D.WALLTHICKNESS;

            
            
            door2 = new cCritterDoor(
                new cVector3(_border.Lox, _border.Loy, _border.Midz),
                new cVector3(_border.Lox, _border.Midy - 3, _border.Midz),
                0.1f, 2, this); 
            cSpriteTextureBox pspritedoor =
                new cSpriteTextureBox(door2.Skeleton, BitmapRes.Door);
            door2.Sprite = pspritedoor;
            door2.HitDoor = false;

            penguin = new cCritter3Dcharacter(this);
            penguin.setHealth(10);
            penguin.Sprite = new cSpriteQuake(ModelsMD2.Penguin);
            penguin.addForce(new cForceObjectSeek(Player, 10.5f));


            /* We'll tile our sprites three times along the long sides, and on the
        short ends, we'll only tile them once, so we reset these two. */
            wentThrough = true;
            startNewRoom = Age;

        }

        public void setRoom2()
        {
            
            Player.moveTo(new cVector3(0.0f, Border.Loy, Border.Hiz - 3.0f));
            Biota.purgeCritters("cCritterWall");
            Biota.purgeCritters("cCritter3Dcharacter");
            setBorder(20.0f, 30.0f, 30.0f);
            cRealBox3 skeleton = new cRealBox3();
            skeleton.copy(_border);
            setSkyBox(skeleton);
            SkyBox.setAllSidesTexture(BitmapRes.purpleYellowDots, 2);
            SkyBox.setSideTexture(cRealBox3.LOY, BitmapRes.Concrete);
            SkyBox.setSideSolidColor(cRealBox3.HIY, Color.Blue);
            _seedcount = 0;
            Player.setMoveBox(new cRealBox3(20.0f, 30.0f, 30.0f));
            float zpos = 0.0f; /* Point on the z axis where we set down the wall.  0 would be center,
			halfway down the hall, but we can offset it if we like. */
            float height = 0.1f * _border.YSize;
            float ycenter = -_border.YRadius + height / 2.0f;
            float wallthickness = cGame3D.WALLTHICKNESS;
            movingWall = new cCritterWall(
                new cVector3(_border.Midx + 2.0f, ycenter, zpos),
                new cVector3(_border.Hix, ycenter, zpos),
                height, //thickness param for wall's dy which goes perpendicular to the 
                //baseline established by the frist two args, up the screen 
                wallthickness, //height argument for this wall's dz  goes into the screen 
                this);
            cSpriteTextureBox pspritebox =
                new cSpriteTextureBox(movingWall.Skeleton, BitmapRes.Wall3, 16); //Sets all sides 
            /* We'll tile our sprites three times along the long sides, and on the
        short ends, we'll only tile them once, so we reset these two. */
            movingWall.Sprite = pspritebox;
            movingWall.addForce(new cForceObjectSeek(Player, 2.0f));

            sailorMoon = new cCritter3Dcharacter(this);
            sailorMoon.Sprite = new cSpriteQuake(ModelsMD2.SailorMoon);
            sailorMoon.setHealth(10);
            sailorMoon.addForce(new cForceObjectSeek(Player, 10.5f));

            wentThrough = true;
            startNewRoom = Age;

            door3 = new cCritterDoor(
                new cVector3(_border.Lox, _border.Loy, _border.Midz),
                new cVector3(_border.Lox, _border.Midy - 3, _border.Midz),
                0.1f, 2, this);
            cSpriteTextureBox pspritedoor =
                new cSpriteTextureBox(door3.Skeleton, BitmapRes.Door);
            door3.Sprite = pspritedoor;
        }

        public void setRoom3()
        {
            Player.moveTo(new cVector3(0.0f, Border.Loy, Border.Hiz - 3.0f));
            Biota.purgeCritters("cCritterWall");
            Biota.purgeCritters("cCritter3Dcharacter");
            setBorder(10.0f, 15.0f, 10.0f);
            cRealBox3 skeleton = new cRealBox3();
            skeleton.copy(_border);
            setSkyBox(skeleton);
            SkyBox.setAllSidesTexture(BitmapRes.Graphics1, 2);
            SkyBox.setSideTexture(cRealBox3.LOY, BitmapRes.Concrete);
            SkyBox.setSideSolidColor(cRealBox3.HIY, Color.Blue);
            _seedcount = 0;
            Player.setMoveBox(new cRealBox3(10.0f, 15.0f, 10.0f));
            float zpos = 0.0f; /* Point on the z axis where we set down the wall.  0 would be center,
			halfway down the hall, but we can offset it if we like. */
            float height = 0.1f * _border.YSize;
            float ycenter = -_border.YRadius + height / 2.0f;
            float wallthickness = cGame3D.WALLTHICKNESS;
            cCritterWall pwall = new cCritterWall(
                new cVector3(_border.Midx + 2.0f, ycenter, zpos),
                new cVector3(_border.Hix, ycenter, zpos),
                height, //thickness param for wall's dy which goes perpendicular to the 
                //baseline established by the frist two args, up the screen 
                wallthickness, //height argument for this wall's dz  goes into the screen 
                this);
            cSpriteTextureBox pspritebox =
                new cSpriteTextureBox(pwall.Skeleton, BitmapRes.Wall3, 16); //Sets all sides 
            /* We'll tile our sprites three times along the long sides, and on the
        short ends, we'll only tile them once, so we reset these two. */
            pwall.Sprite = pspritebox;
            wentThrough = true;
            startNewRoom = Age;


            door2 = new cCritterDoor(
                new cVector3(_border.Lox, _border.Loy, _border.Midz),
                new cVector3(_border.Lox, _border.Midy - 3, _border.Midz),
                0.1f, 2, this);
            cSpriteTextureBox pspritedoor =
                new cSpriteTextureBox(door2.Skeleton, BitmapRes.Door);
            door2.Sprite = pspritedoor;
        }
		
		public override void seedCritters() 
		{
			Biota.purgeCritters( "cCritterBullet" ); 
			Biota.purgeCritters( "cCritter3Dcharacter" );
            for (int i = 0; i < _seedcount; i++) 
				new cCritter3Dcharacter( this );
<<<<<<< HEAD
           
=======
            //Player.moveTo(new cVector3(0.0f, Border.Loy, Border.Hiz - 3.0f)); 
>>>>>>> 842158de08565d77fd8165d34c279e9b7870d5f1
				/* We start at hiz and move towards	loz */ 
		} 

		
		public void setdoorcollision( ) { doorcollision = true; } 
		
		public override ACView View 
		{
            set
            {
                base.View = value; //You MUST call the base class method here.
                value.setUseBackground(ACView.FULL_BACKGROUND); /* The background type can be
			    ACView.NO_BACKGROUND, ACView.SIMPLIFIED_BACKGROUND, or 
			    ACView.FULL_BACKGROUND, which often means: nothing, lines, or
			    planes&bitmaps, depending on how the skybox is defined. */
                value.pviewpointcritter().Listener = new cListenerViewerRide();
            }
		} 

		
		public override cCritterViewer Viewpoint 
		{ 
            set
            {
			    if ( value.Listener.RuntimeClass == "cListenerViewerRide" ) 
			    { 
				    value.setViewpoint( new cVector3( 0.0f, 0.3f, -1.0f ), _border.Center); 
					//Always make some setViewpoint call simply to put in a default zoom.
				    value.zoom( 0.35f ); //Wideangle 
				    cListenerViewerRide prider = ( cListenerViewerRide )( value.Listener); 
				    prider.Offset = (new cVector3( -1.5f, 0.0f, 1.0f)); /* This offset is in the coordinate
				    system of the player, where the negative X axis is the negative of the
				    player's tangent direction, which means stand right behind the player. */ 
			    } 
			    else //Not riding the player.
			    { 
				    value.zoom( 1.0f ); 
				    /* The two args to setViewpoint are (directiontoviewer, lookatpoint).
				    Note that directiontoviewer points FROM the origin TOWARDS the viewer. */ 
				    value.setViewpoint( new cVector3( 0.0f, 0.3f, 1.0f ), _border.Center); 
			    }
            }
		} 

		/* Move over to be above the
			lower left corner where the player is.  In 3D, use a low viewpoint low looking up. */ 
	
		public override void adjustGameParameters() 
		{
		// (1) End the game if the player is dead 
			if ( (Health == 0) && !_gameover ) //Player's been killed and game's not over.
			{ 
				_gameover = true; 
				Player.addScore( _scorecorrection ); // So user can reach _maxscore  
                Framework.snd.play(Sound.IWut);
                return ; 
			} 

            if (sailorMoon != null && sailorMoon.Health==0)
            {
                MessageBox.Show("You defeated all evil!!!");
                Framework.snd.play(Sound.IWut);
                _gameover = true;

            }
		// (2) Also don't let the the model count diminish.
					//(need to recheck propcount in case we just called seedCritters).
			int modelcount = Biota.count( "cCritter3Dcharacter" ); 
			int modelstoadd = _seedcount - modelcount; 
			for ( int i = 0; i < modelstoadd; i++) 
				new cCritter3Dcharacter( this ); 
		// (3) Maybe check some other conditions.


            if (wentThrough && (Age - startNewRoom) > 2.0f)
            {
                wentThrough = false;
      
            }


            if (door2 != null && door2.HitDoor)
            {
                setRoom2();
                door2.HitDoor = false;
                Framework.snd.play(Sound.Tahn); 
            }

            if (door3 != null && door3.HitDoor)
            {
                setRoom3();
                MessageBox.Show("you must defeat the evil that is sailor moon to win")
                door3.HitDoor = false;
                Framework.snd.play(Sound.Tahn); 
            }

            if (door0.HitDoor && doorcollision == true)
            {
                if (!(onRoom2 || onRoom3))
                {
                    setRoom1();
                    doorcollision = false;
                    onRoom2=true;
                    Framework.snd.play(Sound.Tahn); 
                }
    
                /**if (onRoom3)
                {
                    setRoom3();
                }**/
            }
		} 
		
	} 
	
}