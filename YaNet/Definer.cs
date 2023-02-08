namespace YaNet
{
    public class Definer
    {
        private StringBuilder _buffer;
        private Mark[] _rows;
        private Marker _marker;

        private int _cR; // cR - currentRow;
        private int _nR; // nR - nextRow;

        private Mark _currentRow => _rows[_cR];
        private Mark _nextRow => _rows[_nR];

        public Definer(StringBuilder buffer, Mark[] rows)
        {
            _buffer = buffer;
            _rows = rows;
            _marker = new Marker(_buffer);
            _cR = 0;
            _nR = 0;
        }

        public Collection DefineCollection()
        {
            if (_nR == _rows.Length)
            {
                return new Collection(Define());
            }

            List<INode> collection = new List<INode>();

            int currentIndent = LevelIndent();

            collection.Add(Define());

            while (HasNext())
            {
                int nextIndent = new Peeker(_buffer, _nextRow).IndentLevel("\t");

                if (currentIndent != nextIndent)
                {
                    break;
                }

                Next();

                collection.Add(Define());
            }

            return new Collection(collection.ToArray());
        }

        private INode Define()
        {
            if (_nR == _rows.Length)
            {
                Peeker peeker = new Peeker(_buffer, _currentRow);

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

            Peeker current = new Peeker(_buffer, _currentRow);
            Peeker next = new Peeker(_buffer, _nextRow);

            int cI = current.IndentLevel("\t");
            int nI = next.IndentLevel("\t");

            if (cI + 1 < nI)
            {
                throw new Exception($"Not correct indent in line {_nR}: expected level {cI + 1} but actual {nI}.");
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

            throw new Exception($"Has not features on {_cR} row: '{_marker.Buffer(_currentRow)}'.");
        }

        private bool IsEmptyOrComment()
        {
            Mark row = _nextRow;

            if (row.End - row.Start < 1)
            {
                return true;
            }

            Peeker peeker = new Peeker(_buffer, row);

            if (peeker.Contains('#'))
            {
                int startComment = peeker.IndexOf('#');

                // comment for any place
                _rows[_nR] = new Mark(_nextRow.Start, startComment - 1);

                peeker = new Peeker(_buffer, row.Start, startComment - 1);
            }

            if (peeker.IsEmptyOrSpace())
            {
                return true;
            }

            return false;
        }
        public void FirstNext()
        {
            while (_nR < _rows.Length && IsEmptyOrComment())
            {
                _nR++;
            }

            Next();
        }

        private void Next()
        {
            _cR = _nR;
            _nR++;

            while (_nR < _rows.Length && IsEmptyOrComment())
            {
                _nR++;
            }
        }

        private int LevelIndent()
            => new Peeker(_buffer, _currentRow).IndentLevel("\t");

        private bool HasNext() => _nR < _rows.Length;
    }
}