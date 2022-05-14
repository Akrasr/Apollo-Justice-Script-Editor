using System.Collections.Generic;
using System.Linq;

namespace Apollo_Justice_Script_Editor
{
    class ScriptHelper
    {
        public static string GetMessage(string[] data, int cursed) //This function helps you getting the text of the message you clicked on
        {
            int index = 0; //The pointer for the current character
            string tag = ""; //The tag text buffer
            bool inTag = false; //The tag text buffer filling flag
            string res = ""; //result string
            string temp = ""; //Temporary text buffer
            bool found = false; //The needed message is found
            for (int i = 0; i < data.Length; i++)
            {
                char[] st = data[i].ToCharArray(); //Text Line
                for (int j = 0; j < st.Length; j++)
                {
                    if (inTag) //if this is tag
                    {
                        tag += st[j]; //add char to a tag string
                        if (st[j] == '>') //and if that's an end of tag
                        {
                            inTag = false; //We are not in tag name anymore
                            if (TagFinish(tag)) //If that's a finish text writing tag
                            {
                                temp += "<end>"; //add replacer tag for testing purposes
                                if (index > cursed) //if number index is higher than index of point you clicked on
                                {                   //in other words, if that's the end of the message you clicked on
                                    res = temp; //Save text in res
                                    found = true;
                                    break;
                                }
                                else
                                {
                                    temp = "";
                                }
                            }
                            else if (TagNext(tag)) //if that's a go-to-next-line tag, do not delete it, but save it
                            {
                                temp += tag;
                            } //If tag is not any of those, just do not place them into res
                        }
                    } else
                    {
                        if (st[j] == '<') //if that's the start of tag
                        {
                            inTag = true; //start collecting tag chars
                            tag = "<";
                        } else if (st[j] == '>') //if there is any unwanted > (like in the first text from the section start), just erase temp
                        {
                            temp = "";
                        }
                        else
                        {
                            temp += st[j]; //If that's a text, write it
                        }
                    }
                    index++;
                }
                if (found)
                {
                    break;
                }
                index += 2; //end of line means index is incremented by 2
            }
            return res;
        }

        static bool TagFinish(string tag)
        {
            return tag == "<p>" || tag == "<nextpage_button>" || tag == "<nextpage_nobutton>" || tag == "<testimony_jmp>" || tag == "<0x3F>"
                || tag.Contains("choice_jmp") || tag.Contains("rejmp");
        }

        static bool TagNext(string tag)
        {
            return tag == "<b>";
        }

        public static short[][] GetMessageShorts(string Message)
        {
            string[] lines = Message.Replace("<end>", "").Replace("<b>", "+").Split('+'); //deleting end tag and splitting text into array
            short[][] res = new short[lines.Length][];
            for (int i = 0; i < res.Length; i++) //converting chars to shorts
            {
                res[i] = new short[lines[i].Length];
                for (int j = 0; j < res[i].Length; j++)
                {
                    string character = "" + lines[i].ToCharArray()[j]; 
                    if (!aj3dsOrigCharConv.ContainsValue(character)) //if this is unknown char
                    {
                        character = "X"; //Replace it with X
                    }
                    res[i][j] = aj3dsOrigCharConv.Where(d => d.Value == character).ToList()[0].Key;
                }
            }
            return res;
        }

