using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScriptGen.Classes.ISeq;


namespace ScriptGen.Functions
{
    public class StateInterpreter
    {


        public static Tuple<List<string>,List<string>> interpret(List<State> _fsa, List<Widget> widgets, List<string> assumptions, int loopcount, string firstobv, string lastobv)
        {
            List<string> widgetCode = new List<string>();
            List<string> testCode = new List<string>();

            List<State> FSA = _fsa;

            foreach (Widget w in widgets)
            {
                Console.WriteLine("WindowsElement " + w.Name + (w.Observer ? "" : "") + " = session.FindElementByAccessibilityId(\"" + w.Name + "\");");
                widgetCode.Add("WindowsElement " + w.Name + (w.Observer ? "" : "") + " = session.FindElementByAccessibilityId(\"" + w.Name + "\");");
            }

            int obvCount = 0;
            foreach (State s in FSA)
            {
                Button btn = new Button();
                Console.WriteLine("Current state is: " + s.StateName);
                Console.WriteLine("Interaction is: " + s.Action);
                Console.WriteLine("Action should be performed on: " + s.NextState);
                Console.WriteLine(s.canBeLooped ? "Action can be looped" : "Action cannot be looped");
                switch (s.Action)
                {
                    case "click":
                        if (s.canBeLooped)
                        {
                            for (int i = 0; i < loopcount; i++)
                            {
                                Console.WriteLine(s.NextState + ".Click();");
                                testCode.Add(s.NextState + ".Click();");
                            }

                            break;
                        }
                        Console.WriteLine(s.NextState + ".Click();");
                        testCode.Add(s.NextState + ".Click();");
                        break;
                    case "observe":
                        Widget w = widgets.Find(x => x.Name == s.NextState && x.Observer == true);
                        //if (FSA[FSA.Count - 1] == s && lastobv != "")
                        //{
                        //    //Final Observation State
                        //    Console.WriteLine("Assert.AreEqual(" + lastobv + "," +w.Name +".Text);");
                        //    testCode.Add("Assert.AreEqual(" + lastobv + "," + w.Name + ".Text);");
                        //    break;
                        //}
                        //else if (!firstObv && firstobv != "")
                        //{
                        //    firstObv = true;
                        //    Console.WriteLine("Assert.AreEqual(" + firstobv + "," + w.Name + ".Text);");
                        //    testCode.Add("Assert.AreEqual(" + firstobv + "," + w.Name + ".Text);");
                        //    break;
                        //}
                        //figure out if observer has changed
                        Console.WriteLine("Assert.AreEqual("+ assumptions[obvCount] +","+ w.Name + ".Text);");
                        testCode.Add("Assert.IsNotNull("+ assumptions[obvCount] +"," + w.Name + ".Text);");
                        obvCount++;
                        //this could be better!
                        break;
                    default:
                        //throw error
                        break;
                }

            }

            return Tuple.Create(widgetCode, testCode);

        }
    }
}
