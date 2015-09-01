using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorksheetGen
{
    enum eOperation
    {
        addition, subtraction, multiplication, division

    };
    class Problem
    {
        int[] numbers;
        eOperation oper;
        int answer;
        char key;

        public int getAnswer() { return answer; }
        public eOperation getOperation() { return oper; }
        public void setVal(int[] val, eOperation op, char keyval, int ans = 0)
        {
            numbers = new int[val.Length];
            for (int i = 0; i < val.Length; i++)
            {
                numbers[i] = val[i];
            }
            oper = op;
            answer = ans;
            key = keyval;
        }
        public char getKey() { return key; }
        public int[] getNumbers() { return numbers; }
    }
}
