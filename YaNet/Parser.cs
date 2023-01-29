﻿using System.Text;
using YaNet.Features;
using YaNet.Exceptions;

namespace YaNet
{
    public class Parser
    {
        private StringBuilder _buffer;
        private Marker _marker;

        public Parser(StringBuilder buffer)
        {
            _buffer = buffer;
            _marker = new Marker(_buffer);
        }

        public object Parse(object obj)
        {
            Mark[] rows = new Peeker(_buffer).Split('\n');


            Definer definer = new Definer(_buffer, rows);

            Collection collection = definer.DefineCollection();

            collection.Print(_buffer);

            foreach (INode node in collection.Nodes)
                node.Init(obj, _buffer);

            return obj;
        }
    }
}