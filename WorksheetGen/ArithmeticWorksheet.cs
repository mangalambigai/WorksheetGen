using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WorksheetGen
{
    class ArithmeticWorksheet:Worksheet
    {
        decimal iRows;
        decimal iCols;
        eOperation eOper;
        bool bRegroup;
        decimal iDigits;
        string m_strSecret;
        Problem[] problems;

        //only two numbers for now
        int addend = 2;
        int[] key;
        string strHint;

        public ArithmeticWorksheet(string strTitle, string folderName,eOperation oper, int rows, int cols, int digits, string sec, string hint)
            :base(strTitle, folderName)
        {
                eOper = oper;
                iRows = rows;
                iCols = cols;
                iDigits = digits;
                strHint = hint;
                m_strSecret = sec;

        }
        private bool answerAlreadyExists(int newVal)
        {
            for (int i = 0; i < iRows * iCols; i++)
            {
                if (problems != null && problems[i] != null && problems[i].getAnswer() == newVal)
                    return true;

            }
            return false;
        }

        public int findKey(char c)
        {
            for (int row = 0; row < iRows; row++)
            {
                for (int col = 0; col < iCols; col++)
                {
                    int loc = row * (int)iCols + col;
                    if (problems[loc].getKey() == c)
                    {
                        return problems[loc].getAnswer();
                    }

                }
            }
            return 0;
        }

        public bool generate()
        {
            Random rnd = new Random();

            char[] strRem = new char[(int)iRows * (int)iCols];

            int pos = 0;

            if (iRows * iCols < m_strSecret.Length)
            {
                for (int i = 0; i < m_strSecret.Length; i++)
                {
                    if (m_strSecret[i] != ' ')
                    {
                        strRem[pos++] = m_strSecret[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_strSecret.Length; i++)
                {
                    if (pos == iRows * iCols)
                    {
                        Console.WriteLine("not enough problems for the secret code");
                        return false;
                    }
                    if (m_strSecret[i] != ' ')
                    {
                        if (strRem.Contains(m_strSecret[i]) == false)
                            strRem[pos++] = m_strSecret[i];
                    }
                }
            }

            //add enough letters so eachproblem has a letter;
            while (pos < iRows * iCols)
            {
                strRem[pos++] = (char)(rnd.Next(26) + (int)'A');
            }

            int minValue = (int)System.Math.Pow(10, (double)iDigits - 1);
            int maxValue = (int)System.Math.Pow(10, (double)iDigits) - 1;
            problems = new Problem[(int)iRows * (int)iCols];
            key = new int[m_strSecret.Length];

            for (int i = 0; i < iRows * iCols; i++)
            {
                

                int[] nextVal = new int[addend];
                int ans = 0;
                switch (eOper)
                {
                    case eOperation.addition:
                        for (int j = 0; j < addend; j++)
                        {
                            nextVal[j] = rnd.Next(minValue, maxValue);
                            ans += nextVal[j];
                        }
                        break;
                    case eOperation.multiplication:
                        ans = 1;
                        for (int j = 0; j < addend; j++)
                        {
                            nextVal[j] = rnd.Next(minValue, maxValue);
                            ans *= nextVal[j];
                        }
                        break;
                    case eOperation.subtraction:
                        nextVal[0] = rnd.Next(minValue, maxValue);
                        nextVal[1] = rnd.Next(minValue, nextVal[0]);
                        ans = nextVal[0] - nextVal[1];
                        break;
                    case eOperation.division:
                        nextVal[1] = rnd.Next(minValue, maxValue);
                        ans = rnd.Next(minValue, maxValue);
                        nextVal[0] = ans * nextVal[1];
                        break;
                }

                //we can't have two letters matching same number
                if (answerAlreadyExists(ans))
                {
                    i--;
                    continue;
                }
                problems[i] = new Problem();

                //choose a letter from the secret code.
                int secretLetter = rnd.Next(strRem.Length);

                char c = strRem[secretLetter];

                string strRemNew = "";
                if (secretLetter > 0)
                    strRemNew = new string(strRem, 0, secretLetter);
                if (strRem.Length > secretLetter)
                {
                    strRemNew += new string(strRem).Substring(secretLetter + 1);
                }
                strRem = strRemNew.ToCharArray();

                problems[i].setVal(nextVal, eOper, c, ans);


            }
            return true;
        }
        public decimal getRows()
        {
            return iRows;
        }
        public decimal getCols()
        {
            return iCols;
        }

        public Problem[] getProblems()
        {
            return problems;
        }

        public override void writePdf(bool bBlackAndWhite=false)
        {

            BaseColor colorLine = bBlackAndWhite ? BaseColor.BLACK : getNextColor();
            initFile(bBlackAndWhite);
            PdfPTable tableProblems = new PdfPTable((int)iCols);
            for (int i = 0; i < iRows; i++)
            {
                iTextSharp.text.Font f = new iTextSharp.text.Font(m_fNorm);
                f.Size = 25;
                f.Color = bBlackAndWhite?BaseColor.BLACK:getNextColor();
                for (int j = 0; j < iCols; j++)
                {
                    int loc = i * (int)iCols + j;
                    Problem p = problems[loc];
                    if (p == null)
                    {
                        Console.WriteLine("p was null");
                    }
                    else
                    {
                        PdfPTable prb = new PdfPTable(2);

                        PdfPTable tabkeyop = new PdfPTable(1);
                        PdfPCell cellKey = new PdfPCell(new Phrase(p.getKey().ToString(), f));
                        cellKey.VerticalAlignment = 1;
                        cellKey.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        tabkeyop.AddCell(cellKey);

                        string strOp = "+";
                        switch (p.getOperation())
                        {
                            case eOperation.addition:
                                strOp = "+";
                                break;
                            case eOperation.subtraction:
                                strOp = "-";
                                break;
                            case eOperation.multiplication:
                                strOp = "X";
                                break;
                            case eOperation.division:
                                strOp = "/";
                                break;

                        }
                        PdfPCell cellOp = new PdfPCell(new Phrase(strOp, f));
                        cellOp.VerticalAlignment = 3;
                        cellOp.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellOp.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        tabkeyop.AddCell(cellOp);

                        PdfPCell cellkeyop = new PdfPCell(tabkeyop);
                        cellkeyop.Border = iTextSharp.text.Rectangle.NO_BORDER; ;
                        prb.AddCell(cellkeyop);
                        PdfPTable nums = new PdfPTable(1);

                        PdfPCell cellEmpty = new PdfPCell(new Phrase("    "));
                        cellEmpty.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        nums.AddCell(cellEmpty);

                        for (int k = 0; k < 2; k++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(p.getNumbers()[k].ToString() + "  ", f));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            nums.AddCell(cell);

                        }
                        PdfPCell ans = new PdfPCell(new Phrase("    "));
                        //TODO: handle line color..
                        //ans.BorderColor = BaseColor.;
                        ans.MinimumHeight = 50;
                        ans.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                        ans.BorderColor = colorLine;
                        nums.AddCell(ans);

                        PdfPCell cellnums = new PdfPCell(nums);
                        cellnums.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        prb.AddCell(cellnums);

                        PdfPCell cellPrb = new PdfPCell(prb);
                        cellPrb.BorderColor = colorLine;
                        tableProblems.AddCell(cellPrb);
                    }
                }
            }
            m_doc.Add(tableProblems);

            m_doc.Add(new Paragraph("   "));
            m_fNorm.Color = bBlackAndWhite ? BaseColor.BLACK : getNextColor();
            m_doc.Add(new Paragraph(strHint, m_fNorm));
            //            doc.Add(new Paragraph("   "));
            //display key
            m_fNorm.Color = bBlackAndWhite ? BaseColor.BLACK : getNextColor();
            if (m_strSecret.Contains(' ') == false)
                printAnswerTable(m_strSecret);
            else
            {
                string[] strWords = m_strSecret.Split(' ');
                foreach (string word in strWords)
                {
                    printAnswerTable(word);
                }
            }
            printSiteName();
            m_doc.Close();
        }
        private void printAnswerTable(string strSecret)
        {
            m_doc.Add(new Paragraph());
            if (m_strSecret.Length==strSecret.Length || iRows<4)
                m_doc.Add(new Paragraph("   "));
            if (iDigits >= 3 && strSecret.Length > 7)
                m_fNorm.Size = 15;
            if (iDigits >= 2 && strSecret.Length > 10)
                m_fNorm.Size = 15;
            if (strSecret.Length > 0)
            {
                PdfPTable secret = new PdfPTable(strSecret.Length);
                for (int i = 0; i < strSecret.Length; i++)
                {
                    //find the answer that matches
                    PdfPCell cellnum = new PdfPCell(new Phrase(findKey(strSecret[i]).ToString(), m_fNorm));
                    cellnum.HorizontalAlignment = Element.ALIGN_CENTER;
                    secret.AddCell(cellnum);
                }
                for (int i = 0; i < strSecret.Length; i++)
                {
                    PdfPCell ans = new PdfPCell(new Phrase("    "));
                    ans.MinimumHeight = 50;
                    secret.AddCell(ans);
                }
                m_doc.Add(secret);
            }

        }

    }
}
