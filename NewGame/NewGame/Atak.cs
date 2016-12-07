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
    public class Atak
    {
        public Rectangle boundingBox;
        public Texture2D tekstura;
        public Vector2 pozycja;
        public Vector2 skad;
        public bool widzialny;
        public float szybkosc;

        public Atak(Texture2D newTekstura)
        {
            szybkosc = 10;
            tekstura = newTekstura;
            widzialny = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tekstura, pozycja, Color.White);
        }
    }
}
