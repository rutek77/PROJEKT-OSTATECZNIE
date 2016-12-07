using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NewGame
{
    public class Tlo
    {
        public Texture2D tekstura;
        public Vector2 tloPoz1, tloPoz2;
        public int szybkosc;

        public Tlo()
        {
            tekstura = null;
            tloPoz1 = new Vector2(0, 0);
            tloPoz2 = new Vector2(0, -720);
            szybkosc = 5;
        }

        public void LoadContent(ContentManager Content)
        {
            tekstura = Content.Load<Texture2D>("tlo");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tekstura, tloPoz1, Color.White);
            spriteBatch.Draw(tekstura, tloPoz2, Color.White);
        }
        public void Update(GameTime gameTime)
        {
            //ustawienia tla aby sie ruszalo
            tloPoz1.Y = tloPoz1.Y + szybkosc;
            tloPoz2.Y = tloPoz2.Y + szybkosc;            
            //powtarzanie tla od poczatku
            if (tloPoz1.Y >= 720)
            {
                tloPoz1.Y = 0;
                tloPoz2.Y = -720;
            }
        }
    }
}
