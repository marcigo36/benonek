using HandlebarsDotNet;
using HtmlAgilityPack;
using IronPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace benonek
{
    class Program
    {
        static string[] GetUrls()
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load("http://kozlekedes.bme.hu/oktatas/jarmumernok-bsc-2010/");

            var alap = doc.DocumentNode.SelectNodes("//a[@href]")
                    .Select(l => l.Attributes["href"].Value)
                    .Where(s => s.Contains("tantargy"))
                    .Distinct()
                    .ToArray();

            doc = hw.Load("http://kozlekedes.bme.hu/oktatas/jarmumernok-bsc-2010/szakiranyok/");

            //ezt lehet szebben is de fasozm jo ez
            var szakirany = doc.DocumentNode.SelectNodes("//a[contains(text(),'KOJJA')]")
                    .Select(l => l.Attributes["href"].Value)
                    .Where(s => s.Contains("tantargy"))
                    .Distinct()
                    .ToArray();

            return alap.Concat(szakirany).ToArray();

        }
        static void Main(string[] args)
        {
            GetUrls();
            if (!Directory.Exists("data"))
            {
                Directory.CreateDirectory("data");

                foreach (var url in GetUrls())
                {
                    var subject = new SubjectDataSheet(url);

                    using (TextWriter WriteFileStream = new StreamWriter("data/" + subject.NameEng + ".xml"))
                    {
                        XmlSerializer SerializerObj = new XmlSerializer(typeof(SubjectDataSheet));

                        SerializerObj.Serialize(WriteFileStream, subject);

                        WriteFileStream.Close();
                        Console.WriteLine("data/" + subject.NameEng + ".xml done");
                    }
                    
                }

            }
            else
            {
                var template = Handlebars.Compile(File.ReadAllText("Views\\sablon.hbs"));

                //FileStream ReadFileStream = new FileStream("targy.xml", FileMode.Open, FileAccess.Read, FileShare.Read);

                //// Load the object saved above by using the Deserialize function
                //SubjectDataSheet loadedsubject = (SubjectDataSheet)SerializerObj.Deserialize(ReadFileStream);

                //// Cleanup
                //ReadFileStream.Close();
                //subject.WriteToFile("targy.txt");
                //string result = template(subject);

                //File.WriteAllText("out.html", result);

                //HtmlToPdf printer = new IronPdf.HtmlToPdf();
                //printer.RenderHtmlAsPdf(result).SaveAs("File.pdf");
            }
            Console.ReadKey();
        }
    }
}
