using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    internal class Skybox : DrawableGameComponent
    {
        private Model model;
        private Texture2D moonTexture;


        public Skybox(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            moonTexture = Game.Content.Load<Texture2D>("skybox//skyboxtexture");
            model = Game.Content.Load<Model>("skybox//background");

            base.LoadContent();
        }
        public override void Draw(GameTime gameTime)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.DirectionalLight0.DiffuseColor = new Vector3(10, 10, 10);
                    effect.DirectionalLight1.DiffuseColor = new Vector3(10, 10, 10);
                    effect.DirectionalLight2.DiffuseColor = new Vector3(10, 10, 10);
                    effect.PreferPerPixelLighting = true;
                    //effect.Alpha = 0.3f;
                    effect.World = Matrix.CreateTranslation(Game1.CameraPosition);
                    effect.View = Matrix.CreateLookAt(Game1.CameraPosition, Game1.CameraDirection, Game1.CameraUp);
                    float aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
                    float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                    float nearClipPlane = 0.3f;
                    float farClipPlane = 2000f;
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
