using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Demos.Utils
{
    public class SpaceInvaderAnimationFrameGenerator
    {
        public static AnimationFrame[] getAnimationFrames()
        {

            return new[]
            {
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00011000,
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11111111,
                        BinaryValues.b00100100,
                        BinaryValues.b01011010,
                        BinaryValues.b10100101
                    },
                    Duration = 250
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00011000,
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11111111,
                        BinaryValues.b00100100,
                        BinaryValues.b01011010,
                        BinaryValues.b01000010,
                    },
                    Duration = 250
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00011000,
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11111111,
                        BinaryValues.b00100100,
                        BinaryValues.b01011010,
                        BinaryValues.b10100101
                    },
                    Duration = 250
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00011000,
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11111111,
                        BinaryValues.b00100100,
                        BinaryValues.b01011010,
                        BinaryValues.b01000010,
                    },
                    Duration = 250
                },

                // 2nd alien
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00000000,
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11011011,
                        BinaryValues.b01111110,
                        BinaryValues.b00100100,
                        BinaryValues.b11000011,
                    },
                    Duration = 250
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11011011,
                        BinaryValues.b01111110,
                        BinaryValues.b00100100,
                        BinaryValues.b00100100,
                        BinaryValues.b00100100,
                    },
                    Duration = 250
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00000000,
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11011011,
                        BinaryValues.b01111110,
                        BinaryValues.b00100100,
                        BinaryValues.b11000011,
                    },
                    Duration = 250
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11011011,
                        BinaryValues.b01111110,
                        BinaryValues.b00100100,
                        BinaryValues.b00100100,
                        BinaryValues.b00100100,
                    },
                    Duration = 250
                },

                // 3rd alien
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00100100,
                        BinaryValues.b00100100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11111111,
                        BinaryValues.b11111111,
                        BinaryValues.b10100101,
                        BinaryValues.b00100100,
                    },
                    Duration = 250
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00100100,
                        BinaryValues.b10100101,
                        BinaryValues.b11111111,
                        BinaryValues.b11011011,
                        BinaryValues.b11111111,
                        BinaryValues.b01111110,
                        BinaryValues.b00100100,
                        BinaryValues.b01000010,
                    },
                    Duration = 250
                },

                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00100100,
                        BinaryValues.b00100100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11111111,
                        BinaryValues.b11111111,
                        BinaryValues.b10100101,
                        BinaryValues.b00100100,
                    },
                    Duration = 250
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00100100,
                        BinaryValues.b10100101,
                        BinaryValues.b11111111,
                        BinaryValues.b11011011,
                        BinaryValues.b11111111,
                        BinaryValues.b01111110,
                        BinaryValues.b00100100,
                        BinaryValues.b01000010,
                    },
                    Duration = 250
                },


                // 4th alien
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b00110011,
                        BinaryValues.b01111110,
                        BinaryValues.b00111100,
                        BinaryValues.b00000000,
                        BinaryValues.b00001000,
                        BinaryValues.b00000000,
                    },
                    Duration = 125
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b10011001,
                        BinaryValues.b01111110,
                        BinaryValues.b00111100,
                        BinaryValues.b00000000,
                        BinaryValues.b00001000,
                        BinaryValues.b00001000,
                    },
                    Duration = 125
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11001100,
                        BinaryValues.b01111110,
                        BinaryValues.b00111100,
                        BinaryValues.b00000000,
                        BinaryValues.b00000000,
                        BinaryValues.b00001000,
                    },
                    Duration = 125
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b01100110,
                        BinaryValues.b01111110,
                        BinaryValues.b00111100,
                        BinaryValues.b00000000,
                        BinaryValues.b00000000,
                        BinaryValues.b00000000,
                    },
                    Duration = 125
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b00110011,
                        BinaryValues.b01111110,
                        BinaryValues.b00111100,
                        BinaryValues.b00000000,
                        BinaryValues.b00001000,
                        BinaryValues.b00000000,
                    },
                    Duration = 125
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b10011001,
                        BinaryValues.b01111110,
                        BinaryValues.b00111100,
                        BinaryValues.b00000000,
                        BinaryValues.b00001000,
                        BinaryValues.b00001000,
                    },
                    Duration = 125
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11001100,
                        BinaryValues.b01111110,
                        BinaryValues.b00111100,
                        BinaryValues.b00000000,
                        BinaryValues.b00000000,
                        BinaryValues.b00001000,
                    },
                    Duration = 125
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b01100110,
                        BinaryValues.b01111110,
                        BinaryValues.b00111100,
                        BinaryValues.b00000000,
                        BinaryValues.b00000000,
                        BinaryValues.b00000000,
                    },
                    Duration = 125
                },

                // faster repeat of 1-4
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00011000,
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11111111,
                        BinaryValues.b00100100,
                        BinaryValues.b01011010,
                        BinaryValues.b10100101
                    },
                    Duration = 125
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00011000,
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11111111,
                        BinaryValues.b00100100,
                        BinaryValues.b01011010,
                        BinaryValues.b01000010,
                    },
                    Duration = 125
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00011000,
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11111111,
                        BinaryValues.b00100100,
                        BinaryValues.b01011010,
                        BinaryValues.b10100101
                    },
                    Duration = 125
                },
                new AnimationFrame()
                {
                    Values = new[]
                    {
                        BinaryValues.b00011000,
                        BinaryValues.b00111100,
                        BinaryValues.b01111110,
                        BinaryValues.b11011011,
                        BinaryValues.b11111111,
                        BinaryValues.b00100100,
                        BinaryValues.b01011010,
                        BinaryValues.b01000010,
                    },
                    Duration = 125
                },

            };
        }

    }
}
