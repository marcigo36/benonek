using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace benonek
{
    public class SubjectDataSheet
    {
        //just for serialization
        public SubjectDataSheet() { }

        public SubjectDataSheet(string URL)
        {
            //adatlap letoltes
            WebClient wc = new WebClient();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(wc.OpenRead(URL),Encoding.Default);

            this.XPathResolve(doc);
            this.ReplaceResolve();
        }

        private void XPathResolve(HtmlDocument doc)
        {
            //ami propertynknek van ilyen attributuma, azt kikeressuk
            var props = typeof(SubjectDataSheet).GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(LocationInHTMLAttribute)))
                .ToArray();

            //vegigiteralunk, es feltoltjuk
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttributes(typeof(LocationInHTMLAttribute), false)
                             .Cast<LocationInHTMLAttribute>()
                             .FirstOrDefault();

                HtmlNode thisnode = doc.DocumentNode.SelectSingleNode(attr.XPath);

                if (attr.Type == LocationInHTMLAttribute.ContentType.HTML)
                {
                    if (thisnode != null)
                    {
                        string input = "This is   text with   far  too   much   whitespace.";
                        string pattern = "\\s+";
                        string replacement = " ";
                        Regex rgx = new Regex(pattern);
                        string result = rgx.Replace(input, replacement);

                        //nullra rakjuk ha nincs, kesobb azt kulon kezeljuk
                        prop.SetValue(this, thisnode?.InnerHtml);
                    }
                    else
                    {
                        //nullra rakjuk ha nincs, kesobb azt kulon kezeljuk
                        prop.SetValue(this, null);
                    }
                }
                else
                {
                    //nullra rakjuk ha nincs, kesobb azt kulon kezeljuk
                    prop.SetValue(this, thisnode?.InnerText);
                }
            }
        }

        private void ReplaceResolve()
        {
            //most a replaceseket keressuk ki
            var props = typeof(SubjectDataSheet).GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(ReplaceAttribute)))
                .ToArray();

            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(typeof(ReplaceAttribute), false)
                             .Cast<ReplaceAttribute>()
                             .ToArray();

                foreach (var attr in attrs)
                {
                    prop.SetValue(this, (prop.GetValue(this) as string).Replace(attr.Old, attr.New));
                }
            }
        }
        //header
        [LocationInHTML("//*[@id='adatlap']/table//tr[1]/td[2]")]
        public string NameHun { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[2]/td[2]")]
        public string NameEng { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[2]/td[4]")]
        public string NameShort { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[3]/td[2]")]
        public string Code { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[3]/td[4]")]
        public string CreditNo { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[4]/td[2]")]
        [Replace("előadás", "lectures")]
        public string LectureNo { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[4]/td[3]")]
        [Replace("gyakorlat", "practices")]
        public string PracticeNo { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[4]/td[4]")]
        [Replace("labor", "laboratory")]
        public string LabNo { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[4]/td[6]")]
        [Replace("Vizsgajegy", "Examination")]
        [Replace("Félévközi jegy", "Graded upon course work")]
        public string Requirement { get; set; }

        //tanszek info

        [LocationInHTML("//*[@id='adatlap']/table//tr[6]/td[2]")]
        public string DepartmentName { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[6]/td[4]")]
        public string ResponsibleTeacher { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[7]/td[2]")]
        public string Educators { get; set; }

        //elotanulmany

        [LocationInHTML("//*[@id='adatlap']/table//tr[9]/td[2]")]
        [Replace("nincs","none")]
        public string PreStudy { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[10]/td[2]")]
        [Replace("nincs", "none")]
        public string RequiredtFor { get; set; }

        //orak

        [LocationInHTML("//*[@id='adatlap']/table//tr[12]/td[2]")]
        [Replace("óra", "hours")]
        public string TotalHours { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[13]/td[2]")]
        [Replace("óra", "hours")]
        public string ContactHours { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[13]/td[4]")]
        [Replace("óra", "hours")]
        public string WrittenStudyMaterials { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[13]/td[6]")]
        [Replace("óra", "hours")]
        public string HomeworkHours { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[14]/td[2]")]
        [Replace("óra", "hours")]
        public string ClassPrepHours { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[14]/td[4]")]
        [Replace("óra", "hours")]
        public string TestPrepHours { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[14]/td[6]")]
        [Replace("óra", "hours")]
        public string ExamPrepHours { get; set; }

        //reszletes infok

        [LocationInHTML("//*[@id='adatlap']/table//tr[17]/td[1]",Type = LocationInHTMLAttribute.ContentType.HTML)]
        public string TaskAndAim { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[19]/td[1]",Type = LocationInHTMLAttribute.ContentType.HTML)]
        public string Description { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[string()='Gyakorlat']/following::tr[1]/td[1]",Type = LocationInHTMLAttribute.ContentType.HTML)]
        public string ClassPractice { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[string()='Labor']/following::tr[1]/td[1]",Type = LocationInHTMLAttribute.ContentType.HTML)]
        public string LabPractice { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[string()='Egyéni hallgatói feladat']/following::tr[1]/td[1]",Type = LocationInHTMLAttribute.ContentType.HTML)]
        public string IndividualWork { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[last()-2]/td[1]",Type = LocationInHTMLAttribute.ContentType.HTML)]
        public string PassingCriteria { get; set; }

        [LocationInHTML("//*[@id='adatlap']/table//tr[last()]/td[1]",Type = LocationInHTMLAttribute.ContentType.HTML)]
        public string Literature { get; set; }

        //public void WriteToFile(string path)
        //{
        //    //most a replaceseket keressuk ki
        //    var props = typeof(SubjectDataSheet).GetProperties();

        //    using (var file = File.CreateText(path))
        //    {
        //        foreach (var prop in props)
        //        {
        //            file.WriteLine("##" + prop.Name + "#:");
        //            file.WriteLine(prop.GetValue(this) as string);
        //        }
        //    }
        //}
    }
}
