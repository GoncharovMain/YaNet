using YaNet.Nodes;

namespace YaNet
{
    public class Definer
    {
        private int _position = 0;
        private StringBuilder _buffer;
        private Mark[] _rows;
        private Marker _marker;

        // cR - currentRow;
        private int _cR;
        public Definer(StringBuilder buffer, Mark[] rows)
        {
            _buffer = buffer;
            _rows = rows;
            _marker = new Marker(_buffer);
            _cR = 0;
        }

        private void Next()
        {
            _cR++;
            //Console.WriteLine($"get next: {_cR}");
        }

        public Collection DefineCollection()
        {
            if (_cR + 1 == _rows.Length)
            {
                return new Collection(Define());
            }

            List<INode> collection = new List<INode>();

            int currentIndent = LevelIndent();

            // Console.WriteLine($"_cR: {_cR} + 1 < {_rows.Length} indent: {currentIndent} added '{_marker.Buffer(_rows[_cR])}' first");

            collection.Add(Define());



            while (HasNext())
            {
                int nextIndent = new Peeker(_buffer, _rows[_cR + 1]).IndentLevel("\t");

                if (currentIndent != nextIndent)
                {
                    break;
                }

                Next();

                // Console.WriteLine($"_cR: {_cR} + 1 < {_rows.Length} indent: {currentIndent} added '{_marker.Buffer(_rows[_cR])}'");

                collection.Add(Define());
            }

            return new Collection(collection.ToArray());
        }

        private int LevelIndent()
            => new Peeker(_buffer, _rows[_cR]).IndentLevel("\t");

        private bool HasNext() => _cR + 1 < _rows.Length;

        public INode Define()
        {
            if (_cR + 1 == _rows.Length)
            {
                var row = _rows[_cR];

                Peeker peeker = new Peeker(_buffer, row);

                if (peeker.Contains(": "))
                {
                    (Mark key, Mark value) = peeker.ToPair(": ");

                    return new Pair(key, value);
                }

                if (peeker.Contains("- "))
                {
                    return new Item(new Scalar(peeker.ToItemScalar()));
                }

                throw new Exception("Has not features.");
            }

            Peeker current = new Peeker(_buffer, _rows[_cR]);
            Peeker next = new Peeker(_buffer, _rows[_cR + 1]);

            int cI = current.IndentLevel("\t");
            int nI = next.IndentLevel("\t");

            if (cI + 1 < nI)
            {
                throw new Exception($"Not correct indent: expected ");
            }

            if (current.Contains(": "))
            {
                (Mark key, Mark value) = current.ToPair(": ");

                if (cI + 1 == nI)
                {
                    return new NodeReference(key, value, DefineCollection());
                }

                return new Pair(key, value);
            }


            if (current.Contains(':'))
            {
                Mark key = current.ToKey();

                Next();

                return new Node(key, DefineCollection());
            }

            if (current.Contains("- "))
            {
                return new Item(new Scalar(current.ToItemScalar()));
            }

            if (current.Contains('-'))
            {
                return new Item(DefineCollection());
            }

            throw new Exception("");
        }

        private bool IsEmptyOrComment()
        {
            return false;
        }
    }
}