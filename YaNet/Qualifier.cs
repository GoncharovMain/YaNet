using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YaNet.Features;

namespace YaNet
{
    public class Qualifier
    {
        private StringBuilder _buffer;

        private Mark[] _marks;
        private int[] _indents;

        private Mark _currentMark => _marks[_cM];
        private Mark _nextMark => _marks[_nM];

        private int _cM;
        private int _nM;

        private int _currentIndent => _indents[_cM];
        private int _nextIndent => _indents[_nM];


        private char _indentFeature = '\t';
        private char _rowDelimiter = '\n';

        private char _keyDelimiter = ':';
        private string _pairDelimiter = ": ";

        private char _itemOfNodeFeature = '-';
        private string _itemFeature = "- ";

        private char _commentSymbol = '#';

        private string _startReference = "${";
        private char _endReference = '}';

        public Qualifier(StringBuilder buffer)
        {
            _buffer = buffer;

            SplitWithIndents();

            ParseReferences();

            SetStartPosition();
        }

        private void SplitWithIndents()
        {
            int countDelimiters = Counter(new Mark(0, _buffer.Length - 1), _rowDelimiter) + 1;

            _marks = new Mark[countDelimiters];
            _indents = new int[countDelimiters];

            int i = 0;

            int start, end;

            for (start = 0, end = 0; end < _buffer.Length; end++)
            {
                if (_buffer[end] == _rowDelimiter)
                {
                    _marks[i] = new Mark(start, end - 1);

                    _indents[i] = Indent(_marks[i]);

                    i++;

                    start = end + 1;
                }
            }

            _marks[^1] = new Mark(start, _buffer.Length - 1);
            _indents[^1] = Indent(_marks[^1]);
        }

        public void ParseReferences()
        {
            Mark[][] references = new Mark[_marks.Length][];

            for (int i = 0; i < _marks.Length; i++)
            {
                references[i] = DetectReferences(_marks[i]);
            }
        }

        public Mark[] DetectReferences(Mark mark)
        {
            List<Mark> references = new List<Mark>();

            int start = IndexOf(mark, _startReference);
            int end = IndexOf(mark, _endReference);

            while (start != -1 && end != -1)
            {
                references.Add(new Mark(start + _startReference.Length, end - 1));

                Mark biasMark = new Mark(end + 1, mark.End);

                start = IndexOf(biasMark, _startReference);
                end = IndexOf(biasMark, _endReference);
            }
            
            return references.ToArray();
        }

        public Pair ToPair()
        {
            int delimiterPosition = CurrentIndexOf(_pairDelimiter);

            int delimiterLength = _pairDelimiter.Length;

            Mark key = new Mark(_currentMark.Start + _currentIndent, delimiterPosition - 1);
            Mark value = new Mark(delimiterPosition + delimiterLength, _currentMark.End);

            return new Pair(key, value);
        }

        public Item ToItemScalar()
        {
            int itemFeatureLength = _itemFeature.Length;

            Mark scalar = new Mark(_currentMark.Start + _currentIndent + itemFeatureLength, _currentMark.End);

            return new Item(new Scalar(scalar));
        }

        public Node ToNode()
        {
            Mark key = new Mark(_currentMark.Start + _currentIndent, _currentMark.End - 1);

            Next();

            return new Node(key, DefineCollection());
        }

        public NodeReference ToNodeReference()
        {
            int delimiterPosition = CurrentIndexOf(_pairDelimiter);

            int delimiterLength = _pairDelimiter.Length;

            Mark key = new Mark(_currentMark.Start + _currentIndent, delimiterPosition - 1);
            Mark reference = new Mark(delimiterPosition + delimiterLength, _currentMark.End);
            
            Next();

            return new NodeReference(key, reference, DefineCollection());
        }

        public Collection DefineCollection()
        {
            if (!HasNext())
            {
                return new Collection(Define());
            }

            int currentIndent = _currentIndent;

            List<INode> collection = new List<INode>() { Define() };

            while (HasNext())
            {
                if (currentIndent != _nextIndent)
                {
                    break;
                }

                Next();

                collection.Add(Define());
            }

            return new Collection(collection.ToArray());
        }

        public INode Define()
        {
            if (!HasNext())
            {
                if (CurrentContains(_pairDelimiter))
                {
                    return ToPair();
                }

                if (CurrentContains(_itemFeature))
                {
                    return ToItemScalar();
                }

                throw new Exception($"Has not feature.");
            }

            if (_currentIndent + 1 < _nextIndent)
            {
                throw new Exception($"Not correct indent in line {_nM}: expected level {_currentIndent + 1} but actual {_nextIndent}.");
            }

            if (CurrentContains(_pairDelimiter))
            {
                if (_currentIndent + 1 == _nextIndent)
                {
                    return ToNodeReference();
                }

                return ToPair();
            }

            if (CurrentContains(_keyDelimiter))
            {
                return ToNode();
            }

            if (CurrentContains(_itemFeature))
            {
                return ToItemScalar();
            }

            if (CurrentContains(_itemOfNodeFeature))
            {
                Next();

                return new Item(DefineCollection());
            }

            throw new Exception($"Has not features on {_cM} row: '{Buffer()}'.");
        }

