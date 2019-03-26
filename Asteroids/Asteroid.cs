using Asteroids;
using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    internal class Asteroid : DrawableGameComponent
    {
        private Model model;
        private Texture2D moonTexture;
        private BEPUphysics.Entities.Prefabs.Sphere physicsObject;
        private Vector3 currentPosition
        {
            get
            {
                return MathConverter.Convert(physicsObject.Position);
            }
        }

        public Asteroid(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public Asteroid(Game game, Vector3 pos) : this(game)
        {
            //first param is motionstate (I think the position?), and the secondParam is the radius
            physicsObject = new BEPUphysics.Entities.Prefabs.Sphere(MathConverter.Convert(pos), 1);

            //this is friction slowing down a rotation
            physicsObject.AngularDamping = 0f;

            //this is friction slowing down movement
            physicsObject.LinearDamping = 0f;

            //adds this object into the space of the game?
            Game.Services.GetService<Space>().Add(physicsObject);
        }

        public Asteroid(Game game, Vector3 pos, float mass) : this(game, pos)
        {
            physicsObject.Mass = mass;
        }

        public Asteroid(Game game, Vector3 pos, float mass, Vector3 linMomentum) : this(game, pos, mass)
        {
            physicsObject.LinearMomentum = MathConverter.Convert(linMomentum);
        }

        public Asteroid(Game game, Vector3 pos, float mass, Vector3 linMomentum, Vector3 angMomentum) : this(game, pos, mass, linMomentum)
        {
            physicsObject.AngularMomentum = MathConverter.Convert(angMomentum);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            moonTexture = Game.Content.Load<Texture2D>("moonsurface");
            model = Game.Content.Load<Model>("moon");

            //sets the radius of the physics object to match the one for this model
            physicsObject.Radius = model.Meshes[0].BoundingSphere.Radius;

            base.LoadContent();
        }
        public override void Draw(GameTime gameTime)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.DirectionalLight0.DiffuseColor = new Vector3(10, 10, 10);
                    effect.DirectionalLight1.DiffuseColor = new Vector3(10, 10, 10);
                    effect.DirectionalLight2.DiffuseColor = new Vector3(10, 10, 10);
                    effect.PreferPerPixelLighting = true;
                    //effect.Alpha = 0.3f;
                    effect.World = MathConverter.Convert(physicsObject.WorldTransform);
                    effect.View = Matrix.CreateLookAt(Game1.CameraPosition, Game1.CameraDirection, Game1.CameraUp);
                    float aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
                    float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                    float nearClipPlane = 0.3f;
                    float farClipPlane = 400f;
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
