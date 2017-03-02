using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MatrixLedTableController
{
    public static class CharacterLookup
    {
        public const int characterHeight = 8;
        public const int characterWidth = 6;

        public static ulong GetCharUlong(char c)
        {
            switch(c)
            {
                case 'A':    return 227873781661662;
                case 'B':    return 422199034855391;
                case 'C':    return 9565685714534398;
                case 'D':    return 140724398931935;
                case 'E':    return 281410551955455;
                case 'F':    return 9020603847426047;
                case 'G':    return 417801450766302;
                case 'H':    return 228785541988957427;
                case 'I':    return 9223144176159343372;
                case 'J':    return 3513788460580408368;
                case 'K':    return 227860354645235;
                case 'L':    return 281409529589955;
                case 'M':    return 227860698627297;
                case 'N':    return 227860832812275;
                case 'O':    return 136326352420830;
                case 'P':    return 13404056010719;
                case 'Q':    return 277072430710750;
                case 'R':    return 227860363034591;
                case 'S':    return 140721356816382;
                case 'T':    return 53614281281535;
                case 'U':    return 136326352420083;
                case 'V':    return 54906657389811;
                case 'W':    return 148708944461043;
                case 'X':    return 227860337605875;
                case 'Y':    return 53614596799731;
                case 'Z':    return 281409582370815;
                case ' ':    return 0;
                case '.':    return 13400297963520;
                case ',':    return 4607426166784;

                case '0':    return 136326352420830;
                case '1':    return 132779118478220;
                case '2':    return 281409720881119;
                case '3':    return 140720819867615;
                case '4':    return 214457363938547;
                case '5':    return 140721373593599;
                case '6':    return 136326548307966;
                case '7':    return 107228568948735;
                case '8':    return 136325994594270;
                case '9':    return 214457363939294;

                default:     return 0;
            }
        }
    }
}
