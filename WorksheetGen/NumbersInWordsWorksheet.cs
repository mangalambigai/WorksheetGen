using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WorksheetGen
{
    class NumbersInWordsWorksheet:Worksheet
    {
        int m_iMin;
        int m_iMax;
        int m_iCount;
        public NumbersInWordsWorksheet(string strTitle, string strFolder, string type, int start, int end, int count)
            : base(strTitle, strFolder)
        {
            m_iMin = start;
            m_iMax = end;
            m_iCount = count;
            m_strType = type;
        }
        public override void writePdf(bool bBlackAndWhite = false)
        {
            initFile();
            {
                PdfPTable table;
                int cellHeight = ((int)m_doc.PageSize.Height - 200) / m_iCount;
                if (m_strType.Equals("numberstowords"))
                {
                    if (m_iMax < 100)
                    {
                        float[] width = { 2, 10 };
                        table = new PdfPTable(width);
                        m_fNorm.Size = 50;
                    }
                    else if (m_iMax < 1000)
                    {

                        float[] width = { 2, 10 };
                        table = new PdfPTable(width);
                        m_fNorm.Size = 20;
                    }
                    else
                    {
                        float[] width = { 2, 10 };
                        table = new PdfPTable(width);
                        m_fNorm.Size = 20;
                    }

                }
                else
                {
                    float[] width = { 10, 2 };
                    table = new PdfPTable(width);
                    if (m_iMax > 1000)
                        m_fNorm.Size = 15;
                    if (m_iMax < 100)
                        m_fNorm.Size = 30;
                }

                for (int i = 0; i < m_iCount; i++)
                {
                    int val = s_random.Next(1, m_iMax);
                    PdfPCell cell = new PdfPCell();
                    if (i == 0 || m_strType.Equals("numberstowords"))
                        cell.Phrase = new Phrase(val.ToString(), m_fNorm);
                    else
                        cell.Phrase = new Phrase(" ");
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;

                    PdfPCell cellWords = new PdfPCell();
                    if (i == 0 || m_strType.Equals("wordstonumbers"))
                        cellWords.Phrase = new Phrase(NumberTranslator.ToWords(val), m_fNorm);
                    else
                        cellWords.Phrase = new Phrase(" ");

                    cell.MinimumHeight = cellHeight;
                    cellWords.MinimumHeight = cellHeight;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cellWords.VerticalAlignment = Element.ALIGN_MIDDLE;

                    if (m_strType.Equals("numberstowords"))
                    {
                        table.AddCell(cell);
                        table.AddCell(cellWords);
                    }
                    else
                    {
                        table.AddCell(cellWords);
                        table.AddCell(cell);
                    }
                }
                m_doc.Add(table);
            }
            printSiteName();
            m_doc.Close();
        }
    }
}
