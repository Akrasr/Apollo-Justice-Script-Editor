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
            [0xe1] = "`",
            [0xe0] = "‥",
            [0xd6] = " ", //and a space too 
            [0xed] = "◇",
            [0xec] = "♪",
            [0x1eb] = "、",
            [0x1f0] = "°",
            [0xe6] = "«",
            [0xe7] = "»",
            [0x210] = "あ", //Japanese
            [0x1a9] = "い",
            [0x1bf] = "う",
            [0x1d6] = "え",
            [0x1fa] = "お",
            [0x1c0] = "か",
            [0x1b8] = "が",
            [0x19e] = "き",
            [0x20c] = "く",
            [0x1a0] = "け",
            [0x1e8] = "こ",
            [0x1f2] = "さ",
            [0x1b5] = "し",
            [0x203] = "す",
            [0x202] = "せ",
            [0x197] = "そ",
            [0x20e] = "ぞ",
            [0x1d7] = "た",
            [0x1ef] = "だ",
            [0x1e9] = "ち",
            [0x1ba] = "っ",
            [0x21b] = "つ",
            [0x1bb] = "て",
            [0x1f4] = "で",
            [0x1ae] = "と",
            [0x1aa] = "な",
            [0x1c5] = "に",
            [0x205] = "ぬ",
            [0x1f6] = "ね",
            [0x193] = "の",
            [0x1ea] = "は",
            [0x211] = "ぶ",
            [0x21d] = "べ",
            [0x1fb] = "ほ",
            [0x20b] = "ぼ",
            [0x20f] = "ま",
            [0x204] = "み",
            [0x1a8] = "も",
            [0x206] = "ぎ",
            [0x1f5] = "よ",
            [0x20a] = "ら",
            [0x1a1] = "る",
            [0x1e0] = "れ",
            [0x1be] = "ろ",
            [0x21a] = "わ",
            [0x19c] = "を",
            [0x1f3] = "ん",
            [0x1b1] = "ア",
            [0x1b0] = "イ",
            [0x1ec] = "ウ",
            [0x1ed] = "エ",
            [0x1fc] = "オ",
            [0x212] = "カ",
            [0x1ab] = "ガ",
            [0x199] = "キ",
            [0x215] = "ゲ",
            [0x1a5] = "ジ",
            [0x1a2] = "ス",
            [0x1b6] = "タ",
            [0x1af] = "ダ",
            [0x218] = "ッ",
            [0x1a3] = "テ",
            [0x1b4] = "ト",
            [0x1fd] = "ド",
            [0x200] = "ニ",
            [0x213] = "ネ",
            [0x1cb] = "バ",
            [0x21c] = "ヒ",
            [0x214] = "フ",
            [0x1ee] = "ブ",
            [0x1b3] = "ベ",
            [0x1fe] = "ボ",
            [0x198] = "マ",
            [0x1db] = "ミ",
            [0x1ad] = "ュ",
            [0x1cc] = "ラ",
            [0x1ac] = "リ",
            [0x1ff] = "ル",
            [0x1e1] = "レ",
            [0x1dc] = "ロ",
            [0x1b7] = "ワ",
            [0x1b2] = "ン",
            [0x1a4] = "–",
            [0x1cf] = "—",
            [0x1b9] = "上",
            [0x1c3] = "中",
            [0x21e] = "予",
            [0x194] = "事",
            [0x1d2] = "二",
            [0x1f1] = "今",
            [0x1c6] = "会",
            [0x1d4] = "体",
            [0x1c2] = "備",
            [0x216] = "出",
            [0x1a6] = "前",
            [0x195] = "務",
            [0x19f] = "受",
            [0x1de] = "問",
            [0x1d0] = "回",
            [0x192] = "堂",
            [0x1e5] = "場",
            [0x20d] = "土",
            [0x1ca] = "始",
            [0x1e3] = "害",
            [0x1bd] = "屋",
            [0x209] = "師",
            [0x1f7] = "度",
            [0x19a] = "弁",
            [0x19d] = "引",
            [0x1d8] = "怒",
            [0x190] = "成",
            [0x196] = "所",
            [0x1c7] = "捜",
            [0x1df] = "撃",
            [0x1c8] = "査",
            [0x1bc] = "楽",
            [0x191] = "歩",
            [0x1d3] = "死",
            [0x1e2] = "殺",
            [0x1d5] = "消",
            [0x1d9] = "牙",
            [0x1e4] = "現",
            [0x1da] = "琉",
            [0x1d1] = "目",
            [0x217] = "直",
            [0x1e7] = "索",
            [0x1f9] = "聞",
            [0x1c4] = "茜",
            [0x219] = "行",
            [0x208] = "術",
            [0x1f8] = "話",
            [0x201] = "語",
            [0x1a7] = "誰",
            [0x1e6] = "調",
            [0x1c1] = "警",
            [0x19b] = "護",
            [0x1dd] = "質",
            [0x1ce] = "遇",
            [0x1cd] = "遭",
            [0x1c9] = "開",
            [0x207] = "魔",
            [0x3f2] = "ァ",
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
        };
    };
}