        public static Dictionary<short, string> aj3dsOrigCharConv = new Dictionary<short, string> //Characters short codes
        {
            [0x91] = "0", //numbers
            [0x96] = "1",
            [0x90] = "2",
            [0x93] = "3",
            [0x97] = "4",
            [0x95] = "5",
            [0x98] = "6",
            [0x94] = "7",
            [0x99] = "8",
            [0x92] = "9",

            [0xa8] = "A", //English Alphabet
            [0xc5] = "B",
            [0xb9] = "C",
            [0xb1] = "D",
            [0xaa] = "E",
            [0xc2] = "F",
            [0xcd] = "G",
            [0xb2] = "H",
            [0xac] = "I",
            [0xbd] = "J",
            [0xc4] = "K",
            [0xba] = "L",
            [0xb3] = "M",
            [0xbb] = "N",
            [0xc1] = "O",
            [0xb4] = "P",
            [0xc9] = "Q",
            [0xc6] = "R",
            [0x9a] = "S",
            [0xb5] = "T",
            [0xc3] = "U",
            [0xc8] = "V",
            [0xbe] = "W",
            [0xcb] = "X",
            [0xa4] = "Y",
            [0xcc] = "Z",
            [0xae] = "a",
            [0xad] = "b",
            [0xb6] = "c",
            [0x9e] = "d",
            [0xa3] = "e",
            [0xaf] = "f",
            [0xa9] = "g",
            [0x9b] = "h",
            [0xa1] = "i",
            [0xc7] = "j",
            [0xab] = "k",
            [0xa6] = "l",
            [0xa2] = "m",
            [0x9f] = "n",
            [0x9c] = "o",
            [0xb7] = "p",
            [0xc0] = "q",
            [0xb0] = "r",
            [0xa7] = "s",
            [0xa0] = "t",
            [0xa5] = "u",
            [0xbc] = "v",
            [0x9d] = "w",
            [0xbf] = "x",
            [0xb8] = "y",
            [0xca] = "z",
            [0xef] = "_", //Some special marks
            [0xe1] = "ペ",
            [0xe0] = "‥",
            [0xd6] = " ", //and a space too 
            [0xed] = "◇",
            [0xec] = "♪",
            [0x1eb] = "、",
            [0x1f0] = "°",
            [0xe6] = "«",
            [0xe7] = "»",
            [0x210] = "け", //Japanese
            [0x1a9] = "こ",
            [0x1bf] = "さ",
            [0x1d6] = "し",
            [0x1fa] = "す",
            [0x1c0] = "せ",
            [0x1b8] = "そ",
            [0x19e] = "た",
            [0x20c] = "ち",
            [0x1a0] = "つ",
            [0x1e8] = "て",
            [0x1f2] = "と",
            [0x1b5] = "な",
            [0x203] = "に",
            [0x202] = "ぬ",
            [0x197] = "ね",
            [0x20e] = "の",
            [0x1d7] = "は",
            [0x1ef] = "ひ",
            [0x1e9] = "ふ",
            [0x1ba] = "へ",
            [0x21b] = "ほ",
            [0x1bb] = "ま",
            [0x1f4] = "み",
            [0x1ae] = "む",
            [0x1aa] = "め",
            [0x1c5] = "も",
            [0x205] = "や",
            [0x1f6] = "ゆ",
            [0x193] = "よ",
            [0x1ea] = "ら",
            [0x211] = "り",
            [0x21d] = "る",
            [0x1fb] = "れ",
            [0x20b] = "ろ",
            [0x20f] = "わ",
            [0x204] = "を",
            [0x1a8] = "ん",
            [0x206] = "が",
            [0x1f5] = "ぎ",
            [0x20a] = "ぐ",
            [0x1a1] = "げ",
            [0x1e0] = "ご",
            [0x1be] = "ざ",
            [0x21a] = "じ",
            [0x19c] = "ず",
            [0x1f3] = "ぜ",
            [0x1b1] = "ぞ",
            [0x1b0] = "だ",
            [0x1ec] = "ぢ",
            [0x1ed] = "づ",
            [0x1fc] = "で",
            [0x212] = "ど",
            [0x1ab] = "ば",
            [0x199] = "び",
            [0x215] = "ぶ",
            [0x1a5] = "べ",
            [0x1a2] = "ぼ",
            [0x1b6] = "ぱ",
            [0x1af] = "ぴ",
            [0x218] = "ぷ",
            [0x1a3] = "ぺ",
            [0x1b4] = "ぽ",
            [0x1fd] = "ぁ",
            [0x200] = "ぃ",
            [0x213] = "ぅ",
            [0x1cb] = "ぇ",
            [0x21c] = "ぉ",
            [0x214] = "ゃ",
            [0x1ee] = "ゅ",
            [0x1b3] = "ょ",
            [0x1fe] = "っ",
            [0x198] = "ア",
            [0x1db] = "イ",
            [0x1ad] = "ウ",
            [0x1cc] = "エ",
            [0x1ac] = "オ",
            [0x1ff] = "カ",
            [0x1e1] = "キ",
            [0x1dc] = "ク",
            [0x1b7] = "ケ",
            [0x1b2] = "コ",
            [0x1a4] = "サ",
            [0x1cf] = "シ",
            [0x1b9] = "ス",
            [0x1c3] = "セ",
            [0x21e] = "ソ",
            [0x194] = "タ",
            [0x1d2] = "チ",
            [0x1f1] = "ツ",
            [0x1c6] = "テ",
            [0x1d4] = "ト",
            [0x1c2] = "ナ",
            [0x216] = "ニ",
            [0x1a6] = "ヌ",
            [0x195] = "ネ",
            [0x19f] = "ノ",
            [0x1de] = "ハ",
            [0x1d0] = "ヒ",
            [0x192] = "フ",
            [0x1e5] = "ヘ",
            [0x20d] = "ホ",
            [0x1ca] = "マ",
            [0x1e3] = "ミ",
            [0x1bd] = "ム",
            [0x209] = "メ",
            [0x1f7] = "モ",
            [0x19a] = "ヤ",
            [0x19d] = "ユ",
            [0x1d8] = "ヨ",
            [0x190] = "ラ",
            [0x196] = "リ",
            [0x1c7] = "ル",
            [0x1df] = "レ",
            [0x1c8] = "ロ",
            [0x1bc] = "ワ",
            [0x191] = "ヲ",
            [0x1d3] = "ン",
            [0x1e2] = "ガ",
            [0x1d5] = "ギ",
            [0x1d9] = "グ",
            [0x1e4] = "ゲ",
            [0x1da] = "ゴ",
            [0x1d1] = "ザ",
            [0x217] = "ジ",
            [0x1e7] = "ズ",
            [0x1f9] = "ゼ",
            [0x1c4] = "ゾ",
            [0x219] = "ダ",
            [0x208] = "ヂ",
            [0x1f8] = "ヅ",
            [0x201] = "デ",
            [0x1a7] = "ド",
            [0x1e6] = "バ",
            [0x1c1] = "ビ",
            [0x19b] = "ブ",
            [0x1dd] = "ベ",
            [0x1ce] = "ボ",
            [0x1cd] = "パ",
            [0x1c9] = "ピ",
            [0x207] = "プ",
            [0x3f2] = "ュ",
            [0x581] = "ゥ",
            [0xf1] = "!", //marks
            [0xf9] = "\"",
            [0xeb] = "$",
            [0xfd] = "%",
            [0xff] = "&",
            [0xf3] = "'",
            [0xf6] = "(",
            [0xf7] = ")",
            [0xfa] = "*",
            [0xf4] = ",",
            [0xf8] = "-",
            [0xf0] = ".",
            [0xf5] = ":",
            [0xee] = "=",
            [0xf2] = "?",
            [0x133] = "Á", //Borginian
            [0x134] = "À",
            [0x12f] = "Â",
            [0x129] = "Ä",
            [0x126] = "Ç",
            [0x12a] = "É",
            [0x128] = "È",
            [0x12c] = "Ê",
            [0x12d] = "Ë",
            [0x12e] = "Ẽ",
            [0x135] = "Í",
            [0x12b] = "Ì",
            [0x127] = "Î",
            [0x131] = "Ï",
            [0x136] = "Ñ",
            [0x132] = "Ö",
            [0x130] = "Õ",
            [0xfc] = "é", //special symbols
            [0xfb] = "ä",
            [0xe2] = "ȇ",
            [0xe3] = "à",
            [0xe5] = "！",
            [0xe4] = "ï",
            [0xea] = "â",
            [0xe9] = "ñ",
            [0xe8] = "？",
            [0xfe] = "，",
            [0x220] = "А", //Russian alphabet
            [0x221] = "Б",
            [0x222] = "В",
            [0x223] = "Г",
            [0x224] = "Д",
            [0x225] = "Е",
            [0x226] = "Ё",
            [0x227] = "Ж",
            [0x228] = "З",
            [0x229] = "И",
            [0x22a] = "Й",
            [0x22b] = "К",
            [0x22c] = "Л",
            [0x22d] = "М",
            [0x22e] = "Н",
            [0x22f] = "О",
            [0x230] = "П",
            [0x231] = "Р",
            [0x232] = "С",
            [0x233] = "Т",
            [0x234] = "У",
            [0x235] = "Ф",
            [0x236] = "Х",
            [0x237] = "Ц",
            [0x238] = "Ч",
            [0x239] = "Ш",
            [0x23a] = "Щ",
            [0x23b] = "Ъ",
            [0x23c] = "Ы",
            [0x23d] = "Ь",
            [0x23e] = "Э",
            [0x23f] = "Ю",
            [0x240] = "Я",
            [0x241] = "а",
            [0x242] = "б",
            [0x243] = "в",
            [0x244] = "г",
            [0x245] = "д",
            [0x246] = "е",
            [0x247] = "ё",
            [0x248] = "ж",
            [0x249] = "з",
            [0x24a] = "и",
            [0x24b] = "й",
            [0x24c] = "к",
            [0x24d] = "л",
            [0x24e] = "м",
            [0x24f] = "н",
            [0x250] = "о",
            [0x251] = "п",
            [0x252] = "р",
            [0x253] = "с",
            [0x254] = "т",
            [0x255] = "у",
            [0x256] = "ф",
            [0x257] = "х",
            [0x258] = "ц",
            [0x259] = "ч",
            [0x25a] = "ш",
            [0x25b] = "щ",
            [0x25c] = "ъ",
            [0x25d] = "ы",
            [0x25e] = "ь",
            [0x25f] = "э",
            [0x260] = "ю",
            [0x261] = "я"
        };
    }
}
