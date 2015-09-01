using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WorksheetGen
{
    class CountingWorksheet:Worksheet
    {
        int m_iCount;
        int m_iMax;
        iTextSharp.text.Image rectangle;
        public CountingWorksheet(string strTitle, string strFolder, int count, int max)
            :base(strTitle, strFolder)
        {
            m_iCount = count;
            m_iMax = max;
            rectangle = iTextSharp.text.Image.GetInstance("\\Resources\\rectangle.gif");
        }
        override public void writePdf(bool bBlackAndWhite=false)
        {
            initFile();
            PdfPTable table = new PdfPTable(m_iCount + 1);
            int cellHeight = ((int)m_doc.PageSize.Height - 200) / m_iCount;
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("");
            image.ScaleAbsolute(cellHeight / 2, cellHeight / 2);
            for (int j = 0; j < m_iCount; j++)
            {
                int cnt = s_random.Next(1, m_iMax);

                for (int i = 0; i < m_iMax; i++)
                {
                    if (i < cnt)
                    {
                        table.AddCell(image);
                    }
                    else
                    {
                        table.AddCell(new Phrase(" "));
                    }

                }

                table.AddCell(rectangle);

            }
            m_doc.Add(table);

            printSiteName();
            m_doc.Close();
        }
    }
}
