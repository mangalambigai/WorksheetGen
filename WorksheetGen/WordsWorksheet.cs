using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WorksheetGen
{
    class WordsWorksheet:Worksheet
    {
        int m_iCount;
        int m_iStart;
        int m_iEnd;
        int m_iStep;
        public WordsWorksheet(string strTitle, string strFolder, int count, int start, int end, int step)
            :base(strTitle, strFolder)
        {
            m_iCount = count;
            m_iStart = start;
            m_iEnd = end;
            m_iStep = step;
        }
        public override void writePdf(bool bBlackAndWhite = false)
        {
            initFile();

            PdfPTable table;
            iTextSharp.text.Font fNorm = new iTextSharp.text.Font();
            m_iCount = (m_iEnd - m_iStart) / m_iStep;
            if (m_iCount <= 5)
            {
                float[] fWidth = { 4, 10, 10 };
                table = new PdfPTable(fWidth);
                fNorm.Size = 50;
            }
            else if (m_iCount <= 10)
            {
                float[] fWidth = { 4, 10, 10 };
                table = new PdfPTable(fWidth);
                fNorm.Size = 30;
            }
            else
            {
                float[] fWidth = { 4, 10, 10 };
                table = new PdfPTable(fWidth);
                fNorm.Size = 30;
            }
            fNorm.Color = BaseColor.GRAY;
            int cellHeight = ((int)m_doc.PageSize.Height - 200) * m_iStep / (m_iEnd + m_iStep - m_iStart);
            for (int i = m_iStart; i <= m_iEnd; i += m_iStep)
            {
                PdfPCell cell = new PdfPCell(new Phrase(i.ToString(), fNorm));
                PdfPCell cellWords = new PdfPCell(new Phrase(NumberTranslator.ToWords(i), fNorm));
                cell.MinimumHeight = cellHeight;
                cellWords.MinimumHeight = cellHeight;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                cellWords.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                table.AddCell(cellWords);
                table.AddCell(cellWords);
            }
            m_doc.Add(table);


            printSiteName();
            m_doc.Close();
        }
    }
}
