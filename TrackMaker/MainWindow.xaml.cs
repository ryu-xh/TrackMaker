﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrackMaker
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string GetLocalizaionString(string key)
        {
            string uiString;

            ResourceManager rm = new ResourceManager("TrackMaker.Strings", Assembly.GetExecutingAssembly());

            uiString = rm.GetString(key);

            return uiString;
        }


        private string GetSingers()
        {
            string path = Directory.GetCurrentDirectory();

            string[] dirs = Directory.GetDirectories(path + "\\Singers", "*", SearchOption.TopDirectoryOnly);

            foreach (string dir in dirs)
            {
                // string characterFile = File.ReadAllText(dir + "\\character.txt", Encoding.GetEncoding("shift_jis"));
                // Regex nameRegex = new Regex("name=([\\w ]+)");

                // Match m = nameRegex.Match(characterFile);

                // if (m.Success)
                //{
                SingerCombo.Items.Add(dir.Replace(path + "\\Singers\\", ""));
                SingerCombo.SelectedItem = dir.Replace(path + "\\Singers\\", "");
                // }

            }

            return path;
        }

        private void GetPhonemizers()
        {
            PhonemizerCombo.Items.Add("ArpabetG2p");
            PhonemizerCombo.Items.Add("ArpasingPhonemizer");
            PhonemizerCombo.Items.Add("BrazilianPortugueseCVCPhonemizer");
            PhonemizerCombo.Items.Add("ChineseCVVCPhonemizer");
            PhonemizerCombo.Items.Add("ChineseCVVPhonemizer");
            PhonemizerCombo.Items.Add("DefaultPhonemizer");
            PhonemizerCombo.Items.Add("EnglishVCCVPhonemizer");
            PhonemizerCombo.Items.Add("ENtoJAPhonemizer");
            PhonemizerCombo.Items.Add("FrenchCMUSphinxPhonemizer");
            PhonemizerCombo.Items.Add("FrenchCVVCPhonemizer");
            PhonemizerCombo.Items.Add("FrenchG2p");
            PhonemizerCombo.Items.Add("ItalianCVVCPhonemizer");
            PhonemizerCombo.Items.Add("JapaneseCVVCPhonemizer");
            PhonemizerCombo.SelectedItem = "JapaneseCVVCPhonemizer";
            PhonemizerCombo.Items.Add("JapaneseVCVPhonemizer");
            PhonemizerCombo.Items.Add("KoreanCVCCVPhonemizer");
            PhonemizerCombo.Items.Add("KoreanCVCPhonemizer");
            PhonemizerCombo.Items.Add("KoreanCVVCStandardPronunciationPhonemizer");
            PhonemizerCombo.Items.Add("KoreanVCVPhonemizer");
            PhonemizerCombo.Items.Add("LatinDiphonePhonemizer");
            PhonemizerCombo.Items.Add("PolishCVCPhonemizer");
            PhonemizerCombo.Items.Add("PortugueseG2p");
            PhonemizerCombo.Items.Add("RussianCVCPhonemizer");
            PhonemizerCombo.Items.Add("RussianG2p");
            PhonemizerCombo.Items.Add("RussianVCCVPhonemizer");
            PhonemizerCombo.Items.Add("SpanishSyllableBasedPhonemizer");
            PhonemizerCombo.Items.Add("SyllableBasedPhonemizer");
            PhonemizerCombo.Items.Add("TetoEnglishPhonemizer");
            PhonemizerCombo.Items.Add("VietnameseCVVCPhonemizer");
            PhonemizerCombo.Items.Add("VietnameseVCVPhonemizer");
        }

        private Dictionary<string, int> toneDict = new Dictionary<string, int>();
        private void GetTones()
        {
            string[] arr = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
            // C1은 24

            int c1 = 24;
            for(int i = 0; i < 6; i++)
            {
                for(int j = 0; j < arr.Length; j++)
                {
                    toneDict.Add(arr[j] + (i + 1).ToString(), c1 + j + (i * arr.Length));
                    ToneCombo.Items.Add(arr[j] + (i + 1).ToString());
                }
            }

            ToneCombo.SelectedItem = "C3";

        }

        private string GetUstxFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                return File.ReadAllText(openFileDialog.FileName);
            }

            return null;
        }

        string ustxStream = null;

        private void SaveUstxFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Ustx File|*.ustx";

            if (saveFileDialog.ShowDialog() == true)
            {
                string ustx = "name: New Project\ncomment: ''\noutput_dir: Vocal\ncache_dir: UCache\nustx_version: 0.5\n";
                ustx += "bpm: " + bpmText.Text + "\n";
                ustx += "beat_per_bar: 4\nbeat_unit: 4\nresolution: 480\n";
                ustx += "expressions:\n  dyn:\n    name: dynamics (curve)\n    abbr: dyn\n    type: Curve\n    min: -240\n    max: 120\n    default_value: 0\n    is_flag: false\n    flag: ''\n  pitd:\n    name: pitch deviation (curve)\n    abbr: pitd\n    type: Curve\n    min: -1200\n    max: 1200\n    default_value: 0\n    is_flag: false\n    flag: ''\n  clr:\n    name: voice color\n    abbr: clr\n    type: Options\n    min: 0\n    max: 0\n    default_value: 0\n    is_flag: false\n    options:\n    - ''\n  eng:\n    name: resampler engine\n    abbr: eng\n    type: Options\n    min: 0\n    max: 1\n    default_value: 0\n    is_flag: false\n    options:\n    - ''\n    - worldline\n  vel:\n    name: velocity\n    abbr: vel\n    type: Numerical\n    min: 0\n    max: 200\n    default_value: 100\n    is_flag: false\n    flag: ''\n  vol:\n    name: volume\n    abbr: vol\n    type: Numerical\n    min: 0\n    max: 200\n    default_value: 100\n    is_flag: false\n    flag: ''\n  atk:\n    name: attack\n    abbr: atk\n    type: Numerical\n    min: 0\n    max: 200\n    default_value: 100\n    is_flag: false\n    flag: ''\n  dec:\n    name: decay\n    abbr: dec\n    type: Numerical\n    min: 0\n    max: 100\n    default_value: 0\n    is_flag: false\n    flag: ''\n  gen:\n    name: gender\n    abbr: gen\n    type: Numerical\n    min: -100\n    max: 100\n    default_value: 0\n    is_flag: true\n    flag: g\n  genc:\n    name: gender (curve)\n    abbr: genc\n    type: Curve\n    min: -100\n    max: 100\n    default_value: 0\n    is_flag: false\n    flag: ''\n  bre:\n    name: breath\n    abbr: bre\n    type: Numerical\n    min: 0\n    max: 100\n    default_value: 0\n    is_flag: true\n    flag: B\n  brec:\n    name: breathiness (curve)\n    abbr: brec\n    type: Curve\n    min: -100\n    max: 100\n    default_value: 0\n    is_flag: false\n    flag: ''\n  lpf:\n    name: lowpass\n    abbr: lpf\n    type: Numerical\n    min: 0\n    max: 100\n    default_value: 0\n    is_flag: true\n    flag: H\n  mod:\n    name: modulation\n    abbr: mod\n    type: Numerical\n    min: 0\n    max: 100\n    default_value: " + modText.Text + "\n    is_flag: false\n    flag: ''\n  alt:\n    name: alternate\n    abbr: alt\n    type: Numerical\n    min: 0\n    max: 16\n    default_value: 0\n    is_flag: false\n    flag: ''\n  shft:\n    name: tone shift\n    abbr: shft\n    type: Numerical\n    min: -36\n    max: 36\n    default_value: 0\n    is_flag: false\n    flag: ''\n  shfc:\n    name: tone shift (curve)\n    abbr: shfc\n    type: Curve\n    min: -1200\n    max: 1200\n    default_value: 0\n    is_flag: false\n    flag: ''\n  tenc:\n    name: tension (curve)\n    abbr: tenc\n    type: Curve\n    min: -100\n    max: 100\n    default_value: 0\n    is_flag: false\n    flag: ''\n  voic:\n    name: voicing (curve)\n    abbr: voic\n    type: Curve\n    min: 0\n    max: 100\n    default_value: 100\n    is_flag: false\n    flag: ''";
                
                ustx += "tracks:\n";
                // track 반복
                for (int i = 0; i < TrackList.Items.Count; i++)
                {
                    ustx += "- singer: " + SingerCombo.Text + "\n";
                    ustx += "  phonemizer: OpenUtau.Plugin.Builtin." + PhonemizerList.Items[i] + "\n";
                    ustx += "  renderer: CLASSIC\n";
                    ustx += "  mute: false\n  solo: false\n  volume: 0\n";
                }

                // voice_parts 반복
                ustx += "voice_parts:\n";

                for (int i = 0; i < TrackList.Items.Count; i++)
                {

                    Console.WriteLine(TrackList.Items[i]);
                    ustx += "- name: " + TrackList.Items[i].ToString() + "\n";
                    ustx += "  comment: ''\n";
                    ustx += "  track_no: " + i.ToString() + "\n";
                    ustx += "  position: 0\n";

                    // 여기서 글자 분리
                    ustx += "  notes:\n";

                    string str = TrackList.Items[i].ToString();
                    if (SplitType0.IsChecked == true)
                    {
                        // 글자 단위 분리
                        Regex lyricRegex = new Regex("([가-힣あ-んア-ヴ][ぁぃぅぇぉゃゅょァィゥェォャュョ][ぁぃぅぇぉァィゥェォ]|[가-힣あ-んア-ヴ][ぁぃぅぇぉゃゅょァィゥェォャュョ]|[가-힣あ-んア-ヴ])");

                        int j = 0;
                        while(str != "")
                        {
                            Match m = lyricRegex.Match(str);

                            if (m.Success)
                            {

                                ustx += "  - position: " + (960 + (480 * j)).ToString() + "\n";
                                ustx += "    duration: 480\n";
                                ustx += "    tone: " + toneDict[ToneCombo.Text].ToString() + "\n";
                                ustx += "    lyric: " + m.Groups[0].Value + "\n";
                                ustx += "    pitch:\n      data:\n      - {x: -40, y: 0, shape: io}\n      - {x: 40, y: 0, shape: io}\n      snap_first: true\n    vibrato: {length: 0, period: 175, depth: 25, in: 10, out: 10, shift: 0, drift: 0}\n    phoneme_expressions: []\n    phoneme_overrides: []\n";

                            }

                            str = str.Substring(m.Groups[0].Index + m.Groups[0].Value.Length, str.Length - m.Groups[0].Index - m.Groups[0].Value.Length);
                            j++;
                        }

                    } else
                    {
                        // 언더바 단위 분리
                        string[] splitedStr = str.Split('_');

                        for(int j = 0; j < splitedStr.Length; j++)
                        {   
                            if(splitedStr[j] != "")
                            {
                                ustx += "  - position: " + (960 + (480 * j)).ToString() + "\n";
                                ustx += "    duration: 480\n";
                                ustx += "    tone: " + toneDict[ToneCombo.Text].ToString() + "\n";
                                ustx += "    lyric: " + splitedStr[j] + "\n";
                                ustx += "    pitch:\n      data:\n      - {x: -40, y: 0, shape: io}\n      - {x: 40, y: 0, shape: io}\n      snap_first: true\n    vibrato: {length: 0, period: 175, depth: 25, in: 10, out: 10, shift: 0, drift: 0}\n    phoneme_expressions: []\n    phoneme_overrides: []\n";

                            }
                            
                        }
                    }

                    
                }

                File.WriteAllText(saveFileDialog.FileName, ustx);

                MessageBox.Show(GetLocalizaionString("Saved"));
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            SaveUstxFile();
        }

        private string GetTextFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                string[] lines;
                if (UTF8Radio.IsChecked == true)
                {
                    lines = File.ReadAllLines(openFileDialog.FileName, Encoding.UTF8);
                } else
                {
                    lines = File.ReadAllLines(openFileDialog.FileName, Encoding.GetEncoding("shift_jis"));
                }
                


                foreach (string line in lines)
                {
                    if (line != "")
                    {
                        Console.WriteLine(line);
                        TrackList.Items.Add(line);
                        PhonemizerList.Items.Add(PhonemizerCombo.Text);
                    }
                    
                }

            }

            return null;
        }

        string textStream = null;
        private void OpenTextFile_Click(object sender, RoutedEventArgs e)
        {
            GetTextFile();
        }

        public class Track
        {
            public string Name { get; set; }

        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            GetSingers();
            GetPhonemizers();
            GetTones();
        }

        private void bpmScroll_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            bpmText.Text = bpmScroll.Value.ToString();
        }

        private void modScroll_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            modText.Text = modScroll.Value.ToString();
        }
    }
}
