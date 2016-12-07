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
    public class Eksplozje
    {
        public Texture2D tekstura;
        public Vector2 pozycja;
        public float czas;
        public float interwal;
        public Vector2 skad;
        public int obecnaKlatka, spriteWidth, spriteHeight;
        public Rectangle zrodlo;
        public bool widzialny;

        public Eksplozje(Texture2D nowaTekstura, Vector2 nowaPozycja)
        {
            pozycja = nowaPozycja;
            tekstura = nowaTekstura;
            czas = 0f;
            interwal = 20f;
            obecnaKlatka = 1;
            spriteWidth = 118;
            spriteHeight = 118;
            widzialny = true;
        }

        public void LoadContent(ContentManager Content)
        {

        }

        public void Update(GameTime gameTime)
        {
            //zwiekszam czas o milisekundy od ostatniego update
            czas += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (czas > interwal)
            {
                obecnaKlatka++;
                czas = 0f;
            }

            if (obecnaKlatka == 17) // na ostatniej klatce robimy to niewidocznym i resetujemy
            {
                widzialny = false;
                obecnaKlatka = 0;
            }

            zrodlo = new Rectangle(obecnaKlatka * spriteWidth, 0, spriteWidth, spriteHeight);
            skad = new Vector2(zrodlo.Width / 2, zrodlo.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //jak widoczny to draw
            if (widzialny == true)
                spriteBatch.Draw(tekstura, pozycja, zrodlo, Color.White, 0f, skad, 1.0f, SpriteEffects.None, 0);
        }
    }
}
