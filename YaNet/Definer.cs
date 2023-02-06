using YaNet.Features;
using YaNet.Nodes;

namespace YaNet
{
    public class Definer
    {
        private StringBuilder _buffer;
        private Mark[] _rows;
        private Marker _marker;

        private int _cR; // cR - currentRow;
        private int _nR; // nR - nextRow;
        public Definer(StringBuilder buffer, Mark[] rows)
        {
            _buffer = buffer;
            _rows = rows;
            _marker = new Marker(_buffer);
            _cR = 0;
            _nR = 0;
        }
        private bool IsEmptyOrComment()
        {
            Mark row = _rows[_cR];

            if (row.End - row.Start < 1)
            {
                return true;
            }

            Peeker peeker = new Peeker(_buffer, _rows[_cR]);
            
            if (peeker.Contains('#'))
            {
                int startComment = peeker.IndexOf('#');

                Console.WriteLine($"_rows[_cR].Start: {_rows[_cR].Start} startComment: {startComment}");

                peeker = new Peeker(_buffer, _rows[_cR].Start, startComment);
            }

            if (peeker.IsEmptyOrSpace())
            {
                return true;
            }

            return false;
        }

        public void Next()
        {
            _cR++;
        }

        public Collection DefineCollection()
        {
            if (_cR + 1 == _rows.Length)
            {
                return new Collection(Define());
            }

            List<INode> collection = new List<INode>();

            int currentIndent = LevelIndent();

            collection.Add(Define());

            while (HasNext())
            {
                int nextIndent = new Peeker(_buffer, _rows[_cR + 1]).IndentLevel("\t");

                if (currentIndent != nextIndent)
                {
                    break;
                }

                Next();

                collection.Add(Define());
            }

            return new Collection(collection.ToArray());
        }

        private int LevelIndent()
            => new Peeker(_buffer, _rows[_cR]).IndentLevel("\t");

        private bool HasNext() => _cR + 1 < _rows.Length;

        private Mark CurrentRow() => _rows[_cR];

        private Mark NextRow() => _rows[_cR + 1];

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
                throw new Exception($"Not correct indent in line {_cR + 1}: expected level {cI + 1} but actual {nI}.");
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
                Next();
                return new Item(DefineCollection());
            }

            throw new Exception($"Has not features on {_cR} row: '{_marker.Buffer(_rows[_cR])}'.");
        }
    }
}