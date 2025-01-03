﻿namespace Magro.Syake.Syntax
{
    internal class Location
    {
        public int Line { get; private set; }
        public int Column { get; private set; }

        public Location(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return $"{Line}:{Column}";
        }
    }
}
