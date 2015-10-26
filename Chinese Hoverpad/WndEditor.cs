using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chinese_Hoverpad
{
    public partial class WndEditor : Form
    {
        Dictionary<string, ChineseDefinition> DictChinese;
        Popup MainPopup;

        public WndEditor()
        {
            InitializeComponent();
            //TextEditor.ZoomFactor = 2;
            MainPopup = new Popup();
            InitDictionary();
            
            // TextEditor.Cursor = Cursors.IBeam;
            //TextEditor.Cursor = Cursors.
            MainPopup.Visible = false;
            LabelPopup.Hide();
            LabelPopup.BringToFront();

            TextEditor.MouseWheel += TextEditor_MouseWheel;


            #if DEBUG
            TextEditor.Text = "在歷史上那種事件已經發生了很多次。";
            #else
            TextEditor.Clear();
            #endif
        }

        void TextEditor_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                TextEditor.Font = new Font(TextEditor.Font.FontFamily, Math.Max(8f, TextEditor.Font.Size + (e.Delta > 0 ? 2 : -2)) );
            }
        }

        private void InitDictionary()
        {
            DictChinese = new Dictionary<string, ChineseDefinition>();

            Regex regParseLine = new Regex(@"^(?<TRADITIONAL>.*?)\s+(?<SIMPLIFIED>.*?)\s+(?<DEFINITION>.*)$", RegexOptions.Compiled | RegexOptions.Singleline);


            var lineList = Properties.Resources.cedict_ts.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lineList)
            {
                if (line.StartsWith("#")) continue;

                var match = regParseLine.Match(line);

                if (match.Success)
                {
                    var trad = match.Groups["TRADITIONAL"].Value.Trim(); var simp = match.Groups["SIMPLIFIED"].Value.Trim(); var def = match.Groups["DEFINITION"].Value.Trim();
                    
                    ChineseDefinition existDef;
                    if (DictChinese.TryGetValue(trad, out existDef))
                    {
                        existDef.EnglishDefinition = string.Concat(existDef.EnglishDefinition, "\r\n", def);
                    }
                    else
                    {
                        DictChinese[trad] = new ChineseDefinition(def);
                    }

                    // if the simplified is actually different from the traditional
                    if (!simp.Equals(trad, StringComparison.OrdinalIgnoreCase))
                    {
                        if (DictChinese.TryGetValue(simp, out existDef))
                        {
                            existDef.EnglishDefinition = string.Concat(existDef.EnglishDefinition, "\r\n", def);
                        }
                        else
                        {
                            DictChinese[simp] = new ChineseDefinition(def);
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("Failed to parse: " + line);
                }
            }
        }

        // creates a block of text for most and least likely definitions (tries to sort by size)
        public string GetDefinitions(List<string> words)
        {
            string retValue = string.Empty;

            //List<string> defList = new List<string>();
            var defPairs = new List<KeyValuePair<string, string>>();

            

            foreach (var word in words)
            {
                ChineseDefinition chineseDef = null;
                if (DictChinese.TryGetValue(word, out chineseDef))
                {
                    defPairs.Add(new KeyValuePair<string, string>(word, chineseDef.EnglishDefinition));
                }
            }

            var orderedDefs = defPairs.OrderByDescending(dp => dp.Key.Length);


            return string.Join(Environment.NewLine, orderedDefs.Select(od => string.Concat("[", od.Key, "] = ", od.Value)));
        }

        // if person moves mouse but it stays on same char and same index, and program is currently showing popup, then don't recompute

        private void ExamineText(Point cursorPoint, TextBoxBase editor) // TextEditor.PointToClient(Cursor.Position);
        {
            //string text = editor.Text.ToString();
            //return;

            if (string.IsNullOrWhiteSpace(editor.Text)) { LabelPopup.Visible = false; return; }
            // editor.Refresh();
            // NOTE: We subtract half the size of a drawn character before attempting to use the TextBox GetChar methods,
            // as they seem to get the next character after moving the cursor more than halfway widthwise. 
            // Try moving the mouse halfway across a single character than clicking the mouse, you will note that the cursor
            // will be advanced to the next character.

            
            var charIndex = editor.GetCharIndexFromPosition(cursorPoint);


            var actualCharPoint = editor.GetPositionFromCharIndex(charIndex);

            // recompute if halfway past
            if (cursorPoint.X < actualCharPoint.X && charIndex > 0)
            {
                charIndex--;
                actualCharPoint = editor.GetPositionFromCharIndex(charIndex); // actualCharPoint is now the actual character
            }

            var cursorChar = editor.Text[charIndex];

            if (!string.IsNullOrWhiteSpace(cursorChar.ToString()))
            {

                var trueDist = Math.Sqrt(Math.Pow(cursorPoint.X - actualCharPoint.X, 2d) + Math.Pow(cursorPoint.Y - actualCharPoint.Y, 2d));
                Debug.WriteLine("Char: " + cursorChar + " = " + cursorChar + " was " + trueDist + " pixels away from mouse cursor.");


                SizeF charSize = new SizeF();
                using (var graphics = editor.CreateGraphics())
                {
                    // graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    charSize = graphics.MeasureString(cursorChar.ToString(), editor.Font);

                    RichTextBox richText = editor as RichTextBox;
                    if (richText != null)
                    {
                        charSize.Width = charSize.Width * richText.ZoomFactor;
                        charSize.Height = charSize.Height * richText.ZoomFactor;
                    }
                    else
                    {
                        charSize.Width = charSize.Width;
                        charSize.Height = charSize.Height;
                    }


                    //Debug.WriteLine("Size of char: " + charSize.ToString());
                }


                var maxDist = Math.Sqrt(Math.Pow(charSize.Width, 2d) + Math.Pow(charSize.Height, 2d));

                // TODO: We need to let the whole thing re-compute
                // and then at this point, if the char index and char are identical *AND* a popup is already being shown
                // then don't do anything. In this way, if the program doesn't make it to this point, meaning that it
                // couldn't find anything of value, then it should by default set the popup visible value to false, which
                // will hide the popup

                if (trueDist <= maxDist)
                {
                    // get the possible permutations of this character, up to 4 characters (word combinations)
                    var wordCombos = GetValidStringPermutations(editor.Text, charIndex, 4);
                    Debug.WriteLine("Words: " + GetListString<string>(wordCombos));

                    // what are we displaying now?
                    if (!MainPopup.Visible || MainPopup.LastChar != cursorChar || MainPopup.LastCharIndex != charIndex)
                    {
                        var def = GetDefinitions(wordCombos);
                        // some definitions are really long - how do we break these up.

                        def = BreakText(def, 50);

                        if (!string.IsNullOrWhiteSpace(def))
                        {
                            MainPopup.Text = def;
                            MainPopup.LastChar = cursorChar;
                            MainPopup.LastCharIndex = charIndex;
                            
                            LabelPopup.Text = def;

                            // check the size to make sure it is not bordering right side of screen
                            

                            MainPopup.Position = new Point(editor.Left + cursorPoint.X, editor.Top + cursorPoint.Y + (int)charSize.Height);
                            var maxWidth = Width - 10;
                            if (MainPopup.Position.X + LabelPopup.Width > maxWidth) { MainPopup.Position.X -= ((MainPopup.Position.X + LabelPopup.Width) - maxWidth); }

                            LabelPopup.Visible = true;
                            LabelPopup.Location = MainPopup.Position; //PointToClient(MainPopup.Position);

                            // set a boolean to indicate that the program should show a popup
                            Debug.WriteLine("Definition block\r\n" + def);
                        }
                        else
                        {
                            LabelPopup.Visible = false;
                        }
                    }


                }
                else
                {
                    LabelPopup.Visible = false;
                }
            }
            else
            {
                LabelPopup.Visible = false;
            }

        }

        private string BreakText(string text, int maxCharsPerLine)
        {
            var lines = new List<string>(text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));

            StringBuilder sb = new StringBuilder();

            var curLine = 0;
            while( curLine < lines.Count )
            {
                if(lines[curLine].Length > maxCharsPerLine)
                {
                    lines.Insert(curLine + 1, lines[curLine].Substring(maxCharsPerLine));
                    lines[curLine] = lines[curLine].Substring(0, maxCharsPerLine);
                }
                curLine++;
            }

            return string.Join(Environment.NewLine, lines);
        }


        private void TextEditor_MouseMove(object sender, MouseEventArgs e)
        {
            ExamineText(e.Location, (TextBoxBase)sender);
        }

        List<string> GetValidStringPermutations(string text, int centerIndex, int maxWidth)
        {
            //var invalidChars = new[] { " ", "-", "+", "/", ":", ",", ".", "'", "\"", "~", "!", "?", "。", "，", "！", "？", "”", "‘", "：" };
            var combos = new List<string>(new[] { text.ElementAt(centerIndex).ToString() });

            // String: 0 1 2 3 4 5 6 7 8, Center: 4, Max: 3
            // Return: 34, 234, 345, 45, 456

            // strip out any string that contains a space, period, comma, or other punctuation

            for (int curLength = maxWidth; curLength > 1; curLength--)
            {
                for (int startLeft = Math.Max(0, centerIndex - curLength + 1); startLeft <= centerIndex; startLeft++)
                {
                    if (startLeft + curLength > text.Length) break;
                    var s = text.Substring(startLeft, curLength);
                    combos.Add(s);
                }
            }

            return combos.Where(c => !Regex.IsMatch(c, @"\W")).ToList();
        }


        private string GetListString<T>(List<T> list)
        {
            return string.Concat("[", string.Join(", ", list), "]");
        }

        private void TimerPopup_Tick(object sender, EventArgs e)
        {
            //ExamineText(TextEditor.PointToClient(Cursor.Position));
        }

        private void MenuToSimplified_Click(object sender, EventArgs e)
        {
            TextEditor.Text = Microsoft.VisualBasic.Strings.StrConv(TextEditor.Text, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese);
        }

        private void MenuToTraditional_Click(object sender, EventArgs e)
        {
            TextEditor.Text = Microsoft.VisualBasic.Strings.StrConv(TextEditor.Text, Microsoft.VisualBasic.VbStrConv.TraditionalChinese);
        }

      
    }

}
