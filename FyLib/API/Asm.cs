using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FyLib.API
{
    /// <summary>
    /// 提供便捷方法来拼装常用的 x86 函数调用指令序列。
    /// 每个方法都会将相应指令的机器码（十六进制字符串形式）追加到内部缓冲区。
    /// </summary>
    public class Asm
    {
        /// <summary>
        /// 存储生成的机器码（十六进制字符串）。
        /// </summary>
        private StringBuilder code = new StringBuilder();

        /// <summary>
        /// 追加 <c>PUSHAD</c> 指令（保存所有通用寄存器）。
        /// </summary>
        public void Pushad()
        {
            code.Append("60");
        }

        /// <summary>
        /// 生成 <c>SUB ESP, imm</c> 指令，用于为局部变量预留栈空间。
        /// 会根据立即数大小选择短立即数或完整立即数编码。
        /// </summary>
        /// <param name="i">要减去的字节数。</param>
        public void Sub_ESP(int i)
        {
            if (i <= 127 && i >= -128)
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
        /// <summary>
        /// 追加 <c>MOV ECX, ESP</c> 指令。
        /// </summary>
        public void Mov_ECX_ESP()
        {
            code.Append("8BCC");
        }

        /// <summary>
        /// 生成 <c>MOV EAX, imm32</c> 指令，将立即数加载到 <c>EAX</c>。
        /// </summary>
        /// <param name="i">要加载的 32 位值。</param>
        public void Mov_EAX(int i)
        {
            code.Append("B8");
            code.Append(i.ToString("X"));
        }

        /// <summary>
        /// 追加 <c>PUSH EAX</c> 指令。
        /// </summary>
        public void Push_EAX()
        {
            code.Append("50");
        }

        /// <summary>
        /// 生成 <c>CALL dword ptr [imm32]</c> 指令，通过间接地址调用函数指针。
        /// </summary>
        /// <param name="i">存放函数指针地址的内存位置。</param>
        public void Call_Ptr(int i)
        {
            code.Append("FF15");
            code.Append(i.ToString("X"));
        }

        /// <summary>
        /// 生成 <c>ADD ESP, imm</c> 指令，用于清理栈空间。
        /// </summary>
        /// <param name="i">要增加的字节数。</param>
        public void Add_ESP(int i)
        {
            if (i <= 127 && i >= -128)
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
        /// <summary>
        /// 追加 <c>POPAD</c> 指令（恢复所有通用寄存器）。
        /// </summary>
        public void Popad()
        {
            code.Append("61");
        }

        /// <summary>
        /// 生成 <c>RET</c> 或 <c>RET imm16</c> 指令，用于从函数返回。
        /// </summary>
        /// <param name="i">可选的栈空间清理字节数；为 <c>null</c> 时生成普通 <c>RET</c>。</param>
        public void ret(int? i)
        {
            if (i == null)
            {
                code.Append("C3");
            }
            else
            {
                code.Append("C2");
                var value = (int)i;
                code.Append(value.ToString("X"));
            }
        }
    }
}
