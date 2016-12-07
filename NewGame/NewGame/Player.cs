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
    public class Player
    {
        public Texture2D tekstura, atakTekstura, zycieTekstura;
        public Vector2 pozycja, iloscZyciaPozycja, pozycjaStartowa;
        public int szybkosc, iloscZycia;
        public float atakCzas;
        public Rectangle boundingBox, iloscZyciaRectangle; //bounding box - najmniejszy prostopadloscian totalnie otaczajacy dany obiekt (cos jakby naczynie)
        public bool koliduje;
        public List<Atak> atakLista;
        Dzwieki dz = new Dzwieki();
        public static Player gracz; //zrobione na samym koncu by gracz sie respil w miejscu poczatkowym a nie tam gdzie zginal

        public Player()
        {
            atakLista = new List<Atak>();
            tekstura = null;
            pozycja = new Vector2(620, 720); //640 niby idealnie srodek ale wygladalo to inaczej
            szybkosc = 20;
            atakCzas = 1;
            koliduje = false;
            iloscZycia = 200;
            iloscZyciaPozycja = new Vector2(0, -200);
            pozycjaStartowa = new Vector2(620, 720);
            gracz = this;
        }

        //loadContent
        public void LoadContent(ContentManager Content)
        {
            tekstura = Content.Load<Texture2D>("goku");
            atakTekstura = Content.Load<Texture2D>("kama");
            zycieTekstura = Content.Load<Texture2D>("hpbar");
            dz.LoadContent(Content);
        }

        //draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tekstura, pozycja, Color.White);
            spriteBatch.Draw(zycieTekstura, iloscZyciaRectangle, Color.White);

            foreach (Atak b in atakLista)
                b.Draw(spriteBatch);
        }

        //update
        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            //kolizje
            boundingBox = new Rectangle((int)pozycja.X, (int)pozycja.Y, tekstura.Width, tekstura.Height);

            //rectangle dla zycia
            iloscZyciaRectangle = new Rectangle((int)iloscZyciaPozycja.X, (int)iloscZyciaPozycja.Y, iloscZycia, 550);

            //sterowanie
            if (keyState.IsKeyDown(Keys.Up))
                pozycja.Y = pozycja.Y - szybkosc;
            if (keyState.IsKeyDown(Keys.Left))
                pozycja.X = pozycja.X - szybkosc;
            if (keyState.IsKeyDown(Keys.Down))
                pozycja.Y = pozycja.Y + szybkosc;
            if (keyState.IsKeyDown(Keys.Right))
                pozycja.X = pozycja.X + szybkosc;

            // sterowanie dla ataku
            if(keyState.IsKeyDown(Keys.Space))
            {
                Atak();
            }
            UpdateAtak();
            //utrzymanie goku w "granicach ekranu" zeby nie wychodzil poza

            if (pozycja.X <= 0)
                pozycja.X = 0;
            //ograniczenie z lewej
            if (pozycja.X >= 1280 - tekstura.Width) //1280 bo taka rozdzielczosc
                pozycja.X = 1280 - tekstura.Width;
            // ograniczenie prawej
            if (pozycja.Y <= 0)
                pozycja.Y = 0;
            //ograniczenie od gory
            if (pozycja.Y >= 720 - tekstura.Height)
                pozycja.Y = 720 - tekstura.Height;
            //ograniczenie od dolu
        }
        // ustawienia ataku
        public void Atak()
        {
            //atakuje jak czas sie zresetuje
            if (atakCzas >= 0)
                atakCzas--;

            //jak atakczas=0 to tworze nowy "pocisk" w pozycji gracza, widoczny

            if(atakCzas <= 0)
            {
                dz.dzwiekStrzalu.Play();
                Atak pocisk = new Atak(atakTekstura);
                pocisk.pozycja = new Vector2(pozycja.X + 32 - pocisk.tekstura.Width / 2, pozycja.Y + 30);

                pocisk.widzialny = true;

                if (atakLista.Count() < 20) //max 20 pociskow naraz na ekranie
                    atakLista.Add(pocisk);
            }

            //resetowanie czasu
            if (atakCzas == 0)
                atakCzas = 5;            
        }

        //update dla pociskow
        public void UpdateAtak()
        {
            //ustalamy pozycje pocisku, jak pocisk dojdzie do konca ekranu to go usuwany z listy
            foreach(Atak b in atakLista)
            {
                //cdn kolizji, box dla naszych pociskow, kazdego pocisku w liscie
                b.boundingBox = new Rectangle((int)b.pozycja.X, (int)b.pozycja.Y, b.tekstura.Width, b.tekstura.Height);
                //ustawienia ruchu
                b.pozycja.Y = b.pozycja.Y - b.szybkosc;
                //jak dojdzie do gory bool widzialnosci ustawiam na false
                if (b.pozycja.Y <= 0)
                    b.widzialny = false;
            }
            // sprawdzenie, jesli pociski nie sa widoczne to sa usuwane z listy
            for (int i = 0; i < atakLista.Count; i++)
            {
                if(!atakLista[i].widzialny)
                {
                    atakLista.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}
