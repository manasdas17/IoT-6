using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ST.IoT.Demos.Utils
{
    public class OneShotPeriodicTimer
    {
        private CancellationTokenSource _cts;

        public Action TheAction { get; set; }

        public void schedule(Action theAction, int milliesFromNow)
        {
            TheAction = theAction;

            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }

            
            _cts = new CancellationTokenSource();
            var task = Task.Delay(milliesFromNow, _cts.Token).ContinueWith(
                _ =>
                {
                    _cts = null;
                    theAction();
                }, _cts.Token);
        }

    }
}
