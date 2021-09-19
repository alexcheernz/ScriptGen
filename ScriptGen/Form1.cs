using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScriptGen.Classes.ISeq;
using ScriptGen.Functions;

namespace ScriptGen
{
    public partial class Form1 : Form
    {
        static List<Widget> widgetList = new List<Widget>();
        private static List<State> FSA = new List<State>();
        private static List<string> Assumptions = new List<string>();
        private static int loopCount = 1;
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "FSA Text Files (*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] file = File.ReadAllLines(openFileDialog1.FileName);
                foreach (string s in file)
                {
                    switch (s[0])
                    {
                        case 'M':
                            Console.WriteLine("FSA Definitions:");
                            Console.WriteLine(s.Replace("M = ", ""));
                            break;
                        case 'Q':
                            Console.WriteLine("Widgets:");
                            string temp = s.Replace("Q = {", "");
                            temp = temp.Replace("}", "");
                            string[] widgets = temp.Split(',');
                            foreach (string ss in widgets)
                            {
                                Widget newWidget = new Widget();
                                newWidget.Name = ss;
                                if (ss.Contains("textBox"))
                                {
                                    newWidget.Observer = true;
                                }
                                widgetList.Add(newWidget);
                                Console.Write(ss);
                            }
                            Console.Write('\n');
                            break;
                        case 'Σ':
                            Console.WriteLine("Types:");
                            Console.WriteLine(s.Replace("Σ = ", ""));
                            break;
                        case 'δ':
                            Console.WriteLine("State Machine:");
                            string temp2 = s.Replace("δ = {", "");
                            temp2 = temp2.Replace("}", "");
                            string[] states = temp2.Split(new string[]{"),("}, StringSplitOptions.None);
                            State prevState = new State();
                            bool first = true;
                            foreach (string sss in states)
                            {
                                string temp3 = sss.Replace("(", "");
                                temp3 = temp3.Replace(")", "");
                                string[] state = temp3.Split(',');
                                bool loop = false;
                                if (state[0].Equals(state[2]))
                                {
                                    loop = true;
                                }
                                State newState = new State(state[0], first ? new State() : prevState, state[2], state[1], loop);
                                prevState = newState;
                                first = false;
                                FSA.Add(newState);
                            }
                            break;
                        case 'S':
                            Console.WriteLine("Starting state:");
                            Console.WriteLine(s.Replace("S = ", ""));
                            break;
                        case 'F':

                            Console.WriteLine("Final state:");
                            Console.WriteLine(s.Replace("F = ", ""));
                            break;
                        case 'A':
                            Console.WriteLine("Assumptions: ");
                            string data = s.Replace("A = {", "").Replace("}", "");
                            string[] dataArray = data.Split(',');
                            string checkCount = (loopCount + 1).ToString();
                            foreach (string ss in dataArray)
                            {
                                string temp22 = ss.Replace("φ", checkCount);
                                Assumptions.Add(temp22);
                            }
                            break;
                        case 'L':
                            Console.WriteLine("Loop count:");
                            string count = s.Replace("L = {", "").Replace("}", "");
                            loopCount = int.Parse(count);
                            Console.WriteLine(loopCount.ToString());
                            break;
                        default:
                            break;
                    }
                }

                openFileDialog1.Reset();
                openFileDialog1.Filter = "EXE Files (*.exe)|*.exe";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string location = openFileDialog1.FileName;
                    GenTest.GenerateTestScripts(location, FSA, widgetList, Assumptions, loopCount, textBoxStartingObv.Text, textBoxFinalObv.Text);
                }
                
            }
        }
    }
}
