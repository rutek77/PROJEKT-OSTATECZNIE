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
    public class Wrog
    {
        public Rectangle boundingBox;
        public Texture2D tekstura, pociskTekstura;
        public Vector2 pozycja;
        public int zycie, szybkosc, pociskCzas, poziomTrudnosci;
        public bool widzialny;
        public List<Atak> pociskLista;

        public Wrog(Texture2D nowaTekstura, Vector2 nowaPozycja, Texture2D nowyPociskTekstura)
        {
            pociskLista = new List<Atak>();
            tekstura = nowaTekstura;
            pociskTekstura = nowyPociskTekstura;
            zycie = 5;
            pozycja = nowaPozycja;
            poziomTrudnosci = 1;
            pociskCzas = 40;
            szybkosc = 2;
            widzialny = true;
        }

        public void Update(GameTime gameTime)
        {
            //kolizje
            boundingBox = new Rectangle((int)pozycja.X, (int)pozycja.Y, tekstura.Width, tekstura.Height);
            //poruszanie sie wrogow
            pozycja.Y += szybkosc;
            //jak wrog przeleci przez ekran niech wroci na poczatek 
            if (pozycja.Y >= 720)
                pozycja.Y = -75;

            WrogAtak();
            UpdatePocisk();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tekstura, pozycja, Color.White);

            foreach(Atak b in pociskLista)
            {
                b.Draw(spriteBatch);
            }
        }
        public void UpdatePocisk()
        {
            //ustalamy pozycje pocisku, jak pocisk dojdzie do konca ekranu to go usuwany z listy
            foreach (Atak b in pociskLista)
            {

                //cdn kolizji, box dla naszych pociskow, kazdego pocisku w liscie
                b.boundingBox = new Rectangle((int)b.pozycja.X, (int)b.pozycja.Y, b.tekstura.Width, b.tekstura.Height);
                //ustawienia ruchu
                b.pozycja.Y = b.pozycja.Y + b.szybkosc;
                //jak dojdzie do dolu bool widzialnosci ustawiam na false
                if (b.pozycja.Y >= 720)
                    b.widzialny = false;
            }
            // sprawdzenie, jesli pociski nie sa widoczne to sa usuwane z listy
            for (int i = 0; i < pociskLista.Count; i++)
            {
                if (!pociskLista[i].widzialny)
                {
                    pociskLista.RemoveAt(i);
                    i--;
                }
            }
        }
        public void WrogAtak()
        {
            //strzela tylko jak czaspocisku sie zresetuje
            if (pociskCzas >= 0)
                pociskCzas--;
            if (pociskCzas <= 0)
            {
                //tworzy nowy pocisk w pozycji na wprost od polozenia wroga
                Atak nowyPocisk = new Atak(pociskTekstura);
                nowyPocisk.pozycja = new Vector2(pozycja.X + tekstura.Width / 2 - nowyPocisk.tekstura.Width / 2, pozycja.Y + 30);

                nowyPocisk.widzialny = true;

                if (pociskLista.Count() < 10) //max 10wrogich pociskow
                    pociskLista.Add(nowyPocisk);
            }
            //resetowanie czasu wrogiegopocisku
            if (pociskCzas == 0)
                pociskCzas = 40;
        }
    }
}
