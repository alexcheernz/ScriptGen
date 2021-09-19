using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScriptGen.Classes.ISeq;

namespace ScriptGen.Functions
{
    public class GenTest
    {
        public static void GenerateTestScripts(string exeLocation, List<State> fsa, List<Widget> widgets, List<string> assumptions, int loopcount, string startingObv, string finalObv)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var appsession = "ScriptGen.Headers.AppSession.txt";

            string appsessionString = "";
            using (Stream stream = assembly.GetManifestResourceStream(appsession))
            using (StreamReader reader = new StreamReader(stream))
            {
                appsessionString = reader.ReadToEnd();
            }

            appsessionString = appsessionString.Replace("%%EXELOCATION%%", exeLocation);
            
            string appName = Path.GetFileNameWithoutExtension(exeLocation);
            appsessionString = appsessionString.Replace("%%NAMESPACE%%", appName + "Test");
            appsessionString = appsessionString.Replace("%%APPNAME%%", appName);

            Directory.CreateDirectory("./out/");
            File.WriteAllText("./out/AppSession.cs",appsessionString);
            
            var data = StateInterpreter.interpret(fsa, widgets, assumptions,loopcount,String.IsNullOrEmpty(startingObv) ? "" : "startingObv", String.IsNullOrEmpty(finalObv) ? finalObv : "startingObv" );
            List<string> widgetsData = data.Item1;
            List<string> testingCode = data.Item2;

            StringBuilder sb = new StringBuilder();
            foreach (string s in widgetsData)
            {
                sb.Append(s + '\n');
            }
            string wDataFinal = sb.ToString();

            StringBuilder sb2 = new StringBuilder();
            foreach (string ss in testingCode)
            {
                sb2.Append(ss + '\n');
            }

            string tcDataFinal = sb2.ToString();

            var testtemplateres = "ScriptGen.Headers.UnitTestTemplate.txt";
            string testScript = "";
            using (Stream stream = assembly.GetManifestResourceStream(testtemplateres))
            using (StreamReader reader = new StreamReader(stream))
            {
                testScript = reader.ReadToEnd();
            }

            testScript = testScript.Replace("%%NAMESPACE%%", appName + "Test");
            testScript = testScript.Replace("%%APPNAME%%", appName);
            //TODO: change this to FSA name?
            testScript = testScript.Replace("%%TESTNAME%%", "One");
            testScript = testScript.Replace("%%SCRIPTDATA%%", wDataFinal + '\n' + tcDataFinal);
            File.WriteAllText("./out/UnitTestOne.cs", testScript);
            MessageBox.Show("Test scripts generated in ./out/");

        }
    }
}
