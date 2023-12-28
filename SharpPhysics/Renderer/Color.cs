using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.Renderer
{
    public class Color
    {
        public byte R = 255, G = 255, B = 255, A = 255;

        public Color() { }

        public Color(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        /// <summary>
        /// Returns a color from the key.
        /// </summary>
        /// <param name="key">
        /// the color name, e.g gray.
        /// </param>
        public Color(ColorName key)
        {
            string rgbVal = ((int)key).ToString();
            try
            {
                R = byte.Parse(rgbVal[0..3]);
                G = byte.Parse(rgbVal[2..3]);
                B = byte.Parse(rgbVal[5..3]);
            }
            catch
            {
                ErrorHandler.ThrowError("Error, Internal Errror, Color.Color(ColorName key) failed with input " + key.ToString(), true);
            }
        }

        public override string ToString()
        {
            return $"{R},{G},{B}";
        }
    }
}
