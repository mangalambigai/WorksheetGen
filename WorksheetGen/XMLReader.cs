using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace WorksheetGen
{
    class XMLReader
    {
        public static void createHtml(string folder)
        {
            //TODO: create a html file for this group of worksheets
            //System.IO.FileStream f = System.IO.File.Create(folder+"\\"+folder);
            //f.Write
        }
        public static void addHtml(string file)
        {
            //TODO: create a link to the html file for this worksheet
        }
        public static void processXML()
        {
            string xmlfile = Settings.Default["xmlfile"].ToString();
            if (xmlfile == null || xmlfile.Length == 0)
                return;
            if (!System.IO.File.Exists(xmlfile))
            {
                Console.WriteLine(xmlfile + " does not exist");
                return;
            }
            XmlReader rdr = XmlReader.Create(xmlfile.ToString());
            
            string xmlNodeText;
//common            
            string type="";
            string folderName="";
            
//arithmetic
            int iCols = 0;
            int iRows = 0;
            int iDigits = 0;
            eOperation oper = eOperation.addition;

            //trace, missing, words wtc.
            int iStep = 1;
            int iStart = 0;
            int iEnd = 0;
            int iFillpercent=0;
            int iCount=0;

            string colorText = "black";
            string colorLine = "black";
            string colorTitle = "black";
            string strOperation="";
            string hint = "";

            int iSheetIndex = 0;
            while (rdr.Read())
            {
                switch (rdr.NodeType)
                {
                    case XmlNodeType.Element:
                        int numAttributes = rdr.AttributeCount;
                        string nodeName = rdr.Name;
                        if (nodeName.Equals("group"))
                        {
                            iRows=iCols=iDigits=iStart=iEnd=iFillpercent=iCount=0;
                            iStep = 1;
                            folderName=rdr.GetAttribute("name");
                            iSheetIndex = 0;
                            type = rdr.GetAttribute("type");
                            if (type.Equals("arithmetic"))
                            {
                                strOperation = rdr.GetAttribute("operation");

                                string cols = rdr.GetAttribute("columns");
                                if (cols != null && cols.Length > 0)
                                    iCols = int.Parse(cols);

                                string rows = rdr.GetAttribute("rows");
                                if (rows != null&& rows.Length > 0)
                                    iRows = int.Parse(rows);
                                string digits = rdr.GetAttribute("digits");
                                if (digits != null && digits.Length > 0)
                                    iDigits = int.Parse(digits);
                                hint = rdr.GetAttribute("hint");
                            }
                            else
                            {
                                if (rdr.GetAttribute("start") != null)
                                    iStart = int.Parse(rdr.GetAttribute("start"));
                                if (rdr.GetAttribute("end") != null)
                                    iEnd = int.Parse(rdr.GetAttribute("end"));


                                if (rdr.GetAttribute("count") != null)
                                    iCount = int.Parse(rdr.GetAttribute("count"));

                                string strStep = rdr.GetAttribute("step");
                                if (strStep != null && strStep.Length != 0)
                                {
                                    iStep = int.Parse(rdr.GetAttribute("step"));
                                }

                                string strPercent = rdr.GetAttribute("fillpercent");
                                if (strPercent != null)
                                    iFillpercent = int.Parse(strPercent);
                            }
                            string txtclr = rdr.GetAttribute("textcolor");
                            if (txtclr != null && txtclr.Length > 0)
                                colorText = txtclr;  
                            string ttlclr = rdr.GetAttribute("titlecolor");
                            if (ttlclr != null && ttlclr.Length > 0)
                                colorTitle = ttlclr;
                            string lnclr = rdr.GetAttribute("linecolor");
                            if (lnclr != null && lnclr.Length > 0)
                                colorLine = lnclr;
                            createHtml(folderName);
                        }
                        if (nodeName.Equals("sheet"))
                        {
                            
                            string name = rdr.GetAttribute("name");
                            iSheetIndex++;
                            if (name==null || name.Length == 0)
                                name = folderName + iSheetIndex.ToString();
                            int sheetStart = 0;
                            int sheetEnd = 0;
                            int sheetCount = 0;
                            int sheetStep = 0;
                            int sheetPercent = 0;
                            if (rdr.GetAttribute("start") != null)
                                sheetStart = int.Parse(rdr.GetAttribute("start"));
                            else
                                sheetStart = iStart;

                            if (rdr.GetAttribute("end") != null)
                                sheetEnd = int.Parse(rdr.GetAttribute("end"));
                            else
                                sheetEnd = iEnd;



                            string strStep = rdr.GetAttribute("step");
                            if (strStep != null && strStep.Length != 0)
                            {
                                sheetStep = int.Parse(strStep);
                            }
                            else
                                sheetStep = iStep;

                            string strPercent = rdr.GetAttribute("fillpercent");
                            if (strPercent != null)
                                sheetPercent = int.Parse(strPercent);
                            else
                                sheetPercent = iFillpercent;


                            string strCount = rdr.GetAttribute("count");
                            if (strCount != null)
                                sheetCount = int.Parse(strCount);
                            else
                                sheetCount = iCount;

                            Worksheet sht = null;
                            if (type.Equals("numberstowords") || type.Equals("wordstonumbers"))
                            {
                                sht = new NumbersInWordsWorksheet(name, folderName, type, sheetStart, sheetEnd, sheetCount);
                            }
                            else if (type.Equals("missing") || type.Equals("trace"))
                            {
                                sht = new TracingMissingWorksheet(name,folderName, type, sheetStart, sheetEnd, sheetStep, sheetPercent);
                            }
                            else if (type.Equals("words"))
                            {
                                sht = new WordsWorksheet(name, folderName, sheetCount, sheetStart, sheetEnd, sheetStep);
                            }
                            else if (type.Equals("counting"))
                            {
                                //
                            ////    sht = new CountingWorksheet(name, folderName, sheetCount, sheetEnd);
                            }
                            else if (type.Equals("numbers"))
                            {
                                
                            }
                            else if (type.Equals("arithmetic"))
                            {
                                string strOper = rdr.GetAttribute("operation");
                                if (strOper == null || strOper.Length == 0)
                                    strOper = strOperation;

                                int sheetCols = 0;
                                string cols = rdr.GetAttribute("columns");
                                if (cols != null && cols.Length > 0)
                                    sheetCols = int.Parse(cols);
                                else
                                    sheetCols = iCols;

                                int sheetRows;
                                string rows = rdr.GetAttribute("rows");
                                if (rows != null && rows.Length > 0)
                                    sheetRows = int.Parse(rows);
                                else
                                    sheetRows = iRows;

                                int sheetDigits;
                                string digits = rdr.GetAttribute("digits");
                                if (digits != null && digits.Length > 0)
                                    sheetDigits = int.Parse(digits);
                                else
                                    sheetDigits = iDigits;

                                if (strOper.Contains("addition"))
                                {
                                    oper = eOperation.addition;
                                }
                                if (strOper.Contains("subtraction"))
                                {
                                    oper = eOperation.subtraction;
                                }
                                if (strOper.Equals("division"))
                                {
                                    oper = eOperation.division;
                                }
                                if (strOper.Equals("multiplication"))
                                {
                                    oper = eOperation.multiplication;
                                }
                                string sheethint = rdr.GetAttribute("hint");
                                if (sheethint == null || sheethint.Length == 0)
                                    sheethint = hint;

                                string answer = rdr.GetAttribute("answer");
                                sht = new ArithmeticWorksheet(name, folderName, oper, sheetRows, sheetCols, sheetDigits, answer, sheethint);

                            }
/*                            else
                            {
                                if (rdr.GetAttribute("start") != null)
                                    iStart = int.Parse(rdr.GetAttribute("start"));
                                if (rdr.GetAttribute("end") != null)
                                    iEnd = int.Parse(rdr.GetAttribute("end"));


                                int count = 0;
                                if (rdr.GetAttribute("count") != null)
                                    count = int.Parse(rdr.GetAttribute("count"));

                                int step = 1;
                                if (strStep != null && strStep.Length != 0)
                                {
                                    step = int.Parse(rdr.GetAttribute("step"));
                                }


                                int percent = 0;
                                if (strPercent != null)
                                    percent = int.Parse(strPercent);
                            }
                            string txtclr = rdr.GetAttribute("textcolor");
                            if (txtclr != null && txtclr.Length > 0)
                                colorText = txtclr;
                            string ttlclr = rdr.GetAttribute("titlecolor");
                            if (ttlclr != null && ttlclr.Length > 0)
                                colorTitle = ttlclr;
                            string lnclr = rdr.GetAttribute("linecolor");
                            if (lnclr != null && lnclr.Length > 0)
                                colorLine = lnclr;
                            */

                            if (type.Equals("arithmetic"))
                            {
                                ((ArithmeticWorksheet)sht).generate();
                                sht.writePdf();
                                sht.writePdf(true);
                            }
                            else
                            if (sht != null)
                            {
                              //  sht.setColors(System.Drawing.Color.FromName(colorText), System.Drawing.Color.FromName(colorTitle), System.Drawing.Color.FromName(colorLine));
                                sht.writePdf();
                            }
                            addHtml(name);
                        }

                        break;
                    case XmlNodeType.Text:
                        xmlNodeText = rdr.Value;
                        break;

                    case XmlNodeType.EndElement:
                        xmlNodeText = string.Empty;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
