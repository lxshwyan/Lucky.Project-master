using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime;
using System.Text;

namespace Lucky.Proect.Core
{
    public class EnginContext
    {
        private static IEngine _engine;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]    //多线程同时只能访问一次 
        public static IEngine Initialize(IEngine engine)
        {
            if (_engine == null)
                _engine = engine;
            return _engine;
        }

        /// <summary>
        /// 当前引擎
        /// </summary>
        public static IEngine Current
        {
            get
            {
                return _engine;
            }
        }
    }
}
