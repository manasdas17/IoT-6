using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Demos.Utils
{
    public class EmoticonsFrameGenerator
    {
        public static AnimationFrame[] getAnimationFrames()
        {

            return new[]
            {
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00111100,
                        BinaryValues.b01000010,
                        BinaryValues.b10100101,
                        BinaryValues.b10000001,
                        BinaryValues.b10100101,
                        BinaryValues.b10011001,
                        BinaryValues.b01000010,
                        BinaryValues.b00111100
                    },
                    Duration = 250
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00111100,
                        BinaryValues.b01000010,
                        BinaryValues.b10100101,
                        BinaryValues.b10000001,
                        BinaryValues.b10011001,
                        BinaryValues.b10100101,
                        BinaryValues.b01000010,
                        BinaryValues.b00111100
                    },
                    Duration = 250
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00111100,
                        BinaryValues.b01000010,
                        BinaryValues.b10100101,
                        BinaryValues.b10000001,
                        BinaryValues.b10111101,
                        BinaryValues.b10000001,
                        BinaryValues.b01000010,
                        BinaryValues.b00111100
                    },
                    Duration = 250
                },
            };
        }
    }
}
