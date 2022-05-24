using System;
using System.Collections.Generic;
using System.Drawing;
using Apollo_Justice_Script_Editor.Properties;

namespace Apollo_Justice_Script_Editor
{
    class MessageDrawer
    {
        public const float MASCHTAB = 1.1f; //The scale of picture in the pictureBox
        const int initx = 50; //Initializing x coordinate for text in Message Window
        const int inity = 52; //Initializing y coordinate for text in Message Window
        const int endl = 35; //The height of the text cell
        const int MAX_LINE_LENGTH = 32; //The text array length limits for one line
        const float k = 1.28f; //The additional ratio for MessageWindow Image scale
        static List<int> codes = new List<int>(); //CharacterCodes
        static Dictionary<short, int> widthes = new Dictionary<short, int>(); //Widthes of characters' cells
        static Dictionary<short, Image> images = new Dictionary<short, Image>(); //Images of characters
        static Dictionary<short, int> offsets = new Dictionary<short, int>(); //YOffsets of characters
        static bool FontDataLoaded = false; //Flag for not loading font data twice

        public MessageDrawer()
        {
            InitGlyphs();
        }
        public async void DrawMessage(short[][] msg, Graphics g)
        {
            try
            {
                Clear(g);
                int x = initx, y = inity;
                for (int i = 0; i < msg.Length; i++)
                {
                    int lineLength = 0;
                    for (int j = 0; j < msg[i].Length; j++)
                    {
                        if (lineLength >= MAX_LINE_LENGTH) // Text Length Limitations from the game code
                        {
                            break;
                        }
                        short code = (msg[i][j]);
                        if (!codes.Contains(msg[i][j])) // If code is unknown, the character will be 'X'
                        {
                            code = 0xCB;
                        }
                        int width = GetWidth(code);
                        Image glyph = GetGlyph(code);
                        int rwid = (int)((float)glyph.Width / MASCHTAB);
                        int rhei = (int)((float)glyph.Height / MASCHTAB);
                        g.DrawImage(glyph, (float)x / MASCHTAB, (float)(y + GetOffset(code)) / MASCHTAB, rwid, rhei);
                        x += width;
                        lineLength++;
                    }
                    x = initx;
                    y += endl;
                    if (i == 2) // More than 2 lines is not allowed
                        break;
                }
            }
            catch
            {
                return;
            }
        }

        public int GetWidth(short num)
        {
            return widthes[num];
        }

        void InitGlyphs() //The characters' data is being loaded dinamically after reading the script file
        {
            if (FontDataLoaded)
            {
                return;
            }
            FontDataLoaded = true;
            FontInfo.CharacterData[] cds = FontInfo.GetInstance().list;
            for (int i = 0; i < cds.Length; i++)
            {
                Image glyph = CutGlyph(cds[i]);
                int width = FontInfo.GetInstance().CharacterSize - cds[i].rm - cds[i].lm;
                short code = (short)(cds[i].code + 144);
                codes.Add(code);
                if (cds[i].isSpace)
                {
                    width -= 17;
                }
                images.Add(code, glyph);
                widthes.Add(code, width + 4);
                offsets.Add(code, cds[i].yo);
            }
        }

        Image GetGlyph(short x)
        {
            return images[x];
        }

        int GetOffset(short x)
        {
            return offsets[x];
        }

        public Image CutGlyph(FontInfo.CharacterData cd)
        {
            Rectangle rct = new Rectangle((int)cd.rx, (int)FontInfo.GetInstance().TextureSizeY - (int)cd.ry - (int)cd.rh, (int)cd.rw, (int)cd.rh + 1);
            if (cd.isSpace)
            {
                rct = new Rectangle((int)FontInfo.GetInstance().TextureSizeX - 2, (int)FontInfo.GetInstance().TextureSizeY - 2, 1, 1);
            }
            Bitmap bmp = Resources.font_atlas as Bitmap; //The character's place on atlas is saved in rct
            if (bmp == null)
                throw new ArgumentException("No bitmap");
            Bitmap cropBmp = bmp.Clone(rct, bmp.PixelFormat);

            return cropBmp;
        }

        public void Clear(Graphics g)
        {
            g.Clear(Form1.DefaultBackColor);
            g.DrawImage(Resources.MessageWindow, 0, 0, 1024 / k / MASCHTAB, 250 / k / MASCHTAB); //Showing MessageWindow
        }
    }
}
