﻿using System;

namespace MyCSharpLib.Utilities.Collections
{
    public delegate void ActionSequenceLifeCycleEventHandler<TKey>(
        ActionSequence<TKey> sender, 
        TKey key, 
        ActionSequenceAction<TKey> action)
        where TKey : IEquatable<TKey>;
}
