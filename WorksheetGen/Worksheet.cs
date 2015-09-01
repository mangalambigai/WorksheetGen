using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using iTextSharp.text;
using iTextSharp.text.pdf;


namespace WorksheetGen
{
    class Worksheet
    {
        BaseColor[] m_basecolor = { new BaseColor(0,10,155),//blue 
                                      new BaseColor(70,0,150),//purple
                                      new BaseColor(0,98,98),//blue green
                                      new BaseColor(128,0,0),//brown
                                      new BaseColor(128,128,0),//red green
                                      new BaseColor(164,82,0),//orange
                                      new BaseColor(0,64,128),//dark blue
                                      new BaseColor(0,128,0),//dark green
                                      new BaseColor(128,0,128),//purple
                                      new BaseColor(65,0,128),//indigo
                                      new BaseColor(75, 105, 46)//yellow green
                                  };
        protected string m_strTitle="";
        protected string m_strFolder = "";
        protected string m_strType = "";
        protected Color m_colorText = Color.Black;
        protected Color m_colorTitle = Color.Black;
        protected Color m_colorLine = Color.Black;
        protected iTextSharp.text.Font m_fNorm, m_fSmall, m_fBig;
        private PdfWriter m_writer;
        protected static Random s_random = new Random();
        protected Document m_doc;

        protected Worksheet(String title, String folderName, int textSize=0, int titleSize=0)
        {
            m_strFolder = folderName;
            m_strTitle = title;
            
        }
        protected BaseColor getNextColor()
        {
            BaseColor clr = m_basecolor[s_random.Next(m_basecolor.Length - 1)];
            return clr;
        }
        public void setColors(Color text, Color title, Color line)
        {
            m_colorTitle = title;
            m_colorText = text;
            m_colorLine = line;
        }
        virtual public void writePdf(bool bBlackAndWhite = false)
        {   
        }

        protected void initFile(bool bBlackAndWhite = false)
        {
            m_doc = new Document();
            string folder = Settings.Default["pdffolder"].ToString() + m_strFolder;
            System.IO.Directory.CreateDirectory(folder );
            string fileName = folder + "/"+m_strTitle;
            if (bBlackAndWhite)
                fileName += "BW.pdf";
            else
                fileName += ".pdf";

            m_writer = PdfWriter.GetInstance(m_doc, new System.IO.FileStream(fileName, System.IO.FileMode.Create));
            m_doc.Open();
            m_fSmall = new iTextSharp.text.Font();
            m_fSmall.Size = 10;

            m_fBig = new iTextSharp.text.Font();
            m_fBig.Size = 40;
            m_fBig.Color = bBlackAndWhite ? BaseColor.BLACK : getNextColor();
//            m_fBig.SetStyle("bold");

            m_fNorm = new iTextSharp.text.Font();
            m_fNorm.Size = 20;
            m_fNorm.Color = new BaseColor(m_colorText);
            
            m_doc.Add(new Paragraph("Name:____________________________________________  Date:______________"/*, fSmall*/));
            m_doc.Add(new Paragraph(m_strTitle, m_fBig));
            m_doc.Add(new Paragraph("   "));

        }
        protected void printSiteName()
        {
            BaseFont bfSmall = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, false);
            PdfContentByte cb = m_writer.DirectContent;
            cb.MoveTo(m_doc.Left, m_doc.Bottom);
            cb.LineTo(m_doc.Right, m_doc.Bottom);
            cb.Stroke();
            cb.BeginText();
            cb.SetTextMatrix(m_doc.Left, m_doc.Bottom - 10);
            cb.SetFontAndSize(bfSmall, 8);

            cb.ShowText(Settings.Default["website_signature"].ToString());
            cb.EndText();
        }
    }
}
