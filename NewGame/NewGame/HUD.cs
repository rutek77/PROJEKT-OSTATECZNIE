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
    public class HUD
    {
        public int wynikGracza, screenWidth, screenHeight;
        public SpriteFont graczaWynikFont;
        public Vector2 wynikGraczaPozycja;
        public bool pokazHUD;

        //konstruktor
        public HUD()
        {
            wynikGracza = 0;
            pokazHUD = true;
            screenHeight = 720;
            screenWidth = 1280;
            graczaWynikFont = null;
            wynikGraczaPozycja = new Vector2(screenWidth / 2, 50);

        }

        public void LoadContent(ContentManager Content)
        {
            graczaWynikFont = Content.Load<SpriteFont>("georgia");
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState klawiszState = Keyboard.GetState();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // z tego powstaje napis "Wynik" u gory aktualizowany, pokazujacy po prostu wynik, czerwony
            if (pokazHUD)
                spriteBatch.DrawString(graczaWynikFont, "Wynik - " + wynikGracza, wynikGraczaPozycja, Color.Red);
        }
    }
}
