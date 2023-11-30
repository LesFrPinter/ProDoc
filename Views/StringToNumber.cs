using System.Windows;

namespace ProDocEstimate.Views
{
    class StringToNumber {
        //TODO: Deal with RollWidth strings that have additional text after the fraction
        public float Convert(string s)
        {   if(s.Length== 0) return 0;
            float result = 0.0F; float IntPart = 0.0F; float FracPart = 0.0F;

            int blankloc = s.IndexOf(' ');
            if (blankloc == -1) { blankloc = s.Length; }
            string intpart = s.Substring(0, blankloc);
            IntPart = float.Parse(intpart.ToString());

            if (s.IndexOf('/') > 0)
            {   string fraction = s.Substring(blankloc + 1);
                int whereSlash = fraction.IndexOf('/');
                int num = int.Parse(fraction.Substring(0, whereSlash));
                int den = int.Parse(fraction.Substring(whereSlash + 1));
                if (num > den) { MessageBox.Show("Nice try..."); } else { FracPart = (float)num / (float)den; }
            }
            result = IntPart + FracPart; return result;
        }
    }
}