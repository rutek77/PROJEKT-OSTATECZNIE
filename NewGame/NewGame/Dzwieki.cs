using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace NewGame
{
    public class Dzwieki
    {
        public SoundEffect dzwiekStrzalu;
        public SoundEffect dzwiekWybuchu;
        public Song wTlePiosenka;
        public Song menuPiosenka;
        public Song koniecPiosenka;
        public Dzwieki()
        {
            dzwiekStrzalu = null;
            dzwiekWybuchu = null;
            wTlePiosenka = null;
            menuPiosenka = null;
        }
        public void LoadContent(ContentManager Content)
        {
            dzwiekStrzalu = Content.Load<SoundEffect>("pociskgracza");
            dzwiekWybuchu = Content.Load<SoundEffect>("eksplozje");
            wTlePiosenka = Content.Load<Song>("instrumental");
            menuPiosenka = Content.Load<Song>("dandan");
        }
    }
}
