using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NewGame
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum State
        {
            Menu,
            Playing,
            GameOver
        }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
        public int pociskWrogaObrazenia;
        public Texture2D menuObraz;
        public Texture2D GameoverObraz;
        Player gracz = new Player();
        Tlo tlo = new Tlo();
        HUD hud = new HUD();
        List<Wrog> wrogLista = new List<Wrog>();
        List<Smocza> smoczeKuleLista = new List<Smocza>();
        List<Eksplozje> EksplozjaLista = new List<Eksplozje>();
        Dzwieki dz = new Dzwieki();

        //pierwszy state
        State gameState = State.Menu;
        //konstruktor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false; 
            graphics.PreferredBackBufferWidth = 1280; //szerokosc
            graphics.PreferredBackBufferHeight = 720; //wysokosc
            this.Window.Title = "gra projekt"; 
            Content.RootDirectory = "Content";
            pociskWrogaObrazenia = 5;
            menuObraz = null;
            GameoverObraz = null;
        }

        protected override void Initialize()
        {
            // pierwsze miejsce w procesie uruchomienia aplikacji w ktorym mamy jakikolwiek wplyw
            //tutaj ustawiamy podstawowe parametry potrzebne do uruchomienia gry
            base.Initialize();            
        }

        protected override void LoadContent()
        {
            //ladujemy elementy z foleduru contenta, muzyke, grafike itp
            spriteBatch = new SpriteBatch(GraphicsDevice);

            menuObraz = Content.Load<Texture2D>("powitanie");
            hud.LoadContent(Content);
            gracz.LoadContent(Content);
            tlo.LoadContent(Content);
            dz.LoadContent(Content);
            MediaPlayer.Play(dz.menuPiosenka);
            GameoverObraz = Content.Load<Texture2D>("gameover");
        }

        protected override void UnloadContent()
        {
            //zwalniamy nasze zasoby, raczej nie bede jej uzywac
        }

        protected override void Update(GameTime gameTime)
        {
            // ilosc wywolan w trakcie sekundy = 60 razy, metoda update wywolywana wielokrotnie, 
            // tutaj sa aktualizowane stany obiektu np. dostaniemy jakies obrazenia.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //update gry
            switch (gameState)
            {               
                case State.Playing:
                    {
                        tlo.szybkosc = 5;
                        //update dla wrogow, sprawdzanie kolizji wrog -> gracz, wrogi pocisk -> gracz
                        foreach (Wrog e in wrogLista)
                        {
                            //kolizja wrog->gracz
                            if (e.boundingBox.Intersects(gracz.boundingBox))
                            {
                                gracz.iloscZycia -= 40;
                                e.widzialny = false;
                            }
                            //kolizja pocisk wroga -> gracz
                            for (int i = 0; i < e.pociskLista.Count; i++)
                            {
                                if (gracz.boundingBox.Intersects(e.pociskLista[i].boundingBox))
                                {
                                    gracz.iloscZycia -= pociskWrogaObrazenia;
                                    e.pociskLista[i].widzialny = false;
                                }
                            }
                            //kolizja pocisk moj -> wrog + dodaje wynik za trafienie
                            for (int i = 0; i < gracz.atakLista.Count; i++)
                            {
                                if (e.boundingBox.Intersects(gracz.atakLista[i].boundingBox))
                                {
                                    dz.dzwiekWybuchu.Play();
                                    EksplozjaLista.Add(new Eksplozje(Content.Load<Texture2D>("eksplozja"), new Vector2(e.pozycja.X, e.pozycja.Y)));
                                    hud.wynikGracza += 20;
                                    e.widzialny = false;
                                    gracz.atakLista.ElementAt(i).widzialny = false;
                                }
                            }
                            e.Update(gameTime);
                        }

                        foreach (Eksplozje ex in EksplozjaLista)
                        {
                            ex.Update(gameTime);
                        }


                        //dla kazdej smoczej update i sprawdzenie czy nie ma kolizji
                        foreach (Smocza a in smoczeKuleLista)
                        {
                            //sprawdzenie czy smocze bb koliduje z gracza bb, jesli tak to sa widoczne, a przez to usuwane z listy smoczej
                            if (a.boundingBox.Intersects(gracz.boundingBox))
                            {
                                gracz.iloscZycia -= 20; //-20hp za zderzenie ze smocza
                                a.widoczna = false;
                            }
                            //za pomoca ataklisty sprawdzam czy jakis pocisk trafia w smocza, jesli tak to usuwam kule i pocisk
                            for (int i = 0; i < gracz.atakLista.Count; i++)
                            {
                                if (a.boundingBox.Intersects(gracz.atakLista[i].boundingBox))
                                {
                                    dz.dzwiekWybuchu.Play();
                                    EksplozjaLista.Add(new Eksplozje(Content.Load<Texture2D>("eksplozja"), new Vector2(a.pozycja.X, a.pozycja.Y)));
                                    hud.wynikGracza += 5;
                                    a.widoczna = false;
                                    gracz.atakLista.ElementAt(i).widzialny = false;
                                }
                            }
                            a.Update(gameTime);
                        }

                        if (gracz.iloscZycia <= 0)
                        {                            
                            gameState = State.GameOver;
                        }
                        hud.Update(gameTime);
                        gracz.Update(gameTime);
                        tlo.Update(gameTime);
                        ManageEksplozja();
                        LoadSmocze();
                        LoadWrog();
                        break;
                    }

                    //update dla menu
                case State.Menu:
                    {
                        KeyboardState key = Keyboard.GetState();
                        if (key.IsKeyDown(Keys.Enter))
                        {
                            gameState = State.Playing;
                            MediaPlayer.Play(dz.wTlePiosenka);
                        }
                        tlo.Update(gameTime);
                        tlo.szybkosc = 1;
                        break;
                    }

                    //update dla gameover
                case State.GameOver:                    
                    {                        
                        KeyboardState key = Keyboard.GetState();
                        if (key.IsKeyDown(Keys.Escape))
                        {
                            gracz.pozycja = Player.gracz.pozycjaStartowa;
                            wrogLista.Clear();
                            smoczeKuleLista.Clear();
                            gracz.atakLista.Clear();
                            EksplozjaLista.Clear();
                            gracz.iloscZycia = 200;
                            hud.wynikGracza = 0;                            
                            gameState = State.Menu;
                        }
                        MediaPlayer.Stop();
                        break;
                    }
                }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //przerysowywanie obrazu, kod zwiazany z generowaniem obrazu, sprawy graficzne,
            spriteBatch.Begin();

            switch (gameState)
            {
                case State.Playing:
                    {
                        tlo.Draw(spriteBatch); //tlo przed graczem
                        gracz.Draw(spriteBatch);

                        foreach (Eksplozje eksplozja in EksplozjaLista)
                        {
                            eksplozja.Draw(spriteBatch);
                        }

                        foreach (Smocza a in smoczeKuleLista)
                        {
                            a.Draw(spriteBatch);
                        }

                        foreach (Wrog e in wrogLista)
                        {
                            e.Draw(spriteBatch);
                        }

                        hud.Draw(spriteBatch);
                        break;
                    }
                case State.Menu:
                    {                        
                        tlo.Draw(spriteBatch);
                        spriteBatch.Draw(menuObraz, new Vector2(0, 0), Color.White);
                        break;
                    }
                case State.GameOver:
                    {                        
                        spriteBatch.Draw(GameoverObraz, new Vector2(0, 0), Color.White);
                        spriteBatch.DrawString(hud.graczaWynikFont, "Twoj koncowy wynik to - "+ hud.wynikGracza, new Vector2(50, 100), Color.Red);
                        break;
                    }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        //ladowanie smoczych kul
        public void LoadSmocze()
        {
            //wartosci wspolrzednych kul
            int randY = random.Next(-720, -50);
            int randX = random.Next(0, 1280);
            // jak jest mniej niz 5kul na ekranie to tworzy nowe
            if(smoczeKuleLista.Count() < 5)
            {
                smoczeKuleLista.Add(new Smocza(Content.Load<Texture2D>("smocza"), new Vector2(randX, randY)));
            }
            // to samo co w pocisku, jak kula nie jest widoczna / jest zniszczona to jest usuwana z listy
            for (int i = 0; i < smoczeKuleLista.Count; i++)
            {
                if (!smoczeKuleLista[i].widoczna)
                {
                    smoczeKuleLista.RemoveAt(i);
                        i--;
                }
            }
        }
        public void LoadWrog()
        {
            //wartosci wspolrzednych wrogow
            int randY = random.Next(-100, -10); //takie ustawienia by sie co chwile respili
            int randX = random.Next(0, 1200);
            // jak jest mniej niz 5wrogow to tworzy nowych
            if (wrogLista.Count() < 5)
            {
                wrogLista.Add(new Wrog(Content.Load<Texture2D>("wrog"), new Vector2(randX, randY), Content.Load<Texture2D>("pociskWroga")));
            }
            // to samo co w pocisku, jak wrog nie jest widoczna / jest zniszczona to jest usuwana z listy
            for (int i = 0; i < wrogLista.Count; i++)
            {
                if (!wrogLista[i].widzialny)
                {
                    wrogLista.RemoveAt(i);
                    i--;
                }
            }
        }
        public void ManageEksplozja()
        {
            for(int i = 0 ; i < EksplozjaLista.Count; i++)
            {
                if(!EksplozjaLista[i].widzialny)
                {
                    EksplozjaLista.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
