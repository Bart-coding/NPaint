using System;
using System.Collections.Generic;
using System.Text;

namespace NPaint.Iterator___singleton
{
    class RGBIterator
    {
        private RGB RGB;
        private static RGBIterator Instance = new RGBIterator();

        private RGBIterator()
        {
            RGB = new RGB(255, 0, 0);
        }

        public static RGBIterator getIterator()
        {
            return Instance;
        }
        
        public RGB Next()
        {
            RGB temp = new RGB(RGB.R, RGB.G, RGB.B);

            if(RGB.R == 0)
            {
                if(RGB.G == 255)
                {
                    if(RGB.B != 255)
                    {
                        RGB.B += 51;
                    }
                    else
                    {
                        RGB.G -= 51;
                    }
                }
                else
                {
                    if (RGB.G != 0)
                    {
                        RGB.G -= 51;
                    }
                    else
                    {
                        RGB.R += 51;
                    }
                }
            }
            else if(RGB.G == 0)
            {
                if (RGB.B == 255)
                {
                    if (RGB.R != 255)
                    {
                        RGB.R += 51;
                    }
                    else
                    {
                        RGB.B -= 51;
                    }
                }
                else
                {
                    if (RGB.B != 0)
                    {
                        RGB.B -= 51;
                    }
                    else
                    {
                        RGB.G += 51;
                    }
                }
            }
            else if(RGB.B == 0)
            {
                if (RGB.R == 255)
                {
                    if (RGB.G != 255)
                    {
                        RGB.G += 51;
                    }
                    else
                    {
                        RGB.R -= 51;
                    }
                }
                else
                {
                    if (RGB.R != 0)
                    {
                        RGB.R -= 51;
                    }
                    else
                    {
                        RGB.B += 51;
                    }
                }
            }
            return temp;
        }

        public Boolean isDone()
        {
            return false;
        }
    }
}