        public void SetStartPosition()
        {
            _cM = 0;
            _nM = 0;

            while (_nM < _marks.Length && IsEmptyOrComment())
            {
                _nM++;
            }

            Next();
        }

        private void Next()
        {
            _cM = _nM;
            _nM++;

            while (_nM < _marks.Length && IsEmptyOrComment())
            {
                _nM++;
            }
        }
        public bool HasNext() => _nM < _marks.Length;

        private bool IsEmptyOrComment()
        {
            if (_nextMark.End - _nextMark.Start < 1)
            {
                return true;
            }

            if (NextContains(_commentSymbol))
            {
                int startComment = NextIndexOf(_commentSymbol);

                // bad practic!
                _marks[_nM] = new Mark(_nextMark.Start, startComment - 1);
            }

            if (IsEmptyOrSpace(_nextMark))
            {
                return true;
            }

            // move to SplitWithIndents
            TrimRight();

            return false;
        }

        public bool IsEmptyOrSpace(Mark mark)
        {
            char space = ' ', tab = '\t';

            if (mark.End - mark.Start == 0)
            {
                return true;
            }

            for (int i = mark.Start; i <= mark.End; i++)
            {
                if (_buffer[i] != space || _buffer[i] != tab)
                {
                    return false;
                }
            }

            return true;
        }
        public void TrimRight()
        {
            char space = ' ', tab = '\t';
            
            for (int i = _nextMark.End; i >= _nextMark.Start; i--)
            {
                if (_buffer[i] != space && _buffer[i] != tab)
                {
                    _marks[_nM] = new Mark(_nextMark.Start, i);
                    break;
                }
            }
        }

        public string Buffer() => _buffer.ToString(_currentMark.Start, _currentMark.Length);

        public string Buffer(Mark mark) => _buffer.ToString(mark.Start, mark.Length);

        private int IndexOf(Mark mark, char symbol)
        {
            for (int i = mark.Start; i <= mark.End; i++)
            {
                if (_buffer[i] == symbol)
                {
                    return i;
                }
            }

            return -1;
        }
        private int IndexOf(Mark mark, string substring)
        {
            if (mark.Length == 0 || substring.Length == 0)
                return -1;
                // throw new Exception("Characters is empty.");

            if (mark.Length < substring.Length)
                return -1;
                // throw new Exception("Length buffer less than length substring.");

            bool hasSubstring = true;

            int maxLength = mark.End - substring.Length + 2;

            for (int i = mark.Start; i < maxLength; i++)
            {
                for (int j = 0; j < substring.Length; j++)
                {
                    if (_buffer[i + j] != substring[j])
                    {
                        hasSubstring = false;
                        break;
                    }
                }

                if (hasSubstring)
                {
                    return i;
                }

                hasSubstring = true;
            }

            return -1;
        }

        public int CurrentIndexOf(char symbol) => IndexOf(_currentMark, symbol);
        public int NextIndexOf(char symbol) => IndexOf(_nextMark, symbol);
        public int CurrentIndexOf(string substring) => IndexOf(_currentMark, substring);
        public int NextIndexOf(string substring) => IndexOf(_nextMark, substring);

        private bool Contains(Mark mark, char symbol)
        {
            for (int i = mark.Start; i <= mark.End; i++)
            {
                if (_buffer[i] == symbol)
                {
                    return true;
                }
            }

            return false;
        }
        private bool Contains(Mark mark, string substring)
        {
            bool contain;

            int maxLength = mark.End - substring.Length + 1;

            for (int i = mark.Start; i <= maxLength; i++)
            {
                contain = true;

                for (int j = 0; j < substring.Length; j++)
                {
                    if (_buffer[i + j] != substring[j])
                    {
                        contain = false;
                    }
                }

                if (contain)
                    return true;
            }

            return false;
        }

        public bool CurrentContains(char symbol) => Contains(_currentMark, symbol);
        public bool NextContains(char symbol) => Contains(_nextMark, symbol);
        public bool CurrentContains(string substring) => Contains(_currentMark, substring);
        public bool NextContains(string substring) => Contains(_nextMark, substring);

        public int Counter(Mark mark, char symbol)
        {
            int count = 0;

            for (int i = mark.Start; i <= mark.End; i++)
            {
                if (_buffer[i] == symbol)
                {
                    count++;
                }
            }

            return count;
        }

        public int Counter(Mark mark, string substring)
        {
            int count = 0;

            bool isEqual = true;

            int maxLength = mark.End - substring.Length;

            for (int i = mark.Start; i <= maxLength; i++)
            {
                for (int j = 0; j < substring.Length; j++, i++)
                {
                    if (_buffer[i] != substring[j])
                    {
                        isEqual = false;

                        break;
                    }
                }

                if (isEqual)
                {
                    count++;
                }

                isEqual = true;
            }

            return count;
        }

        public int Counter(char symbol) => Counter(_currentMark, symbol);
        public int Counter(string substring) => Counter(_currentMark, substring);

        public int Indent(Mark mark)
        {
            int countIndent = 0;

            for (int i = mark.Start; i <= mark.End; i++)
            {
                if (_buffer[i] != _indentFeature)
                {
                    return countIndent;
                }

                countIndent++;
            }

            return countIndent;
        }
    }
}
