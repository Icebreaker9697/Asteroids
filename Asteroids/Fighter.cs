using Asteroids;
using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Asteroids
{
    internal class Fighter : DrawableGameComponent
    {
        private Model myModel;
        SoundEffect soundEngine;
        SoundEffect soundHyperSpaceActivation;
        //Vector3 modelPosition;
        Vector3 startPosition;
       // Vector3 modelVelocity = Vector3.Zero;
        float aspectRatio;
        float modelYaw = 0.0f;
        float modelPitch = 0.0f;
        float modelRoll = 0.0f;


        SoundEffectInstance soundEngineInstance;
        //private Texture2D fighterTexture;
        private BEPUphysics.Entities.Prefabs.Sphere physicsObject;
        private Vector3 currentPosition
        {
            get
            {
                return MathConverter.Convert(physicsObject.Position);
            }
        }

        public Fighter(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public Fighter(Game game, Vector3 pos) : this(game)
        {
            //first param is motionstate (I think the position?), and the secondParam is the radius
            physicsObject = new BEPUphysics.Entities.Prefabs.Sphere(MathConverter.Convert(pos), 1);
            startPosition = pos;
            //modelPosition = startPosition;

            //this is friction slowing down a rotation
            physicsObject.AngularDamping = .1f;

            //this is friction slowing down movement
            physicsObject.LinearDamping = .1f;

            //adds this object into the space of the game?
            Game.Services.GetService<Space>().Add(physicsObject);
        }

        public Fighter(Game game, Vector3 pos, float mass) : this(game, pos)
        {
            physicsObject.Mass = mass;
        }

        public Fighter(Game game, Vector3 pos, float mass, Vector3 linMomentum) : this(game, pos, mass)
        {
            physicsObject.LinearMomentum = MathConverter.Convert(linMomentum);
            //modelVelocity = linMomentum / 50;
        }

        public Fighter(Game game, Vector3 pos, float mass, Vector3 linMomentum, Vector3 angMomentum) : this(game, pos, mass, linMomentum)
        {
            physicsObject.AngularMomentum = MathConverter.Convert(angMomentum);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //moonTexture = Game.Content.Load<Texture2D>("moonsurface");
            myModel = Game.Content.Load<Model>("SmallSpaceFighter");
            //myModel = Content.Load<Model>("glider_fbx");

            soundEngine = Game.Content.Load<SoundEffect>("Audio\\engine_2");
            soundEngineInstance = soundEngine.CreateInstance();
            soundHyperSpaceActivation = Game.Content.Load<SoundEffect>("Audio\\hyperspace_activate");

            aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;

            //sets the radius of the physics object to match the one for this model
            physicsObject.Radius = myModel.Meshes[0].BoundingSphere.Radius;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //Get some input
            UpdateInput();
            Vector3 upwardOffest = MathConverter.Convert(physicsObject.WorldTransform.Up);
            Game1.CameraPosition = MathConverter.Convert(physicsObject.Position) + upwardOffest;
            Game1.CameraDirection = MathConverter.Convert(physicsObject.Position) + upwardOffest + MathConverter.Convert(physicsObject.WorldTransform.Forward);
            Game1.CameraUp = MathConverter.Convert(physicsObject.WorldTransform.Up);




            //Game1.CameraDirection = MathConverter.Convert(physicsObject.WorldTransform.Forward);
            //Add Velocity to the current position for both the physics object and the model
            //modelPosition += modelVelocity;
            //physicsObject.Position += MathConverter.Convert(modelVelocity);

            //bleed off velocity over time from both the model and the physics object
            //modelVelocity *= 0.95f;
            //physicsObject.LinearMomentum *= 0.95f;
        }

        protected void UpdateInput()
        {

            KeyboardState currentKeyState = Keyboard.GetState();
            GamePadState currentGamePadState = GamePad.GetState(PlayerIndex.One);

            bool engineon = false;

            if (currentGamePadState.IsConnected)
            {
                //rotate the model using the left thumbstick, and scale it down

                modelYaw -= currentGamePadState.ThumbSticks.Left.X * 0.10f;

                //create some velocity if the right trigger is down

                Vector3 modelVelocityAdd = Vector3.Zero;

                //findout what direction we should be thrusting using rotation
                modelVelocityAdd.X = -(float)Math.Sin(modelYaw);
                modelVelocityAdd.Z = -(float)(Math.Cos(modelYaw)*Math.Cos(modelPitch));
                modelVelocityAdd.Y = (float)(Math.Cos(modelYaw) * Math.Sin(modelPitch));

                //Now scale our direction by how hard the trigger is down
                modelVelocityAdd *= currentGamePadState.Triggers.Right;

                if (currentGamePadState.Triggers.Right != 0f)
                {
                    engineon = true;
                }

                //finally, add this vector to our velocity
                //modelVelocity += modelVelocityAdd;

                GamePad.SetVibration(PlayerIndex.One, currentGamePadState.Triggers.Right, currentGamePadState.Triggers.Right);

                //In case you get lost, press A to warp back to center
                if (currentGamePadState.Buttons.A == ButtonState.Pressed)
                {
                    //modelPosition = startPosition;
                    //modelVelocity = Vector3.Zero;
                    modelYaw = 0.0f;
                }
            }

            if (currentKeyState.IsKeyDown(Keys.A))
            {
                BEPUutilities.Vector3 rot = physicsObject.WorldTransform.Up;
                physicsObject.ApplyAngularImpulse(ref rot);
            }

            if (currentKeyState.IsKeyDown(Keys.D))
            {
                BEPUutilities.Vector3 rot = physicsObject.WorldTransform.Down;
                physicsObject.ApplyAngularImpulse(ref rot);
            }

            if (currentKeyState.IsKeyDown(Keys.S))
            {
                BEPUutilities.Vector3 rot = physicsObject.WorldTransform.Right;
                physicsObject.ApplyAngularImpulse(ref rot);
            }

            if (currentKeyState.IsKeyDown(Keys.X))
            {
                BEPUutilities.Vector3 rot = physicsObject.WorldTransform.Left;
                physicsObject.ApplyAngularImpulse(ref rot);
            }

            if (currentKeyState.IsKeyDown(Keys.Q))
            {
                BEPUutilities.Vector3 rot = physicsObject.WorldTransform.Backward;
                physicsObject.ApplyAngularImpulse(ref rot);
            }

            if (currentKeyState.IsKeyDown(Keys.E))
            {
                BEPUutilities.Vector3 rot = physicsObject.WorldTransform.Forward;
                physicsObject.ApplyAngularImpulse(ref rot);
            }

            if (currentKeyState.IsKeyDown(Keys.W))
            {
                physicsObject.ApplyImpulse(physicsObject.Position, physicsObject.WorldTransform.Forward);
                engineon = true;
            }

            if (currentKeyState.IsKeyDown(Keys.P))
            {
                //modelPosition = Vector3.Zero;
               // modelVelocity = Vector3.Zero;
                soundHyperSpaceActivation.Play();
            }

            if (engineon)
            {
                if (soundEngineInstance.State == SoundState.Stopped)
                {
                    soundEngineInstance.Volume = 0.75f;
                    soundEngineInstance.IsLooped = true;
                    soundEngineInstance.Play();
                }
                else
                {
                    soundEngineInstance.Resume();
                }
            }
            else
            {
                soundEngineInstance.Pause();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            //Copy any parent transforms
            Matrix[] transforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(transforms);

            //Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                //This is where the mesh orientation is set, as well as our camera and projection
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.DirectionalLight0.DiffuseColor = new Vector3(10, 10, 10);
                    effect.DirectionalLight1.DiffuseColor = new Vector3(10, 10, 10);
                    effect.DirectionalLight2.DiffuseColor = new Vector3(10, 10, 10);
                    effect.PreferPerPixelLighting = true;
                    ///effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(modelYaw) * Matrix.CreateRotationX(modelPitch) * Matrix.CreateRotationZ(modelRoll) * Matrix.CreateTranslation(modelPosition);
                    effect.World = MathConverter.Convert(physicsObject.WorldTransform);
                    effect.View = Matrix.CreateLookAt(Game1.CameraPosition, Game1.CameraDirection, Game1.CameraUp);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 10000.0f);
                    effect.Alpha = 0.1f;
                }

                //draw the mesh using the effects set above
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
