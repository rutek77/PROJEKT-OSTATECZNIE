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
    public class Smocza
    {
        public Rectangle boundingBox;
        public Texture2D tekstura;
        public Vector2 pozycja;
        public Vector2 skad;
        public float rotacja;
        public int szybkosc;
        public bool widoczna;

        Random random = new Random();
        public float randX, randY;
        
        //konstruktor
        public Smocza(Texture2D newTekstura, Vector2 newPozycja)
        {
            pozycja = newPozycja;
            tekstura = newTekstura;
            szybkosc = 4;
            widoczna = true;
            randX = random.Next(50, 1280);
            randY = random.Next(-720, -25);
        }

        public void LoadContent(ContentManager Content)
        {

        }
        public void Update(GameTime gameTime)
        {
            //ustawienia dla boundingbox
            boundingBox = new Rectangle((int)pozycja.X, (int)pozycja.Y, 45, 45);
            //ustawienia ruchu 
            pozycja.Y = pozycja.Y + szybkosc;
            if (pozycja.Y >= 720)
                pozycja.Y = -50;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (widoczna)
                spriteBatch.Draw(tekstura, pozycja, Color.White);
        }
    }
}
