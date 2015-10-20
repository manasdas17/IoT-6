using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Demos.Utils
{
    public class BackpackAnimator
    {
        private Adafrut8x8LEDBackpack _backpack;
        private AnimationFrame[] _frames;

        private bool _run = false;
        private int _frame = 0;
        private byte[] _buffer = new byte[8];

        public BackpackAnimator(Adafrut8x8LEDBackpack backpack, AnimationFrame[] frames)
        {
            _backpack = backpack;
            _frames = frames;
        }

        public void start()
        {
            _run = true;
            render();
        }

        private void render()
        { 
            if (!_run)
            {
                _backpack.clear();
                return;
            }

            render(_frames[_frame]);

            Task.Delay(_frames[_frame].Duration).ContinueWith(_ => render());
            _frame = (_frame + 1) % _frames.Length;

        }

        public void stop()
        {
            _run = false;
        }

        public void render(AnimationFrame frame)
        {
            for (var i = 0; i < 8; i++)
            {
                var b = frame.Values[i];
                _buffer[i] = (byte)((b >> 1) | (b << 7));
            }

            _backpack.drawFrame(_buffer);

        }

        public void drawFrame(int frameIndex)
        {
            render(_frames[frameIndex]);
        }
    }
}
