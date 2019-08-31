﻿using System;

namespace MyCSharpLib.Utilities.Collections
{
    public class ActionSequenceAction<TKey> 
    {
        public TKey NextAction { get; set; }
        public Action Action { get; set; }


        public void Execute() => Action();
    }
}