using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WorksheetGen
{
    class TracingMissingWorksheet:Worksheet
    {
        int m_iStart;
        int m_iEnd;
        int m_iStep;
        int m_iFillPercent;
        public TracingMissingWorksheet(string strTitle, string strFolder, string type,int start, int end, int step, int fillPercent)
            : base(strTitle, strFolder)
        {
            m_strType = type;
            m_iStart = start;
            m_iEnd = end;
            m_iStep = step;

            m_iFillPercent = fillPercent;

        }
        public override void writePdf(bool bBlackAndWhite = false)
        {
            initFile();
            if (m_iStart > m_iEnd && m_iStep > 0)
                return;
            if (m_iStart < m_iEnd && m_iStep < 0)
                return;
            if (m_iStart == m_iEnd)
                return;
            if (m_strType.Equals("trace"))
                m_fNorm.Color = BaseColor.LIGHT_GRAY;
            int iCols = 5;
            int iNums = 1 + ((m_iEnd - m_iStart) / m_iStep);
            int iRepeat = 1;
            switch (iNums)
            {
                case 100:
                    iCols = 10;
                    break;
                case 10:
                    iCols = 5;
                    iRepeat = 2;
                    break;
                case 50:
                    iCols = 5;
                    break;
                case 20:
                    iCols = 5;
                    break;
                case 5:
                    iCols = 5;
                    iRepeat = 5;
                    break;
                default:
                    iCols = 5;
                    break;
            }
            int iRows = iNums * iRepeat / iCols;
            if (iRows == 0) iRows = 1;
            int cellHeight = ((int)m_doc.PageSize.Height - 250) / iRows;
            if (iNums <= 10 && m_iEnd < 100)
                m_fNorm.Size = 70;
            else if (iNums <= 20)
            {
                if (m_iEnd < 100)
                    m_fNorm.Size = 50;
                else
                    m_fNorm.Size = 40;
            }
            else if (iNums <= 50)
                m_fNorm.Size = 40;
            else
                m_fNorm.Size = 25;
            PdfPTable table;
            if (iNums < 100)
                table = new PdfPTable(iCols);
            else
            {
                float[] f = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                if (m_iEnd >= 100)
                    f[9] = 1.5F;
                else
                    f[0] = 1.5F;
                table = new PdfPTable(f);
            }


            if (m_iStep > 0)
            {
                for (int j = 0; j < iRepeat; j++)
                {
                    for (int i = m_iStart; i <= m_iEnd; i += m_iStep)
                    {
                        writeNum(table, i, cellHeight);
                    }
                }
            }
            else
            {
                for (int j = 0; j < iRepeat; j++)
                {
                    for (int i = m_iStart; i >= m_iEnd; i += m_iStep)
                    {
                        writeNum(table, i, cellHeight);
                    }
                }
            }

            m_doc.Add(table);

            printSiteName();
            m_doc.Close();

        }
        
        void writeNum(PdfPTable table, int i, int cellHeight)
        {
            PdfPCell cell;
            if (m_iFillPercent > 0 && i != m_iStart && i != m_iEnd && s_random.Next(100) > m_iFillPercent)
            {
                cell = new PdfPCell(new Phrase(" ", m_fNorm));
                cell.MinimumHeight = cellHeight;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
            }
            else
            {
                cell = new PdfPCell(new Phrase(i.ToString(), m_fNorm));
                cell.MinimumHeight = cellHeight;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
            }
            table.AddCell(cell);
        }

    }
}
