using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyLib.API
{
    public class Asm
    {
        private StringBuilder code = new StringBuilder();

        public void Pushad()
        {
            code.Append("60");
        }
        public void Sub_ESP(int i)
        {
            if (i <=127 & i >= -128)
            {
                code.Append("83EC");
                code.Append(i.ToString("X"));
            }
            else
            {
                code.Append("81EC");
                code.Append(i.ToString("X"));
            }
        }
        public void Mov_ECX_ESP()
        {
            code.Append("8BCC");
        }
        public void Mov_EAX(int i)
        {
            code.Append("B8");
            code.Append(i.ToString("X"));
        }
        public void Push_EAX()
        {
            code.Append("50");
        }
        public void Call_Ptr(int i )
        {
            code.Append("FF15");
            code.Append(i.ToString("X"));
        }
        public void Add_ESP(int i)
        {
            if (i <= 127 & i >= -128)
            {
                code.Append("83C4");
                code.Append(i.ToString("X"));
            }
            else
            {
                code.Append("81C4");
                code.Append(i.ToString("X"));
            }
        }
        public void Popad()
        {
            code.Append("61");
        }
        public void ret(int? i)
        {
            if (i==null)
            {
                code.Append("C3");
            }
            else
            {
                code.Append("C2");
                int value = (int)i;
                code.Append(value.ToString("X"));
            }
        }
    }
}
